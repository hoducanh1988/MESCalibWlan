using EW30CX.Asset.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Reflection;

namespace EW30CX.Asset.IO {

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
            if (mesh.IsConnected() == false) return false;
            bool r = false;
            int delay_time = 500;
            int max_count = (int.Parse(this.setting.GetType().GetProperty("timeOutBoot").GetValue(this.setting, null).ToString()) * 1000) / delay_time;
            string expected_data = this.setting.GetType().GetProperty("logBootCompleted").GetValue(this.setting, null).ToString();
            int pause_max = (int.Parse(this.setting.GetType().GetProperty("timeWaitLogin").GetValue(this.setting, null).ToString()) * 1000) / delay_time;
            int count = 0, pause = 0;
            string data = "";
            PropertyInfo log_system = this.testing.GetType().GetProperty("logSystem");

        RE:
            count++;
            string s = log_system.GetValue(this.testing, null).ToString();
            s += $"{count}..";
            log_system.SetValue(this.testing, Convert.ChangeType(s, log_system.PropertyType), null);
            Thread.Sleep(delay_time);
            data += mesh.Read();

            //check null
            r = !string.IsNullOrEmpty(data);
            if (!r) {
                if (count < max_count) {
                    pause++;
                    if (pause >= pause_max) {
                        r = mesh.Query("", 3, "root@VNPT:/#");
                        goto END;
                    }
                    else goto RE;
                }
                else goto END;
            }

            //check expected data
            pause = 0;
            r = data.ToLower().Contains(expected_data.ToLower());
            if (!r) {
                if (count < max_count) goto RE;
                else goto END;
            }

        END:
            return r;
        }

        public bool Login() {
            if (mesh.IsConnected() == false) return false;
            int count = 0;
            bool r = false;

        RE:
            count++;
            try { r = mesh.Query("", 1, "root@VNPT:/#"); }
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
            bool r = false;
            r = mesh.Query($"ifconfig br-lan {ip_dut}", 3, "root@VNPT:/#");
            if (!r) return false;
            r = mesh.Query("netmask 255.255.255.0 up", 3, "root@VNPT:/#");
            return r;
        }

        public bool set_FTM_Mode() {
            if (mesh.IsConnected() == false) return false;
            string ip_pc = this.setting.GetType().GetProperty("ipPC").GetValue(this.setting, null).ToString();
            bool r = false;
            r = mesh.Query("wifi down", 10, "root@VNPT:/#");
            if (!r) return false;
            r = mesh.Query("rmmod wifi_3_0", 10, "root@VNPT:/#");
            if (!r) return false;
            r = mesh.Query("rmmod qca_ol", 10, "root@VNPT:/#");
            if (!r) return false;
            r = mesh.Query("insmod qca_ol testmode=1", 10, "root@VNPT:/#");
            if (!r) return false;
            r = mesh.Query("insmod wifi_3_0", 30, "root@VNPT:/#");
            if (!r) return false;
            r = mesh.Query("insmod diagchar", 10, "root@VNPT:/#");
            if (!r) return false;
            r = mesh.Query($"diag_socket_app -a {ip_pc} &", 30, "diag_socket_log: Successful connect to address");
            if (!r) return false;
            r = mesh.Query("/etc/init.d/ftm start", 10, "root@VNPT:/#");
            if (!r) return false;
            r = mesh.Query("ftm -n -dd &", 10, "FTMDaemon: Diag_LSM_Init succesful");
            return r;
        }

        public void Dispose() {
            if (mesh != null) mesh.Close();
        }
    }
}
