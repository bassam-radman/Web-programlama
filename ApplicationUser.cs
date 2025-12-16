using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebHomework.Models
{
    // IdentityUser'dan miras alıyoruz
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Display(Name = "Boy (cm)")]
        public int? Height { get; set; } // AI hesaplaması için

        [Display(Name = "Kilo (kg)")]
        public double? Weight { get; set; } // AI hesaplaması için

        // Üyenin randevuları
    }
}
