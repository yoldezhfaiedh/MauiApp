

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;

public class LostItemsViewModel : INotifyPropertyChanged
    // lorsque une propriete change le view model informe le view
{
    private readonly IAppService _appService;
    private string _currentUserEmail;

    public ObservableCollection<LostItem> LostItems { get; set; } = new();

    public LostItemsViewModel(IAppService appService)
    {
        _appService = appService;
        _ = InitializeAsync();
    }
    private async Task InitializeAsync()
    {
        _currentUserEmail = await SecureStorage.GetAsync("user_email");
        await LoadUserItemsAsync();
    }

    public async Task LoadUserItemsAsync()
    {
        var items = await _appService.GetLostItemsAsync(_currentUserEmail);
        LostItems.Clear();

        foreach (var item in items)
        {
            LostItems.Add(item);
        }
    }

    public async Task LoadLostItemsAsync()
    {
        var items = await _appService.Get1LostItemsAsync();
        LostItems.Clear();

        foreach (var item in items)
        {
            LostItems.Add(item);
        }
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

    public async Task CaptureLostItemAsync(string description, string category, string email, string status)
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


            // recuperer la localisation 
            var location = await Microsoft.Maui.Devices.Sensors.Geolocation.Default.GetLocationAsync();
            if (location == null)
            {
                Console.WriteLine("Impossible de récupérer la localisation.");
                return;
            }

            var address = await GetAddressFromCoordinates(location.Latitude, location.Longitude);

            // Créer un dossier  dans AppDataDirectory si il n'existe pas
            var imagesDirectory = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "Images");

        if (!Directory.Exists(imagesDirectory))
        {
            Directory.CreateDirectory(imagesDirectory);
            Console.WriteLine($"Dossier créé : {imagesDirectory}");
        }

        // on definit  le chemin de l'image dans le dossier iamges 
        var fileName = Path.GetFileName(photo.FullPath);
        var filePath = Path.Combine(imagesDirectory, fileName); 

        //  l'image doit etre ssauv dans le dossier local
        using (var stream = await photo.OpenReadAsync())
        {
            // Ccreation d un fichier
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await stream.CopyToAsync(fileStream);
                Console.WriteLine($"Image sauvegardée à : {filePath}");
            }
        }
            string base64Image = await ConvertImageToBase64Async(filePath);

           
            var user = await _appService.GetUserByEmailAsync(email);
            if (user == null)
            {
                Console.WriteLine("Utilisateur introuvable.");
                return;
            }
            var newItem = new LostItem
        {
                Description = description,
                Category = category,
                Status = "Lost", 
                PhotoPath = base64Image, 
                Latitude = location.Latitude,
                Location = address,
                Email = email,
                UserId = user.Id,
                Longitude = location.Longitude,
                DateReported = DateTime.Now
            };

        
        await _appService.AddLostItemAsync(newItem);

        // load apres lajout
        await LoadItemsAsync();
        OnPropertyChanged(nameof(LostItems));
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur lors de la capture et de la sauvegarde : {ex.Message}");
    }
}


    public async Task UpdateLostItemAsync(LostItem item)
    {
        await _appService.UpdateLostItemAsync(item);
        await LoadItemsAsync();
    }

    public async Task DeleteLostItemAsync(string id)
    {
        await _appService.DeleteLostItemAsync(id);
        await LoadItemsAsync(); 

    }


    public async Task LoadItemsAsync()
    {
        var items = await _appService.GetLostItemsAsync();
        LostItems.Clear();
        foreach (var item in items)
        {
            LostItems.Add(item);
        }
    }

    private async Task<string> ConvertImageToBase64Async(string imagePath)
    {
        if (!File.Exists(imagePath))
            return string.Empty;

        byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);
        string extension = Path.GetExtension(imagePath).ToLowerInvariant().Replace(".", "");
        return $"data:image/{extension};base64,{Convert.ToBase64String(imageBytes)}";
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
