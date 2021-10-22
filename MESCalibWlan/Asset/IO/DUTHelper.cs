using EW30SX.Asset.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace EW30SX.Asset.IO {

    public class DUTHelper<T, S> : IDisposable where T : class, new() where S : class, new() {

        Serial<T> mesh = null;
        T testing = null;
        S setting = null;
        int retry_time = 3;

        public DUTHelper(T t, S s) {
            this.testing = t;
            this.setting = s;
            string port = this.setting.GetType().GetProperty("comPort").GetValue(this.setting, null).ToString();
            string baud = this.setting.GetType().GetProperty("baudRate").GetValue(this.setting, null).ToString();
            mesh = new Serial<T>(t, port, baud);
        }

        public bool isConnected() {
            return mesh.IsConnected();
        }

        public bool Open() {
            return isConnected();
        }

        public bool wait_Boot_Completed() {
            return true;
        }

        public bool Login() {
            if (mesh.IsConnected() == false) return false;
            int count = 0;
            bool r = false;

        RE:
            count++;
            try { r = mesh.Query("", 1, "root@VNPT:/#", "root@VNPT:~#"); }
            catch { r = false; }

            if (!r) {
                if (count < retry_time) goto RE;
                else goto END;
            }

        END:
            return r;
        }

        public string get_Mac_Wan() {
            if (mesh.IsConnected() == false) return null;
            mesh.WriteLine("");
            Thread.Sleep(100);
            mesh.Read();
            int count = 0;

        RE:
            count++;
            mesh.WriteLine("cat /sys/class/net/eth0/address");
            Thread.Sleep(300);
            string data = mesh.Read();
            if (data == null || data == "" || data == string.Empty) data = "null";
            if (data == "null") {
                if (count < retry_time) goto RE;
                else return "";
            }
            data = data.ToLower();
            data = data.Replace("cat /sys/class/net/eth0/address", "").Replace("\r", "").Replace("\n", "").Replace(":", "").Replace("root@vnpt:~#", "");
            data = data.Substring(0, 12).ToUpper();
            bool r = Regex.IsMatch(data, "^[0-9,A-F]{12}$", RegexOptions.IgnoreCase);
            if (!r) {
                if (count < retry_time) goto RE;
                else return null;
            }

            return data;
        }

        public bool set_Ethernet_IP() {
            if (mesh.IsConnected() == false) return false;
            string ip_dut = this.setting.GetType().GetProperty("ipDUT").GetValue(this.setting, null).ToString();
            mesh.WriteLine("");
            Thread.Sleep(100);
            mesh.WriteLine($"ifconfig br-lan {ip_dut}");
            Thread.Sleep(100);
            mesh.WriteLine("");
            Thread.Sleep(100);
            mesh.WriteLine("netmask 255.255.255.0 up");
            Thread.Sleep(100);

            return true;
        }

        public bool set_FTM_Mode() {
            if (mesh.IsConnected() == false) return false;
            string ip_pc = this.setting.GetType().GetProperty("ipPC").GetValue(this.setting, null).ToString();
            mesh.WriteLine("wifi down");
            mesh.WriteLine("rmmod wifi_3_0");
            mesh.WriteLine("rmmod qca_ol");
            mesh.WriteLine("insmod qca_ol testmode=1");
            mesh.WriteLine("insmod wifi_3_0");
            mesh.WriteLine("insmod diagchar");
            mesh.WriteLine($"diag_socket_app -a {ip_pc} &");
            mesh.WriteLine("/etc/init.d/ftm start");
            mesh.WriteLine("ftm -n -dd &");
            return true;
        }
        
        public void Dispose() {
            if (mesh != null) mesh.Close();
        }
    }
}
