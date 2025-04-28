using System.ComponentModel.DataAnnotations;

public class UserModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    [Required]
    [StringLength(100)]
    public string Role { get; set; } // Retiré l'attribut [EmailAddress] ici

    [Required]
    [StringLength(255)]
    public string Password { get; set; } // Stockera le mot de passe hashé en production
}
