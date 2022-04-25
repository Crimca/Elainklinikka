using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElainKlinikka2._0
{
    class Klinikka
    {

        public string _Nimi = "";
        public string _Sijainti = "";

        public Klinikka(string nimi, string sijainti)
        {
            _Nimi = nimi;
            _Sijainti = sijainti;
        }

        public List<Eläin> Eläimet = new List<Eläin>();
        public void LuoMockDataa(int lkm)
        {
            Eläimet.Clear();

            for (int i = 0; i < lkm; i++)
            {
                Eläin t = new Eläin(i, "Nimi" + i, "Kissa" + i);

                Eläimet.Add(t);
            }
        }
    }
}
