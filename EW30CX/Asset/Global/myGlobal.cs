using EW30CX.Asset.Custom;
using EW30CX.Asset.IO;
using EW30CX.Models;
using EW30CX.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EW30CX.Asset.Global {
    public class myGlobal {

        public static List<string> listProduct = null;
        public static SettingViewModel settingviewmodel = new SettingViewModel();
        public static CalibViewModel calibviewmodel = new CalibViewModel();
        public static AttenuationViewModel attenuationviewmodel = new AttenuationViewModel();
        public static MainHeaderViewModel mainheaderviewmodel = new MainHeaderViewModel();
        public static QSPRHelper<CalibModel, AttenuationModel, SettingModel> qsprHelper = null;
        public static List<GoldenFrequencyInfo> goldenStandardValues = null;
        public static List<TestFrequencyInfo> goldenTestResults = null;
        public static List<PowerDifferenceInfo> powerDifferenceValues = null;
        public static List<PathlossFrequencyInfo> pathlossInfos = null;
        
    }
}
