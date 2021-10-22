using EW30SX.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Diagnostics;

namespace EW30SX.Commands {
    public class CalibOpenCalibTesttreeCommand : ICommand {

        private CalibViewModel _cvm;
        public CalibOpenCalibTesttreeCommand(CalibViewModel cvm) {
            _cvm = cvm;
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
            if (File.Exists(_cvm.SM.calibTesttreeFile) == false) return;
            Process.Start(_cvm.SM.calibTesttreeFile);
        }

        #endregion


    }
}
