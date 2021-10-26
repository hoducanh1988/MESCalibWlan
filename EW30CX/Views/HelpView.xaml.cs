using EW30CX.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Xps.Packaging;

namespace EW30CX.Views {
    /// <summary>
    /// Interaction logic for HelpView.xaml
    /// </summary>
    public partial class HelpView : UserControl {
        public HelpView() {
            InitializeComponent();
            var hvm = new HelpViewModel();
            string help_file = hvm.HM.helpFile;

            if (File.Exists(help_file)) {
                XpsDocument xpsDocument = new XpsDocument(help_file, System.IO.FileAccess.Read);
                FixedDocumentSequence fds = xpsDocument.GetFixedDocumentSequence();
                docViewer.Document = fds;
            }
        }
    }
}
