using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class UserModel
{
    [Key]
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
    public string Role { get; set; } // Exemple : "admin", "agent", etc.

    [Required]
    [StringLength(255)]
    public string Password { get; set; } // Doit contenir un mot de passe hashé en production

    // Relation 1:N avec LostItem (un utilisateur peut signaler plusieurs objets)
    public ICollection<LostItem> LostItems { get; set; } = new List<LostItem>();
}
