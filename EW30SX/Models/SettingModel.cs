using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EW30SX.Models {
    public class SettingModel : INotifyPropertyChanged {

        int _log_limit;
        public int logLimitLen {
            get { return _log_limit; }
            set {
                _log_limit = value;
                OnPropertyChanged(nameof(logLimitLen));
            }

        }
        string _ip_dut;
        public string ipDUT {
            get { return _ip_dut; }
            set {
                _ip_dut = value;
                OnPropertyChanged(nameof(ipDUT));
            }
        }
        string _ip_pc;
        public string ipPC {
            get { return _ip_pc; }
            set {
                _ip_pc = value;
                OnPropertyChanged(nameof(ipPC));
            }
        }
        string _com;
        public string comPort {
            get { return _com; }
            set {
                _com = value;
                OnPropertyChanged(nameof(comPort));
            }
        }
        string _baud;
        public string baudRate {
            get { return _baud; }
            set {
                _baud = value;
                OnPropertyChanged(nameof(baudRate));
            }
        }
        string _mac_header;
        public string vnptMacHeader {
            get { return _mac_header; }
            set {
                _mac_header = value;
                OnPropertyChanged(nameof(vnptMacHeader));
            }
        }
        string _log_boot;
        public string logBootCompleted {
            get { return _log_boot; }
            set {
                _log_boot = value;
                OnPropertyChanged(nameof(logBootCompleted));
            }
        }
        string _calib_testtree;
        public string calibTesttreeFile {
            get { return _calib_testtree; }
            set {
                _calib_testtree = value;
                OnPropertyChanged(nameof(calibTesttreeFile));
            }
        }
        string _att_testtree;
        public string attTesttreeFile {
            get { return _att_testtree; }
            set {
                _att_testtree = value;
                OnPropertyChanged(nameof(attTesttreeFile));
            }
        }
        string _path_loss;
        public string pathLossFile {
            get { return _path_loss; }
            set {
                _path_loss = value;
                OnPropertyChanged(nameof(pathLossFile));
            }
        }
        string _sn_variable;
        public string snVariable {
            get { return _sn_variable; }
            set {
                _sn_variable = value;
                OnPropertyChanged(nameof(snVariable));
            }
        }
        string _2g_variable;
        public string macWlan2GVariable {
            get { return _2g_variable; }
            set {
                _2g_variable = value;
                OnPropertyChanged(nameof(macWlan2GVariable));
            }
        }
        string _5g_variable;
        public string macWlan5GVariable {
            get { return _5g_variable; }
            set {
                _5g_variable = value;
                OnPropertyChanged(nameof(macWlan5GVariable));
            }
        }
        string _timeout_boot;
        public string timeOutBoot {
            get { return _timeout_boot; }
            set {
                _timeout_boot = value;
                OnPropertyChanged(nameof(timeOutBoot));
            }
        }
        string _timeout_change_ip;
        public string timeOutChangeIP {
            get { return _timeout_change_ip; }
            set {
                _timeout_change_ip = value;
                OnPropertyChanged(nameof(timeOutChangeIP));
            }
        }
        string _time_wait_login;
        public string timeWaitLogin {
            get { return _time_wait_login; }
            set {
                _time_wait_login = value;
                OnPropertyChanged(nameof(timeWaitLogin));
            }
        }
        string _power_diff_golden;
        public string powerDiffGolden {
            get { return _power_diff_golden; }
            set {
                _power_diff_golden = value;
                OnPropertyChanged(nameof(powerDiffGolden));
            }
        }
        string _value_att_max;
        public string valueAttMax {
            get { return _value_att_max; }
            set {
                _value_att_max = value;
                OnPropertyChanged(nameof(valueAttMax));
            }
        }
        string _cyc_measure_att;
        public string cycleMeasureAtt {
            get { return _cyc_measure_att; }
            set {
                _cyc_measure_att = value;
                OnPropertyChanged(nameof(cycleMeasureAtt));
            }
        }
        string _qty_golden_verify;
        public string qtyGoldenVerify {
            get { return _qty_golden_verify; }
            set {
                _qty_golden_verify = value;
                OnPropertyChanged(nameof(qtyGoldenVerify));
            }
        }
        bool _is_change_ip;
        public bool isChangeIp {
            get { return _is_change_ip; }
            set {
                _is_change_ip = value;
                OnPropertyChanged(nameof(isChangeIp));
            }
        }
        bool _is_run_testtree;
        public bool isRunTesttree {
            get { return _is_run_testtree; }
            set {
                _is_run_testtree = value;
                OnPropertyChanged(nameof(isRunTesttree));
            }
        }
        string _remain;
        public string qty_remain {
            get { return _remain; }
            set {
                _remain = value;
                OnPropertyChanged(nameof(qty_remain));
            }
        }
        string _att;
        public string golden_attenuation {
            get { return _att; }
            set {
                _att = value;
                OnPropertyChanged(nameof(golden_attenuation));
            }
        }
        string _veri;
        public string golden_verification {
            get { return _veri; }
            set {
                _veri = value;
                OnPropertyChanged(nameof(golden_verification));
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
