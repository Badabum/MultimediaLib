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

namespace FinalProjectBionic.Views
{
    /// <summary>
    /// Interaction logic for AddCustomVideoFile.xaml
    /// </summary>
    public partial class AddCustomVideoFile : UserControl
    {
        public AddCustomVideoFile()
        {
            InitializeComponent();
        }

        private void TitleBox_OnError(object sender, ValidationErrorEventArgs e)
        {
            if(e.Action==ValidationErrorEventAction.Added)
            { }
        }
    }
}
