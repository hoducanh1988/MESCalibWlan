using EW30CX.Asset.Global;
using EW30CX.ViewModels;
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
    /// Interaction logic for CalibView.xaml
    /// </summary>
    public partial class CalibView : UserControl {

        DispatcherTimer timer = null;

        public CalibView() {
            InitializeComponent();
            this.DataContext = myGlobal.calibviewmodel;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += (s, e) => {
                var testing = myGlobal.calibviewmodel.CM;
                if (testing.totalResult.Contains("Waiting...")) {
                    this.scr_dut.ScrollToEnd();
                    this.scr_system.ScrollToEnd();
                    this.scr_qspr.ScrollToEnd();
                }
            };
            timer.Start();

        }

        ~CalibView() {
            if (timer != null && timer.IsEnabled) timer.Stop();
        }
    }
}
