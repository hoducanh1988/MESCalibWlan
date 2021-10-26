using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EW30CX.Models {

    public class MainContentModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public MainContentModel() {
            Init();
            isSmall = false;
        }

        public void Init() {
            isAttenuation = isCalib = isVerification = isSetting = isLog = isHelp = isAbout = false;
        }

        bool _is_att;
        public bool isAttenuation {
            get { return _is_att; }
            set {
                _is_att = value;
                OnPropertyChanged(nameof(isAttenuation));
            }
        }
        bool _is_calib;
        public bool isCalib {
            get { return _is_calib; }
            set {
                _is_calib = value;
                OnPropertyChanged(nameof(isCalib));
            }
        }
        bool _is_veri;
        public bool isVerification {
            get { return _is_veri; }
            set {
                _is_veri = value;
                OnPropertyChanged(nameof(isVerification));
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
        bool _is_log;
        public bool isLog {
            get { return _is_log; }
            set {
                _is_log = value;
                OnPropertyChanged(nameof(isLog));
            }
        }
        bool _is_help;
        public bool isHelp {
            get { return _is_help; }
            set {
                _is_help = value;
                OnPropertyChanged(nameof(isHelp));
            }
        }
        bool _is_about;
        public bool isAbout {
            get { return _is_about; }
            set {
                _is_about = value;
                OnPropertyChanged(nameof(isAbout));
            }
        }
        bool _is_small;
        public bool isSmall {
            get { return _is_small; }
            set {
                _is_small = value;
                OnPropertyChanged(nameof(isSmall));
            }
        }
    }
}
