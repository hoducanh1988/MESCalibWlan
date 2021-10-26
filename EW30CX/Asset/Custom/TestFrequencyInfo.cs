﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EW30CX.Asset.Custom {
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
            return $"{Frequency.PadLeft(20,' ')}{Antenna.PadLeft(20, ' ')}{string.Join(",", averagePowers)}";
        }
    }
}