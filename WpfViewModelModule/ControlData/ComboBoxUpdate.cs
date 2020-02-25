using System.ComponentModel;

namespace WpfViewModelModule.ControlData
{
    public class ComboBoxUpdate
        : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected internal void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }

        private string _controlName;
        private string _text;
        private string _number;

        public ComboBoxUpdate(string controlName)
        {
            _controlName = controlName;
        }
        public ComboBoxUpdate(string controlName, string text, string number)
        {
            _controlName = controlName;
            this._text = text;
            this._number = number;
        }

        public string Text
        {
            get { return this._text; }
            set
            {
                this._text = value;
                var property = _controlName + nameof(Text);
                OnPropertyChanged(property);
            }
        }

        public string Number
        {
            get { return _number; }
            set
            {
                _number = value;
                var property = _controlName + nameof(Number);
                OnPropertyChanged(_controlName + property);
            }
        }
    }
}
