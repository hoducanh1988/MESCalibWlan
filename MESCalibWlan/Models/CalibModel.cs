using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EW30SX.Models {
    public class CalibModel : INotifyPropertyChanged {

        public CalibModel() {
            buttonStartContent = "START";
            this.Init();
        }

        public void Init() {
            openComResult = bootCompleteResult 
                          = loginResult
                          = getMacWanResult
                          = changeIPResult
                          = pingDUTResult
                          = switchModeResult
                          = calibWlanResult 
                          = "-";

            openComElapsedTime = bootCompleteElapsedTime
                               = loginElapsedTime
                               = getMacWanElapsedTime
                               = changeIPElapsedTime
                               = pingDUTElapsedTime
                               = switchModeElapsedTime
                               = calibWlanElapsedTime 
                               = "-";

            macWan = "";
            totalTime = "00:00:00";
            totalResult = "-";
            logSystem = logQSPR = logDUT = "";
        }

        public bool Passed() {
            buttonStartContent = "START";
            totalResult = "Passed";
            return true;
        }

        public bool Failed() {
            buttonStartContent = "START";
            totalResult = "Failed";
            return true;
        }

        public bool Waiting() {
            buttonStartContent = "STOP";
            totalResult = "Waiting...";
            return true;
        }

        string _mac;
        public string macWan {
            get { return _mac; }
            set {
                _mac = value;
                OnPropertyChanged(nameof(macWan));
            }
        }
        string _total_time;
        public string totalTime {
            get { return _total_time; }
            set {
                _total_time = value;
                OnPropertyChanged(nameof(totalTime));
            }
        }
        string _total_result;
        public string totalResult {
            get { return _total_result; }
            set {
                _total_result = value;
                OnPropertyChanged(nameof(totalResult));
            }
        }
        string _start_content;
        public string buttonStartContent {
            get { return _start_content; }
            set {
                _start_content = value;
                OnPropertyChanged(nameof(buttonStartContent));
            }
        }
        string _open_com_result;
        public string openComResult {
            get { return _open_com_result; }
            set {
                _open_com_result = value;
                OnPropertyChanged(nameof(openComResult));
            }
        }
        string _open_com_elapsed;
        public string openComElapsedTime {
            get { return _open_com_elapsed; }
            set {
                _open_com_elapsed = value;
                OnPropertyChanged(nameof(openComElapsedTime));
            }
        }
        string _boot_complete_result;
        public string bootCompleteResult {
            get { return _boot_complete_result; }
            set {
                _boot_complete_result = value;
                OnPropertyChanged(nameof(bootCompleteResult));
            }
        }
        string _boot_complete_elapsed;
        public string bootCompleteElapsedTime {
            get { return _boot_complete_elapsed; }
            set {
                _boot_complete_elapsed = value;
                OnPropertyChanged(nameof(bootCompleteElapsedTime));
            }
        }
        string _login_result;
        public string loginResult {
            get { return _login_result; }
            set {
                _login_result = value;
                OnPropertyChanged(nameof(loginResult));
            }
        }
        string _login_elapsed;
        public string loginElapsedTime {
            get { return _login_elapsed; }
            set {
                _login_elapsed = value;
                OnPropertyChanged(nameof(loginElapsedTime));
            }
        }
        string _get_mac_result;
        public string getMacWanResult {
            get { return _get_mac_result; }
            set {
                _get_mac_result = value;
                OnPropertyChanged(nameof(getMacWanResult));
            }
        }
        string _get_mac_elapsed;
        public string getMacWanElapsedTime {
            get { return _get_mac_elapsed; }
            set {
                _get_mac_elapsed = value;
                OnPropertyChanged(nameof(getMacWanElapsedTime));
            }
        }
        string _change_ip_result;
        public string changeIPResult {
            get { return _change_ip_result; }
            set {
                _change_ip_result = value;
                OnPropertyChanged(nameof(changeIPResult));
            }
        }
        string _change_ip_elapsed;
        public string changeIPElapsedTime {
            get { return _change_ip_elapsed; }
            set {
                _change_ip_elapsed = value;
                OnPropertyChanged(nameof(changeIPElapsedTime));
            }
        }
        string _ping_dut_result;
        public string pingDUTResult {
            get { return _ping_dut_result; }
            set {
                _ping_dut_result = value;
                OnPropertyChanged(nameof(pingDUTResult));
            }
        }
        string _ping_dut_elapsed;
        public string pingDUTElapsedTime {
            get { return _ping_dut_elapsed; }
            set {
                _ping_dut_elapsed = value;
                OnPropertyChanged(nameof(pingDUTElapsedTime));
            }
        }
        string _switch_mode_result;
        public string switchModeResult {
            get { return _switch_mode_result; }
            set {
                _switch_mode_result = value;
                OnPropertyChanged(nameof(switchModeResult));
            }
        }
        string _switch_mode_elapsed;
        public string switchModeElapsedTime {
            get { return _switch_mode_elapsed; }
            set {
                _switch_mode_elapsed = value;
                OnPropertyChanged(nameof(switchModeElapsedTime));
            }
        }
        string _calib_wlan_result;
        public string calibWlanResult {
            get { return _calib_wlan_result; }
            set {
                _calib_wlan_result = value;
                OnPropertyChanged(nameof(calibWlanResult));
            }
        }
        string _calib_wlan_elapsed;
        public string calibWlanElapsedTime {
            get { return _calib_wlan_elapsed; }
            set {
                _calib_wlan_elapsed = value;
                OnPropertyChanged(nameof(calibWlanElapsedTime));
            }
        }
        string _log_system;
        public string logSystem {
            get { return _log_system; }
            set {
                _log_system = value;
                OnPropertyChanged(nameof(logSystem));
            }
        }
        string _log_qspr;
        public string logQSPR {
            get { return _log_qspr; }
            set {
                _log_qspr = value;
                OnPropertyChanged(nameof(logQSPR));
            }
        }
        string _log_dut;
        public string logDUT {
            get { return _log_dut; }
            set {
                _log_dut = value;
                OnPropertyChanged(nameof(logDUT));
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
