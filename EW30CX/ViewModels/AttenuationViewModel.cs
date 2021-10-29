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
    public class AttenuationViewModel {

        public AttenuationViewModel() {
            _am = new AttenuationModel();
            _sm = myGlobal.settingviewmodel.SM;

            MeasureCommand = new AttenuationMeasureCommand(this);
            VerifyCommand = new AttenuationVerifyCommand(this);
            OpenCalibTestTreeCommand = new AttenuationOpenCalibTesttreeCommand(this);
            OpenAttTestTreeCommand = new AttenuationOpenAttTesttreeCommand(this);
            OpenPathlossCommand = new AttenuationOpenPathlossCommand(this);
        }

        AttenuationModel _am;
        public AttenuationModel AM {
            get => _am;
        }
        SettingModel _sm;
        public SettingModel SM {
            get => _sm;
        }

        //command
        public ICommand MeasureCommand {
            get;
            private set;
        }
        public ICommand VerifyCommand {
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
