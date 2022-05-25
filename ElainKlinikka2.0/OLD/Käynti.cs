using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElainKlinikka2._0
{
    class Käynti
    {
       
        public int KäyntiID
        { get; private set; } = 0;

        public string ToimenpideNimi
        { get; private set; } = "";

        
        public Käynti(int id, string nimi )
        {
            KäyntiID = id;
            ToimenpideNimi = nimi;
        }
    }
}
