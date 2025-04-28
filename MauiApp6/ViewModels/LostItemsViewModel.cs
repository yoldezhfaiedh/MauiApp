//using System;
//using System;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text.Json;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using Microsoft.Maui.ApplicationModel;
//using Microsoft.Maui.Media;

//public class SignalementViewModel : INotifyPropertyChanged, IDisposable
//{
//    private readonly ISignalementService _service;
//    private bool _isBusy;
//    private string _message;
//    private string _currentUserEmail;
//    private Signalements _currentSignalement = new();

//    public event PropertyChangedEventHandler PropertyChanged;
//    public event Action StateHasChanged;

//    #region Properties

//    public bool IsBusy
//    {
//        get => _isBusy;
//        set
//        {
//            if (_isBusy == value) return;
//            _isBusy = value;
//            OnPropertyChanged();
//            OnPropertyChanged(nameof(IsNotBusy));
//        }
//    }

//    public bool IsNotBusy => !IsBusy;

//    public string Message
//    {
//        get => _message;
//        set => SetProperty(ref _message, value);
//    }

//    public string CurrentUserEmail
//    {
//        get => _currentUserEmail;
//        private set => SetProperty(ref _currentUserEmail, value);
//    }

//    public Signalements CurrentSignalement
//    {
//        get => _currentSignalement;
//        set => SetProperty(ref _currentSignalement, value);
//    }

//    public ObservableCollection<Signalements> Signalements { get; } = new();

//    #endregion

//    #region Commands

//    public ICommand LoadCommand { get; }
//    public ICommand SaveCommand { get; }
//    public ICommand DeleteCommand { get; }
//    public ICommand CaptureCommand { get; }
//    public ICommand TestConnectionCommand { get; }

//    #endregion

//    public SignalementViewModel(ISignalementService service)
//    {
//        _service = service ?? throw new ArgumentNullException(nameof(service));

//        // Initialisation des commandes
//        LoadCommand = new Command(async () => await ExecuteSafeAsync(LoadSignalements));
//        SaveCommand = new Command(async () => await ExecuteSafeAsync(SaveSignalement));
//        DeleteCommand = new Command<int>(async (id) => await ExecuteSafeAsync(() => DeleteSignalement(id)));
//        CaptureCommand = new Command(async () => await ExecuteSafeAsync(CapturePhoto));
//        TestConnectionCommand = new Command(async () => await ExecuteSafeAsync(TestDatabaseConnection));
//    }

//    #region Public Methods

//    public async Task InitAsync()
//    {
//        await ExecuteSafeAsync(async () =>
//        {
//            await LoadCurrentUserEmailAsync();
//            await LoadSignalements();
//        }, nameof(InitAsync));
//    }

//    public void Dispose()
//    {
//        StateHasChanged = null;
//    }

//    #endregion

//    #region Private Methods

//    private async Task LoadCurrentUserEmailAsync()
//    {
//        try
//        {
//            var email = await SecureStorage.GetAsync("Email");
//            CurrentUserEmail = email ?? "non-connecté";
//            Debug.WriteLine($"Email chargé: {CurrentUserEmail}");
//        }
//        catch (Exception ex)
//        {
//            Debug.WriteLine($"Erreur chargement email: {ex}");
//            CurrentUserEmail = "erreur-de-chargement";
//        }
//    }

//    private async Task LoadSignalements()
//    {
//        var list = await _service.GetAllSignalementsAsync();
//        Signalements.Clear();
//        foreach (var s in list.OrderByDescending(x => x.DateSignalement))
//        {
//            Signalements.Add(s);
//        }
//    }

//    private async Task SaveSignalement()
//    {
//        // Validation
//        if (!ValidateSignalement(out var validationMessage))
//        {
//            Message = validationMessage;
//            return;
//        }

//        // Valeurs par défaut
//        CurrentSignalement.Email ??= CurrentUserEmail;
//        CurrentSignalement.DateSignalement ??= DateTime.Now;

//        Debug.WriteLine($"Tentative enregistrement: {JsonSerializer.Serialize(CurrentSignalement)}");

//        bool success;
//        string operation;

//        if (CurrentSignalement.Id == 0)
//        {
//            success = await _service.AddSignalementAsync(CurrentSignalement);
//            operation = "création";
//        }
//        else
//        {
//            success = await _service.UpdateSignalementAsync(CurrentSignalement);
//            operation = "mise à jour";
//        }

//        Message = success ? $"✅ Signalement {operation} réussie" : $"❌ Échec {operation}";

//        if (success)
//        {
//            await LoadSignalements();
//            ResetCurrentSignalement();
//        }
//    }

//    private bool ValidateSignalement(out string errorMessage)
//    {
//        errorMessage = null;

//        if (string.IsNullOrWhiteSpace(CurrentSignalement.Titre))
//        {
//            errorMessage = "❌ Le titre est obligatoire";
//            return false;
//        }

//        if (string.IsNullOrWhiteSpace(CurrentSignalement.Description))
//        {
//            errorMessage = "❌ La description est obligatoire";
//            return false;
//        }

//        if (string.IsNullOrWhiteSpace(CurrentSignalement.Email))
//        {
//            errorMessage = "❌ L'email est obligatoire";
//            return false;
//        }

//        return true;
//    }

//    private void ResetCurrentSignalement()
//    {
//        CurrentSignalement = new Signalements
//        {
//            // Conserve certaines valeurs
//            Email = CurrentSignalement.Email,
//            Statut = "En cours"
//        };
//    }

//    private async Task DeleteSignalement(int id)
//    {
//        var success = await _service.DeleteSignalementAsync(id);
//        Message = success ? "🗑 Signalement supprimé" : "❌ Échec suppression";
//        await LoadSignalements();
//    }

//    private async Task CapturePhoto()
//    {
//        try
//        {
//            var photo = await MediaPicker.CapturePhotoAsync();
//            if (photo == null) return;

//            var fileName = Path.Combine(FileSystem.AppDataDirectory, $"{Guid.NewGuid()}.jpg");

//            using (var stream = await photo.OpenReadAsync())
//            using (var fileStream = File.OpenWrite(fileName))
//            {
//                await stream.CopyToAsync(fileStream);
//            }

//            await UpdateLocationAsync();

//            CurrentSignalement.ImagePath = fileName;
//            Message = "📸 Photo capturée avec succès";
//        }
//        catch (Exception ex)
//        {
//            Message = $"❌ Erreur capture photo: {ex.Message}";
//            Debug.WriteLine($"Erreur capture: {ex}");
//        }
//    }

//    private async Task UpdateLocationAsync()
//    {
//        try
//        {
//            var location = await Geolocation.GetLocationAsync(new GeolocationRequest
//            {
//                DesiredAccuracy = GeolocationAccuracy.Medium,
//                Timeout = TimeSpan.FromSeconds(10)
//            });

//            if (location != null)
//            {
//                CurrentSignalement.Latitude = location.Latitude;
//                CurrentSignalement.Longitude = location.Longitude;
//            }
//        }
//        catch (Exception ex)
//        {
//            Debug.WriteLine($"Erreur géolocalisation: {ex}");
//        }
//    }

//    private async Task TestDatabaseConnection()
//    {
//        try
//        {
//            // Test de connexion basique
//            var canConnect = await _service.TestConnectionAsync();
//            Message = canConnect ? "✅ DB connectée" : "❌ DB non accessible";

//            if (!canConnect) return;

//            // Test CRUD complet
//            var testItem = new Signalements
//            {
//                Titre = "TEST-" + DateTime.Now.ToString("HHmmss"),
//                Description = "Description de test",
//                Email = CurrentUserEmail ?? "test@example.com",
//                DateSignalement = DateTime.Now,
//                Statut = "En cours",
//                Latitude = 0,
//                Longitude = 0
//            };

//            // Create
//            var createSuccess = await _service.AddSignalementAsync(testItem);
//            Message += createSuccess ? " | ✅ Création OK" : " | ❌ Échec création";

//            if (!createSuccess) return;

//            // Read
//            var items = await _service.GetAllSignalementsAsync();
//            var readSuccess = items.Any(i => i.Titre == testItem.Titre);
//            Message += readSuccess ? " | ✅ Lecture OK" : " | ❌ Échec lecture";

//            if (readSuccess)
//            {
//                // Cleanup
//                var itemToDelete = items.First(i => i.Titre == testItem.Titre);
//                var deleteSuccess = await _service.DeleteSignalementAsync(itemToDelete.Id);
//                Message += deleteSuccess ? " | ✅ Nettoyage OK" : " | ❌ Échec nettoyage";
//            }
//        }
//        catch (Exception ex)
//        {
//            Message = $"❌ Test échoué: {ex.Message}";
//            Debug.WriteLine($"Erreur test DB: {ex}");
//        }
//    }

//    private async Task ExecuteSafeAsync(Func<Task> operation, [CallerMemberName] string operationName = null)
//    {
//        if (IsBusy)
//        {
//            Debug.WriteLine($"Opération {operationName} ignorée - Déjà en cours");
//            return;
//        }

//        try
//        {
//            IsBusy = true;
//            Debug.WriteLine($"Début opération: {operationName}");
//            await operation();
//        }
//        catch (Exception ex)
//        {
//            Message = $"❌ Erreur {operationName}: {ex.Message}";
//            Debug.WriteLine($"Erreur {operationName}: {ex}");
//        }
//        finally
//        {
//            IsBusy = false;
//            Debug.WriteLine($"Fin opération: {operationName}");
//            StateHasChanged?.Invoke();
//        }
//    }

//    private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
//    {
//        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
//        field = value;
//        OnPropertyChanged(propertyName);
//        return true;
//    }

//    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
//    {
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//    }

//    #endregion
//}


using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;

public class LostItemsViewModel : INotifyPropertyChanged
{
    private readonly IAppService _appService;

    public ObservableCollection<LostItem> LostItems { get; set; } = new();

    public LostItemsViewModel(IAppService appService)
    {
        _appService = appService;
    }

    public async Task<string> GetAddressFromCoordinates(double latitude, double longitude)
    {
        // 1. Validation des coordonnées GPS
        if (Math.Abs(latitude) > 90 || Math.Abs(longitude) > 180)
        {
            return "Coordonnées GPS invalides";
        }

        // 2. Construction de l'URL avec paramètres obligatoires
        var url = $"https://nominatim.openstreetmap.org/reverse?" +
                  $"lat={latitude.ToString(CultureInfo.InvariantCulture)}&" +
                  $"lon={longitude.ToString(CultureInfo.InvariantCulture)}&" +
                  "format=json&" +
                  "addressdetails=1&" +
                  "accept-language=fr&" +  // Français
                  "zoom=18";  // Niveau de détail

        using (var client = new HttpClient())
        {
            // 3. Configuration REQUISE du User-Agent
            client.DefaultRequestHeaders.UserAgent.ParseAdd("MyApp/1.0 (contact@example.com)");

            // 4. Respect du rate limiting (1 req/sec)
            await Task.Delay(1100);

            try
            {
                // 5. Utilisation de GetAsync + vérification du statut
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return $"Erreur API: {response.StatusCode} - {errorContent}";
                }

                // 6. Lecture et parsing sécurisé
                var responseBody = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(responseBody);

                // 7. Extraction robuste de l'adresse
                if (!json.RootElement.TryGetProperty("address", out var address))
                {
                    return "Adresse non trouvée dans la réponse";
                }

                var addressParts = new List<string>();

                // Champs prioritaires (en français)
                if (address.TryGetProperty("road", out var road) && !string.IsNullOrWhiteSpace(road.GetString()))
                    addressParts.Add(road.GetString());

                if (address.TryGetProperty("postcode", out var postcode) && !string.IsNullOrWhiteSpace(postcode.GetString()))
                    addressParts.Add(postcode.GetString());

                if (address.TryGetProperty("city", out var city) && !string.IsNullOrWhiteSpace(city.GetString()))
                    addressParts.Add(city.GetString());

                return addressParts.Count > 0
                    ? string.Join(", ", addressParts)
                    : "Adresse incomplète (voir JSON brut)";
            }
            catch (Exception ex)
            {
                // 8. Gestion fine des erreurs
                return ex switch
                {
                    HttpRequestException => "Erreur réseau: " + ex.Message,
                    JsonException => "Erreur de format JSON: " + ex.Message,
                    _ => "Erreur inattendue: " + ex.Message
                };
            }
        }
    }

    public async Task CaptureLostItemAsync(string description, string category, string email)
{
    try
    {
        // Capture la photo
        var photo = await Microsoft.Maui.Media.MediaPicker.CapturePhotoAsync();
        
        if (photo == null)
        {
            Console.WriteLine("Aucune photo n'a été capturée.");
            return;
        }

            // Récupère la localisation de l'utilisateur

            // Récupère la localisation de l'utilisateur
            var location = await Microsoft.Maui.Devices.Sensors.Geolocation.Default.GetLocationAsync();
            if (location == null)
            {
                Console.WriteLine("Impossible de récupérer la localisation.");
                return;
            }

            // Obtenir l'adresse à partir des coordonnées GPS
            var address = await GetAddressFromCoordinates(location.Latitude, location.Longitude);

            // Créer un dossier "Images" dans AppDataDirectory si il n'existe pas
            var imagesDirectory = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "Images");

        // Vérifier si le dossier existe déjà, sinon créer le dossier
        if (!Directory.Exists(imagesDirectory))
        {
            Directory.CreateDirectory(imagesDirectory);
            Console.WriteLine($"Dossier créé : {imagesDirectory}");
        }

        // Définir le chemin de l'image dans le dossier "Images"
        var fileName = Path.GetFileName(photo.FullPath);
        var filePath = Path.Combine(imagesDirectory, fileName); // Chemin complet dans le dossier Images

        // Sauvegarder l'image dans le dossier local
        using (var stream = await photo.OpenReadAsync())
        {
            // Crée un fichier et écrit le flux de l'image
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await stream.CopyToAsync(fileStream);
                Console.WriteLine($"Image sauvegardée à : {filePath}");
            }
        }

        // Créer un nouvel élément LostItem avec le chemin de l'image sauvegardée
        var newItem = new LostItem
        {
            Description = description,
            Category = category,
            PhotoPath = filePath, // Chemin complet pour l'image
            Latitude = location.Latitude,
            Location = address, // Ajouter l'adresse obtenue
            Email = email, // Ajouter l'adresse obtenue

            Longitude = location.Longitude,
            DateReported = DateTime.Now
        };

        // Sauvegarde l'élément dans la base de données via le service
        await _appService.AddLostItemAsync(newItem);

        // Rafraîchit les éléments après l'ajout
        await LoadItemsAsync();
        OnPropertyChanged(nameof(LostItems));
    }
    catch (Exception ex)
    {
        // Gérer l'exception
        Console.WriteLine($"Erreur lors de la capture et de la sauvegarde : {ex.Message}");
    }
}


    public async Task LoadItemsAsync()
    {
        // Récupère les éléments depuis la base de données
        var items = await _appService.GetLostItemsAsync();
        LostItems.Clear();
        foreach (var item in items)
        {
            LostItems.Add(item);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
