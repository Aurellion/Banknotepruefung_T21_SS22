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
using System.Text.RegularExpressions;

namespace Banknoteprüfung
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool Eingabeprüfung(string seriennummer)
        {
            if (seriennummer.Length != 12)
            {
                TB_Ausgabe.Text = "Die Seriennummer muss folgendes Format haben: XX0000000000.";
                return false;
            }
            else if (!char.IsLetter(seriennummer[0]) || !char.IsLetter(seriennummer[1]))
            {
                TB_Ausgabe.Text = "Die beiden ersten Zeichen müssen Buchstaben sein.";
                return false;
            }
            else if (!Regex.IsMatch(seriennummer.Substring(2, seriennummer.Length - 2), "^[0-9]+$"))
            {
                TB_Ausgabe.Text = "Die letzten zehn Stellen müssen Ziffern sein.";
                return false;
            }
            else
            {
                TB_Ausgabe.Text = "";
                return true;
            }            
        }

        private void Prüfung(string seriennummer)
        {
            //Umwandlung des Eingabestrings
            int Ländercode1, Ländercode2;

            Ländercode1 = seriennummer.ToUpper()[0]-64;
            Ländercode2 = seriennummer.ToUpper()[1]-64;
            string zahl = Ländercode1.ToString() + Ländercode2.ToString() + seriennummer.Substring(2, seriennummer.Length - 3);
            int quersumme=0;

            foreach (char c in zahl)
            {
                quersumme += Convert.ToInt32(c.ToString());
            }

            //TB_Ausgabe.Text = quersumme.ToString();
            int rest = quersumme % 9;
            int kontrollnummer = Convert.ToInt32(seriennummer.Last().ToString());

            if (7 - rest == kontrollnummer || (7 - rest == 0 && kontrollnummer == 9) || (7 - rest == -1 && kontrollnummer == 8))
            {
                TB_Ausgabe.Text = "Die Seriennummer ist gültig!";
            }
            else TB_Ausgabe.Text = "Die Seriennummer ist nicht gültig!";

            //BSP: WA8930983573
        }

        private void Button_Prüfen_Click(object sender, RoutedEventArgs e)
        {
            if (Eingabeprüfung(TB_Eingabe.Text))
            {
                Prüfung(TB_Eingabe.Text);
            }

        }

        private void TB_Eingabe_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Eingabeprüfung(TB_Eingabe.Text))
                {
                    Prüfung(TB_Eingabe.Text);
                }
            }
        }
    }
}
