using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;
using System.IO.IsolatedStorage;

namespace Hackathon.WP7.MultiLib
{
    public partial class MainPage : PhoneApplicationPage
    {
        public static string BASE_URI = "http://dry-peak-5299.herokuapp.com/";

        private RestfulSilverlight.RestFacilitator _restFacilitator = null;
        private RestfulSilverlight.RestService _restService = null;
        private RestfulSilverlight.AsyncDelegation _restAsyncDelegation = null;
        private RestfulSilverlight.DelayExecution _restDelayExec = null;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            if (IsolatedStorageSettings.ApplicationSettings.Contains("PlayerName"))
                this.tbxPlayerName.Text = IsolatedStorageSettings.ApplicationSettings["PlayerName"].ToString();
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            string gameId = Guid.NewGuid().ToString();

            _restAsyncDelegation.Post("add", new { id = gameId, name = "wp7 game" })
                .WhenFinished(() => { })
                    .ThenPost("joingame", new { gameId = gameId, playerId = GetPhoneId(), playerName = this.tbxPlayerName.Text })
                        .WhenFinished(() =>
                        {
                            NavigationService.Navigate(new Uri(string.Format("/GameBoard.xaml?gameId={0}", gameId), UriKind.Relative));
                        }
            );

            _restAsyncDelegation.Go();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // initially load the games
            getAvailableGames();
        }

        private void getAvailableGames()
        {
            if (_restFacilitator == null)
                _restFacilitator = new RestfulSilverlight.RestFacilitator();

            if (_restService == null)
                _restService = new RestfulSilverlight.RestService(_restFacilitator, BASE_URI);

            if (_restAsyncDelegation == null)
                _restAsyncDelegation = new RestfulSilverlight.AsyncDelegation(_restService);

            _restAsyncDelegation.Get<List<Entities.Game>>("list", new { cache = Guid.NewGuid() })
                   .WhenFinished(
                   result =>
                   {
                       var yourgames = (from g in result select g).Distinct();

                       lbYourGames.ItemsSource = yourgames;
                   });

            // execute the async request
            _restAsyncDelegation.Go();

            // create the delay object if it hasn't been created
            if (_restDelayExec == null)
                _restDelayExec = new RestfulSilverlight.DelayExecution();

            // reset the delay timer
            _restDelayExec.SetTimeout(5000, new Action(getAvailableGames));
        }

        private void JoinButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// here we need to join the available game
            Entities.Game game = (sender as Control).Tag as Entities.Game;

            var playerFound = game.players.Select(p => p.id).Contains(GetPhoneId());

            if (playerFound)
            {
                NavigationService.Navigate(new Uri(string.Format("/GameBoard.xaml?gameId={0}", game.id), UriKind.Relative));
            }
            else
            {
                _restAsyncDelegation.Post("joingame", new { gameId = game.id, playerId = GetPhoneId(), playerName = this.tbxPlayerName.Text })
                    .IfFailure((failEx) =>
                        {
                            MessageBox.Show(failEx.Message);
                        }
                    )
                    .WhenFinished(() =>
                    {
                        NavigationService.Navigate(new Uri(string.Format("/GameBoard.xaml?gameId={0}", game.id), UriKind.Relative));
                    }
                );

                _restAsyncDelegation.Go();
            }
        }

        private void tbxPlayerName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("PlayerName"))
                IsolatedStorageSettings.ApplicationSettings["PlayerName"] = tbxPlayerName.Text;
            else
                IsolatedStorageSettings.ApplicationSettings.Add("PlayerName", tbxPlayerName.Text);
        }

        #region static method(s)
        public static string GetPhoneId()
        {
            string result = string.Empty;
            object anid;
            int ANIDLength = 32;
            int ANIDOffset = 2;

            if (UserExtendedProperties.TryGetValue("ANID", out anid))
            {
                if (anid != null && anid.ToString().Length >= (ANIDLength + ANIDOffset))
                {
                    result = anid.ToString().Substring(ANIDOffset, ANIDLength);
                }
            }

            if (string.IsNullOrEmpty(result) == false)
            {
                return result;
            }

            if (IsolatedStorageSettings.ApplicationSettings.Contains("PhoneId") == false)
            {
                IsolatedStorageSettings.ApplicationSettings["PhoneId"] = Guid.NewGuid();
            }

            var guid = (Guid)IsolatedStorageSettings.ApplicationSettings["PhoneId"];
            return guid.ToString();
        }
        #endregion
    }
}