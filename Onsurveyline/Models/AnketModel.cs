using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onsurveyline.Models
{
    public class AnketModel
    {
        public int KullaniciId { get; set; }
        public int anketid { get; set; }
        public string baslik { get; set; }
        public string createdTime { get; set; }
        public bool onoff { get; set; }
        public int kod { get; set; }
        public CevapModel cevaplar { get; set; }
    }
}
