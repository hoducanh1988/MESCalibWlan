using EW30SX.Asset.Global;
using EW30SX.Commands;
using EW30SX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EW30SX.ViewModels {
    public class CalibViewModel {

        public CalibViewModel() {
            _cm = new CalibModel();
            _sm = myGlobal.settingviewmodel.SM;

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
