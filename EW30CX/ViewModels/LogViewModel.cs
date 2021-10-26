using EW30CX.Commands;
using EW30CX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EW30CX.ViewModels {
    public class LogViewModel {

        public LogViewModel() {
            _lm = new LogModel();
            GoCommand = new LogGoCommand(this);
        }

        LogModel _lm;
        public LogModel LM {
            get => _lm;
        }

        public ICommand GoCommand {
            get;
            private set;
        }
    }
}
