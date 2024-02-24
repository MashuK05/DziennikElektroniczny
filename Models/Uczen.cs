using System.ComponentModel;

namespace DziennikElektroniczny.Models
{
    public class Uczen : INotifyPropertyChanged
    {
        private int numer;
        public int Numer
        {
            get { return numer; }
            set
            {
                if (numer != value)
                {
                    numer = value;
                    OnPropertyChanged(nameof(Numer));
                }
            }
        }

        private string nazwa;
        public string Nazwa
        {
            get { return nazwa; }
            set
            {
                if (nazwa != value)
                {
                    nazwa = value;
                    OnPropertyChanged(nameof(Nazwa));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
