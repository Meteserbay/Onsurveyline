using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Onsurveyline.Models;

namespace Onsurveyline.Pages
{
    public class LinkleKatilModel : PageModel 
    {
        
        public AnketModel istenilenAnket { get; set; }

        [BindProperty]
        public AnketModel yeni›stenilenAnket { get; set; }

      


        public IWebHostEnvironment webHostEnvironment { get; }
        public LinkleKatilModel(IWebHostEnvironment WebHostEnvironment)
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

        public string JsonFileNameAnket›sim
        {
            get { return Path.Combine(webHostEnvironment.WebRootPath, "data", "anket.json"); }
        }

        public IEnumerable<AnketModel> AnketleriGetir()
        {
            using var json = System.IO.File.OpenText(JsonFileNameAnket);
            return System.Text.Json.JsonSerializer.Deserialize<AnketModel[]>(json.ReadToEnd());
        }

        public IEnumerable<KullaniciModel> KullanicilariGetir()
        {
            using var json = System.IO.File.OpenText(JsonFileName);
            return System.Text.Json.JsonSerializer.Deserialize<KullaniciModel[]>(json.ReadToEnd());
        }


        public void OnGet(int kod)
        {
            var anketler = AnketleriGetir();
            istenilenAnket = anketler.FirstOrDefault(x => x.kod == kod);

            using var json = System.IO.File.OpenWrite(JsonFileNameAnket›sim);
            System.Text.Json.JsonSerializer.Serialize<AnketModel>(
                new Utf8JsonWriter(json, new JsonWriterOptions { Indented = true }), istenilenAnket);
        }


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

        public AnketModel AnketGetir()
        {
            using var json = System.IO.File.OpenText(JsonFileNameAnket›sim);
            return System.Text.Json.JsonSerializer.Deserialize<AnketModel>(json.ReadToEnd());
            
        }

        public void OnPostOyla()
        {
            AnketModel istenilenAnket = new AnketModel();
            istenilenAnket = AnketGetir();

            //List<AnketModel> sifirla = new List<AnketModel>();
            //using var jjson = System.IO.File.OpenWrite(JsonFileNameAnket›sim);
            //System.Text.Json.JsonSerializer.Serialize<IEnumerable<AnketModel>>(
            //    new Utf8JsonWriter(jjson, new JsonWriterOptions { Indented = true }), sifirla);


            var anketler = AnketleriGetir();
            yeni›stenilenAnket = anketler.FirstOrDefault(x => x.anketid == istenilenAnket.anketid);
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

            yeni›stenilenAnket = istenilenAnket;
            temp.Add(istenilenAnket);

            secim1yuzde = (Convert.ToDouble(Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac1) / (Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac1) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac2) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac3) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac4))) * 100).ToString("0.##");
            secim2yuzde = (Convert.ToDouble(Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac2) / (Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac1) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac2) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac3) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac4))) * 100).ToString("0.##");
            secim3yuzde = (Convert.ToDouble(Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac3) / (Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac1) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac2) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac3) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac4))) * 100).ToString("0.##");
            secim4yuzde = (Convert.ToDouble(Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac4) / (Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac1) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac2) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac3) + Convert.ToDouble(yeni›stenilenAnket.cevaplar.sayac4))) * 100).ToString("0.##");




            using var json = System.IO.File.OpenWrite(JsonFileNameAnket);
            System.Text.Json.JsonSerializer.Serialize<IEnumerable<AnketModel>>(
                new Utf8JsonWriter(json, new JsonWriterOptions { Indented = true }), temp);
        }
    }
}
