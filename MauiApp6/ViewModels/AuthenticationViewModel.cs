using MauiApp6.Models;
using System.Threading.Tasks;
using Windows.System;

public class AuthenticationViewModel
{
    private readonly IAppService _appService;

    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;


    public AuthenticationViewModel(IAppService appService)
    {
        _appService = appService;
    }

    public async Task<(bool Success, string Role, string Message)> LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            return (false, null, "Veuillez remplir tous les champs.");
        }

        return await _appService.Login(Email, Password);
        //await SecureStorage.SetAsync("Email", User.Email);

    }

}

//    public async Task<(bool Success, string Message)> RegisterAsync()
//    {
//        // Vérification des champs obligatoires
//        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password)
//            || string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName)
//            || string.IsNullOrWhiteSpace(Role) || string.IsNullOrWhiteSpace(ConfirmPassword))
//        {
//            return (false, "Tous les champs sont obligatoires.");
//        }

//        // Vérification de la correspondance des mots de passe
//        if (Password != ConfirmPassword)
//        {
//            return (false, "Le mot de passe et sa confirmation ne correspondent pas.");
//        }

//        // Création du modèle d'inscription
//        var model = new RegistrationModel
//        {
//            FirstName = FirstName,
//            LastName = LastName,
//            Email = Email,
//            Password = Password,
//            Role = Role
//        };

//        // Enregistrement via le service
//        return await _appService.Register(model);
//    }
//}