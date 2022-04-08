using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Onsurveyline.Models;

namespace Onsurveyline.Pages
{
    public class KatilModel : PageModel
    {

        [BindProperty]
        public int kod { get; set; }

        public IWebHostEnvironment webHostEnvironment { get; }
        public KatilModel(IWebHostEnvironment WebHostEnvironment)
        {
            webHostEnvironment = WebHostEnvironment;
        }
        public string JsonFileName
        {
            get { return Path.Combine(webHostEnvironment.WebRootPath, "data", "kullanicilar.json"); }
        }
        public string JsonFileNameAnket
        {
            get { return Path.Combine(webHostEnvironment.WebRootPath, "data", "anketler.json"); }
        }

        public string JsonFileNameKod
        {
            get { return Path.Combine(webHostEnvironment.WebRootPath, "data", "kod.json"); }
        }

        public IEnumerable<KullaniciModel> KullanicilariGetir()
        {
            using var json = System.IO.File.OpenText(JsonFileName);
            return System.Text.Json.JsonSerializer.Deserialize<KullaniciModel[]>(json.ReadToEnd());
        }

        public Kod KodGetir()
        {
            using var json = System.IO.File.OpenText(JsonFileNameKod);
            return System.Text.Json.JsonSerializer.Deserialize<Kod>(json.ReadToEnd());
        }

        public IEnumerable<AnketModel> AnketleriGetir()
        {
            using var json = System.IO.File.OpenText(JsonFileNameAnket);
            return System.Text.Json.JsonSerializer.Deserialize<AnketModel[]>(json.ReadToEnd());
        }

        public AnketModel istenilenAnket { get; set; }
        public void OnGet()
        {
            ViewData["login"] = HttpContext.Session.GetString("_Nick");
        }

        public void OnPostCikis()
        {
            HttpContext.Session.Clear();
        }


        [BindProperty]
        public bool oyKullandiMi { get; set; } = false;


        public void OnPostKatil()
        {
            ViewData["login"] = HttpContext.Session.GetString("_Nick");
            var anketler = AnketleriGetir();
            istenilenAnket = anketler.FirstOrDefault(x => x.kod == kod);
            
            Kod yenikod = new Kod();
            yenikod.kod = kod;          

            using var json = System.IO.File.OpenWrite(JsonFileNameKod);
            System.Text.Json.JsonSerializer.Serialize<Kod>(
                new Utf8JsonWriter(json, new JsonWriterOptions { Indented = true }), yenikod);
        }

        [BindProperty]
        public AnketModel yeni›stenilenAnket { get; set; }

        [BindProperty]
        public string secilenCevap { get; set; }

        [BindProperty]
        public string Secim1 { get; set; }
        [BindProperty]
        public string Secim2 { get; set; }
        [BindProperty]
        public string Secim3 { get; set; }
        [BindProperty]
        public string Secim4 { get; set; }



        [BindProperty]
        public string secim1yuzde { get; set; }
        [BindProperty]
        public string secim2yuzde { get; set; }
        [BindProperty]
        public string secim3yuzde { get; set; }
        [BindProperty]
        public string secim4yuzde { get; set; }

        public void OnPostOyla()
        {
            ViewData["login"] = HttpContext.Session.GetString("_Nick");

            Kod kod = new Kod();
            kod = KodGetir();
            var anketlerr = AnketleriGetir();
            var istenilenAnket = anketlerr.FirstOrDefault(x => x.kod == kod.kod);
            var anketler = AnketleriGetir();
            var query = anketler.FirstOrDefault(x => x.anketid == istenilenAnket.anketid);
            var temp = anketler.ToList();
            temp.Remove(query);

            if (Secim1 != null)
            {
                istenilenAnket.cevaplar.sayac1 += 1;
                secilenCevap = Secim1;
            }
            else if (Secim2 != null)
            {
                istenilenAnket.cevaplar.sayac2 += 1;
                secilenCevap = Secim2;
            }
            else if (Secim3 != null)
            {
                istenilenAnket.cevaplar.sayac3 += 1;
                secilenCevap = Secim3;
            }
            else if (Secim4 != null)
            {
                istenilenAnket.cevaplar.sayac4 += 1;
                secilenCevap = Secim4;
            }

            oyKullandiMi = true;
            yeni›stenilenAnket = istenilenAnket;
            temp.Add(istenilenAnket);

            secim1yuzde = (Convert.ToDouble(Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac1) / (Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac1) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac2) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac3) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac4)))*100).ToString("0.##");
            secim2yuzde = (Convert.ToDouble(Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac2) / (Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac1) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac2) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac3) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac4)))*100).ToString("0.##");
            secim3yuzde = (Convert.ToDouble(Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac3) / (Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac1) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac2) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac3) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac4)))*100).ToString("0.##");
            secim4yuzde = (Convert.ToDouble(Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac4) / (Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac1) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac2) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac3) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac4)))*100).ToString("0.##");




            using var json = System.IO.File.OpenWrite(JsonFileNameAnket);
            System.Text.Json.JsonSerializer.Serialize<IEnumerable<AnketModel>>(
                new Utf8JsonWriter(json, new JsonWriterOptions { Indented = true }), temp);


        }
    }
}
