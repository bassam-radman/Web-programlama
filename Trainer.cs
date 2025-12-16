using System.ComponentModel.DataAnnotations;

namespace WebHomework.Models
{
    public class Trainer
    {
        public int Id { get; set; }

        [Display(Name = "Ad Soyad")]
        [Required(ErrorMessage = "Ad Soyad zorunludur")]
        public string FullName { get; set; }

        [Display(Name = "Uzmanlık Alanı")]
        public string Specialization { get; set; } // Örn: Pilates, Fitness

        [Display(Name = "Çalışma Saatleri")]
        public string WorkingHours { get; set; } // Örn: 09:00 - 18:00

        // Bir eğitmenin birden fazla siparişi/randevusu olabilir
        public ICollection<Order>? Orders { get; set; }
    }
}
