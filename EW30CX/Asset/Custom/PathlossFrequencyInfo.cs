using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EW30CX.Asset.Custom {

    public class PathlossFrequencyInfo {
        public PathlossFrequencyInfo() {
            bhName = "";
            Frequency = "";
            lossValue = 0;
            lineNumber = 0;
        }

        public string bhName { get; set; }
        public string Frequency { get; set; }
        public double lossValue { get; set; }
        public int lineNumber { get; set; }

    }
}
