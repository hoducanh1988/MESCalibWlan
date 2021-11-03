using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EW30SX.Asset.IO
{
    public class LogHelper <T> where T : class, new() {

        public enum log_type { Calibration, Attenuation, Verification };

        T testing = null;
        string dir = "";
        string mac = "";
        string result = "";
        log_type l_type;

        public LogHelper(T t, log_type _type) {
            testing = t;
            l_type = _type;
            mac = testing.GetType().GetProperty("macWan").GetValue(testing, null).ToString();
            result = testing.GetType().GetProperty("totalResult").GetValue(testing, null).ToString();

            dir = $"{AppDomain.CurrentDomain.BaseDirectory}Log_{l_type.ToString()}\\{DateTime.Now.ToString("yyyy-MM-dd")}\\{string.Format("{0}_{1}_{2}", mac, DateTime.Now.ToString("HHmmss"), result)}";
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);
            
        }


        public bool save_all_log() {
            bool r = false;
            r = save_log_dut(); if (!r) goto END;
            r = save_log_qspr(); if (!r) goto END;
            r = save_log_system(); if (!r) goto END;
            r = save_log_total(); if (!r) goto END;

            END:
            return r;
        }

        bool save_log_system() {
            try {
                
                string log_content = testing.GetType().GetProperty("logSystem").GetValue(testing, null).ToString();
                string d = dir;
                string f = $"{d}\\{mac}_logsystem_{result}.txt";
                File.WriteAllText(f, log_content);
                return true;
            } catch { return false; }
        }


        bool save_log_dut() {
            try {
                string log_content = testing.GetType().GetProperty("logDUT").GetValue(testing, null).ToString();
                string d = dir;
                
                string f = $"{d}\\{mac}_logdut_{result}.txt";
                File.WriteAllText(f, log_content);
                return true;
            }
            catch { return false; }
        }


        bool save_log_qspr() {
            try {
                string log_content = testing.GetType().GetProperty("logQSPR").GetValue(testing, null).ToString();
                string d = dir;
                string f = $"{d}\\{mac}_logqspr_{result}.txt";
                File.WriteAllText(f, log_content);
                return true;
            }
            catch { return false; }
        }

        bool save_log_total() {
            try {
                string title = "DATE_TIME_CREATED,MAC_WAN,RESULT,TOTAL_TIME,OPEN_COM,BOOT,LOGIN,GET_MAC,CHECK_GOLDEN,CHANGE_IP,PING_IP,FTM_MODE,CALIB";
                string log_content = $"{DateTime.Now.ToString()}," +
                                     $"{testing.GetType().GetProperty("macWan").GetValue(testing, null).ToString()}," +
                                     $"{testing.GetType().GetProperty("totalResult").GetValue(testing, null).ToString()}," +
                                     $"{testing.GetType().GetProperty("totalTime").GetValue(testing, null).ToString()}," +
                                     $"{testing.GetType().GetProperty("openComResult").GetValue(testing, null).ToString()}," +
                                     $"{testing.GetType().GetProperty("bootCompleteResult").GetValue(testing, null).ToString()}," +
                                     $"{testing.GetType().GetProperty("loginResult").GetValue(testing, null).ToString()}," +
                                     $"{testing.GetType().GetProperty("getMacWanResult").GetValue(testing, null).ToString()}," +
                                     $"{testing.GetType().GetProperty("checkGoldenResult").GetValue(testing, null).ToString()}," +
                                     $"{testing.GetType().GetProperty("changeIPResult").GetValue(testing, null).ToString()}," +
                                     $"{testing.GetType().GetProperty("pingDUTResult").GetValue(testing, null).ToString()}," +
                                     $"{testing.GetType().GetProperty("switchModeResult").GetValue(testing, null).ToString()}," +
                                     $"{testing.GetType().GetProperty("calibWlanResult").GetValue(testing, null).ToString()}";

                string f = $"{AppDomain.CurrentDomain.BaseDirectory}Log_{l_type.ToString()}\\{DateTime.Now.ToString("yyyy-MM-dd")}\\{DateTime.Now.ToString("yyyy-MM-dd")}.csv";
                if (File.Exists(f) == false) {
                    using (var sw = new StreamWriter(f, true, Encoding.Unicode)) {
                        sw.WriteLine(title);
                        sw.WriteLine(log_content);
                    }
                }
                else {
                    using (var sw = new StreamWriter(f, true, Encoding.Unicode)) {
                        sw.WriteLine(log_content);
                    }
                }
                return true;
            } catch { return false; }
        }


    }
}
