using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ITD.ETC.VETC.Synchronization.MtcAndEtc.ViewModel
{
    [DataContract(Namespace = "")]
    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string ViewModelName;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        protected ViewModelBase()
        {

        }

        public void Dispose()
        {
            this.OnDispose();
        }

        protected virtual void OnDispose()
        {

        }


    }
}
