using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EW30SX.Models {
    public class LogModel : INotifyPropertyChanged {

        public LogModel() {
            isAccess = isSetting = isSoftware = isGolden = isLog = isQPST = false;
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
        bool _is_golden;
        public bool isGolden {
            get { return _is_golden; }
            set {
                _is_golden = value;
                OnPropertyChanged(nameof(isGolden));
            }
        }
        bool _is_log;
        public bool isLog {
            get { return _is_log; }
            set {
                _is_log = value;
                OnPropertyChanged(nameof(isLog));
            }
        }
        bool _is_qpst;
        public bool isQPST {
            get { return _is_qpst; }
            set {
                _is_qpst = value;
                OnPropertyChanged(nameof(isQPST));
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
