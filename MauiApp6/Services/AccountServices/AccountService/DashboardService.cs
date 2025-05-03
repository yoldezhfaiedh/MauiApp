using MauiApp6.Data;
using MauiApp6.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class DashboardService : IDashboardService
{
    private readonly RecruitingContext _context;
            private readonly ILogger<DashboardService> _logger;

    public DashboardService(RecruitingContext context, ILogger<DashboardService> logger)
    {
        _context = context;
        _logger = logger;

    }

    #region User Methods
    public async Task<List<UserModel>> GetAllUsersSortedAsync()
    {
        return await _context.Users
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .ToListAsync();
    }

    public async Task<List<UserModel>> GetUsersByRoleAsync(string role)
    {
        return await _context.Users
            .Where(u => u.Role.ToLower() == role.ToLower())
            .ToListAsync();
    }

    public async Task<string> CountUsersPerRoleAsync()
    {
        var results = await _context.Users
            .GroupBy(u => u.Role)
            .Select(g => new { Role = g.Key, Count = g.Count() })
            .ToListAsync();

        return string.Join("\n", results.Select(r => $"Role: {r.Role} - Count: {r.Count}"));
    }

    public async Task<List<UserModel>> SearchUsersByFirstLetterAsync(char letter)
    {
        return await _context.Users
            .Where(u => u.FirstName.StartsWith(letter.ToString(), StringComparison.OrdinalIgnoreCase))
            .ToListAsync();
    }

    public async Task<List<UserModel>> GetUsersWithoutLostItemsAsync()
    {
        return await _context.Users
            .Where(u => !u.LostItems.Any())
            .ToListAsync();
    }

    public async Task<List<UserModel>> GetUsersWithMultipleLostItemsAsync()
    {
        return await _context.Users
            .Where(u => u.LostItems.Count >= 2)
            .ToListAsync();
    }
    #endregion

    #region LostItem Methods
    public async Task<List<LostItem>> GetAllLostItemsAsync()
    {
        return await _context.LostItems.ToListAsync();
    }

    public async Task AddLostItemAsync(LostItem item)
    {
        _context.LostItems.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task<UserModel> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task UpdateLostItemAsync(LostItem item)
    {
        _context.LostItems.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteLostItemAsync(int itemId)
    {
        var item = await _context.LostItems.FindAsync(itemId);
        if (item == null) return false;

        _context.LostItems.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<LostItem>> GetUserLostItemsAsync(string email)
    {
        return await _context.LostItems
            .Where(item => item.Email == email)
            .OrderByDescending(item => item.DateReported)
            .ToListAsync();
    }

    //public async Task<List<LostItem>> GetLostItemsByLocationAsync(string location)
    //{
    //    return await _context.LostItems
    //        .Where(item => item.Location.Contains(location))
    //        .ToListAsync();
    //}
    public async Task<Dictionary<string, List<LostItem>>> GetLostItemsByLocationAsync()
    {
        try
        {
            var items = await _context.LostItems
                .Where(item => item.Location != null && item.Location != string.Empty)
                .OrderByDescending(item => item.DateReported)
                .AsNoTracking()
                .ToListAsync();

            var result = items
                .GroupBy(item => item.Location)
                .OrderByDescending(g => g.Count())
                .ToDictionary(
                    g => g.Key,
                    g => g.ToList()
                );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la récupération des objets perdus par lieu");
            return new Dictionary<string, List<LostItem>>();
        }
    }

    public async Task<List<LostItem>> GetFoundItemsAsync()
    {
        return await _context.LostItems
            .Where(item => item.Status.ToLower() == "found")
            .ToListAsync();
    }
    public async Task<List<LostItem>> GetLostItemsAsync()
    {
        return await _context.LostItems
            .Where(item => item.Status.ToLower() == "lost")
            .ToListAsync();
    }
    public async Task<Dictionary<string, List<LostItem>>> GetLostItemsGroupedByLocationAsync()
    {
        return await _context.LostItems
            .GroupBy(item => item.Location)
            .ToDictionaryAsync(g => g.Key, g => g.ToList());
    }
    #endregion

    #region Statistics Methods
    public async Task<List<LostItem>> GetRecentLostItemsAsync(int days = 7)
    {
        var cutoffDate = DateTime.Now.AddDays(-days);
        return await _context.LostItems
            .Where(item => item.DateReported >= cutoffDate)
            .OrderByDescending(item => item.DateReported)
            .ToListAsync();
    }

    public async Task<Dictionary<string, int>> GetItemsByCategoryStatsAsync()
    {
        return await _context.LostItems
            .GroupBy(item => item.Category)
            .ToDictionaryAsync(g => g.Key, g => g.Count());
    }

    public async Task<int> GetTotalLostItemsCountAsync()
    {
        return await _context.LostItems.CountAsync();
    }

    public async Task<int> GetTotalUsersCountAsync()
    {
        return await _context.Users.CountAsync();
    }

    public async Task<List<LostItem>> GetItemsByEmailAsync(string email)
    {
        return await _context.LostItems
            .Where(item => item.Email.ToLower() == email.ToLower().Trim())
            .OrderByDescending(item => item.DateReported)
            .ToListAsync();
    }
    #endregion
}