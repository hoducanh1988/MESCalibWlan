using EW30SX.Asset.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EW30SX.Asset.IO {
    public class LogQSPRHelper {

        public List<TestFrequencyInfo> Extract(string data) {
            try {
                var list_freq = new List<TestFrequencyInfo>();
                string start_loop = "Test started: Clear_Instrument_Errors at:";
                string end_loop = "OutputParam_TxRlatestMeasPwrOut:";
                if (data.Contains(start_loop) == false || data.Contains(end_loop) == false) return null;

                string[] buffer = data.Split(new string[] { start_loop }, StringSplitOptions.None);
                foreach (var s in buffer) {
                    if (s.Contains(end_loop) == false) continue;
                    TestFrequencyInfo tfi = new TestFrequencyInfo();
                    tfi.Frequency = get_value_from_loop(s, "channel__", '_');
                    tfi.Antenna = get_value_from_loop(s, "chainMask__WLAN_CHAIN_", '_');
                    tfi.averagePower = double.Parse(get_value_from_loop(s, "OutputParam_chAvgTxPowerDbm:", '\r'));
                    list_freq.Add(tfi);
                    //aii.MCS = get_value_from_loop(s, "rate__RATE_MCS_", '_');
                    //aii.BW = get_value_from_loop(s, "rateBw__RateBW_", '_');
                    //aii.CHAIN = get_value_from_loop(s, "chainMask__WLAN_CHAIN_", '_');
                    //aii.chEvmDb = get_value_from_loop(s, "OutputParam_chEvmDb:", '\r');
                    //aii.chAmpImbDb = get_value_from_loop(s, "OutputParam_chAmpImbDb:", '\r');
                    //aii.chPhaseImbDeg = get_value_from_loop(s, "OutputParam_chPhaseImbDeg:", '\r');
                    //aii.chCarrierFreqErrorPpm = get_value_from_loop(s, "OutputParam_chCarrierFreqErrorPpm:", '\r');
                    //aii.chSymbolClockErrorPpm = get_value_from_loop(s, "OutputParam_chSymbolClockErrorPpm:", '\r');
                    //aii.chLoLeakageDbc = get_value_from_loop(s, "OutputParam_chLoLeakageDbc:", '\r');
                    //aii.chAvgTxPowerDbm = get_value_from_loop(s, "OutputParam_chAvgTxPowerDbm:", '\r');
                    //aii.chAvgTxPowerDeltaDb = get_value_from_loop(s, "OutputParam_chAvgTxPowerDeltaDb:", '\r');
                    //aii.powerLevel = get_value_from_loop(s, "powerLevel__", '\r');
                }

                return list_freq;
            }
            catch { return null; }
        }

        private string get_value_from_loop(string loop_string, string pattern, char split_char) {
            if (loop_string.Contains(pattern) == false) return null;
            string[] buffer = loop_string.Split(new string[] { pattern }, StringSplitOptions.None);
            return buffer[1].Split(split_char)[0];
        }
    }
}
