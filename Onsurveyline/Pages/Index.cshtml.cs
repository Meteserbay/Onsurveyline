using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Onsurveyline.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Onsurveyline.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IEnumerable<AnketModel> anketler;

        public IWebHostEnvironment webHostEnvironment { get; }
        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment _webHostEnvironment)
        {
            webHostEnvironment = _webHostEnvironment;
            _logger = logger;
        }

        public string JsonFileName2
        {
            get { return Path.Combine(webHostEnvironment.WebRootPath, "data", "anketler.json"); }
        }
        public IEnumerable<AnketModel> AnketleriGetir()
        {
            using var json = System.IO.File.OpenText(JsonFileName2);
            return System.Text.Json.JsonSerializer.Deserialize<AnketModel[]>(json.ReadToEnd());
        }

        [BindProperty(SupportsGet = true)]
        public int Status { get; set; }

        
        public void OnGet()
        {
            ViewData["login"] = HttpContext.Session.GetString("_Nick");
            anketler = AnketleriGetir();
            
        }

        public void OnPostCikis()
        {
            HttpContext.Session.Clear();
            
        }
    }
}
