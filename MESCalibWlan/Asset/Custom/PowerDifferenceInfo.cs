﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EW30SX.Asset.Custom {
    public class PowerDifferenceInfo {

        public PowerDifferenceInfo() {
            Frequency = "";
            powerDifferenceAnten1 = 0;
            powerDifferenceAnten2 = 0;
        }

        public string resultAnten1 { get; set; }
        public string resultAnten2 { get; set; }
        public string resultTotal { get; set; }

        public string Frequency { get; set; }
        double _pw_dif1;
        public double powerDifferenceAnten1 {
            get { return _pw_dif1; }
            set {
                _pw_dif1 = value;
                resultAnten1 = Math.Abs(value) <= 1 ? "PASS" : "FAIL";
                if (resultAnten2 != null)
                    resultTotal = (resultAnten1.Equals("PASS") && resultAnten2.Equals("PASS")) ? "PASS" : "FAIL";
            }
        }
        double _pw_dif2;
        public double powerDifferenceAnten2 {
            get { return _pw_dif2; }
            set {
                _pw_dif2 = value;
                resultAnten2 = Math.Abs(value) <= 1 ? "PASS" : "FAIL";
                if (resultAnten1 != null)
                    resultTotal = (resultAnten1.Equals("PASS") && resultAnten2.Equals("PASS")) ? "PASS" : "FAIL";
            }
        }
    }
}
