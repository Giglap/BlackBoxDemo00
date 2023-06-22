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
        private bool PreviewKeyEvents=true;

        public MainWindow()
        {
            InitializeComponent();
            //VideoPlayer.Play();
            //VideoPlayer.IsMuted = false;
            TransportControls.InitializeControls(VideoPlayer);
            Anno.Initialize(VideoPlayer);


            //// Create OpenFileDialog
            //Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            //// Set filter for file extension and default file extension
            //dlg.DefaultExt = ".mp4";
            //dlg.Filter = "mp4 Files (*.mp4)|*.mp4|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            //// Display OpenFileDialog by calling ShowDialog method
            //Nullable<bool> result = dlg.ShowDialog();

            //// Get the selected file name and display in a TextBox
            //if (result == true)
            //{
            //    // Open document
            //    string filename = dlg.FileName;
            //    //textBox1.Text = filename;
            //    VideoPlayer.Source = new Uri(filename);
            //}



        }
       
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!PreviewKeyEvents)
            {
                return;
            }
            if(e.Key == Key.D) 
            {
                _ = Anno.DropMarker();
            }
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
                if (Anno.Visibility.ToString() == "Visible")
                {
                    Anno.Visibility = Visibility.Hidden;
                }
                else
                {
                    Anno.Visibility = Visibility.Visible;
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

        private void Anno_Annotator_tb_GotFocus(object sender, EventArgs e)
        {
            PreviewKeyEvents = false;
        }

        private void Anno_Annotator_tb_LostFocus(object sender, EventArgs e)
        {
            PreviewKeyEvents = true;
        }

        private void VideoPlayer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (GetMediaState(VideoPlayer) == MediaState.Play)
            {
                VideoPlayer.Pause();
            }
            else
            {
                VideoPlayer.Play();
            }
        }
    }
}
