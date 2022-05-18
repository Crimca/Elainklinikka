using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.OleDb;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElainKlinikka2._0
{
    class DatabaseHandler
    {
        #region SINGLETON_PATTERN
        /// <summary>
        /// The database path to load from.
        /// </summary>
        private static string DB_PATH = @"..\..\lib.accdb";

        /// <summary>
        /// The instance that has been created in the application.
        /// </summary>
        private static DatabaseHandler inst = null;

        /// <summary>
        /// This is the only way for other classes to use DatabaseHandler.
        /// If it is not created (first time this property is accessed),
        /// create a new instance internally.
        /// </summary>
        public static DatabaseHandler Instance // Singleton
        {
            get
            {
                // If the instance has not been created yet (is null)
                if (inst == null)
                {
                    // Create it once
                    inst = new DatabaseHandler();
                }

                // Return a reference to the instance
                return inst;
            }
        }

        /// <summary>
        /// Define explicit private constructor, so no one can create this
        /// instance. All access to be done via "Instance" property.
        /// </summary>
        private DatabaseHandler()
        {
            try
            {
                CreateConnection();
            }
            catch (Exception exc)
            {
                Trace.WriteLine(exc.Message + ", " + exc.StackTrace);
            }
        }

        #endregion

        #region CONNECTION_MANAGEMENT

        /// <summary>
        /// Here we store an opened connection to the database.
        /// </summary>
        private OleDbConnection openConnection = null;

        /// <summary>
        /// Used to create a connection.
        /// </summary>
        private void CreateConnection()
        {
            openConnection = new OleDbConnection(
                "Provider=Microsoft.ACE.OLEDB.12.0;" +
                @"Data Source=" + DB_PATH + ";"
                );

            openConnection.Open();
        }

        /// <summary>
        /// Used to close a connection.
        /// </summary>
        public void CloseConnection()
        {
            if (openConnection != null && openConnection.State == ConnectionState.Open)
            {
                openConnection.Close();
            }
        }

        #endregion

        #region DATA_ACCESS

        #region KÄYTTÄJÄT ETSIMINEN
        /// <summary>
        /// Try to find users with the matching email.
        /// </summary>
        /// <param name="email">Email to check for.</param>
        /// <returns>A list of users, 0...N results.</returns>
        public List<User> FindUsersWithEmail(string email)
        {
            if (openConnection == null)
            {
                inst.CreateConnection();
            }

            List<User> foundUsers = new List<User>();

            // Create command
            OleDbCommand myQuery = new OleDbCommand("SELECT * FROM Users WHERE UserEmail='" + email + "';", openConnection);

            // Execute command
            OleDbDataReader myReader = myQuery.ExecuteReader();

            // Check if we got rows
            if (myReader.HasRows)
            {
                // Loop through results
                while (myReader.Read() == true)
                {
                    // Read data from each row
                    int userId = myReader.GetInt32(0);
                    string userName = myReader.GetString(1);
                    string userEmail = myReader.GetString(2);
                    string passWord = myReader.GetString(3);
                    string userRooli = myReader.GetString(4);
                    // Create user
                    User u = new User(userId, userName, userEmail, passWord, userRooli);

                    // Add to the list to be returned
                    foundUsers.Add(u);
                }
            }

            // Return the list (may have 0...N users)
            return foundUsers;

        }
        #endregion

        #region ELÄIN ETSIMINEN
        public List<Eläin> FindEläinsWithName(string name)
        {
            if (openConnection == null)
            {
                inst.CreateConnection();
            }

            List<Eläin> foundEläin = new List<Eläin>();

             // Create command
             OleDbCommand myQuery = new OleDbCommand("SELECT * FROM Eläin WHERE Nimi ='" + name + "';", openConnection);

            // Execute command
            OleDbDataReader myReader = myQuery.ExecuteReader();

            // Check if we got rows
            if (myReader.HasRows)
            {
                // Loop through results
                while (myReader.Read() == true)
                {
                    // Read data from each row
                    int IDNumero = myReader.GetInt32(0);
                    string EläinNimi = myReader.GetString(1);
                    string EläimenKuvaus = myReader.GetString(2);
                    
                    // Create user
                    Eläin e = new Eläin(IDNumero, EläinNimi, EläimenKuvaus );

                    // Add to the list to be returned
                    foundEläin.Add(e);
                }
            }


            // Return the list (may have 0...N users)
            return foundEläin;
        }

        #endregion

        #region LISÄÄ_TIETOJA

        #region ASIAKAS
        public void AddAsiakas(string id, string etunimi, string sukunimi)
        {
            try
            {
                if (openConnection == null) 
                {
                    inst.CreateConnection();
                }

                OleDbCommand command = new OleDbCommand("insert into Omistaja (OmistajaID,Etunimi,Sukunimi) " +
                    "values('" + id + "' , '" + etunimi + "','" + sukunimi + "') ;", openConnection);

                command.ExecuteNonQuery();

                MessageBox.Show("Tiedot tallennettu onnistuneesti");
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error " + ex);
            }


        }

        #endregion

        #region ELÄIN

        public void AddEläin(string nimi, string laji, string rotu, int ikä)
        {
            try
            {
                if (openConnection == null)
                {
                    inst.CreateConnection();
                }

                OleDbCommand command = new OleDbCommand("insert into Lemmikki (LemmikkiNimi,Laji,Rotu,Ikä) " +
                    "values('" + nimi + "' , '" + laji + "' , '" + rotu + "' , '" + ikä + "') ;", openConnection);

                command.ExecuteNonQuery();

                MessageBox.Show("Tiedot tallennettu onnistuneesti");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
