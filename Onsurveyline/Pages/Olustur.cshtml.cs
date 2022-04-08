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
    public class OlusturModel : PageModel
    {
        public void OnGet()
        {
            ViewData["login"] = HttpContext.Session.GetString("_Nick");

        }

        [BindProperty]
        public KullaniciModel kullanici { get; set; }


        [BindProperty]
        public AnketModel anket { get; set; }

        public IWebHostEnvironment webHostEnvironment { get; }
        public OlusturModel(IWebHostEnvironment WebHostEnvironment)
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
        public IEnumerable<KullaniciModel> KullanicilariGetir()
        {
            using var json = System.IO.File.OpenText(JsonFileName);
            return System.Text.Json.JsonSerializer.Deserialize<KullaniciModel[]>(json.ReadToEnd());
        }
        public IEnumerable<AnketModel> AnketleriGetir()
        {
            using var json = System.IO.File.OpenText(JsonFileNameAnket);
            return System.Text.Json.JsonSerializer.Deserialize<AnketModel[]>(json.ReadToEnd());
        }
        
        public void OnPostCikis()
        {
            HttpContext.Session.Clear();
        }

        public void kullanicisizAnket()
        {

        }

        public void kullaniciliAnket()
        {
            var oturumKullanici = ViewData["login"].ToString();
            var temp2 = KullanicilariGetir().ToList();
            kullanici = temp2.FirstOrDefault(x => x.kullaniciAdi == oturumKullanici);
            var anketler = AnketleriGetir();


            anket.kod = anketler.Max(x => x.kod) + 3;

            anket.anketid = anketler.Max(x => x.anketid) + 1;
            var temp = anketler.ToList();
            temp.Add(anket);
            anket.KullaniciId = kullanici.id;
            anket.createdTime = DateTime.Now.ToString();
            anket.onoff = true;
            anket.cevaplar.anketid = anket.anketid;
            IEnumerable<AnketModel> guncelAnket = temp.ToArray();
            using var json = System.IO.File.OpenWrite(JsonFileNameAnket);
            System.Text.Json.JsonSerializer.Serialize<IEnumerable<AnketModel>>(
                new Utf8JsonWriter(json, new JsonWriterOptions { Indented = true }), guncelAnket);


            //var temp = KullanicilariGetir().ToList();
            //kullanici = temp.FirstOrDefault(x => x.kullaniciAdi == oturumKullanici);
            //kullanici.anket = _anket;
            //km.Add(kullanici);

            //using var json = System.IO.File.OpenWrite(JsonFileName);
            //System.Text.Json.JsonSerializer.Serialize<IEnumerable<KullaniciModel>>(
            //    new Utf8JsonWriter(json, new JsonWriterOptions { Indented = true }), km);

        }

        public IActionResult OnPostOlustur()
        {
            ViewData["login"] = HttpContext.Session.GetString("_Nick");
            if (ViewData["login"] !=null)
            {
                kullaniciliAnket();
            }
            else
            {
                kullanicisizAnket();
            }

            return RedirectToPage("/Index", new { Status = 2 });
        }
    }
}
