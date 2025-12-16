using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Bu gerekli

namespace WebHomework.Models
{
    public class GymService
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Hizmet Adı")]
        public string Name { get; set; }

        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        // DÜZELTME BURADA:
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Display(Name = "Süre (Dakika)")]
        public int Duration { get; set; }
    }
}
