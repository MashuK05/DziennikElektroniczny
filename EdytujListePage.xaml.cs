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

            // Odczytanie listy uczniów z pliku tekstowego
            string[] uczniowie = File.ReadAllLines(SciezkaDoPliku);

            // Dodanie uczniów do listy
            foreach (var ucznik in uczniowie)
            {
                Uczniowie.Add(new Uczen { Nazwa = ucznik });
            }

            // Ustawienie Ÿród³a danych dla listy
            UczniowieListView.ItemsSource = Uczniowie;

            // Ustawienie numerów uczniom
            AktualizujNumeryUczniow();
        }

        private async void DodajUczniaClicked(object sender, EventArgs e)
        {
            string nowyUczenNazwa = NowyUczenEntry.Text;
            if (!string.IsNullOrWhiteSpace(nowyUczenNazwa))
            {
                Uczniowie.Add(new Uczen { Nazwa = ". " + nowyUczenNazwa });
                NowyUczenEntry.Text = string.Empty;

                // Aktualizacja numerów uczniów
                AktualizujNumeryUczniow();
            }
            else
            {
                await DisplayAlert("B³¹d", "WprowadŸ nazwê nowego ucznia.", "OK");
            }
        }

        private async void EdytujUczniaClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var uczen = (Uczen)button.BindingContext;

            // Pobranie aktualnej nazwy ucznia, ignoruj¹c pierwsze dwa znaki ". "
            var aktualnaNazwa = uczen.Nazwa.Substring(2);

            var nowaNazwa = await DisplayPromptAsync("Edytuj ucznia", "WprowadŸ now¹ nazwê:", initialValue: aktualnaNazwa);
            if (!string.IsNullOrWhiteSpace(nowaNazwa))
            {
                // Dodanie z powrotem dwóch pierwszych znaków ". " przed now¹ nazw¹
                uczen.Nazwa = ". " + nowaNazwa;
            }
        }

        private void UsunUczniaClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var uczen = (Uczen)button.BindingContext;
            Uczniowie.Remove(uczen);

            // Aktualizacja numerów uczniów
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

            // Zapisanie zmienionej listy uczniów do pliku tekstowego
            File.WriteAllLines(SciezkaDoPliku, Uczniowie.Select(uczen => uczen.Nazwa));
        }
    }
}