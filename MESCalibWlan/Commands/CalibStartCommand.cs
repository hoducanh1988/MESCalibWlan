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

namespace EW30SX.Commands {
    public class CalibStartCommand : ICommand {

        private CalibViewModel _cvm;
        private CalibModel testing = null;
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
                
                DUTHelper<CalibModel, SettingModel> dut = new DUTHelper<CalibModel, SettingModel>(testing, setting);
                testing.Init();
                testing.Waiting();

                r = open_comport(dut, testing, setting); //open comport
                r = is_boot_completed(dut, testing, setting); //wait dut boot completed
                r = login_to_dut(dut, testing, setting); //login to dut
                r = get_mac_wan(dut, testing, setting); //get mac wan
                r = change_ip(dut, testing, setting); //change ip dut
                r = ping_to_new_ip(testing, setting); //ping new ip dut
                r = switch_ftm_mode(dut, testing, setting); //switch to ftm mode
                r = run_test_tree(testing, setting); //run testtree calib

            END:
                if (dut != null) dut.Dispose();
                bool ___ = r ? testing.Passed() : testing.Failed();
                
            }));
            t.IsBackground = true;
            t.Start();

            //count total time
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

        private bool open_comport(DUTHelper<CalibModel, SettingModel> dut, CalibModel t, SettingModel s) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"{DateTime.Now}, Mở cổng {s.comPort},{s.baudRate}...\n";
            t.openComResult = "Waiting...";

            Thread.Sleep(3000);
            r = dut.Open();

            t.logSystem += $"...Kết quả = {r}\n";
            t.openComResult = r ? "Passed" : "Failed";
            st.Stop();
            t.openComElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }

        private bool is_boot_completed(DUTHelper<CalibModel, SettingModel> dut, CalibModel t, SettingModel s) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"{DateTime.Now}, Chờ sản phẩm khởi động xong...\n";
            t.bootCompleteResult = "Waiting...";

            Thread.Sleep(3000);
            r = dut.wait_Boot_Completed();

            t.logSystem += $"...Kết quả = {r}\n";
            t.bootCompleteResult = r ? "Passed" : "Failed";
            st.Stop();
            t.bootCompleteElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }

        private bool login_to_dut(DUTHelper<CalibModel, SettingModel> dut, CalibModel t, SettingModel s) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"{DateTime.Now}, Đăng nhập vào sản phẩm...\n";
            t.loginResult = "Waiting...";

            Thread.Sleep(3000);
            r = dut.Login();

            t.logSystem += $"...Kết quả = {r}\n";
            t.loginResult = r ? "Passed" : "Failed";
            st.Stop();
            t.loginElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }

        private bool get_mac_wan(DUTHelper<CalibModel, SettingModel> dut, CalibModel t, SettingModel s) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"{DateTime.Now}, Đọc mac wan từ sản phẩm...\n";
            t.getMacWanResult = "Waiting...";

            Thread.Sleep(3000);
            t.macWan = dut.get_Mac_Wan();
            r = t.macWan != null;

            t.logSystem += $"...Kết quả = {r}\n";
            t.getMacWanResult = r ? "Passed" : "Failed";
            st.Stop();
            t.getMacWanElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }

        private bool change_ip(DUTHelper<CalibModel, SettingModel> dut, CalibModel t, SettingModel s) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"{DateTime.Now}, Đổi ip sản phẩm thành {s.ipDUT}...\n";
            t.changeIPResult = "Waiting...";

            Thread.Sleep(3000);
            r = dut.set_Ethernet_IP();

            t.logSystem += $"...Kết quả = {r}\n";
            t.changeIPResult = r ? "Passed" : "Failed";
            st.Stop();
            t.changeIPElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }

        private bool ping_to_new_ip(CalibModel t, SettingModel s) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"{DateTime.Now}, Ping tới địa chỉ ip {s.ipDUT}...\n";
            t.pingDUTResult = "Waiting...";



            t.logSystem += $"...Kết quả = {r}\n";
            t.pingDUTResult = r ? "Passed" : "Failed";
            st.Stop();
            t.pingDUTElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }

        private bool switch_ftm_mode(DUTHelper<CalibModel, SettingModel> dut, CalibModel t, SettingModel s) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"{DateTime.Now}, Chuyển mode dut sang FTM...\n";
            t.switchModeResult = "Waiting...";

            Thread.Sleep(3000);
            r = dut.set_FTM_Mode();

            t.logSystem += $"...Kết quả = {r}\n";
            t.switchModeResult = r ? "Passed" : "Failed";
            st.Stop();
            t.switchModeElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }

        private bool run_test_tree(CalibModel t, SettingModel s) {
            Stopwatch st = new Stopwatch();
            st.Start();
            bool r = false;
            t.logSystem += $"{DateTime.Now}, Run testtree calib {s.calibTesttreeFile}...\n";
            t.calibWlanResult = "Waiting...";



            t.logSystem += $"...Kết quả = {r}\n";
            t.calibWlanResult = r ? "Passed" : "Failed";
            st.Stop();
            t.calibWlanElapsedTime = st.ElapsedMilliseconds.ToString();
            return r;
        }

        #endregion

    }
}
