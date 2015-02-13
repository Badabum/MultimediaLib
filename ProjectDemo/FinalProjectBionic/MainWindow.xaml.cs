using System.Collections.Generic;
using System.Windows;
using DataBaseAccess.Data;
using FinalProjectBionic.ViewModels;
using MahApps.Metro.Controls;

namespace FinalProjectBionic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
     
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new LibraryViewModel();

          
              
        }
        private void CustomVideoFile_OnClick(object sender, RoutedEventArgs e)
        {
            addCustomVideoFile.IsOpen = true;
        }

        private void WebInfoButton_OnClick(object sender, RoutedEventArgs e)
        {
            OmdbInformation.IsOpen = true;
        }

        private void Settings_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsFlyout.IsOpen = (SettingsFlyout.IsOpen != true);
        }

        private void RemoveAudio_OnClick(object sender, RoutedEventArgs e)
        {
            if (AudioGrid.SelectedItems == null) return;
            var viewModel = (this.DataContext as LibraryViewModel);
            viewModel.ToRemoveListAudios = new List<Audio>();
            foreach (var item in AudioGrid.SelectedItems)
            {
                viewModel.ToRemoveListAudios.Add((Audio)item);
            }
        }

        private void RemoveVideo_OnClick(object sender, RoutedEventArgs e)
        {
            if (VideoGrid.SelectedItems != null)
            {
                var viewModel = (this.DataContext as LibraryViewModel);
                viewModel.ToRemoveVideos = new List<Video>();
                foreach (var item in VideoGrid.SelectedItems)
                {
                    viewModel.ToRemoveVideos.Add((Video)item);
                }
            }
        }

       
    }
}
