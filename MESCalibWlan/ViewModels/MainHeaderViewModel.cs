using EW30SX.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityPack.IO;

namespace EW30SX.ViewModels {
    public class MainHeaderViewModel {

        public MainHeaderViewModel() {
            //load main info
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "main.xml") == false) _mhm = new MainHeaderModel();
            else _mhm = XmlHelper<MainHeaderModel>.FromXmlFile(AppDomain.CurrentDomain.BaseDirectory + "main.xml");
        }

        MainHeaderModel _mhm;
        public MainHeaderModel MHM {
            get => _mhm;
        }

    }
}
