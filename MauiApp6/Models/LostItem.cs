using System;
using System.ComponentModel.DataAnnotations;

public class LostItem
{
    // Identifiant unique généré
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // Description de l'objet perdu
    public string Description { get; set; }

    // Catégorie de l'objet perdu (ex: "Téléphone", "Clés", etc.)
    public string Category { get; set; }

    // Chemin vers la photo (si disponible)
    public string PhotoPath { get; set; }

    // Coordonnées géographiques de l'objet perdu
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    // Adresse obtenue via géocodage inversé
    public string Location { get; set; }

    // Date de signalement de l'objet perdu
    public DateTime DateReported { get; set; } = DateTime.Now;

    // Clé étrangère pour lier le LostItem à un User via son Email
    public string Email { get; set; } // Email comme clé étrangère

    // Propriété de navigation (permet de récupérer l'utilisateur associé)
    //public string UserId { get; set; } // Email comme clé étrangère

    //public UserModel User { get; set; }
}
