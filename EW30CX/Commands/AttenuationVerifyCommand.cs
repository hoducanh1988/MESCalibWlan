using EW30CX.Asset.Global;
using EW30CX.Asset.IO;
using EW30CX.Models;
using EW30CX.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilityPack.Converter;
using UtilityPack.IO;
using System.IO;
using EW30CX.Asset.Custom;

namespace EW30CX.Commands {
    public class AttenuationVerifyCommand : ICommand {

        private AttenuationViewModel _avm;
        private CalibModel cm = null;
        private AttenuationModel testing = null;
        private SettingModel setting = null;

        public AttenuationVerifyCommand(AttenuationViewModel avm) {
            _avm = avm;
            testing = _avm.AM;
            setting = _avm.SM;
        }

        #region ICommand Members

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        //enable button save setting
        public bool CanExecute(object parameter) {
            return true;
        }

        //save setting
        public void Execute(object parameter) {

            Thread t = new Thread(new ThreadStart(() => {
                bool r = false;

                //check QPST app running or not
                r = WindowProcess.isProcessRunning("QPSTServer");
                if (!r) {
                    System.Windows.MessageBox.Show(@"Vui lòng chạy file QPSTServer.exe trong đường dẫn C:\Program Files (x86)\Qualcomm\QPST\bin trước khi calib sản phẩm.",
                        "QPST ERROR",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Error);
                    return;
                }

                DUTHelper<AttenuationModel, SettingModel> dut = new DUTHelper<AttenuationModel, SettingModel>(testing, setting);
                testing.Init();
                testing.Waiting(true);

                //open comport
                r = open_comport(dut, testing, setting); if (!r) goto END;
                //wait dut boot completed
                r = is_boot_completed(dut, testing, setting); if (!r) goto END;
                //login to dut
                r = login_to_dut(dut, testing, setting); if (!r) goto END;
                //get mac wan
                r = get_mac_wan(dut, testing, setting); if (!r) goto END;
                //check mac golden
                r = is_mac_golden(testing); if (!r) goto END;
                //get golden data
                r = get_golden_data(testing); if (!r) goto END;

                if (setting.isChangeIp) {
                    //change ip dut
                    r = change_ip(dut, testing, setting); if (!r) goto END;
                    //ping new ip dut
                    r = ping_to_new_ip(testing, setting); if (!r) goto END;
                }

                //switch to ftm mode
                r = switch_ftm_mode(dut, testing, setting); if (!r) goto END;
                //run testtree calib
                r = run_test_tree(testing, setting);
                //extract qspr log
                r = extract_qspr_log(testing); if (!r) goto END;
                //calculate golden data
                r = calculate_golden_data(testing); if (!r) goto END;
                //đánh giá dữ liệu suy hao
                r = judge_Golden(testing);

            END:
                bool ___ = r ? testing.Passed() : testing.Failed();
                if (dut != null) dut.Dispose();
                LogHelper<AttenuationModel> log = new LogHelper<AttenuationModel>(testing, LogHelper<AttenuationModel>.log_type.Verification);
                log.save_all_log();
                if (r) {
                    setting.golden_verification += string.IsNullOrEmpty(setting.golden_verification) ? testing.macWan : $"::{testing.macWan}";
                    XmlHelper<SettingModel>.ToXmlFile(setting, AppDomain.CurrentDomain.BaseDirectory + "setting.xml");
                }
            }));
            t.IsBackground = true;
            t.Start();

            //count time
            Thread z = new Thread(new ThreadStart(() => {
                int count = 0;
            RE:
                count++;
                Thread.Sleep(500);
                if (t.IsAlive == true) {
                    testing.totalTime = myConverter.intToTimeSpan(count * 500);
                    goto RE;
                }
            }));
            z.IsBackground = true;
            z.Start();

        }

        #endregion

        #region calib

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dut"></param>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool open_comport(DUTHelper<AttenuationModel, SettingModel> dut, AttenuationModel t, SettingModel s) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"\n{DateTime.Now}, Mở cổng {s.comPort},{s.baudRate}:\n";
            t.openComResult = "Waiting...";

            r = dut.Open();

            t.logSystem += $"...Kết quả = {r}\n";
            t.openComResult = r ? "Passed" : "Failed";
            st.Stop();
            t.openComElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dut"></param>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool is_boot_completed(DUTHelper<AttenuationModel, SettingModel> dut, AttenuationModel t, SettingModel s) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"\n{DateTime.Now}, Chờ sản phẩm khởi động xong:\n";
            t.bootCompleteResult = "Waiting...";

            r = dut.wait_Boot_Completed();

            t.logSystem += $"\n...Kết quả = {r}\n";
            t.bootCompleteResult = r ? "Passed" : "Failed";
            st.Stop();
            t.bootCompleteElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dut"></param>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool login_to_dut(DUTHelper<AttenuationModel, SettingModel> dut, AttenuationModel t, SettingModel s) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"\n{DateTime.Now}, Đăng nhập vào sản phẩm:\n";
            t.loginResult = "Waiting...";

            r = dut.Login();

            t.logSystem += $"...Kết quả = {r}\n";
            t.loginResult = r ? "Passed" : "Failed";
            st.Stop();
            t.loginElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dut"></param>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool get_mac_wan(DUTHelper<AttenuationModel, SettingModel> dut, AttenuationModel t, SettingModel s) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"\n{DateTime.Now}, Đọc mac wan từ sản phẩm:\n";
            t.getMacWanResult = "Waiting...";

            t.macWan = dut.get_Mac_Wan();
            r = t.macWan != null;
            t.logSystem += $"...mac wan = {t.macWan}\n";
            if (t.macWan != null) {
                r = !s.golden_verification.ToUpper().Contains(t.macWan.ToUpper());
                t.logSystem += $"...sản phẩm đã verify suy hao rồi.\n";
            }

           
            t.logSystem += $"...Kết quả = {r}\n";
            t.getMacWanResult = r ? "Passed" : "Failed";
            st.Stop();
            t.getMacWanElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool is_mac_golden(AttenuationModel t) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"\n{DateTime.Now}, Check mac sản phẩm có phải là golden hay không:\n";
            t.checkGoldenResult = "Waiting...";

            var fs = new DirectoryInfo($"{AppDomain.CurrentDomain.BaseDirectory}Goldens").GetFiles();
            if (fs != null) {
                var buffer = fs.Select(x => x.Name);
                t.logSystem += $"...mac wan = {t.macWan}\n";
                t.logSystem += "...Golden:\n";
                foreach (var f in buffer) t.logSystem += $"...{f}\n";
                foreach (var f in buffer) {
                    r = f.ToUpper().Contains(t.macWan.ToUpper().Trim().Replace("\n", "").Replace("\r", ""));
                    if (r) break;
                }
            }

            t.logSystem += $"...Kết quả = {r}\n";
            t.checkGoldenResult = r ? "Passed" : "Failed";
            st.Stop();
            t.checkGoldenElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool get_golden_data(AttenuationModel t) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"\n{DateTime.Now}, Đọc dữ liệu tiêu chuẩn của mạch golden {t.macWan}:\n";
            t.getGoldenDataResult = "Waiting...";

            string dir = AppDomain.CurrentDomain.BaseDirectory + "Goldens";
            string golden_file = string.Format("GOLDEN_{0}.txt", t.macWan.ToUpper());
            string golden_file_full_name = Path.Combine(dir, golden_file);
            t.logSystem += $"...{golden_file}\n";

            if (!File.Exists(golden_file_full_name)) goto END;

            string golden_content = File.ReadAllText(golden_file_full_name);
            if (golden_content == null || golden_content.Length == 0) goto END;

            string[] buffer = golden_content.Split('\n');
            myGlobal.goldenStandardValues = new List<GoldenFrequencyInfo>();
            int count = 0;
            int max_count = buffer.Length - 1;

        RE:
            count++;
            string data_line = buffer[count];
            if (data_line != null && data_line.Length > 0) {
                string[] bff = data_line.Split(',');
                GoldenFrequencyInfo item = new GoldenFrequencyInfo();
                item.Frequency = bff[0];
                item.powerAntenna1 = double.Parse(bff[1]);
                item.powerAntenna2 = double.Parse(bff[2]);
                t.logSystem += $"...{item.ToString()}\n";
                myGlobal.goldenStandardValues.Add(item);
            }
            if (count < max_count) goto RE;
            r = myGlobal.goldenStandardValues.Count > 0;

        END:
            t.logSystem += $"...Kết quả = {r}\n";
            t.getGoldenDataResult = r ? "Passed" : "Failed";
            st.Stop();
            t.getGoldenDataElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dut"></param>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool change_ip(DUTHelper<AttenuationModel, SettingModel> dut, AttenuationModel t, SettingModel s) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"\n{DateTime.Now}, Đổi ip sản phẩm thành {s.ipDUT}:\n";
            t.changeIPResult = "Waiting...";

            r = dut.set_Ethernet_IP();

            t.logSystem += $"...Kết quả = {r}\n";
            t.changeIPResult = r ? "Passed" : "Failed";
            st.Stop();
            t.changeIPElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool ping_to_new_ip(AttenuationModel t, SettingModel s) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"\n{DateTime.Now}, Ping tới địa chỉ ip {s.ipDUT}:\n";
            t.logSystem += $"...Pinging {s.ipDUT} with 32 bytes of data:\n";
            t.pingDUTResult = "Waiting...";
            int count = 0;

        RE:
            count++;
            Thread.Sleep(1000);
            r = pingNetwork(s.ipDUT);
            t.logSystem += r ? $"...Reply from {s.ipDUT}: bytes=32 time<1ms TTL=64\n" : "...Request timed out.\n";
            if (!r) {
                if (count < int.Parse(s.timeOutChangeIP)) {
                    goto RE;
                }
            }

            t.logSystem += $"...Kết quả = {r}\n";
            t.pingDUTResult = r ? "Passed" : "Failed";
            st.Stop();
            t.pingDUTElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dut"></param>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool switch_ftm_mode(DUTHelper<AttenuationModel, SettingModel> dut, AttenuationModel t, SettingModel s) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"\n{DateTime.Now}, Chuyển mode dut sang FTM:\n";
            t.switchModeResult = "Waiting...";

            Thread z = new Thread(new ThreadStart(() => {
                int count = 0;
            RE:
                Thread.Sleep(500);
                count++;
                bool a = t.switchModeResult.Equals("Waiting...");
                if (a) {
                    t.logSystem += $"{count}..";
                    goto RE;
                }
            }));
            z.IsBackground = true;
            z.Start();

            r = dut.set_FTM_Mode();

            t.logSystem += $"\n...Kết quả = {r}\n";
            t.switchModeResult = r ? "Passed" : "Failed";
            st.Stop();
            t.switchModeElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool run_test_tree(AttenuationModel t, SettingModel s) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"\n{DateTime.Now}, Run testtree calib {s.attTesttreeFile}:\n";
            t.calibWlanResult = "Waiting...";

            Thread z = new Thread(new ThreadStart(() => {
                int count = 0;
            R:
                Thread.Sleep(500);
                count++;
                bool a = t.calibWlanResult.Equals("Waiting...");
                if (a) {
                    t.logSystem += $"{count}..";
                    goto R;
                }
            }));
            z.IsBackground = true;
            z.Start();

            if (myGlobal.qsprHelper == null) myGlobal.qsprHelper = new QSPRHelper<CalibModel, AttenuationModel, SettingModel>();
            myGlobal.qsprHelper.setObject(cm, testing, setting, false);
            myGlobal.qsprHelper.run_Test_Tree(setting.attTesttreeFile);
            int c = 0;
        RE:
            c++;
            int x = myGlobal.qsprHelper.tree_Execution_Status();
            r = x == 1 || x == 2;
            if (!r) {
                Thread.Sleep(1000);
                goto RE;
            }
            r = x == 1 || x == 2;

            myGlobal.qsprHelper.close_Test_Tree();
            t.logSystem += $"\n...Kết quả = {r}\n";
            t.calibWlanResult = r ? "Passed" : "Failed";
            st.Stop();
            t.calibWlanElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }


        private bool extract_qspr_log(AttenuationModel t) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"\n{DateTime.Now}, Đọc dữ liệu QSPR của golden {t.macWan}:\n";
            t.extractQsprResult = "Waiting...";

            try {
                string log_content = t.logQSPR;

                if (log_content == null || log_content.Length == 0) goto END;
                LogQSPRHelper log_qspr = new LogQSPRHelper();
                myGlobal.goldenTestResults = log_qspr.Extract(log_content);
                r = !(myGlobal.goldenTestResults == null || myGlobal.goldenTestResults.Count == 0);
            }
            catch (Exception ex) {
                t.logSystem += $"...{ex.ToString()}\n";
                goto END;
            }

        END:
            if (r) foreach (var item in myGlobal.goldenTestResults) t.logSystem += item.ToString() + "\n";
            t.logSystem += $"...Kết quả = {r}\n";
            t.extractQsprResult = r ? "Passed" : "Failed";
            st.Stop();
            t.extractQsprElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool calculate_golden_data(AttenuationModel t) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"\n{DateTime.Now}, Tính độ lệch công suất của golden {t.macWan}:\n";
            t.calcGoldenResult = "Waiting...";

            if (myGlobal.goldenStandardValues == null || myGlobal.goldenStandardValues.Count == 0) goto END;
            if (myGlobal.goldenTestResults == null || myGlobal.goldenTestResults.Count == 0) goto END;

            myGlobal.powerDifferenceValues = new List<PowerDifferenceInfo>();
            foreach (var item in myGlobal.goldenStandardValues) {
                double pw_at1 = _getWlanAveragePower(item.Frequency, "1", myGlobal.goldenTestResults);
                double pw_at2 = _getWlanAveragePower(item.Frequency, "2", myGlobal.goldenTestResults);
                if (pw_at1 != double.MaxValue && pw_at2 != double.MaxValue) {
                    PowerDifferenceInfo power_diff = new PowerDifferenceInfo() { Frequency = item.Frequency, powerDifferenceAnten1 = Math.Round(item.powerAntenna1 - pw_at1, 4), powerDifferenceAnten2 = Math.Round(item.powerAntenna2 - pw_at2, 4) };
                    myGlobal.powerDifferenceValues.Add(power_diff);
                }
            }

            

            r = myGlobal.powerDifferenceValues.Count > 0;

        END:
            if (r) foreach (var item in myGlobal.powerDifferenceValues) t.logSystem += item.ToString() + "\n";
            t.logSystem += $"...Kết quả = {r}\n";
            t.calcGoldenResult = r ? "Passed" : "Failed";
            st.Stop();
            t.calcGoldenElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool judge_Golden(AttenuationModel t) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"\n{DateTime.Now}, Phán định suy hao trạm calib:\n";
            t.updatePathlossResult = "Waiting...";

            if (myGlobal.powerDifferenceValues == null || myGlobal.powerDifferenceValues.Count == 0) goto END;

            r = true;
            foreach (var item in myGlobal.powerDifferenceValues) {
                if (!item.resultTotal.Equals("PASS")) { r = false; break; }
            }

        END:
            t.logSystem += $"...Kết quả = {r}\n";
            t.updatePathlossResult = r ? "Passed" : "Failed";
            st.Stop();
            t.updatePathlossElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private bool pingNetwork(string ip) {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            // Use the default Ttl value which is 128, 
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted. 
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 60;

            try {
                PingReply reply = pingSender.Send(ip, timeout, buffer, options);
                if (reply.Status == IPStatus.Success) {
                    return true;
                }
                else {
                    return false;
                }
            }
            catch {
                return false;
            }
        }


        double _getWlanAveragePower(string freq, string anten, List<TestFrequencyInfo> wlanTestResults) {
            try {
                var items = wlanTestResults.Where(x => x.Frequency.Equals(freq) && x.Antenna.Equals(anten)).ToList();
                return items[0].averagePower;
            }
            catch {
                return double.MaxValue;
            }
        }

        #endregion
    }
}
