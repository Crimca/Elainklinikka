﻿using System;
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


namespace ElainKlinikka2._0.Helpers
{
    class database
    {
        private OleDbConnection conn = null;
        private static string DB_PATH = @"..\..\lib.accdb";

        private string animalTable = "Animal";
        private string appointmentTable = "Appointment";
        private string employeeTable = "Employee";
        private string ownerTable = "Owner";
        private string paymentTable = "Payment";
        private string petTable = "Pet";
        private string priceListTable = "PriceList";
        private string userTable = "Users";

        //Constructor
        public database()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data Source=" + DB_PATH + ";");
        }

        #region OPEN AND CLOSE CONNECTION
        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }


        //Close connection
        private bool CloseConnection()
        {
            try
            {
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        #endregion

        #region KÄYTTÄJÄT
        /// <summary>
        /// Try to find users with the matching email.
        /// </summary>
        /// <param name="email">Email to check for.</param>
        /// <returns>A list of users, 0...N results.</returns>
        public List<User> FindUsersWithEmail(string email)
        {
            //Open connection
            conn.Open();

            List<User> foundUsers = new List<User>();

            // Create command
            OleDbCommand myQuery = new OleDbCommand("SELECT * FROM " + userTable + " WHERE UserEmail='" + email + "';", conn);

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

            //Close connection
            conn.Close();
            // Return the list (may have 0...N users)
            return foundUsers;

        }
        #endregion


        #region Pets    
        //Load All Pets
        public List<Pet> LoadPetTable()
        {
            string query = "SELECT * FROM " + petTable;
            List<Pet> list = new List<Pet>();

            if (this.OpenConnection() == true)
            {
                try
                {
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    OleDbDataReader myReader = cmd.ExecuteReader();

                    while (myReader.Read())
                    {
                        Pet p = new Pet
                        {
                            petID = myReader["petID"].ToString(),
                            petName = myReader["petName"].ToString(),
                            weight = float.Parse(myReader["weight"].ToString()),
                            age = Int32.Parse(myReader["age"].ToString()),
                            vaccinations = myReader["vaccinations"].ToString(),
                            prescriptions = myReader["prescriptions"].ToString(),
                            diagnoses = myReader["diagnoses"].ToString(),
                            ownerID = myReader["ownerID"].ToString(),
                            animalID = myReader["animalID"].ToString(),
                            comment = myReader["comment"].ToString()
                        };
                        list.Add(p);
                    }

                    myReader.Close();
                    this.CloseConnection();
                    return list;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return list;
                }
            }
            else
            {
                return list;
            }
        }

        //Load One Pet
        public Pet LoadPet(string id)
        {
            string query = "SELECT * FROM " + petTable + " WHERE petID = " + id + "";

            if (this.OpenConnection() == true)
            {

                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader myReader = cmd.ExecuteReader();
                Pet p = new Pet();

                while (myReader.Read())
                {
                    p = new Pet
                    {
                        petID = myReader["petID"].ToString(),
                        petName = myReader["petName"].ToString(),
                        weight = float.Parse(myReader["weight"].ToString()),
                        age = Int32.Parse(myReader["age"].ToString()),
                        vaccinations = myReader["vaccinations"].ToString(),
                        prescriptions = myReader["prescriptions"].ToString(),
                        diagnoses = myReader["diagnoses"].ToString(),
                        ownerID = myReader["ownerID"].ToString(),
                        animalID = myReader["animalID"].ToString(),
                        comment = myReader["comment"].ToString()
                    };
                }

                myReader.Close();
                this.CloseConnection();
                return p;
            }
            else
            {
                return null;
            }
        }

        //Update Pet
        public void UpdatePet(Pet pet)
        {
            if (this.OpenConnection() == true)
            {
                try
                {
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE " + petTable + " SET "
                    + "petName =  '" + pet.petName + "', "
                    + "Weight = " + pet.weight + ", "
                    + "Age = " + pet.age + ", "
                    + "Vaccinations = '" + pet.vaccinations + "', "
                    + "Prescriptions = '" + pet.prescriptions + "', "
                    + "diagnoses ='" + pet.diagnoses + "', "
                    + "ownerID = '" + pet.ownerID + "', "
                    + "animalID = " + pet.animalID + ", "
                    + "comment = '" + pet.comment + "' "
                    + "WHERE petID = " + pet.petID ;

                    Console.WriteLine(cmd.CommandText);
                    cmd.Connection = conn;

                    cmd.ExecuteNonQuery();
                    {
                        MessageBox.Show("Päivitys onnistui!");
                    }
                }
                catch (Exception ex)
                {
                    string Message = ex.Message;
                    throw ex;
                }
            }
        }


        //Insert New Pet
        public void InsertPet(Pet pet)
        {
            if (this.OpenConnection() == true)
            {
                try
                {
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO " + petTable + " VALUES ('" + pet.petID
                    + "', '" + pet.petName
                    + "', '" + pet.weight
                    + "', '" + pet.age
                    + "', '" + pet.vaccinations
                    + "', '" + pet.prescriptions
                    + "', '" + pet.diagnoses
                    + "', '" + pet.ownerID
                    + "', '" + pet.animalID
                    + "', '" + pet.comment + "')";
                    
                    
                    cmd.Connection = conn;



                    cmd.ExecuteNonQuery();
                    {
                        MessageBox.Show("Päivitys onnistui!");
                    }
                }
                catch (Exception ex)
                {
                    string Message = ex.Message;
                    throw ex;
                }
            }
        }

        public int GetNextPetID()
        {
            string query = "SELECT Count(petID) FROM " + petTable;

            if (this.OpenConnection() == true)
            {
                OleDbCommand cmd = new OleDbCommand(query, conn);
                int nextId = (int)cmd.ExecuteScalar() + 1;
                this.CloseConnection();
                return nextId;
            }
            else
            {
                return 0;
            }
        }


        #endregion Pets  

        #region animals   
        public List<Animal> GetAnimalTypes()
        {
            string query = "SELECT * FROM " + animalTable;
            List<Animal> list = new List<Animal>();

            if (this.OpenConnection() == true)
            {
                try
                {
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    OleDbDataReader myReader = cmd.ExecuteReader();

                    while (myReader.Read())
                    {
                        Animal p = new Animal
                        {
                            animalID = myReader["animalID"].ToString(),
                            species = myReader["species"].ToString(),
                            breed = myReader["breed"].ToString()
                        };
                        list.Add(p);
                    }

                    List<Animal> SortedList = list.OrderBy(o => o.animalID).ToList();
                    myReader.Close();
                    this.CloseConnection();
                    return SortedList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return list;
                }
            }
            else
            {
                return list;
            }
        }

        #endregion animals   

        #region Owners   
        public List<Owner> GetOwners()
        {
            string query = "SELECT * FROM " + ownerTable;
            List<Owner> list = new List<Owner>();

            if (this.OpenConnection() == true)
            {
                try
                {
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    OleDbDataReader myReader = cmd.ExecuteReader();

                    while (myReader.Read())
                    {
                        Owner p = new Owner
                        {
                            ownerID = Int32.Parse(myReader["ownerID"].ToString()),
                            Forename = myReader["Forename"].ToString(),
                            Surname = myReader["Surname"].ToString(),
                            Streetname = myReader["Streetname"].ToString(),
                            Postalcode = myReader["Postalcode"].ToString(),
                            City = myReader["City"].ToString(),
                            Phonenum = myReader["Phonenum"].ToString()
                        };
                        list.Add(p);
                    }

                    myReader.Close();
                    this.CloseConnection();
                    return list;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return list;
                }
            }
            else
            {
                return list;
            }
        }

        //Load One Owner
        public Owner GetOwner(string id)
        {
            string query = "SELECT * FROM " + ownerTable + " WHERE ownerID = " + id + "";

            if (this.OpenConnection() == true)
            {

                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader myReader = cmd.ExecuteReader();
                Owner o = new Owner();

                while (myReader.Read())
                {
                    o = new Owner
                    {
                        ownerID = Int32.Parse(myReader["ownerID"].ToString()),
                        Forename = myReader["Forename"].ToString(),
                        Surname = myReader["Surname"].ToString(),
                        Streetname = myReader["Streetname"].ToString(),
                        Postalcode = myReader["Postalcode"].ToString(),
                        City = myReader["City"].ToString(),
                        Phonenum = myReader["Phonenum"].ToString()
                    };
                }

                myReader.Close();
                this.CloseConnection();
                return o;
            }
            else
            {
                return null;
            }
        }

        //Update Owner
        public void UpdateOwner(Owner owner)
        {
            if (this.OpenConnection() == true)
            {
                try
                {
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE " + ownerTable + " SET "
                    + "Forename =  '" + owner.Forename + "', "
                    + "Surname = '" + owner.Surname + "', "
                    + "Streetname = '" + owner.Streetname + "', "
                    + "Postalcode = '" + owner.Postalcode + "', "
                    + "City = '" + owner.City + "', "
                    + "Phonenum = '" + owner.Phonenum + "' "
                    + "WHERE ownerID = " + owner.ownerID;

                    Console.WriteLine(cmd.CommandText);
                    cmd.Connection = conn;

                    cmd.ExecuteNonQuery();
                    {
                        MessageBox.Show("Päivitys onnistui!");
                    }
                }
                catch (Exception ex)
                {
                    string Message = ex.Message;
                    throw ex;
                }
            }
        }


        //Insert New Owner
        public void InsertOwner(Owner owner)
        {
            if (this.OpenConnection() == true)
            {
                try
                {
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO " + ownerTable + " VALUES ('" + owner.ownerID
                    + "', '" + owner.Forename
                    + "', '" + owner.Surname
                    + "', '" + owner.Streetname
                    + "', '" + owner.Postalcode
                    + "', '" + owner.City
                    + "', '" + owner.Phonenum + "')";


                    cmd.Connection = conn;



                    cmd.ExecuteNonQuery();
                    {
                        MessageBox.Show("Päivitys onnistui!");
                    }
                }
                catch (Exception ex)
                {
                    string Message = ex.Message;
                    throw ex;
                }
            }
        }

        public int GetNextOwnerID()
        {
            string query = "SELECT Count(ownerID) FROM " + ownerTable;

            if (this.OpenConnection() == true)
            {
                OleDbCommand cmd = new OleDbCommand(query, conn);
                int nextId = (int)cmd.ExecuteScalar() + 1;
                this.CloseConnection();
                return nextId;
            }
            else
            {
                return 0;
            }
        }
        #endregion Owners   

        #region Prices   
        public List<PriceList> GetPrices()
        {
            string query = "SELECT * FROM " + priceListTable;
            List<PriceList> list = new List<PriceList>();

            if (this.OpenConnection() == true)
            {
                try
                {
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    OleDbDataReader myReader = cmd.ExecuteReader();

                    while (myReader.Read())
                    {
                        PriceList p = new PriceList
                        {
                            itemID = Int32.Parse(myReader["itemID"].ToString()),
                            Price = myReader["Price"].ToString(),
                            Procedure = myReader["Procedure"].ToString(),
                            MedProc = myReader["MedProc"].ToString()
                        };
                        list.Add(p);
                    }

                    myReader.Close();
                    this.CloseConnection();
                    return list;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return list;
                }
            }
            else
            {
                return list;
            }
        }

        #endregion Prices   


        #region Appointments
        public List<Appointment> GetAppointments()
        {
            string query = "SELECT * FROM " + appointmentTable;
            List<Appointment> list = new List<Appointment>();

            if (this.OpenConnection() == true)
            {
                try
                {
                    OleDbCommand cmd = new OleDbCommand(query, conn);
                    OleDbDataReader myReader = cmd.ExecuteReader();

                    while (myReader.Read())
                    {
                        Appointment p = new Appointment
                        {
                            appointmentID = myReader["appointmentID"].ToString(),
                            Reason = myReader["Reason"].ToString(),
                            petID = myReader["petID"].ToString(),
                            employeeID = myReader["employeeID"].ToString(),
                            Day = myReader["Day"].ToString(),
                            Month = myReader["Month"].ToString(),
                            Year = myReader["Year"].ToString(),
                        };
                        list.Add(p);
                    }

                    List<Appointment> SortedList = list.OrderBy(o => o.appointmentID).ToList();
                    myReader.Close();
                    this.CloseConnection();
                    return SortedList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return list;
                }
            }
            else
            {
                return list;
            }
        }
        #endregion Appointments
    }
}