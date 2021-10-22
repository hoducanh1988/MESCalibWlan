using EW30SX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EW30SX.ViewModels {
    public class AttenuationViewModel {

        public AttenuationViewModel() {
            _am = new AttenuationModel();
        }

        AttenuationModel _am;
        public AttenuationModel AM {
            get => _am;
        }
    }
}
