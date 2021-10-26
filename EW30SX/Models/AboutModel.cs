using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EW30SX.Models {

    public class AboutModel : INotifyPropertyChanged {

        string _id;
        public string ID {
            get { return _id; }
            set {
                _id = value;
                OnPropertyChanged(nameof(ID));
            }
        }
        string _version;
        public string Version {
            get { return _version; }
            set {
                _version = value;
                OnPropertyChanged(nameof(Version));
            }
        }
        string _content;
        public string Content {
            get { return _content; }
            set {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }
        string _date;
        public string Date {
            get { return _date; }
            set {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }
        string _change_type;
        public string ChangeType {
            get { return _change_type; }
            set {
                _change_type = value;
                OnPropertyChanged(nameof(ChangeType));
            }
        }
        string _person;
        public string Person {
            get { return _person; }
            set {
                _person = value;
                OnPropertyChanged(nameof(Person));
            }
        }

        public AboutModel() {
            ID = Version = Content = Date = ChangeType = Person = "";
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
