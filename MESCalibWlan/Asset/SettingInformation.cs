using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESCalibWlan.Asset {

    public class SettingInformation : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public SettingInformation() {
            ModelName = "";
        }

        string _model_name;
        public string ModelName {
            get { return _model_name; }
            set {
                _model_name = value;
                OnPropertyChanged(nameof(ModelName));
            }
        }
    }

}
