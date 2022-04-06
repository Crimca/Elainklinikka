using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElainKlinikka2._0
{
    class Eläin
    {
        public int _Id = 0;
        public string _Nimi = "";
        public string _Kuvaus = "";
        public int _OletusLainaAika = 30;
        public bool _Kadonnut = false;


        //Property, joka palauttaa id-numeron muuttujasta _ID
        public int IDNumero
        {
            get { return _Id; }
        }

        public string EläimenNimi
        {
            get { return _Nimi; }
        }

        public string EläimenKuvaus
        {
            get { return _Kuvaus; }
            set { _Kuvaus = value; }
        }

        public int LainaAika
        {
            get { return _OletusLainaAika; }
        }

        
        public Eläin(int id, string nimi, string kuvaus)
        {
            _Id = id;
            _Nimi = nimi;
            _Kuvaus = kuvaus;
        }
    }
}
