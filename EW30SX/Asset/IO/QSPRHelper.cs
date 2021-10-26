using QC.QSPRSchedulerWrapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EW30SX.Asset.IO {
    public class QSPRHelper <T,U,S> where T : class, new() where S: class, new() where U :class, new() {

        QSPRScheduler _scheduler;
        QSPRTestTree _testTree;
        T testinfo_1;
        U testinfo_2;
        S settinginfo;
        bool is_T_not_U = false;
        string xmsg_name = "logQSPR";

        public QSPRHelper() {
            _scheduler = new QSPRScheduler();
            _scheduler.OnDebugMessage += new QSPRScheduler.OnDebugMessageEventHandler(_scheduler_OnDebugMessage);
            _scheduler.OnTestMessage += new QSPRScheduler.OnTestMessageEventHandler(_scheduler_OnTestMessage);
            _scheduler.LoadWorkspaceConfig(@"C:\Qualcomm\QSPR\QSPRConfigurations\Workspace.config");
        }

        public void setObject(T obj_t1, U obj_t2, S obj_s, bool is_t) {
            testinfo_1 = obj_t1;
            testinfo_2 = obj_t2;
            settinginfo = obj_s;
            is_T_not_U = is_t;
        }

        private void _scheduler_OnDebugMessage(string strWin, string strText, int traceLevel, bool NoEndOfLine) {
            UpdateStatusWindow(strWin + ": " + strText);
        }

        private void _scheduler_OnTestMessage(int messageType, QSPRTestMessage messageData) {
            switch ((TestMsgTypes)messageType) {

                case TestMsgTypes.ON_UNIT_START: {
                        string sn = messageData.GetValue(TestMsgItemNames.SN);
                        string testCount = messageData.GetValue(TestMsgItemNames.TEST_COUNT);
                        UpdateStatusWindow("Tree contains " + testCount + " tests and was started with serial number: " + sn);
                        break;
                    }
                case TestMsgTypes.ON_UNIT_END: {
                        string errorCode = messageData.GetValue(TestMsgItemNames.ERROR_CODE);
                        string testResult = messageData.GetValue(TestMsgItemNames.TESTRESULT);
                        UpdateStatusWindow("Tree finished with result: " + testResult + " and error code: " + errorCode);
                        break;
                    }
                case TestMsgTypes.ON_TEST_START: {
                        string testName = messageData.GetValue(TestMsgItemNames.TESTNAME);
                        string startTime = messageData.GetValue(TestMsgItemNames.START_TIME);
                        UpdateStatusWindow("Test started: " + testName + " at: " + startTime);
                        break;
                    }
                case TestMsgTypes.ON_TEST_RESULT: {
                        string testName = messageData.GetValue(TestMsgItemNames.TESTNAME);
                        string testResult = messageData.GetValue(TestMsgItemNames.TESTRESULT);
                        string loopInfo = messageData.GetValue(TestMsgItemNames.LOOP_DETAILS);
                        UpdateStatusWindow("Test finished: " + testName + " with result: " + testResult + " LOOP_DETAILS=" + loopInfo ?? "");

                        string[] msgItemNames = messageData.GetItemNames();

                        foreach (string itemName in msgItemNames) {
                            // if the item is an output parameter
                            if (itemName.StartsWith(TestMsgItemNames.OUTPUT_PARAM_PREFIX)) {
                                string outParamValue = messageData.GetValue(itemName);
                            }
                        }

                        break;
                    }
            }
        }

        private void UpdateStatusWindow(string msg) {
            PropertyInfo xmsg = is_T_not_U ? testinfo_1.GetType().GetProperty(xmsg_name) : testinfo_2.GetType().GetProperty(xmsg_name);
            string str_curr = "";
            if (is_T_not_U == true) str_curr = xmsg.GetValue(testinfo_1, null).ToString();
            else str_curr = xmsg.GetValue(testinfo_2, null).ToString();
            str_curr += string.Format("{0}\r\n", msg);
            if (is_T_not_U) xmsg.SetValue(testinfo_1, Convert.ChangeType(str_curr, xmsg.PropertyType), null);
            else xmsg.SetValue(testinfo_2, Convert.ChangeType(str_curr, xmsg.PropertyType), null);
        }

        private void ClearStatusWindow() {
            PropertyInfo xmsg = is_T_not_U ? testinfo_1.GetType().GetProperty(xmsg_name) : testinfo_2.GetType().GetProperty(xmsg_name);
            if (is_T_not_U) xmsg.SetValue(testinfo_1, Convert.ChangeType("", xmsg.PropertyType), null);
            else xmsg.SetValue(testinfo_2, Convert.ChangeType("", xmsg.PropertyType), null);
        }


        public bool run_Test_Tree(string cxttFileName) {
            if (File.Exists(cxttFileName) && cxttFileName.ToLower().EndsWith(".cxtt")) {
                ClearStatusWindow();
                _testTree = _scheduler.OpenXTT(cxttFileName);

                string g_mac2G_onboard = "";
                string g_mac5G_onboard = "";
                string g_macEth0 = "";
                string g_SN = "";

                if (is_T_not_U) g_macEth0 = testinfo_1.GetType().GetProperty("macWan").GetValue(testinfo_1, null).ToString();
                else g_macEth0 = testinfo_2.GetType().GetProperty("macWan").GetValue(testinfo_2, null).ToString();
                g_SN = "IMAP_" + g_macEth0;
                string AddMac_Value_tam = g_macEth0.Substring(6, 6);
                g_mac2G_onboard = g_macEth0.Substring(0, 6) + GetMAC2G(AddMac_Value_tam);
                g_mac5G_onboard = g_macEth0.Substring(0, 6) + GetMAC5G(AddMac_Value_tam);

                set_global_variable(settinginfo.GetType().GetProperty("snVariable").GetValue(settinginfo, null).ToString(), g_SN);
                set_global_variable(settinginfo.GetType().GetProperty("macWlan2GVariable").GetValue(settinginfo, null).ToString(), g_mac2G_onboard);
                set_global_variable(settinginfo.GetType().GetProperty("macWlan5GVariable").GetValue(settinginfo, null).ToString(), g_mac5G_onboard);
                _testTree.RunTree();

                return true;
            }
            else {
                PropertyInfo xmsg = is_T_not_U ? testinfo_1.GetType().GetProperty(xmsg_name) : testinfo_2.GetType().GetProperty(xmsg_name);
                string str_curr = "";
                if (is_T_not_U) str_curr = xmsg.GetValue(testinfo_1, null).ToString();
                else str_curr = xmsg.GetValue(testinfo_2, null).ToString();
                string msg = "Error in command line arguments. The first cmd argument should be an cxtt filename that exists.";
                str_curr += string.Format("{0}\r\n", msg);
                if (is_T_not_U) xmsg.SetValue(testinfo_1, Convert.ChangeType(str_curr, xmsg.PropertyType), null);
                else xmsg.SetValue(testinfo_2, Convert.ChangeType(str_curr, xmsg.PropertyType), null);
                return false;
            }
        }

        public bool close_Test_Tree() {
            if (this._testTree == null) return false;
            this._testTree.StopTest();
            return true;
        }

        public void Dispose() {
            _scheduler.Exit(); _scheduler = null;
            if (this._testTree == null) return;
            this._testTree.StopTest();
        }

        public int tree_Execution_Status() {
            try {
                PropertyInfo xmsg = is_T_not_U ? testinfo_1.GetType().GetProperty(xmsg_name) : testinfo_2.GetType().GetProperty(xmsg_name);
                string str_curr = "";
                if (is_T_not_U) str_curr = xmsg.GetValue(testinfo_1, null).ToString();
                else str_curr = xmsg.GetValue(testinfo_2, null).ToString();
                bool r = str_curr.Contains("Tree finished with result: ");
                if (!r) return 4;
                else {
                    bool kq = str_curr.Contains("Tree finished with result: Passed");
                    return kq == true ? 1 : 2;
                }
            }
            catch { return 2; }
        }

        private bool set_global_variable(string var_name, string var_value) {
            try {
                bool r = false;
                int count = 0;
            RE:
                count++;
                _scheduler.SetGlobalVariable(var_name, var_value);
                string fvalue = "";
                _scheduler.GetGlobalVariable(var_name, out fvalue);
                r = fvalue.ToLower().Equals(var_value.ToLower());
                if (!r) {
                    if (count < 3) goto RE;
                }
                return r;
            }
            catch { return false; }
        }

        private string GetMAC2G(string mac) {
            string hexMAC = "FAIL";
            try {
                int num = Int32.Parse(mac, System.Globalization.NumberStyles.HexNumber);
                num = num + 1;
                hexMAC = num.ToString("X").Trim();
                int hexMAC_len = hexMAC.Length;
                if (hexMAC_len < 6) {
                    for (int i = 0; i < 6 - hexMAC_len; i++) {
                        hexMAC = "0" + hexMAC;
                    }
                }
                else
                    if (hexMAC_len == 7) {
                    hexMAC = hexMAC.Substring(0, 6);
                }
            }
            catch { }

            return hexMAC;
        }

        private string GetMAC5G(string mac) {
            string hexMAC = "FAIL";
            try {
                int num = Int32.Parse(mac, System.Globalization.NumberStyles.HexNumber);
                num = num + 2;
                hexMAC = num.ToString("X").Trim();
                int hexMAC_len = hexMAC.Length;
                if (hexMAC_len < 6) {
                    for (int i = 0; i < 6 - hexMAC_len; i++) {
                        hexMAC = "0" + hexMAC;
                    }
                }
                else
                    if (hexMAC_len == 7) {
                    hexMAC = hexMAC.Substring(0, 6);
                }
            }
            catch { }

            return hexMAC;
        }

    }
}
