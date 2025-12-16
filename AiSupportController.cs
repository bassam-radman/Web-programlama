using Microsoft.AspNetCore.Mvc;

namespace WebHomework.Controllers
{
    public class AiSupportController : Controller
    {
        // Sayfayı Açma (GET)
        public IActionResult Index()
        {
            return View();
        }

        // Form Gönderilince Çalışacak Kısım (POST)
        [HttpPost]
        public IActionResult GetAdvice(int height, int weight, string goal)
        {
            // 1. Vücut Kitle Endeksi (BMI) Hesaplama
            // Formül: Kilo / (Boy * Boy) [Metre cinsinden]
            double heightInMeters = height / 100.0;
            double bmi = weight / (heightInMeters * heightInMeters);

            // 2. Yapay Zeka Tavsiyesi Oluşturma (Simülasyon)
            string advice = "";
            string aiTitle = "";

            if (bmi < 18.5)
            {
                aiTitle = "Analiz Sonucu: Zayıf";
                advice = "Vücut analizine göre kilonuz idealin altında. **AI Önerisi:** Karbonhidrat ve protein ağırlıklı beslenmelisiniz. Günde en az 3000 kalori hedeflemelisiniz. Önerilen Egzersiz: Ağırlık kaldırma ve düşük tempolu kardiyo.";
            }
            else if (bmi >= 18.5 && bmi < 25)
            {
                aiTitle = "Analiz Sonucu: İdeal Kilo";
                advice = "Mükemmel! Vücut dengeniz harika görünüyor. **AI Önerisi:** Mevcut formunuzu korumak için dengeli beslenmeye devam edin. Haftada 3 gün Fitness veya Pilates yaparak kas oranınızı artırabilirsiniz.";
            }
            else if (bmi >= 25 && bmi < 30)
            {
                aiTitle = "Analiz Sonucu: Hafif Kilolu";
                advice = "Biraz fazlalığımız var gibi görünüyor. **AI Önerisi:** Şeker ve hamur işini azaltmalısınız. Akşam 19:00'dan sonra yemek yemeyi kesin. Önerilen Egzersiz: Yüksek tempolu yürüyüş (HIIT) ve yüzme.";
            }
            else
            {
                aiTitle = "Analiz Sonucu: Obezite Riski";
                advice = "Sağlığınız için harekete geçme zamanı! **AI Önerisi:** Acilen bir diyetisyen eşliğinde sıkı bir diyete başlamalısınız. Eklemlerinizi yormamak için Yüzme veya Bisiklet egzersizleri ile başlamanızı öneriyorum.";
            }

            // Hedefe Göre Ek Tavsiye
            if (goal == "muscle")
            {
                advice += " Ayrıca kas kütlesini artırmak için protein tozu veya doğal protein kaynaklarına (tavuk, yumurta) ağırlık verin.";
            }

            // Sonucu Ekrana Göndermek İçin ViewBag Kullanıyoruz
            ViewBag.ResultTitle = aiTitle;
            ViewBag.ResultAdvice = advice;
            ViewBag.Bmi = Math.Round(bmi, 2); // Virgülden sonra 2 hane

            return View("Index");
        }
    }
}
