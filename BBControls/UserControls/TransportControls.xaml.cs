using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace BBControls
{
    /// <summary>
    /// Interaction logic for TransportControls.xaml
    /// </summary>
    public partial class TransportControls : UserControl
    {
        MediaElement _player = null;
        double _naturalDuration = 0;
        Timer VideoPoll;
        bool _updateTime = true;
        public TransportControls()
        {
            InitializeComponent();
            VideoPoll = new Timer(VideoPollCallback, null, 0, 500);
        }
        public void InitializeControls(MediaElement Player)
        {
            _player = Player;
            _player.MediaOpened += OnMediaOpened;
        }

        private void VideoPollCallback(object? blahNull)
        {
            if(!_updateTime) { return;  }
            double elapsed;
            try
            {
                Dispatcher.Invoke(() =>
                {
                    elapsed = _player.Position.TotalSeconds;
                    DisplayTime(elapsed, _naturalDuration);
                });

            }
            catch (Exception)
            {

                //on shut down this sometimes errors.  It can be ignored
            }
 
        }

        private void DisplayTime(double elapsed, double naturalDuration)
        {
           string Elapsed = convertSecondsToTimeString(elapsed);
           string NaturalDuration = convertSecondsToTimeString(naturalDuration);
            TimeDisplay.Text = $"     {Elapsed} / {NaturalDuration}     ";
            ProgBar.Value = elapsed;
            SliderBar.Value = elapsed;
        }

        private string convertSecondsToTimeString(double Seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(Seconds);
            string timeString = time.ToString(@"hh\:mm\:ss");
            return timeString;
        }

        private void OnMediaOpened(object sender, RoutedEventArgs e)
        {
            _naturalDuration = _player.NaturalDuration.TimeSpan.TotalSeconds;
            ProgBar.Maximum = _naturalDuration;
            SliderBar.Maximum = _naturalDuration;   
        }
 
        private void ButtonDim(object sender, MouseEventArgs e)
        {
            if (sender is Button)
            {
                ((Button)sender).Opacity = .6;
            }
            else if (sender is Image)
            {
                ((Image)sender).Opacity = .6;
            }
           
        }
        private void ButtonBright(object sender, MouseEventArgs e)
        {
            if (sender is Button)
            {
                ((Button)sender).Opacity = 1;
            }
            else if (sender is Image)
            {
                ((Image)sender).Opacity = 1;
            }
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((Image)sender).Margin = new Thickness(((Image)sender).Margin.Left + 25, ((Image)sender).Margin.Top + 25, 0, 0);
        }

        private void Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((Image)sender).Margin = new Thickness(((Image)sender).Margin.Left - 25, ((Image)sender).Margin.Top - 25, 0, 0);
        }

        private void SliderBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _player.Position = new TimeSpan(0,0,0, (int)SliderBar.Value,0);
            _updateTime = true;
        }

        private void SliderBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _updateTime = false;
        }



        private void Play_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _player.Play();
            Play.Margin = new Thickness(1,1,0,0);
        }

        private void transportImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((Image)sender).Margin = new Thickness(0, 0, 0, 0);
        }

        private void Pause_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _player.Pause();
            ((Image)sender).Margin = new Thickness(1, 1, 0, 0);
        }

        private void Stop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _player.Stop();
            ((Image)sender).Margin = new Thickness(1, 1, 0, 0);
        }

        private void SkipBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _player.Position = _player.Position.Add(new TimeSpan(0, 0, 0, -5, 0));
            ((Image)sender).Margin = new Thickness(1, 1, 0, 0);
        }

        private void SkipFwd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _player.Position = _player.Position.Add(new TimeSpan(0, 0, 0, 30, 0));
            ((Image)sender).Margin = new Thickness(1, 1, 0, 0);
        }
    }
}
