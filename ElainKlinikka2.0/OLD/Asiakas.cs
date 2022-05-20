using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElainKlinikka2._0
{
    class Asiakas
    {
        public int _Id = 0;
        public string _ENimi = "";
        public string _SNimi = "";


        //Property, joka palauttaa id-numeron muuttujasta _ID
        public int AsiakasID
        {
            get { return _Id; }
        }

        public string Asiakas_ENimi
        {
            get { return _ENimi; }
        }

        public string Asiakas_SNimi
        {
            get { return _SNimi; }
           
        }

        public Asiakas(int id, string ENimi, string SNimi)
        {
            _Id = id;
            _ENimi = ENimi;
            _SNimi = SNimi;
        }
    }
}
