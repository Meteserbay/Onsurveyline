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
using Onsurveyline.Services;

namespace Onsurveyline.Pages
{
    public class GirisModel : PageModel
    {
        public IWebHostEnvironment webHostEnvironment { get; }
        public GirisModel(IWebHostEnvironment WebHostEnvironment) 
        { 
            webHostEnvironment = WebHostEnvironment;
            kullanicilar = KullanicilariGetir();
        }
        public string JsonFileName
        {
            get { return Path.Combine(webHostEnvironment.WebRootPath, "data", "kullanicilar.json"); }
        }

        public IEnumerable<KullaniciModel> kullanicilar;

        public const string keyNick = "_Nick";
        public const string keySifre = "_Sifre";


        public IEnumerable<KullaniciModel> KullanicilariGetir()
        {
            using var json = System.IO.File.OpenText(JsonFileName);
            return System.Text.Json.JsonSerializer.Deserialize<KullaniciModel[]>(json.ReadToEnd());
        }
        [BindProperty]
        public string kullaniciAd { get; set; }

        [BindProperty]
        public string sifre { get; set; }

        //public KullaniciServis kservis;

        //public GirisModel(KullaniciServis _kservis)
        //{
        //    kservis = _kservis;
        //}

        [BindProperty(SupportsGet =true)]
        public int Status { get; set; }
        public void OnGet()
        {
            ViewData["login"] = HttpContext.Session.GetString("_Nick");

        }

        public IActionResult OnPost()
        {
            var kullanici = kullanicilar.FirstOrDefault(x => x.kullaniciAdi == kullaniciAd && x.sifre==sifre);
            
            if (kullanici!=null)
            {
                HttpContext.Session.SetString(keyNick,kullanici.kullaniciAdi);
                HttpContext.Session.SetString(keySifre, kullanici.sifre);
                return RedirectToPage("/Index", new { Status = 1 });    
            }
            else
            {
                return RedirectToPage("/Giris",new { Status=2});
            }
        }
        public void OnPostCikis()
        {
            HttpContext.Session.Clear();
        }

    }
}
