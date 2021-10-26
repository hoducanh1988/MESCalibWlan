using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EW30SX.Asset.Custom {
    public class TestFrequencyInfo {

        public TestFrequencyInfo() {
            Frequency = "";
            Antenna = "";
            averagePowers = new List<double>();
        }

        public string Frequency { get; set; }
        public string Antenna { get; set; }
        public List<double> averagePowers { get; set; }

        public override string ToString() {
            return $"{Frequency.PadLeft(10,' ')}{Antenna.PadLeft(10, ' ')}{string.Join(",", averagePowers)}";
        }
    }
}
