using MauiApp6.Models;
using Microsoft.VisualStudio.Services.Licensing;

public interface IAppService
{
    // Students CRUD
    Task<List<UserModel>> GetAllUsers();
    Task<UserModel?> GetUserById(int id);
    Task<bool> AddUser(UserModel student);
    Task<bool> UpdateUser(UserModel student);
    Task<bool> DeleteUser(int id);
    Task<UserModel> GetUserByEmailAsync(string email);
    Task UpdateLostItemAsync(LostItem item);
    Task DeleteLostItemAsync(string itemId);
    Task<IEnumerable<LostItem>> GetLostItemsAsync(string email);
    //Task<Dictionary<string, object>> GetDashboardStats();

    Task LogoutAsync();


    // Auth
    //Task<bool> Login(string email, string password);
    //Task<bool> RegisterStudent(StudentModel student);

    Task<(bool Success, string Role, string Message)> Login(string email, string password);
    Task<(bool Success, string Message)> Register(RegistrationModel model);
    //Task<(bool Success, string Message)> Register(StudentModel newStudent);


    Task AddLostItemAsync(LostItem item);
    Task<List<LostItem>> GetLostItemsAsync();
    Task<IEnumerable<LostItem>> Get1LostItemsAsync();
    //Task<List<LostItem>> GetLost1ItemsAsync();



}
