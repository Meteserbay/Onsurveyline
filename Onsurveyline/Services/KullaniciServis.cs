using Microsoft.AspNetCore.Hosting;
using Onsurveyline.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Onsurveyline.Services
{
    public class KullaniciServis
    {
        public IWebHostEnvironment webHostEnvironment { get; }
        public KullaniciServis(IWebHostEnvironment WebHostEnvironment)
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
    }
}
