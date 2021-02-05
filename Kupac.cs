using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Korpa2019
{
    class Kupac
    {
        public int KupacId { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }

        public override string ToString() => Ime + " " + Prezime; 
        
    }
}
