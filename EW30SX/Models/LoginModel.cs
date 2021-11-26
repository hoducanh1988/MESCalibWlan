using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EW30SX.Models {

    public class LoginModel : INotifyPropertyChanged {

        public LoginModel() {
            Clear();

        }

        public void Clear() {
            User = Password = errorMessage = "";
        }

        string _user;
        public string User {
            get { return _user; }
            set {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }
        string _password;
        public string Password {
            get { return _password; }
            set {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        string _error;
        public string errorMessage {
            get { return _error; }
            set {
                _error = value;
                OnPropertyChanged(nameof(errorMessage));
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }

}
