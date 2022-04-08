using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onsurveyline.Models
{
    public class SoruModel
    {
        public int anketid { get; set; }
        public int soruid { get; set; }
        public IEnumerable<CevapModel> cevaplar { get; set; }

    }
}
