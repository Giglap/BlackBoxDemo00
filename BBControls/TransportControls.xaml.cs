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
        }
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            _player.Pause();
        }
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            _player.Play();
        }
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            _player.Stop();
        }
        private void SkipBack_Click(object sender, RoutedEventArgs e)
        {
            _player.Position = _player.Position.Add(new TimeSpan(0, 0, 0, -5, 0));

        }
        private void SkipFwd_Click(object sender, RoutedEventArgs e)
        {
            _player.Position = _player.Position.Add(new TimeSpan(0, 0, 0, 5, 0));
        }
        private void ButtonDim(object sender, MouseEventArgs e)
        {
            ((Button)sender).Opacity = .6;
        }
        private void ButtonBright(object sender, MouseEventArgs e)
        {
            ((Button)sender).Opacity = 1;
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((Button)sender).Margin = new Thickness(((Button)sender).Margin.Left + 25, ((Button)sender).Margin.Top + 25, 0, 0);
        }

        private void Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((Button)sender).Margin = new Thickness(((Button)sender).Margin.Left - 25, ((Button)sender).Margin.Top - 25, 0, 0);
        }
    }
}
