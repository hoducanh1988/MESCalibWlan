using EW30SX.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;

namespace EW30SX.Commands {
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
                case var a when _lvm.LM.isAccess: { Process.Start(AppDomain.CurrentDomain.BaseDirectory + "References\\GetWeight.accdb"); break; }
                case var b when _lvm.LM.isSetting: { Process.Start(AppDomain.CurrentDomain.BaseDirectory + "setting.xml"); break; }
                case var c when _lvm.LM.isSoftware: { Process.Start(AppDomain.CurrentDomain.BaseDirectory + "main.xml"); break; }
            }
        }

        #endregion

    }
}
