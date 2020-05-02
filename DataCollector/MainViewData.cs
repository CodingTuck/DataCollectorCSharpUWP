using System.ComponentModel;

namespace DataCollector
{
    class MainViewData
    {
        //these fields are binded to UI
        private string measurement;
        private string history;

        public string History
        {
            get { return this.history; }
            set { this.history = value; }
        }

        public string Measurement
        {
            set
            {
                if (measurement != value)
                {
                    OnPropertyChanging("Measurement");
                    measurement = value;
                    OnPropertyChanged("Measurement");
                }
            }
            get
            {
                return measurement;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        public void OnPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
                PropertyChanging.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
