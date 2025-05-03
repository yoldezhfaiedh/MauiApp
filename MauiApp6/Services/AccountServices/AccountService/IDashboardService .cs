using MauiApp6.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IDashboardService
{
    // User methods
    Task<List<UserModel>> GetAllUsersSortedAsync();
    Task<List<UserModel>> GetUsersByRoleAsync(string role);
    Task<string> CountUsersPerRoleAsync();
    Task<List<UserModel>> SearchUsersByFirstLetterAsync(char letter);
    Task<List<UserModel>> GetUsersWithoutLostItemsAsync();
    Task<List<UserModel>> GetUsersWithMultipleLostItemsAsync();

    // LostItem methods
    Task<List<LostItem>> GetAllLostItemsAsync();
    Task AddLostItemAsync(LostItem item);
    Task<UserModel> GetUserByEmailAsync(string email);
    Task UpdateLostItemAsync(LostItem item);
    Task<bool> DeleteLostItemAsync(int itemId);
    Task<List<LostItem>> GetUserLostItemsAsync(string email);
    Task<Dictionary<string, List<LostItem>>> GetLostItemsByLocationAsync();

    //Task<List<LostItem>> GetLostItemsByLocationAsync(string location);
    Task<Dictionary<string, List<LostItem>>> GetLostItemsGroupedByLocationAsync();

    // Statistics methods
    Task<List<LostItem>> GetRecentLostItemsAsync(int days = 7);
    Task<Dictionary<string, int>> GetItemsByCategoryStatsAsync();
    Task<int> GetTotalLostItemsCountAsync();
    Task<int> GetTotalUsersCountAsync();
    Task<List<LostItem>> GetLostItemsAsync();
    Task<List<LostItem>> GetFoundItemsAsync();
    Task<List<LostItem>> GetItemsByEmailAsync(string email);



}

