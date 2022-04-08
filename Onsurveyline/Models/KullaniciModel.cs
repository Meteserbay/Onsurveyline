using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onsurveyline.Models
{
    public class KullaniciModel
    {
        public int id { get; set; }
        public string kullaniciAdi { get; set; }
        public string sifre { get; set; }
        public AnketModel anket { get; set; }
        public string createdTime { get; set; }
        public int puan { get; set; }
    }
}
