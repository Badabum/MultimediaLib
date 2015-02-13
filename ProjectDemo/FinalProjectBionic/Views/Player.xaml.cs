using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FinalProjectBionic.Views
{
    /// <summary>
    /// Interaction logic for Player.xaml
    /// </summary>
    public partial class Player : UserControl
    {
            private bool mediaPlayerIsPlaying = false;
            private bool userIsDraggingSlider = false;

            public Player()
            {
                InitializeComponent();

                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += timer_Tick;
                timer.Start();
            }

            private void timer_Tick(object sender, EventArgs e)
            {
                if ((MyPlayer.Source != null) && (MyPlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
                {
                    SliProgress.Minimum = 0;
                    SliProgress.Maximum = MyPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                    SliProgress.Value = MyPlayer.Position.TotalSeconds;
                }
            }

            private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
            }

            private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = (MyPlayer != null) && (MyPlayer.Source != null);
            }

            private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
            {
                MyPlayer.Play();
                mediaPlayerIsPlaying = true;
            }

            private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = mediaPlayerIsPlaying;
            }

            private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
            {
                MyPlayer.Pause();
            }

            private void Stop_CanExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = mediaPlayerIsPlaying;
            }

            private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
            {
                MyPlayer.Stop();
                mediaPlayerIsPlaying = false;
            }

            private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
            {
                userIsDraggingSlider = true;
            }

            private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
            {
                userIsDraggingSlider = false;
                MyPlayer.Position = TimeSpan.FromSeconds(SliProgress.Value);
            }

            private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                LblProgressStatus.Text = TimeSpan.FromSeconds(SliProgress.Value).ToString(@"hh\:mm\:ss");
            }

            private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
            {
                MyPlayer.Volume += (e.Delta > 0) ? 0.1 : -0.1;
            }

        }
    }
