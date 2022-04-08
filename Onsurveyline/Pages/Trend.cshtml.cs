using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Onsurveyline.Pages
{
    public class TrendModel : PageModel
    {
        public void OnGet()
        {
            ViewData["login"] = HttpContext.Session.GetString("_Nick");
        }
        public void OnPostCikis()
        {
            HttpContext.Session.Clear();
        }
    }
}
