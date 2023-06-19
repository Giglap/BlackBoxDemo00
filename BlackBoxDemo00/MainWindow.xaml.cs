using BBControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace BlackBoxDemo00
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            VideoPlayer.Play();
            VideoPlayer.IsMuted = true;
            TransportControls.InitializeControls(VideoPlayer);
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            MediaState state = GetMediaState(VideoPlayer);
            if (e.Key == Key.T)
            {
                if (this.WindowStyle == WindowStyle.SingleBorderWindow)
                {
                    this.WindowStyle = WindowStyle.None;
                }
                else
                {
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                }
            }
            if (e.Key == Key.F)
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                }
            }
            if (e.Key == Key.X)
            {
                if (state == MediaState.Play || state == MediaState.Pause)
                {
                    VideoPlayer.Position = VideoPlayer.Position.Add(new TimeSpan(0, 0, 0, 30, 0));
                }
            }
            if (e.Key == Key.Z)
            {
                if (state == MediaState.Play || state == MediaState.Pause)
                {
                    VideoPlayer.Position = VideoPlayer.Position.Add(new TimeSpan(0, 0, 0, -30, 0));
                }
            }
            if (e.Key == Key.P)
            {


                switch (state)
                {
                    case MediaState.Manual:
                        VideoPlayer.Play();
                        break;
                    case MediaState.Play:
                        VideoPlayer.Pause();
                        break;
                    case MediaState.Close:
                        VideoPlayer.Play();
                        break;
                    case MediaState.Pause:
                        VideoPlayer.Play();
                        break;
                    case MediaState.Stop:
                        break;
                    default:
                        break;
                }
            }
            if (e.Key == Key.M)
            {
                VideoPlayer.IsMuted = !VideoPlayer.IsMuted;
            }
            if (e.Key == Key.Q)  // Hide transport controls
            {
                if (TransportControls.Visibility.ToString() == "Visible")
                {
                    TransportControls.Visibility = Visibility.Hidden;
                }
                else
                {
                    TransportControls.Visibility = Visibility.Visible;
                }
            }
            if (e.Key == Key.A)  // Hide Annotation Panel
            {
                if (AnnoPanel.Visibility.ToString() == "Visible")
                {
                    AnnoPanel.Visibility = Visibility.Hidden;
                }
                else
                {
                    AnnoPanel.Visibility = Visibility.Visible;
                }
            }
        }
        private MediaState GetMediaState(MediaElement myMedia)
        {
            FieldInfo hlp = typeof(MediaElement).GetField("_helper", BindingFlags.NonPublic | BindingFlags.Instance);
            object helperObject = hlp.GetValue(myMedia);
            FieldInfo stateField = helperObject.GetType().GetField("_currentState", BindingFlags.NonPublic | BindingFlags.Instance);
            MediaState state = (MediaState)stateField.GetValue(helperObject);
            return state;
        }

        private void TransportControls_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void VideoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            VideoPlayer.Position = new TimeSpan();
            VideoPlayer.Play();
        }
    }
}
