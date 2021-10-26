using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EW30CX.Asset.Protocol {
    public class Serial<T> where T : class, new() {

        SerialPort serialport = null;
        string _PortName = "";
        string _BaudRate = "";
        T t = null;

        ~Serial() {
            this.Close();
        }

        public Serial(T _t, string _portname, string _baud_rate) {
            this.t = _t;
            this._PortName = _portname;
            this._BaudRate = _baud_rate;

            int count = 0;
            bool result = false;
        REP:
            count++;
            try {
                this.serialport = new SerialPort();
                this.serialport.PortName = _PortName;
                this.serialport.BaudRate = int.Parse(this._BaudRate);
                this.serialport.DataBits = 8;
                this.serialport.Parity = Parity.None;
                this.serialport.StopBits = StopBits.One;
                this.serialport.Open();
                result = serialport.IsOpen;
            }
            catch {
                this.serialport.Close();
                result = false;
            }
            if (!result) { if (count < 3) { this.serialport.Close(); Thread.Sleep(100); goto REP; } }
        }

        public bool IsConnected() {
            return serialport.IsOpen;
        }

        public bool Write(string cmd) {
            if (serialport.IsOpen == false) return false;
            serialport.Write(cmd);
            return true;
        }

        public bool WriteLine(string cmd) {
            if (serialport.IsOpen == false) return false;
            serialport.WriteLine(cmd);
            return true;
        }

        public bool Query(string cmd, int timeout_sec, params string[] expected_datas) {
            if (serialport.IsOpen == false) return false;
            int delay_time = 100;
            int max_count = (timeout_sec * 1000) / delay_time;
            int count = 0;
            string data = "";
            bool r = false;

            //write dummy data
            this.WriteLine("");
            Thread.Sleep(delay_time);
            this.Read();

            this.WriteLine(cmd);
        RE:
            count++;
            Thread.Sleep(delay_time);
            data += this.Read();

            if (data == null) {
                if (count < max_count) goto RE;
                else goto END;
            }

            foreach (var ep in expected_datas) {
                r = data.ToLower().Contains(ep.ToLower());
                if (r) break;
            }

            if (r == false) {
                if (count < max_count) goto RE;
                else goto END;
            }

        END:
            return r;
        }

        public string Read() {
            if (serialport.IsOpen == false) return null;
            string data = serialport.ReadExisting();

            //set data to log dut
            PropertyInfo pi = t.GetType().GetProperty("logDUT");
            string s = (string)pi.GetValue(t, null);
            s += data;
            pi.SetValue(t, Convert.ChangeType(s, pi.PropertyType), null);

            return data;
        }

        public bool Close() {
            if (serialport != null && serialport.IsOpen) serialport.Close();
            return true;
        }

    }
}
