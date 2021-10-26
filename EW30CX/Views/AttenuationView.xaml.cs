using EW30CX.Asset.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EW30CX.Views {
    /// <summary>
    /// Interaction logic for AttenuationView.xaml
    /// </summary>
    public partial class AttenuationView : UserControl {

        DispatcherTimer timer = null;

        public AttenuationView() {
            InitializeComponent();
            this.DataContext = myGlobal.attenuationviewmodel;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += (s, e) => {
                var testing = myGlobal.attenuationviewmodel.AM;
                if (testing.totalResult.Contains("Waiting...")) {
                    this.scr_att_dut.ScrollToEnd();
                    this.scr_att_system.ScrollToEnd();
                    this.scr_att_qspr.ScrollToEnd();
                }
            };
            timer.Start();
        }

        ~AttenuationView() {
            if (timer != null && timer.IsEnabled) timer.Stop();
        }
    }
}
