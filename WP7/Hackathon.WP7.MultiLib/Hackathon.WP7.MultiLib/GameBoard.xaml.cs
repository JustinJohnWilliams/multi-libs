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

namespace Hackathon.WP7.MultiLib
{
    public partial class GameBoard : PhoneApplicationPage
    {
        private RestfulSilverlight.RestFacilitator _restFacilitator;
        private RestfulSilverlight.RestService _restService;
        private RestfulSilverlight.AsyncDelegation _restAsyncDelegation;
        private RestfulSilverlight.DelayExecution _restDelayExec = null;

        private string _gameId = "";
        private Entities.Game _gameState = null;
        private Entities.Game _transitionState = null;
        private string _selected;

        public GameBoard()
        {
            InitializeComponent();
            IntializeStoryboard();
        }

        private void IntializeStoryboard()
        {
            sbCardIn.Completed += ((s, e) =>
                {
                    // do nothing
                }
            );

            sbCardOut.Completed += ((s, e) =>
                {
                    // set the card text
                    tbkCardText.Text = _transitionState.currentBlackCard;
                    // transition the new card in
                    sbCardIn.Begin();
                }
            );

            sbShiftLeft.Completed += (
                (s, e) =>
                {
                    var player = _gameState.players.Where(p => p.id == MainPage.GetPhoneId()).First();

                    if (player.isCzar)
                    {
                        var cards = _gameState.players.Where(p => p.selectedWhiteCardId != null).Select(p => p.selectedWhiteCardId).ToList();

                        if (cards.Count > 0)
                        {
                            int index = cards.IndexOf(_selected);
                            index--;

                            brdSelected.DataContext = cards[index];
                            _selected = cards[index];

                            CompositeTransform ct = selection.RenderTransform as CompositeTransform;
                            ct.TranslateX = -485;

                            if (index == 0)
                            {
                                brdLeft2.DataContext = null;
                                brdLeft.DataContext = null;

                                brdLeft2.Visibility = System.Windows.Visibility.Collapsed;
                                brdLeft.Visibility = System.Windows.Visibility.Collapsed;

                                brdShiftLeft.Opacity = .3;
                            }
                            else if (index == 1)
                            {
                                brdLeft2.DataContext = null;
                                brdLeft.DataContext = cards[index - 1];

                                brdLeft2.Visibility = System.Windows.Visibility.Collapsed;
                                brdLeft.Visibility = System.Windows.Visibility.Visible;

                                brdShiftLeft.Opacity = 1;
                            }
                            else
                            {
                                brdLeft2.DataContext = cards[index - 2];
                                brdLeft.DataContext = cards[index - 1];

                                brdLeft2.Visibility = System.Windows.Visibility.Visible;
                                brdLeft.Visibility = System.Windows.Visibility.Visible;

                                brdShiftLeft.Opacity = 1;
                            }

                            brdShiftRight.Opacity = 1;

                            if (index < cards.Count - 2)
                            {
                                brdRight.DataContext = cards[index + 1];
                                brdRight2.DataContext = cards[index + 2];

                                brdRight.Visibility = System.Windows.Visibility.Visible;
                                brdRight2.Visibility = System.Windows.Visibility.Visible;
                            }
                            else
                            {
                                brdRight.DataContext = cards[index + 1];
                                brdRight2.DataContext = null;

                                brdRight.Visibility = System.Windows.Visibility.Visible;
                                brdRight2.Visibility = System.Windows.Visibility.Collapsed;
                            }
                        }
                    }
                    else
                    {
                        var sIndex = player.cards.IndexOf(brdLeft.DataContext.ToString());

                        brdSelected.DataContext = player.cards[sIndex];
                        _selected = player.cards[sIndex];

                        CompositeTransform ct = selection.RenderTransform as CompositeTransform;
                        ct.TranslateX = -485;

                        if (sIndex == 0)
                        {
                            brdLeft2.DataContext = null;
                            brdLeft.DataContext = null;

                            brdLeft2.Visibility = System.Windows.Visibility.Collapsed;
                            brdLeft.Visibility = System.Windows.Visibility.Collapsed;

                            brdShiftLeft.Opacity = .3;
                        }
                        else if (sIndex == 1)
                        {
                            brdLeft2.DataContext = null;
                            brdLeft.DataContext = player.cards[sIndex - 1];

                            brdLeft2.Visibility = System.Windows.Visibility.Collapsed;
                            brdLeft.Visibility = System.Windows.Visibility.Visible;

                            brdShiftLeft.Opacity = 1;
                        }
                        else
                        {
                            brdLeft2.DataContext = player.cards[sIndex - 2];
                            brdLeft.DataContext = player.cards[sIndex - 1];

                            brdLeft2.Visibility = System.Windows.Visibility.Visible;
                            brdLeft.Visibility = System.Windows.Visibility.Visible;

                            brdShiftLeft.Opacity = 1;
                        }

                        brdShiftRight.Opacity = 1;

                        if (sIndex < player.cards.Count - 2)
                        {
                            brdRight.DataContext = player.cards[sIndex + 1];
                            brdRight2.DataContext = player.cards[sIndex + 2];

                            brdRight.Visibility = System.Windows.Visibility.Visible;
                            brdRight2.Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            brdRight.DataContext = player.cards[sIndex + 1];
                            brdRight2.DataContext = null;

                            brdRight.Visibility = System.Windows.Visibility.Visible;
                            brdRight2.Visibility = System.Windows.Visibility.Collapsed;
                        }
                    }
                }
            );

            sbShiftRight.Completed += (
                (s, e) =>
                {
                    var player = _gameState.players.Where(p => p.id == MainPage.GetPhoneId()).First();

                    if (player.isCzar)
                    {
                        var cards = _gameState.players.Where(p => p.selectedWhiteCardId != null).Select(p => p.selectedWhiteCardId).ToList();

                        if (cards.Count > 0)
                        {
                            int index = cards.IndexOf(_selected);
                            index++;

                            brdSelected.DataContext = cards[index];
                            _selected = cards[index];

                            CompositeTransform ct = selection.RenderTransform as CompositeTransform;
                            ct.TranslateX = -485;

                            if (cards.Count - index > 2)
                            {
                                brdRight2.DataContext = cards[index + 2];
                                brdRight.DataContext = cards[index + 1];

                                brdRight2.Visibility = System.Windows.Visibility.Visible;
                                brdRight.Visibility = System.Windows.Visibility.Visible;

                                brdShiftRight.Opacity = 1;
                            }
                            else if (cards.Count - index > 1)
                            {
                                brdRight2.DataContext = null;
                                brdRight.DataContext = cards[index + 1];

                                brdRight2.Visibility = System.Windows.Visibility.Collapsed;
                                brdRight.Visibility = System.Windows.Visibility.Visible;

                                brdShiftRight.Opacity = 1;
                            }
                            else
                            {
                                brdRight2.DataContext = null;
                                brdRight.DataContext = null;

                                brdRight2.Visibility = System.Windows.Visibility.Collapsed;
                                brdRight.Visibility = System.Windows.Visibility.Collapsed;

                                brdShiftRight.Opacity = .3;
                            }

                            brdShiftLeft.Opacity = 1;

                            if (index > 1)
                            {
                                brdLeft.DataContext = cards[index - 1];
                                brdLeft2.DataContext = cards[index - 2];

                                brdLeft.Visibility = System.Windows.Visibility.Visible;
                                brdLeft2.Visibility = System.Windows.Visibility.Visible;
                            }
                            else
                            {
                                brdLeft.DataContext = cards[index - 1];
                                brdLeft2.DataContext = null;

                                brdLeft.Visibility = System.Windows.Visibility.Visible;
                                brdLeft2.Visibility = System.Windows.Visibility.Collapsed;
                            }
                        }
                    }
                    else
                    {
                        var sIndex = player.cards.IndexOf(brdSelected.DataContext.ToString());
                        sIndex++;

                        brdSelected.DataContext = player.cards[sIndex];
                        _selected = player.cards[sIndex];

                        CompositeTransform ct = selection.RenderTransform as CompositeTransform;
                        ct.TranslateX = -485;

                        if (player.cards.Count - sIndex > 2)
                        {
                            brdRight2.DataContext = player.cards[sIndex + 2];
                            brdRight.DataContext = player.cards[sIndex + 1];

                            brdRight2.Visibility = System.Windows.Visibility.Visible;
                            brdRight.Visibility = System.Windows.Visibility.Visible;

                            brdShiftRight.Opacity = 1;
                        }
                        else if (player.cards.Count - sIndex > 1)
                        {
                            brdRight2.DataContext = null;
                            brdRight.DataContext = player.cards[sIndex + 1];

                            brdRight2.Visibility = System.Windows.Visibility.Collapsed;
                            brdRight.Visibility = System.Windows.Visibility.Visible;

                            brdShiftRight.Opacity = 1;
                        }
                        else
                        {
                            brdRight2.DataContext = null;
                            brdRight.DataContext = null;

                            brdRight2.Visibility = System.Windows.Visibility.Collapsed;
                            brdRight.Visibility = System.Windows.Visibility.Collapsed;

                            brdShiftRight.Opacity = .3;
                        }

                        brdShiftLeft.Opacity = 1;

                        if (sIndex > 1)
                        {
                            brdLeft.DataContext = player.cards[sIndex - 1];
                            brdLeft2.DataContext = player.cards[sIndex - 2];

                            brdLeft.Visibility = System.Windows.Visibility.Visible;
                            brdLeft2.Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            brdLeft.DataContext = player.cards[sIndex - 1];
                            brdLeft2.DataContext = null;

                            brdLeft.Visibility = System.Windows.Visibility.Visible;
                            brdLeft2.Visibility = System.Windows.Visibility.Collapsed;
                        }
                    }
                }
            );
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            refreshDisplay(null);

            _gameId = this.NavigationContext.QueryString["gameId"];

            InitializeRestful();
        }

        private void InitializeRestful()
        {
            if (_restFacilitator == null)
                _restFacilitator = new RestfulSilverlight.RestFacilitator();

            if (_restService == null)
                _restService = new RestfulSilverlight.RestService(_restFacilitator, MainPage.BASE_URI);

            if (_restAsyncDelegation == null)
                _restAsyncDelegation = new RestfulSilverlight.AsyncDelegation(_restService);

            getGameUpdate();
        }

        private void getGameUpdate()
        {
            _restAsyncDelegation.Get<Entities.Game>("gamebyid", new { id = _gameId, cache = Guid.NewGuid() })
                   .WhenFinished(
                   result =>
                   {
                       refreshDisplay(result);
                   });

            // execute the async request
            _restAsyncDelegation.Go();

            // create the delay object if it hasn't been created
            if (_restDelayExec == null)
                _restDelayExec = new RestfulSilverlight.DelayExecution();

            // reset the delay timer
            _restDelayExec.SetTimeout(5000, new Action(getGameUpdate));
        }

        private void refreshDisplay(Entities.Game gameState)
        {
            if (gameState != null && gameState.isStarted)
            {
                btnCool.Visibility = System.Windows.Visibility.Collapsed;
                sbWaiting.Stop();
                tbkWaiting.Visibility = System.Windows.Visibility.Collapsed;

                // we need to check the state of the game
                if (_gameState == null)
                {
                    // first time the game is loaded, setup the inital state
                    _gameState = gameState;

                    // define the selection grid
                    selection.Visibility = System.Windows.Visibility.Visible;
                    brdLeft2.Visibility = System.Windows.Visibility.Collapsed;
                    brdLeft.Visibility = System.Windows.Visibility.Collapsed;
                    brdSelected.Visibility = System.Windows.Visibility.Collapsed;
                    brdRight.Visibility = System.Windows.Visibility.Collapsed;
                    brdRight2.Visibility = System.Windows.Visibility.Collapsed;
                    brdShiftLeft.Opacity = .3;
                    brdShiftRight.Opacity = .3;

                    btnSelect.IsEnabled = false;
                    
                    var player = _gameState.players.Where(p => p.id == MainPage.GetPhoneId()).First();

                    rnPoints.Text = player.awesomePoints.ToString();

                    if (_gameState.winningCardId != null)
                    {
                        tbkPlayerState.Text = "The winning card!";

                        brdLeft2.Visibility = System.Windows.Visibility.Collapsed;
                        brdLeft.Visibility = System.Windows.Visibility.Collapsed;
                        brdSelected.Visibility = System.Windows.Visibility.Collapsed;
                        brdRight.Visibility = System.Windows.Visibility.Collapsed;
                        brdRight2.Visibility = System.Windows.Visibility.Collapsed;
                        brdShiftLeft.Opacity = .3;
                        brdShiftRight.Opacity = .3;

                        brdSelected.DataContext = _gameState.winningCardId;
                        brdSelected.Visibility = System.Windows.Visibility.Visible;

                        btnPlay.Visibility = System.Windows.Visibility.Collapsed;
                        btnSelect.Visibility = System.Windows.Visibility.Collapsed;
                        btnCool.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        if (player.isCzar)
                        {
                            tbkPlayerState.Text = "You are the card czar.";

                            btnSelect.Visibility = System.Windows.Visibility.Visible;
                            btnPlay.Visibility = System.Windows.Visibility.Collapsed;

                            displayCzar();
                        }
                        else
                        {
                            tbkPlayerState.Text = "Please select a card.";

                            btnSelect.Visibility = System.Windows.Visibility.Collapsed;
                            btnPlay.Visibility = System.Windows.Visibility.Visible;

                            brdSelected.DataContext = player.cards.ElementAt(0);
                            brdRight.DataContext = (player.cards.Count > 0 ? player.cards.ElementAt(1) : null);
                            brdRight2.DataContext = (player.cards.Count > 1 ? player.cards.ElementAt(2) : null);

                            _selected = player.cards.ElementAt(0);


                            brdSelected.Visibility = System.Windows.Visibility.Visible;
                            brdRight.Visibility = (brdRight.DataContext == null ? Visibility.Collapsed : Visibility.Visible);
                            brdRight2.Visibility = (brdRight2.DataContext == null ? Visibility.Collapsed : Visibility.Visible);

                            brdShiftRight.Opacity = (brdRight.DataContext == null ? .3 : 1);
                        }
                    }

                    tbkCardText.Text = _gameState.currentBlackCard;
                    sbCardIn.Begin();
                }
                else
                {
                    // we need to check for updates
                    _transitionState = gameState;
                    if (_gameState.currentBlackCard != gameState.currentBlackCard)
                    {
                        sbCardOut.Begin();
                    }

                    var player = _gameState.players.Where(p => p.id == MainPage.GetPhoneId()).First();

                    rnPoints.Text = player.awesomePoints.ToString();

                    if (_gameState.winningCardId != null)
                    {
                        tbkPlayerState.Text = "The winning card!";

                        brdLeft2.Visibility = System.Windows.Visibility.Collapsed;
                        brdLeft.Visibility = System.Windows.Visibility.Collapsed;
                        brdSelected.Visibility = System.Windows.Visibility.Collapsed;
                        brdRight.Visibility = System.Windows.Visibility.Collapsed;
                        brdRight2.Visibility = System.Windows.Visibility.Collapsed;
                        brdShiftLeft.Opacity = .3;
                        brdShiftRight.Opacity = .3;

                        brdSelected.DataContext = _gameState.winningCardId;
                        brdSelected.Visibility = System.Windows.Visibility.Visible;

                        tbkSelectionMsg.Visibility = System.Windows.Visibility.Collapsed;
                        brdSelectionContainer.Visibility = System.Windows.Visibility.Visible;

                        btnPlay.Visibility = System.Windows.Visibility.Collapsed;
                        btnSelect.Visibility = System.Windows.Visibility.Collapsed;
                        btnCool.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        if (player.isCzar)
                        {
                            tbkPlayerState.Text = "You are the card czar.";

                            _gameState = _transitionState;

                            displayCzar();
                        }
                        else
                        {
                            tbkPlayerState.Text = "Please select a card.";

                            if (_transitionState.winningCardId != _gameState.winningCardId)
                            {
                                // new round
                                _selected = null;

                                _gameState = _transitionState;
                                player = _gameState.players.Where(p => p.id == MainPage.GetPhoneId()).First();

                                btnSelect.Visibility = System.Windows.Visibility.Collapsed;
                                btnPlay.Visibility = System.Windows.Visibility.Visible;

                                brdSelected.DataContext = player.cards.ElementAt(0);
                                brdRight.DataContext = (player.cards.Count > 0 ? player.cards.ElementAt(1) : null);
                                brdRight2.DataContext = (player.cards.Count > 1 ? player.cards.ElementAt(2) : null);

                                _selected = player.cards.ElementAt(0);

                                brdSelected.Visibility = System.Windows.Visibility.Visible;
                                brdRight.Visibility = (brdRight.DataContext == null ? Visibility.Collapsed : Visibility.Visible);
                                brdRight2.Visibility = (brdRight2.DataContext == null ? Visibility.Collapsed : Visibility.Visible);

                                brdShiftRight.Opacity = (brdRight.DataContext == null ? .3 : 1);
                            }
                        }
                    }
                }
            }
            else
            {
                if (sbWaiting.GetCurrentState() != ClockState.Active)
                {
                    sbWaiting.Begin();
                    tbkWaiting.Visibility = System.Windows.Visibility.Visible;

                    selection.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        private void displayCzar()
        {
            var cards = _gameState.players.Where(p => p.selectedWhiteCardId != null).Select(p => p.selectedWhiteCardId).ToList();

            brdShiftLeft.Opacity = .3;
            brdShiftRight.Opacity = .3;

            if (cards.Count > 0)
            {
                btnSelect.IsEnabled = (cards.Count == 3);

                if (_selected == null)
                    _selected = cards.First();

                int index = cards.IndexOf(_selected);

                brdSelected.DataContext = cards.ElementAt(index);
                brdSelected.Visibility = System.Windows.Visibility.Visible;

                brdRight.DataContext = (cards.Count - index > 1 ? cards.ElementAt(index + 1) : null);
                brdRight2.DataContext = (cards.Count - index > 2 ? cards.ElementAt(index + 2) : null);

                brdLeft.DataContext = (index > 1 ? cards.ElementAt(index - 1) : null);
                brdLeft2.DataContext = (index > 2 ? cards.ElementAt(index - 2) : null);

                brdRight.Visibility = (brdRight.DataContext == null ? Visibility.Collapsed : Visibility.Visible);
                brdRight2.Visibility = (brdRight2.DataContext == null ? Visibility.Collapsed : Visibility.Visible);

                brdShiftLeft.Opacity = (brdLeft.DataContext == null ? .3 : 1);
                brdShiftRight.Opacity = (brdRight.DataContext == null ? .3 : 1);
            }
        }

        private void PlayCard_Click(object sender, RoutedEventArgs e)
        {
            // submit the card as the player card
            var player = _gameState.players.First(p => p.id == MainPage.GetPhoneId());
            _restAsyncDelegation.Post("selectcard", new { gameId = _gameId, playerId = player.id, whiteCardId = _selected })
                .WhenFinished(
                    () =>
                    {
                        tbkSelectionMsg.Visibility = System.Windows.Visibility.Visible;
                        brdSelectionContainer.Visibility = System.Windows.Visibility.Collapsed;

                        brdShiftLeft.Opacity = .3;
                        brdShiftRight.Opacity = .3;

                        _selected = null;

                        btnPlay.IsEnabled = false;
                    }
            );

            _restAsyncDelegation.Go();
        }

        private void ShiftLeft_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if ((sender as FrameworkElement).Opacity == 1)
            {
                // the control is enabled
                if (sbShiftLeft.GetCurrentState() != ClockState.Active)
                    sbShiftLeft.Begin();
            }
        }

        private void ShiftRight_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
        	if ((sender as FrameworkElement).Opacity == 1)
            {
                // the control is enabled
                if (sbShiftRight.GetCurrentState() != ClockState.Active)
                    sbShiftRight.Begin();
            }
        }

        private void SelectPlayer_Click(object sender, RoutedEventArgs e)
        {
            // submit the card as the player card
            var player = _gameState.players.First(p => p.selectedWhiteCardId == _selected);
            _restAsyncDelegation.Post("selectWinner", new { gameId = _gameId, cardId = _selected })
                .WhenFinished(
                    () =>
                    {
                        _selected = null;
                        brdSelected.DataContext = null;
                        brdLeft.DataContext = null;
                        brdLeft2.DataContext = null;
                        brdRight.DataContext = null;
                        brdRight2.DataContext = null;

                        brdLeft.Visibility = Visibility.Collapsed;
                        brdLeft2.Visibility = Visibility.Collapsed;
                        brdRight.Visibility = Visibility.Collapsed;
                        brdRight2.Visibility = Visibility.Collapsed;
                    }
            );

            _restAsyncDelegation.Go();
        }

        private void Cool_Click(object sender, RoutedEventArgs e)
        {
            // submit the card as the player card
            var player = _gameState.players.First(p => p.id == MainPage.GetPhoneId());
            _restAsyncDelegation.Post("readyForNextRound", new { gameId = _gameId, playerId = player.id })
                .WhenFinished(
                    () =>
                    {
                        
                    }
            );

            _restAsyncDelegation.Go();
        }
    }
}