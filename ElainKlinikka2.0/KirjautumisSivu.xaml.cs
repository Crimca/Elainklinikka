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

        private void kirjauduClick(object sender, RoutedEventArgs e)
        {
            // Tällä informoidaan MainWindow:lle, että käyttäjä suoritti dialogin loppuun
            this.DialogResult = true;
        }

        /// <summary>
        /// Property, jolla voimme lukea käyttäjän syöttämä salasana
        /// </summary>
        public string EnteredPassword
        {
            get { return salasanaBox.Password; }
        }

        /// <summary>
        /// Property, jolla voimme lukea käyttäjän syöttämä username
        /// </summary>
        public string EnteredUsername
        {
            get { return usernameBox.Text; }
        }
    }
}
