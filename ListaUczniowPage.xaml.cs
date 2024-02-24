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
        Headbar.Text = "Lista uczniów klasy: " + nazwaKlasy; // Ustawiamy tytu³ strony na nazwê klasy

        // Tworzymy interfejs u¿ytkownika dla ka¿dego ucznia i dodajemy go do layoutu
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


            // Dodanie CheckBoxa do zaznaczania obecnoœci
            var checkBox = new CheckBox();
            checkBoxes.Add(uczen, checkBox);

            uczniowieLayout.Children.Add(numerLabel);
            uczniowieLayout.Children.Add(nazwaLabel);
            uczniowieLayout.Children.Add(checkBox);
            UczniowieLayout.Children.Add(uczniowieLayout);

            numerUcznia++;
        }
    }

    // Obs³uga klikniêcia przycisku "Wyczyœæ obecnoœæ"
    private void WyczyscObecnoscClicked(object sender, EventArgs e)
    {
        foreach (var checkBox in checkBoxes.Values)
        {
            checkBox.IsChecked = false;
        }
    }

    // Obs³uga klikniêcia przycisku "Losuj do odpowiedzi"
    private void LosujDoOdpowiedziClicked(object sender, EventArgs e)
    {
        // Pobranie listy nazw obecnych uczniów
        var obecniUczniowie = checkBoxes.Where(kvp => kvp.Value.IsChecked).Select(kvp => kvp.Key).ToList();

        // Sprawdzenie, czy s¹ obecni uczniowie do losowania
        if (obecniUczniowie.Count == 0)
        {
            // Wyœwietlenie komunikatu o braku obecnych uczniów
            DisplayAlert("Uwaga", "Brak obecnych uczniów do losowania!", "OK");
            return;
        }

        // Losowanie ucznia do odpowiedzi spoœród obecnych uczniów
        Random random = new Random();
        int index = random.Next(obecniUczniowie.Count);
        string wybranyUczen = obecniUczniowie[index];

        var aktualnaNazwa = wybranyUczen.Substring(2);
        // Wyœwietlenie komunikatu z wylosowanym uczniem
        DisplayAlert("Wylosowany uczeñ", $"Uczeñ do odpowiedzi: {aktualnaNazwa}", "OK");
    }
}
