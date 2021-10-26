using EW30CX.Asset.Global;
using EW30CX.Asset.IO;
using EW30CX.Commands;
using EW30CX.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using UtilityPack.IO;

namespace EW30CX.ViewModels {
    public class SettingViewModel {

        public SettingViewModel() {

            //load setting file
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "setting.xml") == false) _sm = new SettingModel();
            else _sm = XmlHelper<SettingModel>.FromXmlFile(AppDomain.CurrentDomain.BaseDirectory + "setting.xml");

            List<string> listCom = new List<string>();
            for (int i = 1; i <= 99; i++) listCom.Add($"COM{i}");
            _collection_serial_port = new CollectionView(listCom);
            _collection_baud_rate = new CollectionView(new List<int>() { 110, 300, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200 });
            
            SaveCommand = new SettingSaveCommand(this);
            BrowserCalibTesttreeCommand = new SettingBrowserCalibTestTreeCommand(this);
            BrowserAttTesttreeCommand = new SettingBrowserAttTestTreeCommand(this);
            BrowserPathLossCommand = new SettingBrowserPathLossCommand(this);
            OpenCalibTesttreeCommand = new SettingOpenCalibTestTreeCommand(this);
            OpenAttTesttreeCommand = new SettingOpenAttTestTreeCommand(this);
            OpenPathLossCommand = new SettingOpenPathLossCommand(this);
        }


        SettingModel _sm;
        public SettingModel SM {
            get => _sm;
        }

        #region collection view

        CollectionView _collection_baud_rate;
        public CollectionView collectionBaudRate {
            get => _collection_baud_rate;
        }
        CollectionView _collection_serial_port;
        public CollectionView collectionSerialPort {
            get => _collection_serial_port;
        }

        #endregion

        //command
        public ICommand SaveCommand {
            get;
            private set;
        }
        public ICommand BrowserCalibTesttreeCommand {
            get;
            private set;
        }
        public ICommand BrowserAttTesttreeCommand {
            get;
            private set;
        }
        public ICommand BrowserPathLossCommand {
            get;
            private set;
        }
        public ICommand OpenCalibTesttreeCommand {
            get;
            private set;
        }
        public ICommand OpenAttTesttreeCommand {
            get;
            private set;
        }
        public ICommand OpenPathLossCommand {
            get;
            private set;
        }

    }
}
