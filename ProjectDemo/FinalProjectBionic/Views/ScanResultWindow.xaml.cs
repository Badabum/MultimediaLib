using System.Collections;
using System.Collections.Generic;
using FinalProjectBionic.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace FinalProjectBionic.Views
{
    /// <summary>
    /// Interaction logic for ScanResultWindow.xaml
    /// </summary>
    public partial class ScanResultWindow : MetroWindow
    {
        public ICollection<string> SelectedFiles { get; private set; }

        public ScanResultWindow(ICollection<string> foundedFiles )
        {
            InitializeComponent();
            if (foundedFiles.Count == 0)
            {
                
            }
            DataContext = new ScanResultViewModel(foundedFiles);
            SelectedFiles = new List<string>();
            
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
            foreach (var item in mainGrid.SelectedItems)
            {
                SelectedFiles.Add((string)item);
            }
            
        }
        

    }
}
