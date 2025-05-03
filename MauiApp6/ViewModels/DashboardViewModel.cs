//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Runtime.CompilerServices;
//using System.Threading.Tasks;

//public class DashboardViewModel : INotifyPropertyChanged
//{
//    private readonly IAppService _appService;

//    public ObservableCollection<LostItem> LostItemsByLocation { get; set; } = new();
//    public ObservableCollection<LostItem> LostItemsGroupedByLocation { get; set; } = new();


//    public ObservableCollection<UserModel> UsersSorted { get; set; } = new();
//    public ObservableCollection<UserModel> UsersByRole { get; set; } = new();
//    public ObservableCollection<UserModel> UsersNoLostItems { get; set; } = new();
//    public ObservableCollection<UserModel> UsersWithMultipleLostItems { get; set; } = new();
//    public ObservableCollection<UserModel> UsersByFirstLetter { get; set; } = new();
//    // Collections pour le binding
//    public ObservableCollection<LostItem> AvailableLostItems { get; } = new();
//    public ObservableCollection<LostItem> RecentLostItems { get; } = new();


//    private string _statusMessage;
//    public string StatusMessage
//    {
//        get => _statusMessage;
//        set
//        {
//            _statusMessage = value;
//            OnPropertyChanged();
//        }
//    }

//    public DashboardViewModel(IAppService appService)
//    {
//        _appService = appService;
//    }

//    public async Task LoadUsersSorted()
//    {
//        var list = await Task.Run(() => _appService.GetAllUsersSorted());
//        UsersSorted.Clear();
//        foreach (var user in list)
//            UsersSorted.Add(user);
//    }

//    public async Task LoadUsersByRole(string role)
//    {
//        var list = await Task.Run(() => _appService.GetUsersByRole(role));
//        UsersByRole.Clear();
//        foreach (var user in list)
//            UsersByRole.Add(user);
//    }

//    public async Task LoadUsersNoLostItems()
//    {
//        var list = await Task.Run(() => _appService.GetUsersWithoutLostItems());
//        UsersNoLostItems.Clear();
//        foreach (var user in list)
//            UsersNoLostItems.Add(user);
//    }

//    public async Task LoadUsersWithMultipleLostItems()
//    {
//        var list = await Task.Run(() => _appService.GetUsersWithMultipleLostItems());
//        UsersWithMultipleLostItems.Clear();
//        foreach (var user in list)
//            UsersWithMultipleLostItems.Add(user);
//    }

//    public async Task LoadUsersByFirstLetter(char letter)
//    {
//        var list = await Task.Run(() => _appService.SearchUsersByFirstLetter(letter));
//        UsersByFirstLetter.Clear();
//        foreach (var user in list)
//            UsersByFirstLetter.Add(user);
//    }

//    public event PropertyChangedEventHandler? PropertyChanged;
//    protected void OnPropertyChanged([CallerMemberName] string? name = null)
//    {
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
//    }


//    public async Task LoadLostItemsByLocation(string location)
//    {
//        var items = await _appService.GetLostItemsByLocation(location);
//        LostItemsByLocation.Clear();
//        foreach (var item in items)
//            LostItemsByLocation.Add(item);
//    }

//    public async Task LoadLostItemsGroupedByLocation()
//    {
//        var items = await _appService.GetLostItemsGroupedByLocation();
//        LostItemsGroupedByLocation.Clear();
//        foreach (var item in items)
//            LostItemsGroupedByLocation.Add(item);
//    }

//    / Méthodes pour charger les données
//    public async Task LoadAvailableLostItemsAsync()
//    {
//        var items = await Task.Run(() => _appService.GetAvailableLostItems());
//        AvailableLostItems.Clear();
//        foreach (var item in items)
//            AvailableLostItems.Add(item);
//    }

//    public async Task LoadUsersWithMultipleLostItemsAsync()
//    {
//        var users = await Task.Run(() => _appService.GetUsersWithMultipleLostItems());
//        UsersWithMultipleLostItems.Clear();
//        foreach (var user in users)
//            UsersWithMultipleLostItems.Add(user);
//    }

//    public async Task LoadRecentLostItemsAsync()
//    {
//        var items = await Task.Run(() => _appService.GetLostItemsOrderedByDate());
//        RecentLostItems.Clear();
//        foreach (var item in items.Take(10)) // On prend seulement les 10 plus récents
//            RecentLostItems.Add(item);
//    }

//    // Méthode pour calculer la durée depuis la perte
//    public string GetTimeSinceReported(LostItem item)
//    {
//        var duration = DateTime.Now - item.DateReported;
//        return $"{duration.Days} jours, {duration.Hours} heures";
//    }

//    public event PropertyChangedEventHandler PropertyChanged;
//    protected void OnPropertyChanged([CallerMemberName] string name = null)
//    {
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
//    }
//}

//}
