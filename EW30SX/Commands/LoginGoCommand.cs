using EW30SX.Asset.Global;
using EW30SX.Asset.IO;
using EW30SX.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EW30SX.Commands {
    public class LoginGoCommand : ICommand {

        private LoginViewModel _lvm;
        public LoginGoCommand(LoginViewModel lvm) {
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
            AccountHelper accs = new AccountHelper();
            var list_acc = accs.Get();
            if (list_acc == null || list_acc.Count == 0) goto OK;
            bool r = false;
            foreach (var acc in list_acc) {
                if (_lvm.LM.User.Equals(acc.User) && _lvm.LM.Password.Equals(acc.Password)) {
                    r = true;
                    break;
                }
            }
            if (r) goto OK;
            else goto NG;


            OK:
            myGlobal.calibviewmodel.CM.isLogin = true;
            accs.saveLog(_lvm.LM.User, _lvm.LM.Password, "success");
            _lvm.LM.Clear();
            return;

        NG:
            accs.saveLog(_lvm.LM.User, _lvm.LM.Password, "failured");
            _lvm.LM.Clear();
            _lvm.LM.errorMessage = "Thông tin user hoặc password không đúng. Vui lòng nhập lại.";
            return;
        }

        #endregion

    }
}
