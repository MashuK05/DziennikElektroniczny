using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Linq;

namespace DziennikElektroniczny
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            OdswiezListeKlas();
        }

        // Obsługa kliknięcia przycisku
        private async void OnDodajKlaseClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DodajKlasePage()); // Otwórz nową stronę
        }
        private void OdswiezListeKlasClicked(object sender, EventArgs e)
        {
            OdswiezListeKlas();
        }

        // Metoda odświeżająca listę klas
        private void OdswiezListeKlas()
        {
            KlasyStackLayout.Children.Clear(); // Wyczyść istniejące contentView przed dodaniem nowych

            // Tworzenie ścieżki do folderu klasy w głównym katalogu projektu
            string folderPath = Path.Combine(AppContext.BaseDirectory, "klasy");

            // Sprawdzenie, czy folder istnieje
            if (Directory.Exists(folderPath))
            {
                // Pobranie listy plików tekstowych z folderu klasy
                var plikiKlas = Directory.GetFiles(folderPath, "*.txt");

                // Wyświetlenie każdego pliku jako osobny contentView
                foreach (var plikKlasy in plikiKlas)
                {
                    var nazwaKlasy = Path.GetFileNameWithoutExtension(plikKlasy);

                    // Utworzenie warstwy z tekstem klasy i przyciskiem usuwania
                    var contentView = new ContentView();
                    var label = new Label { Text = nazwaKlasy };
                    label.StyleClass ??= new List<string>();
                    label.StyleClass.Add("KlasaLabel");
                    var deleteButton = new Button { Text = "Usuń" };
                    deleteButton.StyleClass ??= new List<string>();
                    deleteButton.StyleClass.Add("DelButton");

                    // Dodanie obsługi zdarzenia kliknięcia na contentView
                    contentView.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(() => PokazUczniow(plikKlasy))
                    });

                    // Dodanie obsługi zdarzenia kliknięcia na przycisk "Usuń"
                    deleteButton.Clicked += async (sender, e) =>
                    {
                        var result = await DisplayAlert("Potwierdzenie", $"Czy na pewno chcesz usunąć klasę {nazwaKlasy}?", "Tak", "Anuluj");
                        if (result)
                        {
                            File.Delete(plikKlasy); // Usunięcie pliku
                            OdswiezListeKlas(); // Odświeżenie listy klas
                        }
                    };

                    // Układanie elementów w jednym wierszu
                    var stackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
                    stackLayout.Children.Add(label);
                    stackLayout.Children.Add(deleteButton);

                    // Dodanie warstwy do contentView
                    contentView.Content = stackLayout;

                    // Dodanie contentView do stackLayout
                    KlasyStackLayout.Children.Add(contentView);
                }
            }
        }

        // Metoda wywoływana po kliknięciu na contentView z nazwą klasy
        private async void PokazUczniow(string sciezkaDoPliku)
        {
            // Pobranie nazwy klasy z nazwy pliku
            var nazwaKlasy = Path.GetFileNameWithoutExtension(sciezkaDoPliku);

            // Wyświetlenie listy opcji dla wybranej klasy
            var opcje = await DisplayActionSheet($"Opcje dla klasy {nazwaKlasy}", "Anuluj", null, "Wyświetl listę uczniów", "Edytuj listę uczniów");

            switch (opcje)
            {
                case "Wyświetl listę uczniów":
                    // Odczytanie listy uczniów z pliku tekstowego
                    string[] uczniowie = File.ReadAllLines(sciezkaDoPliku);

                    // Utworzenie nowej strony do wyświetlenia pełnej listy uczniów
                    var listaUczniowPage = new ListaUczniowPage(nazwaKlasy, uczniowie);

                    // Nawigacja do nowej strony
                    await Navigation.PushAsync(listaUczniowPage);
                    break;

                case "Edytuj listę uczniów":
                    // Otwarcie strony do edycji listy uczniów
                    var edytujListePage = new EdytujListePage(sciezkaDoPliku);
                    await Navigation.PushAsync(edytujListePage);
                    break;

                default:
                    break;
            }
        }
    }
}