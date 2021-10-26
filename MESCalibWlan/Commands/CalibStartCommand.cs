using EW30SX.Asset.IO;
using EW30SX.Models;
using EW30SX.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using UtilityPack.Converter;
using System.Diagnostics;
using System.Net.NetworkInformation;
using EW30SX.Asset.Global;
using UtilityPack.IO;
using System.IO;

namespace EW30SX.Commands {
    public class CalibStartCommand : ICommand {

        private CalibViewModel _cvm;
        private CalibModel testing = null;
        private AttenuationModel am = null;
        private SettingModel setting = null;

        public CalibStartCommand(CalibViewModel cvm) {
            _cvm = cvm;
            testing = _cvm.CM;
            setting = _cvm.SM;
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

                DUTHelper<CalibModel, SettingModel> dut = new DUTHelper<CalibModel, SettingModel>(testing, setting);
                testing.Init();
                testing.Waiting();
                
                //open comport
                r = open_comport(dut, testing, setting); if (!r) goto END;
                //wait dut boot completed
                r = is_boot_completed(dut, testing, setting); if (!r) goto END;
                //login to dut
                r = login_to_dut(dut, testing, setting); if (!r) goto END;
                //get mac wan
                r = get_mac_wan(dut, testing, setting); if (!r) goto END;
                //check mac is golden or not
                r = is_mac_golden(testing); if (r) goto END;

                if (setting.isChangeIp) {
                    //change ip dut
                    r = change_ip(dut, testing, setting); if (!r) goto END;
                    //ping new ip dut
                    r = ping_to_new_ip(testing, setting); if (!r) goto END;
                }

                //switch to ftm mode
                r = switch_ftm_mode(dut, testing, setting); if (!r) goto END;

                if (setting.isRunTesttree) {
                    //run testtree calib
                    r = run_test_tree(testing, setting); if (!r) goto END;
                }

            END:
                bool ___ = r ? testing.Passed() : testing.Failed();
                if (dut != null) dut.Dispose();
                LogHelper<CalibModel> log = new LogHelper<CalibModel>(testing);
                log.save_all_log();
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
        private bool open_comport(DUTHelper<CalibModel, SettingModel> dut, CalibModel t, SettingModel s) {
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
        private bool is_boot_completed(DUTHelper<CalibModel, SettingModel> dut, CalibModel t, SettingModel s) {
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
        private bool login_to_dut(DUTHelper<CalibModel, SettingModel> dut, CalibModel t, SettingModel s) {
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
        private bool get_mac_wan(DUTHelper<CalibModel, SettingModel> dut, CalibModel t, SettingModel s) {
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
        private bool is_mac_golden(CalibModel t) {
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
                foreach (var f in buffer) t.logSystem += $"{f}\n";
                foreach (var f in buffer) {
                    r = f.ToLower().Contains(t.macWan.ToLower());
                    if (r) break;
                }
            }

            t.logSystem += $"...Kết quả = {r}\n";
            t.checkGoldenResult = r == false ? "Passed" : "Failed";
            st.Stop();
            t.checkGoldenElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dut"></param>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool change_ip(DUTHelper<CalibModel, SettingModel> dut, CalibModel t, SettingModel s) {
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
        private bool ping_to_new_ip(CalibModel t, SettingModel s) {
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
        private bool switch_ftm_mode(DUTHelper<CalibModel, SettingModel> dut, CalibModel t, SettingModel s) {
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
        private bool run_test_tree(CalibModel t, SettingModel s) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"\n{DateTime.Now}, Run testtree calib {s.calibTesttreeFile}:\n";
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
            myGlobal.qsprHelper.setObject(testing, am, setting, true);
            myGlobal.qsprHelper.run_Test_Tree(setting.calibTesttreeFile);
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

        #endregion

    }
}
