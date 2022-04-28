using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// <summary>
    /// Interaction logic for KirjautumisSivu.xaml
    /// </summary>
    /// <summary>
    /// Interaction logic for KirjautumisSivu.xaml
    /// </summary>

    public partial class KirjautumisSivu : Window
    {
        private string kirjautunutUsername = "";
        public KirjautumisSivu()
        {
            InitializeComponent();
        }
        private void salasanaBox_TextChanged(object sender, RoutedEventArgs e)
        {
            // Tarkistetaan, pitäisikö "Kirjaudu" painike enabloida
            controlLoginEnabled();
        }

        private void usernameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Tarkistetaan, pitäisikö "Kirjaudu" painike enabloida
            controlLoginEnabled();
        }

        private void controlLoginEnabled()
        {
            // Tutkitaan, onko jotain syötetty sekä username että password laatikoihin
            bool usernameOK = String.IsNullOrWhiteSpace(usernameBox.Text) == false;
            bool passwordOK = String.IsNullOrWhiteSpace(salasanaBox.Password) == false;

            // Jos molemmissa on merkkejä, enabloi se - muuten disabloi
            kirjauduButton.IsEnabled = usernameOK && passwordOK;
        }

        private void peruutaClick(object sender, RoutedEventArgs e)
        {
            // Tämä kertoo, että dialogin suoritus peruuntui
            this.DialogResult = false;
        }
        public string EnteredUsername = "";
        private void kirjauduClick(object sender, RoutedEventArgs e)
        {
            // Tällä informoidaan MainWindow:lle, että käyttäjä suoritti dialogin loppuun
            List<User> u = DatabaseHandler.Instance.FindUsersWithEmail(usernameBox.Text);

            if (u.Count == 1)
            {
                if (u[0].UserPassword.CompareTo(salasanaBox.Password) == 0)
                {
                    //salasana täsmää
                    MessageBox.Show("Kirjautunut käyttäjä: " + usernameBox.Text);

                    // Tallennetaan kirjautuneen käyttäjän username muuttujaan, tilatietona
                    EnteredUsername = usernameBox.Text;

                    // Päivitetään käyttöliittymä, koska käyttäjä on kirjautunut
                    this.DialogResult = true;
                }
                else
                {
                    // ei täsmää
                    MessageBox.Show("Väärä tunnus tai salasana");
                }
            }
            else if (u.Count == 0)
            {
                MessageBox.Show("Found no matching users.");
            }
            else
            {
                // More than one user
                MessageBox.Show("Found " + u.Count + " matching users.");
            }
        }
    }
}
