using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ElainKlinikka2._0.Helpers;
using ElainKlinikka2._0.Pages;
using Microsoft.Win32;
namespace ElainKlinikka2._0.Helpers
{
    internal class UnpaidCalculation
    {
        database db;

        public UnpaidCalculation()
        {
            db = new database();
        }

        public String CalculateUnpaidVisits()
        {
            //selecting path
            string directorySelected = "";
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
            openFileDialog.ShowDialog();
            directorySelected = openFileDialog.SelectedPath;

            var csv = new StringBuilder();

            List<Payment> payments = db.GetAllPayments();
            List<Payment> unpaid = new List<Payment>();

            foreach (Payment p in payments)
            {
                if (!p.paid.Contains("Maksettu|")) { unpaid.Add(p); }
            }

            List<Payment> SortedList = unpaid.OrderBy(o => o.ownerID).ToList();

            List<PriceList> prices = db.GetPrices();
            int value = 0;
            string curName = "";
            int curNum = 0;

            //string info = "Ei Maksettu\n";
            var newLine = string.Format("Maksu ID, Omistajan nimi, Puhelinnumero, Lemmikin nimi, Lemmikin rotu, Varauksen tyyppi, Maksut yhteensä, Maksun tila");
            csv.AppendLine(newLine);
            foreach (Payment payment in SortedList)
            {

                Owner o = db.GetOwner(payment.ownerID);
                Pet pet = db.LoadPet(payment.petID);
                Animal animal = db.GetAnimal(pet.animalID);
                Appointment appointment = db.GetAppointment(payment.appointmentID);


                //value += Int32.Parse(payment.paymentValue);
                //curNum++;
                curName = payment.ownerID;

                string id = payment.paymentID.ToString();
                string name = o.Forename + " " + o.Surname;
                string num = o.Phonenum;
                string petName = pet.petName;
                string animalInfo = animal.species + " | " + animal.breed;
                string reason = appointment.Reason == "CANCELLED" ? "CANCELLED" : prices[Int32.Parse(appointment.Reason) - 1].Procedure;
                string payVal = payment.paymentValue;
                string status = payment.paid;
                var Line = string.Format(id + "," + name + "," + num + "," + petName + "," + animalInfo + "," + reason + "," + payVal + "," + status);
                csv.AppendLine(Line);

            }

            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(directorySelected + "\\Test.csv"))
                {
                    File.Delete(directorySelected + "\\Test.csv");
                }
                FileStream fs = File.Create(directorySelected + "\\Test.csv");
                fs.Close();
                File.WriteAllText(directorySelected + "\\Test.csv", csv.ToString());
            }
            catch (Exception ex) 
            {
                csv.Clear();                
                csv.AppendLine(ex.Message);
            }
            return csv.ToString();
        }
    }

    /*
    public String CalculateUnpaidVisits()
    {
        //selecting path
        string directorySelected = "";
        FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
        openFileDialog.ShowDialog();
        directorySelected = openFileDialog.SelectedPath;

        //before your loop
        var csv = new StringBuilder();

        //after your loop
        File.WriteAllText(directorySelected, csv.ToString());



        List<Payment> payments = db.GetAllPayments();
        List<Payment> unpaid = new List<Payment>();

        foreach (Payment p in payments)
        {
            if (!p.paid.Contains("Maksettu|")) { unpaid.Add(p); }
        }

        List<Payment> SortedList = unpaid.OrderBy(o => o.ownerID).ToList();

        int value = 0;
        string curName = "";
        int curNum = 0;

        //string info = "Ei Maksettu\n";
        string info = "Payment ID, Owner Name, Owner Contact Number,  Pet Name, Pet Type, Appointment Type, Paymeny Value, payment Status";

        foreach (Payment payment in SortedList)
        {

            Owner o = db.GetOwner(payment.ownerID);
            Pet pet = db.LoadPet(payment.petID);
            Animal animal = db.GetAnimal(pet.animalID);
            Appointment appointment = db.GetAppointment(payment.appointmentID);


            //value += Int32.Parse(payment.paymentValue);
            //curNum++;
            curName = payment.ownerID;

            var newLine = payment.paymentID + "," + o.Forename + " " + o.Surname + "," + o.Phonenum + "," + pet.petName + "," + animal.species + " " + animal.breed + "," + prices[Int32.Parse(appointment.Reason) - 1].Procedure + "," + payment.paid;
            csv.AppendLine(newLine);
            
         if (curName != payment.ownerID)
         {
             if (curNum != 0)
             {
                 info += "\nYhteensä______________________";
                 info += "\n\t\t€" + value;
                 info += "\n\n";
             }

             value = 0;
             Owner o = db.GetOwner(payment.ownerID);
             Pet pet = db.LoadPet(payment.petID);
             Animal animal = db.GetAnimal(pet.animalID);

             //value += Int32.Parse(payment.paymentValue);
             //curNum++;
             curName = payment.ownerID;

             info += payment.paymentID + "," + o.Forename + " " + o.Surname + "," + o.Phonenum + ","
         }
         else
         {
             info += "\n\t\t€" + payment.paymentValue;
             value += Int32.Parse(payment.paymentValue);
         }

         
        }
        info += "\nYhteensä______________________";
        info += "\n\t\t€" + value;

        return info;
    }
    
}*/
}