using MauiApp6.Data;
using MauiApp6.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI;

public class AppService : IAppService
{
    private readonly RecruitingContext _context;

    public AppService(RecruitingContext context)
    {
        _context = context;
    }

    public async Task<List<UserModel>> GetAllUsers()
    {
        try
        {
            return await _context.Users
                .AsNoTracking()
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la recup  {ex.Message}");
            return new List<UserModel>();
        }
    }

    public async Task<UserModel?> GetUserById(int id)
    {
        try
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la recup {id}: {ex.Message}");
            return null;
        }
    }

    // Ajouter un nouvel user
    public async Task<bool> AddUser(UserModel user)
    {
        try
        {
            if (await _context.Users.AnyAsync(s => s.Email == user.Email))
                return false;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Erreur DB lors de l'ajout: {ex.InnerException?.Message ?? ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'ajout: {ex.Message}");
            return false;
        }
    }

    // Mettre à jour un  user existant
    public async Task<bool> UpdateUser(UserModel user)
    {
        try
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
                return false;

            // verifier l email
            if (await _context.Users.AnyAsync(s => s.Email == user.Email && s.Id != user.Id))
                return false;
            // entry et asnotracking sont opposees
            _context.Entry(existingUser).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Erreur DB lors de la mise    jour: {ex.InnerException?.Message ?? ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la mise a jour: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteUser(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Erreur db lors de la suppression: {ex.InnerException?.Message ?? ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la suppression: {ex.Message}");
            return false;
        }
    }
    private async Task<string> GetUserEmailAsync()
    {
        var email = await SecureStorage.GetAsync("email");
        return email;
    }
    public async Task<(bool Success, string Role, string Message)> Login(string email, string password)
    {
        try
        {
            var student = await _context.Users
                .FirstOrDefaultAsync(s => s.Email == email);

            if (student == null)
                return (false, null, "Email non trouvé");

            if (student.Password != password)
                return (false, null, "Mot de passe incorrect");

            return (true, student.Role, "Connexion réussie");

        }
        catch (Exception ex)
        {
            return (false, null, $"Erreur technique: {ex.Message}");
        }
    }
    // 
    public async Task<bool> IsUserAuthenticatedAsync()
    {
        var email = await SecureStorage.GetAsync("user_email");
        return !string.IsNullOrEmpty(email);
    }


    public async Task LogoutAsync()
    {
        SecureStorage.Remove("user_email");
        SecureStorage.Remove("user_role");


    }




    public async Task<(bool Success, string Message)> Register(RegistrationModel model)
    { // creation du nouvel user pour linscription
        var newUser = new UserModel
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Password = model.Password,
            Role = model.Role 
        };
        try
        {
            //any assync prend le premier match
            if (await _context.Users.AnyAsync(s => s.Email == newUser.Email))
                return (false, "Cet email est déjà utilisé");

            // Validation du roole
            var validRoles = new[] { "Admin", "User" }; 
            if (!validRoles.Contains(newUser.Role))
                return (false, "Rôle invalide");

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return (true, "Inscription réussie");
        }
        catch (DbUpdateException ex)
        {
            return (false, $"Erreur base de données: {ex.InnerException?.Message}");
        }
    }



    public async Task<UserModel> GetUserByEmailAsync(string email)
    {
        using (var context = new RecruitingContext())
        {
            var user = await (from u in context.Users
                              where u.Email == email
                              select u).FirstOrDefaultAsync();

            return user;
        }
    }
    public async Task<List<LostItem>> GetLostItemsAsync()
    {
        return await _context.LostItems.ToListAsync();
    }

    // le reste dans viewmodel 
    public async Task AddLostItemAsync(LostItem item)
    {
        _context.LostItems.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateLostItemAsync(LostItem item)
    {
        var existingItem = await _context.LostItems.FindAsync(item.Id);
        if (existingItem != null)
        {
            existingItem.Description = item.Description;
            existingItem.Category = item.Category;
            existingItem.Status = item.Status; 
            existingItem.PhotoPath = item.PhotoPath;
            existingItem.Latitude = item.Latitude;
            existingItem.Longitude = item.Longitude;
            existingItem.Location = item.Location;
            existingItem.Email = item.Email;

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteLostItemAsync(string itemId)
    {
        var item = await _context.LostItems.FindAsync(itemId);
        if (item != null)
        {
            _context.LostItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<IEnumerable<LostItem>> GetLostItemsAsync(string email)
    {
        var userLostItems = await _context.LostItems
            .Where(item => item.Email == email)
            .OrderByDescending(item => item.DateReported)
            .ToListAsync();

        return userLostItems;
    }
  
    public async Task<IEnumerable<LostItem>> Get1LostItemsAsync()
    {
        return await _context.LostItems
            .OrderByDescending(item => item.DateReported)
            .ToListAsync();
    }




}

