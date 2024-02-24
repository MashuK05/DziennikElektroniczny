using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using DziennikElektroniczny.Models;

namespace DziennikElektroniczny
{
    public partial class EdytujListePage : ContentPage
    {
        private ObservableCollection<Uczen> Uczniowie { get; set; }
        private string SciezkaDoPliku { get; set; }

        public EdytujListePage(string sciezkaDoPliku)
        {
            InitializeComponent();
            SciezkaDoPliku = sciezkaDoPliku;
            Uczniowie = new ObservableCollection<Uczen>();

            // Odczytanie listy uczni�w z pliku tekstowego
            string[] uczniowie = File.ReadAllLines(SciezkaDoPliku);

            // Dodanie uczni�w do listy
            foreach (var ucznik in uczniowie)
            {
                Uczniowie.Add(new Uczen { Nazwa = ucznik });
            }

            // Ustawienie �r�d�a danych dla listy
            UczniowieListView.ItemsSource = Uczniowie;

            // Ustawienie numer�w uczniom
            AktualizujNumeryUczniow();
        }

        private async void DodajUczniaClicked(object sender, EventArgs e)
        {
            string nowyUczenNazwa = NowyUczenEntry.Text;
            if (!string.IsNullOrWhiteSpace(nowyUczenNazwa))
            {
                Uczniowie.Add(new Uczen { Nazwa = ". " + nowyUczenNazwa });
                NowyUczenEntry.Text = string.Empty;

                // Aktualizacja numer�w uczni�w
                AktualizujNumeryUczniow();
            }
            else
            {
                await DisplayAlert("B��d", "Wprowad� nazw� nowego ucznia.", "OK");
            }
        }

        private async void EdytujUczniaClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var uczen = (Uczen)button.BindingContext;

            // Pobranie aktualnej nazwy ucznia, ignoruj�c pierwsze dwa znaki ". "
            var aktualnaNazwa = uczen.Nazwa.Substring(2);

            var nowaNazwa = await DisplayPromptAsync("Edytuj ucznia", "Wprowad� now� nazw�:", initialValue: aktualnaNazwa);
            if (!string.IsNullOrWhiteSpace(nowaNazwa))
            {
                // Dodanie z powrotem dw�ch pierwszych znak�w ". " przed now� nazw�
                uczen.Nazwa = ". " + nowaNazwa;
            }
        }

        private void UsunUczniaClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var uczen = (Uczen)button.BindingContext;
            Uczniowie.Remove(uczen);

            // Aktualizacja numer�w uczni�w
            AktualizujNumeryUczniow();
        }

        private void AktualizujNumeryUczniow()
        {
            for (int i = 0; i < Uczniowie.Count; i++)
            {
                Uczniowie[i].Numer = i + 1;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Zapisanie zmienionej listy uczni�w do pliku tekstowego
            File.WriteAllLines(SciezkaDoPliku, Uczniowie.Select(uczen => uczen.Nazwa));
        }
    }
}