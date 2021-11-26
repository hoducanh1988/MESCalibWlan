using EW30SX.Commands;
using EW30SX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EW30SX.ViewModels {
    public class LoginViewModel {

        public LoginViewModel() {
            _lm = new LoginModel();
            GoCommand = new LoginGoCommand(this);
        }

        LoginModel _lm;
        public LoginModel LM {
            get => _lm;
        }

        public ICommand GoCommand {
            get;
            private set;
        }
    }
}
