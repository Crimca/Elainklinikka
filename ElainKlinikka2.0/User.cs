using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElainKlinikka2._0
{
    public class User
    {
        /// <summary>
        /// Tietokannassa käyttäjällä on uniikki ID-numero.
        /// </summary>
        public int Id { get; private set; } = 0;

        /// <summary>
        /// Tietokannassa käyttäjällä on myös nimi.
        /// </summary>
        public string UserName { get; private set; } = "";

        /// <summary>
        /// Ja sähköpostiosoite.
        /// </summary>
        public string UserEmail { get; private set; } = "";

        public string UserPassword { get; private set; } = "";

        public string UserRooli { get; set; } = "";



        public User(int id, string name, string email, string password, string rooli)
        {
            Id = id;
            UserName = name;
            UserEmail = email;
            UserPassword = password;
            UserRooli = rooli;
        }

        public string MakeDescription()
        {
            return Id + ": " + UserName + ", " + UserEmail;
        }
    }
}
