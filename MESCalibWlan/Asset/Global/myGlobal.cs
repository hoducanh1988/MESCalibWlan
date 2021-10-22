using EW30SX.Asset.IO;
using EW30SX.Models;
using EW30SX.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EW30SX.Asset.Global {
    public class myGlobal {

        public static List<string> listProduct = null;
        public static SettingViewModel settingviewmodel = new SettingViewModel();
        public static CalibViewModel calibviewmodel = new CalibViewModel();
        public static MainHeaderViewModel mainheaderviewmodel = new MainHeaderViewModel();
    }
}
