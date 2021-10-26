using EW30CX.Asset.Global;
using EW30CX.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EW30CX.Views {
    /// <summary>
    /// Interaction logic for MainContentView.xaml
    /// </summary>
    public partial class MainContentView : UserControl {
        MainContentViewModel mcvm = null;
        CalibView cbv = new CalibView();
        AttenuationView atv = new AttenuationView();
        SettingView stv = new SettingView();
        LogView lgv = new LogView();
        HelpView hpv = new HelpView();
        AboutView abv = new AboutView();

        public MainContentView() {
            InitializeComponent();
            mcvm = new MainContentViewModel();
            this.DataContext = mcvm;

            Thread t = new Thread(new ThreadStart(() => {
                myGlobal.stationinfo.load_from_db();
            }));
            t.IsBackground = true;
            t.Start();
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e) {
            string control_tag = "";
            if (sender is Label) control_tag = (sender as Label).Tag.ToString();
            if (sender is Image) control_tag = (sender as Image).Tag.ToString();

            if (e.LeftButton == MouseButtonState.Pressed) {
                mcvm.MCM.Init();
                this.grid_content.Children.Clear();
                switch (control_tag) {
                    case "calibrate": {
                            mcvm.MCM.isCalib = true;
                            this.grid_content.Children.Add(cbv);
                            break;
                        }
                    case "attenuation": {
                            mcvm.MCM.isAttenuation = true;
                            this.grid_content.Children.Add(atv);
                            break;
                        }
                    case "setting": {
                            mcvm.MCM.isSetting = true;
                            this.grid_content.Children.Add(stv);
                            break; 
                        }
                    case "log": {
                            mcvm.MCM.isLog = true;
                            this.grid_content.Children.Add(lgv);
                            break;
                        }
                    case "help": {
                            mcvm.MCM.isHelp = true;
                            this.grid_content.Children.Add(hpv);
                            break;
                        }
                    case "about": {
                            mcvm.MCM.isAbout = true;
                            this.grid_content.Children.Add(abv);
                            break; 
                        }
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e) {
            this.grid_main.ColumnDefinitions[0].Width = new GridLength(45, GridUnitType.Pixel);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e) {
            this.grid_main.ColumnDefinitions[0].Width = new GridLength(200, GridUnitType.Pixel);
        }

    }

}
