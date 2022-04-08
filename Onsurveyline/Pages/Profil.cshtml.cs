using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Onsurveyline.Models;

namespace Onsurveyline.Pages
{
    public class ProfilModel : PageModel
    {
        public IEnumerable<KullaniciModel> kullanicilar;
        public IEnumerable<AnketModel> anketler;
        public List<AnketModel> filtreanketler= new List<AnketModel>();

        public string kullaniciAd { get; set; }
        public KullaniciModel kullanici = new KullaniciModel();
        
        public IWebHostEnvironment webHostEnvironment { get; }
        public ProfilModel(IWebHostEnvironment WebHostEnvironment)
        {
            webHostEnvironment = WebHostEnvironment;
        }
        public string JsonFileName
        {
            get { return Path.Combine(webHostEnvironment.WebRootPath, "data", "kullanicilar.json"); }
        }
        public string JsonFileName2
        {
            get { return Path.Combine(webHostEnvironment.WebRootPath, "data", "anketler.json"); }
        }
        [BindProperty]
        public ProfilModel Kullanici { get; set; }
        
        public IEnumerable<KullaniciModel> KullanicilariGetir()
        {
            
            using var json = System.IO.File.OpenText(JsonFileName);
            return System.Text.Json.JsonSerializer.Deserialize<KullaniciModel[]>(json.ReadToEnd());
        }

        public IEnumerable<AnketModel> AnketleriGetir()
        {
            using var json = System.IO.File.OpenText(JsonFileName2);
            return System.Text.Json.JsonSerializer.Deserialize<AnketModel[]>(json.ReadToEnd());
        }

        public void OnGet()
        {
            kullanicilar = KullanicilariGetir();
            anketler = AnketleriGetir();
            ViewData["login"] = HttpContext.Session.GetString("_Nick");
            kullaniciAd = ViewData["login"].ToString();
            kullanici = kullanicilar.FirstOrDefault(x => x.kullaniciAdi == kullaniciAd);
            foreach(var anket in anketler)
            {
                if(kullanici.id==anket.KullaniciId)
                {
                    filtreanketler.Add(anket);
                    kullanici.puan += 1;
                }
            }
        }
        public void OnPostCikis()
        {
            HttpContext.Session.Clear();
        }
        public void OnPost()
        {

        }

    }
}
