using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UWPUtilities.Util
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T field, T val, [CallerMemberName]string propertyName = null)
        {
            if (Equals(field, val)) { return; }
            field = val;
            OnPropertyChanged(propertyName);
        }
    }
}
