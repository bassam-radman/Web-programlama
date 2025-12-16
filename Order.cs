using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebHomework.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }

        // --- BU KISIM VAR MI? YOKSA EKLE ---
        public int GymServiceId { get; set; } // <--- BU SATIR ŞART
        public GymService? GymService { get; set; }
        // -----------------------------------

        public int? TrainerId { get; set; }
        public Trainer? Trainer { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal Price { get; set; }
    }
}
