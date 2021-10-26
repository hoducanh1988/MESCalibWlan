using EW30CX.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;
using System.IO;

namespace EW30CX.Commands {
    public class SettingOpenPathLossCommand : ICommand {

        private SettingViewModel _svm;
        public SettingOpenPathLossCommand(SettingViewModel svm) {
            _svm = svm;
        }

        #region ICommand Members

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        //enable button save setting
        public bool CanExecute(object parameter) {
            return true;
        }

        //save setting
        public void Execute(object parameter) {
            if (File.Exists(_svm.SM.pathLossFile) == false) return;
            Process.Start(_svm.SM.pathLossFile);
        }

        #endregion
    }
}
