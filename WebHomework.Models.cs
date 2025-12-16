using WebHomework.Models;

namespace WebHomework.Models
{
    public class HomeViewModel
    {
        // Hizmetlerin listesi
        public IEnumerable<GymService> Services { get; set; }

        // Eğitmenlerin listesi
    }
}
