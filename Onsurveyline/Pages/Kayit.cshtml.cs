using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Onsurveyline.Models;
using Onsurveyline.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Onsurveyline.Pages
{
    public class KayitModel : PageModel
    {

        public IWebHostEnvironment webHostEnvironment { get; }
        public KayitModel(IWebHostEnvironment WebHostEnvironment)
        {
            webHostEnvironment = WebHostEnvironment;
        }
        public string JsonFileName
        {
            get { return Path.Combine(webHostEnvironment.WebRootPath, "data", "kullanicilar.json"); }
        }

        public IEnumerable<KullaniciModel> KullanicilariGetir()
        {
            using var json = System.IO.File.OpenText(JsonFileName);
            return System.Text.Json.JsonSerializer.Deserialize<KullaniciModel[]>(json.ReadToEnd());
        }


        [BindProperty]
        public KullaniciModel Kullanici { get; set; }


        [BindProperty]
        public string sifreDogrulama { get; set; }

        //public KullaniciServis kservis;

        [BindProperty(SupportsGet = true)]
        public int Status { get; set; }

        //public KayitModel(KullaniciServis _kservis)
        //{
        //    kservis = _kservis;
        //}

        public void OnGet()
        {
            ViewData["login"] = HttpContext.Session.GetString("_Nick");
        }

        public void OnPostCikis()
        {
            HttpContext.Session.Clear();
        }
        public bool KullaniciEkle()
        {
            var kullanicilar = KullanicilariGetir();
            KullaniciModel query = kullanicilar.FirstOrDefault(x => x.kullaniciAdi == Kullanici.kullaniciAdi);
            if(query!=null)
            {
                return true;
            }
            else
            {
                
                Kullanici.id = kullanicilar.Max(x => x.id) + 1;
                Kullanici.createdTime = DateTime.Now.ToString();
                var temp = kullanicilar.ToList();
                temp.Add(Kullanici);
                IEnumerable<KullaniciModel> guncelKullanici = temp.ToArray();

                using var json = System.IO.File.OpenWrite(JsonFileName);
                System.Text.Json.JsonSerializer.Serialize<IEnumerable<KullaniciModel>>(
                    new Utf8JsonWriter(json, new JsonWriterOptions { Indented = true }), guncelKullanici);
                return false;
            }
        }

        public IActionResult OnPost()
        {
            if(Kullanici.sifre==sifreDogrulama)
            {
                if (KullaniciEkle())
                {
                    return RedirectToPage("/Kayit", new { Status = 3 });
                }
                else
                {

                    return RedirectToPage("/Giris", new { Status = 1 });
                }
            }
            else
            {
                return RedirectToPage("/Kayit", new { Status = 4 });
            }
            
        }
    }
}
