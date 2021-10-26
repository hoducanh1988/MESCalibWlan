using EW30CX.Asset.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using EW30CX.Asset.IO;

namespace EW30CX.Asset.Custom {
    public class StationInfo : INotifyPropertyChanged {

        public StationInfo() {
            qty_remain = "0";
            golden_attenuation = golden_verification = "";
        }
        
        public void remain_reset() {
            qty_remain = myGlobal.settingviewmodel.SM.cycleMeasureAtt;
        }
        public void remain_down() {
            if (qty_remain != "0") qty_remain = $"{int.Parse(qty_remain) - 1}";
            else qty_remain = "0";
            save_to_db();
        }
        public void set_golden_att(string mac) {
            golden_attenuation = mac;
            golden_verification = "";
            remain_reset();
            save_to_db();
        }
        public void set_golden_vri(string mac) {
            if (golden_verification.Contains(mac) == true) return;
            golden_verification += golden_verification == "" ?  $"{mac}" : $"::{mac}";
            save_to_db();
        }

        public void save_to_db() {
            DBHelper db = new DBHelper($"{AppDomain.CurrentDomain.BaseDirectory}References\\GetWeight.accdb");
            bool r = db.OpenConnection();
            if (!r) return;
            db.QueryDeleteOrUpdate("DELETE FROM tb_station_info");
            Thread.Sleep(100);
            db.InsertDataToTable<StationInfo>(new StationInfo() { qty_remain = this.qty_remain, golden_attenuation = this.golden_attenuation, golden_verification = this.golden_verification }, "tb_station_info");
            db.Close();
        }
        public void load_from_db() {
            DBHelper db = new DBHelper($"{AppDomain.CurrentDomain.BaseDirectory}References\\GetWeight.accdb");
            bool r = db.OpenConnection();
            if (!r) return;
            var si = db.QueryDataReturnObject<StationInfo>("SELECT * FROM tb_station_info");
            this.qty_remain = si == null ? "0" : si.qty_remain;
            this.golden_attenuation = si == null ? "" : si.golden_attenuation;
            this.golden_verification = si == null ? "" : si.golden_verification;
            db.Close();
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
