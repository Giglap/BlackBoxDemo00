using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Accessibility;
using BBControls.Models;
using BBControls.WebApiHelpers;
using Annotation = BBControls.Models.Annotation;

namespace BBControls
{
    /// <summary>
    /// Interaction logic for Annotator.xaml
    /// </summary>
    public partial class Annotator : UserControl
    {
        public event EventHandler? Annotator_tb_GotFocus;
        public event EventHandler? Annotator_tb_LostFocus;
        WebApiHelper Wah = new WebApiHelper();
        MediaElement _player = new MediaElement();
        List<Show> Shows = new List<Show>();
        Show? show = new Show();
        Annotation currentAnnotation = new Annotation();
        List<Annotation> annos = new List<Annotation>();

        public Annotator()
        {
            InitializeComponent();
            Shows.Add(new Show()
            {
                Title = "Avatar",
                ImdbId = "tt0499549",
                Path = @"C:\Users\jimcr\Videos\BBTestVideos\Movies\Avatar219.mp4",
                User = "jamescr"
            });
            Shows.Add(new Show()
            {
                Title = "Serenity",
                ImdbId = "tt0379786",
                Path = @"C:\Users\jimcr\Videos\BBTestVideos\WMV\Serenity-2014-07-23 0008.wmv",
                User = "jamescr"
            });
            Shows.Add(new Show()
            {
                Title = "Angels & Demons-2014-07-23 1408",
                ImdbId = "tt0808151",
                Path = @"C:\Users\jimcr\Videos\BBTestVideos\Movies\Angels & Demons-2014-07-23 1408.mp4",
                User = "jamescr"
            });
            Shows.Add(new Show()
            {
                Title = "Jurassic Park-2014-08-28 0008",
                ImdbId = "tt0107290",
                Path = @"C:\Users\jimcr\Videos\BBTestVideos\Movies\Jurassic Park-2014-08-28 0008.mp4",
                User = "jamescr"
            });
            Shows.Add(new Show()
            {
                Title = "The American President-2014-09-12 1609",
                ImdbId = "tt0112346",
                Path = @"C:\Users\jimcr\Videos\BBTestVideos\Movies\The American President-2014-09-12 1609.mp4",
                User = "jamescr"
            });

            Shows.Add(new Show()
            {
                Title = "Dirty Dancing-2014-09-03 0009",
                ImdbId = "tt0092890",
                Path = @"C:\Users\jimcr\Videos\BBTestVideos\Movies\Dirty Dancing-2014-09-03 0009.mp4",
                User = "jamescr"
            });

            Shows.Add(new Show()
            {
                Title = "Footloose-2014-09-02 1909",
                ImdbId = "tt0087277",
                Path = @"C:\Users\jimcr\Videos\BBTestVideos\Movies\Footloose-2014-09-02 1909.mp4",
                User = "jamescr"
            });

            foreach (Show show in Shows)
            {
                cmbMovie.Items.Add(show);
                cmbMovie.DisplayMemberPath = "Title";
            }
        }
        private async void cmbMovie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            show = (Show)cmbMovie.SelectedItem;
            _player.Source = new Uri(show.Path);
            LblImdbId.Content = show.ImdbId;
            LblUser.Content = show.User;
            LblTitle.Content = show.Title;
            annos = await Wah.GetAnnosForIdAndUser(show.ImdbId, show.User);
            populateTimeIndexList(annos);
            _player.Play();
            
        }
        public void Initialize(MediaElement player)
        {
            _player = player;
            show = Shows.FirstOrDefault();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (GetMediaState(_player!) != MediaState.Play)
            {
                _player!.Play();
            }
            else
            {
                _player!.Pause();
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

        private void BtnDropMarker_Click(object sender, RoutedEventArgs e)
        {
            DropMarker();
        }
        public async Task DropMarker()
        {
            //Get Time, imdbid and user
            string ImdbId = show.ImdbId;
            string user = show.User;
            int timeIndex = (int)_player.Position.TotalSeconds;
            Annotation anno = new Annotation() { ImdbId = ImdbId, User = user, BeginTime = timeIndex };
            await Wah.WriteAnnotation(anno);
            annos = await Wah.GetAnnosForIdAndUser(ImdbId, user);
            populateTimeIndexList(annos);
            return;
            //save to the db
            //populate ui from db.
        }
        private void BtnListItemUpdate_Click(object sender, RoutedEventArgs e)
        {
            int selected = MarkerList.SelectedIndex;
            annos[selected].Title = tbTitle.Text;
            annos[selected].Description = tbDescription.Text;
            annos[selected].HashTags = tbHashtags.Text;
            _ = Wah.UpdateAnnotation(annos[selected]);

        }
        private void populateTimeIndexList(List<Annotation>? annos)
        {
            MarkerList.Items.Clear();
            if (annos == null) { return; }
            foreach (Annotation item in annos)
            {
                MarkerList.Items.Add(convertSecondsToTimeString((double)item.BeginTime));
            }
        }
        private string convertSecondsToTimeString(double Seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(Seconds);
            string timeString = time.ToString(@"hh\:mm\:ss");
            return timeString;
        }

        private void MarkerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var item = (TextBlock)this.MarkerList.InputHitTest(e.GetPosition(this.MarkerList));
                _player.Position = new TimeSpan(0, 0, TimeStringToSeconds(item.Text));
                if (GetMediaState(_player) != MediaState.Play) { _player.Play(); }
            }
            catch (Exception)
            {

                // user has clicked on the list but not on an item
            }

        }
        private void MarkerList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {

                    _player.Position = new TimeSpan(0, 0, TimeStringToSeconds((string)this.MarkerList.SelectedItem));

                }
            }
            catch (Exception)
            {

                // user has pressed the enter key but no item is selected
            }
        }
        private void MarkerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(annos == null)
            {
                ClearFields();
                return;
            }
            try
            {

                tbTitle.Text = annos[MarkerList.SelectedIndex].Title;
                tbDescription.Text = annos[MarkerList.SelectedIndex].Description;
                tbHashtags.Text = annos[MarkerList.SelectedIndex].HashTags;
            }
            catch (Exception)
            {
                ClearFields();
            }
        }

        private void ClearFields()
        {
            tbTitle.Text = "";
            tbDescription.Text = "";
            tbHashtags.Text = "";
        }

        // This method converts a time string in the format 'hh:mm:ss' to seconds
        private int TimeStringToSeconds(string timeString)
        {
            // Check if the time string is valid
            if (string.IsNullOrEmpty(timeString) || !TimeSpan.TryParse(timeString, out var timeSpan))
            {
                // Throw an exception or return a default value
                throw new ArgumentException("Invalid time string");
            }

            // Return the total seconds of the time span
            return (int)timeSpan.TotalSeconds;
        }

        private async void BtnListRefresh_Click(object sender, RoutedEventArgs e)
        {
            annos = await Wah.GetAnnosForIdAndUser(show.ImdbId, show.User);
            populateTimeIndexList(annos);
        }

        private async void BtnListItemDelete_Click(object sender, RoutedEventArgs e)
        {
            int selected = MarkerList.SelectedIndex;
            if (selected == -1) { return; }
            //MessageBox.Show($"List: {selected} annos: {convertSecondsToTimeString(annos[selected].BeginTime)} Id: {annos[selected].Id}");
            await Wah.DeleteAnnotation(annos[selected].Id);
            annos = await Wah.GetAnnosForIdAndUser(show.ImdbId, show.User);
            populateTimeIndexList(annos);
        }

        private void tb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Annotator_tb_GotFocus != null) Annotator_tb_GotFocus(this, new EventArgs());
        }

        private void tb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Annotator_tb_LostFocus != null) Annotator_tb_LostFocus(this, new EventArgs());
        }


    }
    public class Show
    {
        public string? Title { get; set; }
        public string? User { get; set; }
        public string? ImdbId { get; set; }
        public string? Path { get; set; }
    }
}
