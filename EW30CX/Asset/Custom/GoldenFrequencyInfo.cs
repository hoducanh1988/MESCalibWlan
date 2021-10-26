using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EW30CX.Asset.Custom {

    public class GoldenFrequencyInfo {
        public GoldenFrequencyInfo() {
            Frequency = "";
            powerAntenna1 = 0;
            powerAntenna2 = 0;
        }

        public string Frequency { get; set; }
        public double powerAntenna1 { get; set; }
        public double powerAntenna2 { get; set; }

        public override string ToString() {
            return $"{Frequency.PadLeft(20, ' ')}{powerAntenna1.ToString().PadLeft(20, ' ')}{powerAntenna2.ToString().PadLeft(20, ' ')}";
        }

    }
}
