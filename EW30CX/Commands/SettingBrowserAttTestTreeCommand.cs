using EW30CX.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EW30CX.Commands {
    public class SettingBrowserAttTestTreeCommand : ICommand {

        private SettingViewModel _svm;
        public SettingBrowserAttTestTreeCommand(SettingViewModel svm) {
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
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "*.cxtt | *.cxtt";
            dlg.Title = "Select testtree đo suy hao";
            if (dlg.ShowDialog() == true) {
                _svm.SM.attTesttreeFile = dlg.FileName;
            }
        }

        #endregion

    }
}
