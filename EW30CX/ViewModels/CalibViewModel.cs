using EW30CX.Asset.Custom;
using EW30CX.Asset.Global;
using EW30CX.Commands;
using EW30CX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EW30CX.ViewModels {
    public class CalibViewModel {

        public CalibViewModel() {
            _cm = new CalibModel();
            _sm = myGlobal.settingviewmodel.SM;
            _si = myGlobal.stationinfo;

            StartCommand = new CalibStartCommand(this);
            OpenCalibTestTreeCommand = new CalibOpenCalibTesttreeCommand(this);
            OpenAttTestTreeCommand = new CalibOpenAttTesttreeCommand(this);
            OpenPathlossCommand = new CalibOpenPathlossCommand(this);

        }

        CalibModel _cm;
        public CalibModel CM {
            get => _cm;
        }
        SettingModel _sm;
        public SettingModel SM {
            get => _sm;
        }
        StationInfo _si;
        public StationInfo SI {
            get => _si;
        }

        //command
        public ICommand StartCommand {
            get;
            private set;
        }
        public ICommand OpenCalibTestTreeCommand {
            get;
            private set;
        }
        public ICommand OpenAttTestTreeCommand {
            get;
            private set;
        }
        public ICommand OpenPathlossCommand {
            get;
            private set;
        }
    }
}
