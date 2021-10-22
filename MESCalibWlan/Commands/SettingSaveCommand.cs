using EW30SX.Models;
using EW30SX.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Reflection;
using UtilityPack.IO;
using EW30SX.Asset.IO;
using EW30SX.Asset.Global;

namespace EW30SX.Commands {
    public class SettingSaveCommand : ICommand {

        private SettingViewModel _svm;
        public SettingSaveCommand(SettingViewModel svm) {
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
            XmlHelper<SettingModel>.ToXmlFile(_svm.SM, AppDomain.CurrentDomain.BaseDirectory + "setting.xml");
            
            string msg;
            bool r = mySupport.checkSettingInfo(_svm, out msg);
            if (r == false) System.Windows.MessageBox.Show(msg, "Setting Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            else System.Windows.MessageBox.Show("Sucess!", "Save Setting", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }

        #endregion

    }
}
