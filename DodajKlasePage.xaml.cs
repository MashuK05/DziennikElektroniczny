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
    // Obs�uga dodawania klasy
    private void DodajKlase(object sender, EventArgs e)
    {
        // Pobieranie nazwy klasy i imion uczni�w z formularza
        string nazwaKlasy = NazwaKlasyEntry.Text;

        // Tworzenie �cie�ki do folderu klasy w g��wnym katalogu projektu
        string folderPath = Path.Combine(AppContext.BaseDirectory, "klasy");

        // Sprawdzenie, czy folder istnieje, je�li nie, to go utw�rz
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Tworzenie �cie�ki do pliku tekstowego z nazw� klasy
        string filePath = Path.Combine(folderPath, nazwaKlasy + ".txt");

        // Zapisywanie danych do pliku tekstowego
        using (StreamWriter writer = File.CreateText(filePath))

            // Powiadomienie u�ytkownika o pomy�lnym dodaniu klasy
            DisplayAlert("Sukces", "Klasa zosta�a dodana pomy�lnie.", "OK");

        // Wyczy�� formularz
        ClearForm();

        // Powr�� do poprzedniej strony
        Navigation.PopAsync();
    }

    // Metoda do czyszczenia formularza
    private void ClearForm()
    {
        NazwaKlasyEntry.Text = string.Empty;
    }
}