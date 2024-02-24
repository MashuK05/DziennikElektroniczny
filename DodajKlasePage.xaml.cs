using Microsoft.Maui.Controls;
using System;
using System.IO;

namespace DziennikElektroniczny;

public partial class DodajKlasePage : ContentPage
{
	public DodajKlasePage()
	{
		InitializeComponent();
	}
    // Obs³uga dodawania klasy
    private void DodajKlase(object sender, EventArgs e)
    {
        // Pobieranie nazwy klasy i imion uczniów z formularza
        string nazwaKlasy = NazwaKlasyEntry.Text;

        // Tworzenie œcie¿ki do folderu klasy w g³ównym katalogu projektu
        string folderPath = Path.Combine(AppContext.BaseDirectory, "klasy");

        // Sprawdzenie, czy folder istnieje, jeœli nie, to go utwórz
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Tworzenie œcie¿ki do pliku tekstowego z nazw¹ klasy
        string filePath = Path.Combine(folderPath, nazwaKlasy + ".txt");

        // Zapisywanie danych do pliku tekstowego
        using (StreamWriter writer = File.CreateText(filePath))

            // Powiadomienie u¿ytkownika o pomyœlnym dodaniu klasy
            DisplayAlert("Sukces", "Klasa zosta³a dodana pomyœlnie.", "OK");

        // Wyczyœæ formularz
        ClearForm();

        // Powróæ do poprzedniej strony
        Navigation.PopAsync();
    }

    // Metoda do czyszczenia formularza
    private void ClearForm()
    {
        NazwaKlasyEntry.Text = string.Empty;
    }
}