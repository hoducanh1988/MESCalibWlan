using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EW30SX.Models {
    public class LogModel : INotifyPropertyChanged {

        public LogModel() {
            isAccess = isSetting = isSoftware = isProduct = false;
        }


        bool _is_access;
        public bool isAccess {
            get { return _is_access; }
            set {
                _is_access = value;
                OnPropertyChanged(nameof(isAccess));
            }
        }
        bool _is_setting;
        public bool isSetting {
            get { return _is_setting; }
            set {
                _is_setting = value;
                OnPropertyChanged(nameof(isSetting));
            }
        }
        bool _is_software;
        public bool isSoftware {
            get { return _is_software; }
            set {
                _is_software = value;
                OnPropertyChanged(nameof(isSoftware));
            }
        }
        bool _is_product;
        public bool isProduct {
            get { return _is_product; }
            set {
                _is_product = value;
                OnPropertyChanged(nameof(isProduct));
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
