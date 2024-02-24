using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DziennikElektroniczny;

public partial class ListaUczniowPage : ContentPage
{
    private Dictionary<string, CheckBox> checkBoxes = new Dictionary<string, CheckBox>();

    public ListaUczniowPage(string nazwaKlasy, string[] uczniowie)
    {
        InitializeComponent();
        Headbar.Text = "Lista uczni�w klasy: " + nazwaKlasy; // Ustawiamy tytu� strony na nazw� klasy

        // Tworzymy interfejs u�ytkownika dla ka�dego ucznia i dodajemy go do layoutu
        int numerUcznia = 1;
        foreach (var uczen in uczniowie)
        {
            var uczniowieLayout = new StackLayout { Orientation = StackOrientation.Horizontal };

            // Dodajemy numer ucznia obok nazwy
            var numerLabel = new Label { Text = numerUcznia.ToString()};
            numerLabel.StyleClass ??= new List<string>();
            numerLabel.StyleClass.Add("NazwaUcznia");
            var nazwaLabel = new Label { Text = uczen};
            nazwaLabel.StyleClass ??= new List<string>();
            nazwaLabel.StyleClass.Add("NazwaUcznia");


            // Dodanie CheckBoxa do zaznaczania obecno�ci
            var checkBox = new CheckBox();
            checkBoxes.Add(uczen, checkBox);

            uczniowieLayout.Children.Add(numerLabel);
            uczniowieLayout.Children.Add(nazwaLabel);
            uczniowieLayout.Children.Add(checkBox);
            UczniowieLayout.Children.Add(uczniowieLayout);

            numerUcznia++;
        }
    }

    // Obs�uga klikni�cia przycisku "Wyczy�� obecno��"
    private void WyczyscObecnoscClicked(object sender, EventArgs e)
    {
        foreach (var checkBox in checkBoxes.Values)
        {
            checkBox.IsChecked = false;
        }
    }

    // Obs�uga klikni�cia przycisku "Losuj do odpowiedzi"
    private void LosujDoOdpowiedziClicked(object sender, EventArgs e)
    {
        // Pobranie listy nazw obecnych uczni�w
        var obecniUczniowie = checkBoxes.Where(kvp => kvp.Value.IsChecked).Select(kvp => kvp.Key).ToList();

        // Sprawdzenie, czy s� obecni uczniowie do losowania
        if (obecniUczniowie.Count == 0)
        {
            // Wy�wietlenie komunikatu o braku obecnych uczni�w
            DisplayAlert("Uwaga", "Brak obecnych uczni�w do losowania!", "OK");
            return;
        }

        // Losowanie ucznia do odpowiedzi spo�r�d obecnych uczni�w
        Random random = new Random();
        int index = random.Next(obecniUczniowie.Count);
        string wybranyUczen = obecniUczniowie[index];

        var aktualnaNazwa = wybranyUczen.Substring(2);
        // Wy�wietlenie komunikatu z wylosowanym uczniem
        DisplayAlert("Wylosowany ucze�", $"Ucze� do odpowiedzi: {aktualnaNazwa}", "OK");
    }
}
