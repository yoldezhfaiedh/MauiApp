using System.ComponentModel.DataAnnotations;

public class RegistrationModel
{
    [Required(ErrorMessage = "Le prénom est obligatoire")]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Le nom est obligatoire")]
    [StringLength(50)]
    public string LastName { get; set; }

    [Required(ErrorMessage = "L'email est obligatoire")]
    [EmailAddress(ErrorMessage = "Email invalide")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Le Role est obligatoire")]
    [StringLength(50)]
    public string Role { get; set; }

    [Required(ErrorMessage = "Le mot de passe est obligatoire")]
    [MinLength(6, ErrorMessage = "6 caractères minimum")]
    public string Password { get; set; }

    [Compare(nameof(Password), ErrorMessage = "Les mots de passe ne correspondent pas")]
    public string ConfirmPassword { get; set; }
}