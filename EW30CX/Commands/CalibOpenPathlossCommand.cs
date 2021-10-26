using EW30CX.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EW30CX.Commands {
    public class CalibOpenPathlossCommand : ICommand {

        private CalibViewModel _cvm;
        public CalibOpenPathlossCommand(CalibViewModel cvm) {
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
            if (File.Exists(_cvm.SM.pathLossFile) == false) return;
            Process.Start(_cvm.SM.pathLossFile);
        }

        #endregion

    }
}
