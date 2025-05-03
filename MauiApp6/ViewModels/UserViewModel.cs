using MauiApp6.Models;
using MauiApp6.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

public class UserViewModel : INotifyPropertyChanged
{
    private readonly IAppService _appService;

    public ObservableCollection<UserModel> Users { get; set; } = new();

    private UserModel? _selectedUser;
    public UserModel? SelectedUser
    {
        get => _selectedUser;
        set
        {
            _selectedUser = value;
            OnPropertyChanged();
        }
    }

    private string _statusMessage;
    public string StatusMessage
    {
        get => _statusMessage;
        set
        {
            _statusMessage = value;
            OnPropertyChanged();
        }
    }

    public UserViewModel(IAppService appService)
    {
        _appService = appService;
        _ = LoadUsers();
    }

    public async Task LoadUsers()
    {
        var list = await _appService.GetAllUsers();
        Users.Clear();
        foreach (var user in list)
            Users.Add(user);

        // Déclenchement des stats à chaque chargement
        OnPropertyChanged(nameof(TotalUsers));
        OnPropertyChanged(nameof(AdminCount));
        OnPropertyChanged(nameof(AgentCount));
        OnPropertyChanged(nameof(OtherCount));
        OnPropertyChanged(nameof(MostCommonRole));
        OnPropertyChanged(nameof(UsersByRole));
    }

    public async Task AddUser(UserModel user)
    {
        var result = await _appService.AddUser(user);
        StatusMessage = result ? "User ajouté avec succès." : "Échec de l’ajout.";
        if (result) await LoadUsers();
    }

    public async Task UpdateUser(UserModel user)
    {
        var result = await _appService.UpdateUser(user);
        StatusMessage = result ? "Mis à jour avec succès." : "Échec de la mise à jour.";
        if (result) await LoadUsers();
    }

    public async Task DeleteUser(int id)
    {
        var result = await _appService.DeleteUser(id);
        StatusMessage = result ? "Supprimé avec succès." : "Échec de la suppression.";
        if (result) await LoadUsers();
    }

    // === Statistiques LINQ ===

    public int TotalUsers => Users.Count;

    public int AdminCount => Users.Count(u => u.Role.ToLower() == "admin");

    public int AgentCount => Users.Count(u => u.Role.ToLower() == "agent");

    public int OtherCount => Users.Count(u => u.Role.ToLower() != "admin" && u.Role.ToLower() != "agent");

    public string MostCommonRole =>
        Users.GroupBy(u => u.Role)
             .OrderByDescending(g => g.Count())
             .Select(g => g.Key)
             .FirstOrDefault() ?? "N/A";

    public Dictionary<string, int> UsersByRole =>
        Users.GroupBy(u => u.Role)
             .ToDictionary(g => g.Key, g => g.Count());

    // === Génération état CSV ===

    public string GenerateCsvReport()
    {
        var lines = new List<string> { "Id,FirstName,LastName,Email,Role" };
        foreach (var user in Users)
        {
            lines.Add($"{user.Id},{user.FirstName},{user.LastName},{user.Email},{user.Role}");
        }
        return string.Join(Environment.NewLine, lines);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
