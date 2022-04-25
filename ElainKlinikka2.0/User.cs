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
        public string Username { get; private set; } = "";

        /// <summary>
        /// Ja sähköpostiosoite.
        /// </summary>
        public string UserEmail { get; private set; } = "";

        public string UserPassword { get; private set; } = "";



        public User(int id, string name, string email, string password)
        {
            Id = id;
            Username = name;
            UserEmail = email;
            UserPassword = password;
        }

        public string MakeDescription()
        {
            return Id + ": " + Username + ", " + UserEmail;
        }
    }
}
