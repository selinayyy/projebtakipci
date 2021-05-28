using System;
using System.Collections.Generic;
using System.Text;

namespace projebtakipci.model
{
    class Duty
    {
        public int duty_id { get; set; }
        public bool duty_make { get; set; }
        public string duty_text { get; set; }
        public int kullanici_id { get; set; }
    }
}
