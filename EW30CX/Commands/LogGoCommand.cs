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
    public class LogGoCommand : ICommand {

        private LogViewModel _lvm;
        public LogGoCommand(LogViewModel lvm) {
            _lvm = lvm;
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
            bool r = false;
            switch (r) {
                case var b when _lvm.LM.isSetting: { Process.Start(AppDomain.CurrentDomain.BaseDirectory + "setting.xml"); break; }
                case var c when _lvm.LM.isSoftware: { Process.Start(AppDomain.CurrentDomain.BaseDirectory + "main.xml"); break; }
                case var d when _lvm.LM.isGolden: { Process.Start(AppDomain.CurrentDomain.BaseDirectory + "Goldens"); break; }
                case var e when _lvm.LM.isLog: { Process.Start(AppDomain.CurrentDomain.BaseDirectory); break; }
            }
        }

        #endregion

    }
}
