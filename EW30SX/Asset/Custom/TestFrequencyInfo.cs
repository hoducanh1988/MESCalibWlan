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
            averagePower = 0;
        }

        public string Frequency { get; set; }
        public string Antenna { get; set; }
        public double averagePower { get; set; }

        public override string ToString() {
            return $"{Frequency.PadLeft(20,' ')}{Antenna.PadLeft(20, ' ')}{averagePower.ToString().PadLeft(20, ' ')}";
        }
    }
}
