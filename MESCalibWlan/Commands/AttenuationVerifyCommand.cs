using EW30SX.Asset.Global;
using EW30SX.Asset.IO;
using EW30SX.Models;
using EW30SX.ViewModels;
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
using EW30SX.Asset.Custom;

namespace EW30SX.Commands {
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
                r = run_test_tree(testing, setting); if (!r) goto END;
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
                    myGlobal.stationinfo.set_golden_vri(testing.macWan);
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
            r = x == 1;

            myGlobal.qsprHelper.close_Test_Tree();
            t.logSystem += $"\n...Kết quả = {r}\n";
            t.calibWlanResult = r ? "Passed" : "Failed";
            st.Stop();
            t.calibWlanElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool extract_qspr_log(AttenuationModel t) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"\n{DateTime.Now}, Đọc dữ liệu QSPR của golden {t.macWan}:\n";
            t.extractQsprResult = "Waiting...";

            try {
                string log_content = t.logQSPR;

                if (log_content == null || log_content.Length == 0) goto END;
                string[] buffer = log_content.Split('\n');

                myGlobal.goldenTestResults = new List<TestFrequencyInfo>();
                int count = 0;
                int max_count = buffer.Length - 1;
                string result_channel = "";
                const string STRING_START = "Test started: WlanTxVerifyPowerTest at:";
                const string STRING_END = "Test finished: WlanTxVerifyPowerTest with result:";
                bool add_flag = false;

            RE:
                string data_line = buffer[count];
                if (data_line.ToLower().Contains(STRING_START.ToLower())) { add_flag = true; }
                if (add_flag == true) {
                    result_channel += data_line + "\n";
                }
                if (data_line.ToLower().Contains(STRING_END.ToLower())) {
                    var item = _getWlanVerifyResultItem(result_channel);
                    t.logSystem += $"{item.ToString()}\n";
                    myGlobal.goldenTestResults.Add(item);
                    result_channel = "";
                    add_flag = false;
                }
                count++;
                if (count < max_count) goto RE;

                r = myGlobal.goldenTestResults.Count > 0;
            }
            catch (Exception ex) {
                t.logSystem += $"...{ex.ToString()}\n";
                goto END;
            }

        END:
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
        /// <param name="s"></param>
        /// <returns></returns>
        private bool save_path_loss(AttenuationModel t, SettingModel s) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"\n{DateTime.Now}, Lưu dữ liệu suy hao ra file pathloss {s.pathLossFile}:\n";
            t.updatePathlossResult = "Waiting...";

            myGlobal.pathlossInfos = _getPathLossInfoFromFile(s.pathLossFile);
            if (myGlobal.pathlossInfos == null || myGlobal.pathlossInfos.Count == 0) goto END;

            foreach (var item in myGlobal.powerDifferenceValues) {
                _updateLossInfo("bh0_lp", item.Frequency, item.powerDifferenceAnten1, myGlobal.pathlossInfos);
                _updateLossInfo("bh1_lp", item.Frequency, item.powerDifferenceAnten2, myGlobal.pathlossInfos);
            }

            _savePathLossFile(s.pathLossFile, myGlobal.pathlossInfos);

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

        TestFrequencyInfo _getWlanVerifyResultItem(string result_channel) {
            TestFrequencyInfo itemResult = new TestFrequencyInfo();
            string[] buffer = result_channel.Split('\n');
            foreach (var data_line in buffer) {
                //get average power
                if (data_line.ToLower().Contains("Average Power:".ToLower())) {
                    string s = data_line.ToLower();
                    string[] bff = s.Split(new string[] { "average power:" }, StringSplitOptions.None);
                    string pw_str = bff[1].Split(new string[] { "dbm" }, StringSplitOptions.None)[0].Trim();
                    double pw_double;
                    bool r = double.TryParse(pw_str, out pw_double);
                    if (r) itemResult.averagePowers.Add(pw_double);
                }
                //get frequency && antenna
                if (data_line.ToLower().Contains("channel") && data_line.ToLower().Contains("wlan_chain")) {
                    string s = data_line.ToLower();
                    int idx = s.IndexOf("channel");
                    string freq = s.Substring(idx + 9, 4);
                    idx = s.IndexOf("wlan_chain");
                    string anten = s.Substring(idx + 11, 1);
                    itemResult.Antenna = anten;
                    itemResult.Frequency = freq;
                }
            }
            return itemResult;
        }


        double _getWlanAveragePower(string freq, string anten, List<TestFrequencyInfo> wlanTestResults) {
            try {
                var items = wlanTestResults.Where(x => x.Frequency.Equals(freq) && x.Antenna.Equals(anten)).Select(x => x.averagePowers);
                List<double> pw_doubles = new List<double>();
                foreach (var item in items) pw_doubles.Add(item.Average());
                return Math.Round(pw_doubles.Average(), 4);
            }
            catch {
                return double.MaxValue;
            }
        }

        List<PathlossFrequencyInfo> _getPathLossInfoFromFile(string file_path_loss) {
            if (System.IO.File.Exists(file_path_loss) == false) return null;
            string[] lines = File.ReadAllLines(file_path_loss);

            List<PathlossFrequencyInfo> lossInfos = new List<PathlossFrequencyInfo>();
            for (int i = 0; i < lines.Length; i++) {
                string line = lines[i].ToLower();
                if (line.Contains("<frequency>")) {
                    PathlossFrequencyInfo lossinfo = new PathlossFrequencyInfo();
                    lossinfo.lineNumber = i;
                    lossinfo.Frequency = line.Replace("<frequency>", "").Replace("</frequency>", "").Trim();
                    lossinfo.lossValue = double.Parse(lines[i + 1].ToLower().Replace("<value>", "").Replace("</value>", "").Trim());

                    for (int k = i; k >= 0; k--) {
                        if (lines[k].ToLower().Contains("<pathname>")) {
                            lossinfo.bhName = lines[k].ToLower().Replace("<pathname>", "").Replace("</pathname>", "").Trim();
                            break;
                        }
                    }
                    lossInfos.Add(lossinfo);
                }
            }
            return lossInfos;
        }


        bool _updateLossInfo(string bh_name, string center_frequency, double diff_value, List<PathlossFrequencyInfo> pathlossInfo) {
            //update center frequency
            double center_loss_value = 0;
            foreach (var item in pathlossInfo) {
                if (item.Frequency.Equals(center_frequency) && item.bhName.Equals(bh_name)) {
                    item.lossValue = item.lossValue + diff_value;
                    center_loss_value = item.lossValue;
                    break;
                }
            }
            if (center_loss_value == 0) return true;

            //update side frequency
            string lower_freq = (int.Parse(center_frequency) - 10).ToString();
            string upper_freq = (int.Parse(center_frequency) + 10).ToString();
            bool r1 = false;
            bool r2 = false;

            foreach (var item in pathlossInfo) {
                if (item.Frequency.Equals(lower_freq) && item.bhName.Equals(bh_name)) {
                    item.lossValue = center_loss_value;
                    r1 = true;
                }
                if (item.Frequency.Equals(upper_freq) && item.bhName.Equals(bh_name)) {
                    item.lossValue = center_loss_value;
                    r2 = true;
                }
                if (r1 == true && r2 == true) break;
            }

            return true;
        }


        bool _savePathLossFile(string file_path_loss, List<PathlossFrequencyInfo> pathlossInfo) {
            if (System.IO.File.Exists(file_path_loss) == false) return false;
            string[] buffer = File.ReadAllLines(file_path_loss);

            foreach (var item in pathlossInfo) {
                //        <Value>8.0299</Value>
                string s = buffer[item.lineNumber + 1].ToLower();
                string[] bff = s.Split(new string[] { "value" }, StringSplitOptions.None);
                buffer[item.lineNumber + 1] = string.Format("{0}Value>{1}</Value{2}", bff[0], item.lossValue, bff[2]);
            }

            File.WriteAllLines(file_path_loss, buffer);
            //System.Diagnostics.Process.Start(file_path_loss);

            return true;
        }


        #endregion
    }
}
