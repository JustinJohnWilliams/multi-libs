
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using RestfulAdapter;

namespace multilibs
{
	public partial class GameViewController : UIViewController
	{
		private String baseUri = "http://localhost:3000/";
//		private String baseUri = "http://dry-peak-5299.herokuapp.com/";
		private RestFacilitator restFacilitator;
		private RestService restService;
		private bool shouldPool;

		private TableSource _whiteCardSource;
		private List<TableItemGroup> _whiteCards;
		private string _gameId;
		private Game _game;
		private bool _isCzar;

		public GameViewController () : this(string.Empty){}

		public GameViewController (string gameId) : base("GameViewController", null)
		{
			Title = NSBundle.MainBundle.LocalizedString ("Name", "Name");
			restFacilitator = new RestFacilitator();
			restService = new RestService(restFacilitator, baseUri);
			_gameId = gameId;
			_game = null;
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			_whiteCards = new List<TableItemGroup>();
			_whiteCardSource = new TableSource(_whiteCards);
			WhiteCardTable.Source = _whiteCardSource;

			_whiteCardSource.RowClicked += (cardId) => {
				SelectCard(cardId);
			};
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			shouldPool = true;
			PollGameData();
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			shouldPool = false;
		}

		[Obsolete]
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
			ReleaseDesignerOutlets ();
		}

		[Obsolete]
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}

		void SelectCard (string cardId)
		{
			var asyncDelegation = new AsyncDelegation (restService);

			if (_isCzar) {
				asyncDelegation.Post ("selectWinner", new {gameId = _gameId, cardId = cardId})
				.WhenFinished (() => {
					FetchGame ();
				});
				asyncDelegation.Go ();
			} else {
				asyncDelegation.Post ("selectCard", new {gameId = _gameId, playerId = Application.PlayerId, whiteCardId = cardId})
					.WhenFinished (() => {
						FetchGame ();
					});
				asyncDelegation.Go ();
			}
		}

		void PollGameData ()
		{
			if(!shouldPool) 
				return;

			FetchGame();

			NSTimer.CreateScheduledTimer (3.0, delegate {
				PollGameData();
			});
		}

		void FetchGame ()
		{
			var asyncDelegation = new AsyncDelegation (restService);
			asyncDelegation.Get<Game> ("gamebyid", new {
				id = _gameId,
				playerId = Application.PlayerId
			}).WhenFinished (result =>  {
				InvokeOnMainThread (() =>  {
					UpdateView (result);
				});
			});
			asyncDelegation.Go ();
		}

		void ReadyForNextRound ()
		{
			var asyncDelegation = new AsyncDelegation(restService);
			asyncDelegation.Post("readyForNextRound", new {gameId = _gameId, playerId = Application.PlayerId})
				.WhenFinished(()=> {
					shouldPool = true;
					PollGameData();
				});
			asyncDelegation.Go();
		}

		void UpdateView (Game game)
		{
			Title = game.Name;
			_game = game;
			_whiteCards.Clear ();

			if (game.IsStarted) {
				BlackCard.Text = game.CurrentBlackCard;
			} else {
				BlackCard.Text = "waiting on round to start";
				return;
			}
			
			WhiteCardTable.AllowsSelection = false;
			// Web games Section
			var tGroup = new TableItemGroup{ Name = "Select a card to play"};

			TableItemGroup status = null;
			if (game.Players.Any (p => p.Id == Application.PlayerId)) {

				var player = game.Players.First (p => p.Id == Application.PlayerId);

				if (string.IsNullOrWhiteSpace (player.SelectedWhiteCardId)) {
					tGroup = new TableItemGroup{ Name = "Select a card to play"};
					Title = tGroup.Name;
					foreach (var whiteCard in player.Cards) {
						tGroup.Items.Add (whiteCard);
						tGroup.ItemIds.Add (whiteCard);
						
						WhiteCardTable.AllowsSelection = true;
					}
				} else {
					tGroup = new TableItemGroup{ Name = "You're selection:"};
					tGroup.Items.Add (player.SelectedWhiteCardId);
					status = new TableItemGroup{ Name = "Waiting for the Czar to pick winner..."};
				}

				PointsLabel.Text = string.Format ("{0}", player.AwesomePoints);

				if (!string.IsNullOrWhiteSpace (game.WinningCardId)) {

					if (game.WinningCardId == player.SelectedWhiteCardId) {
						status = new TableItemGroup{ Name = "You're the winner!"};
					} else {
						status = new TableItemGroup{ Name = "Winner Selected"};
						status.Items.Add (game.WinningCardId);
					}

					tGroup = new TableItemGroup{ Name = "Cards Played:"};
					foreach (var oPlayer in game.Players) {
						var card = oPlayer.SelectedWhiteCardId;
						if (string.IsNullOrWhiteSpace (card))
							continue;
						tGroup.Items.Add (card);
					}
				}

				if (game.IsReadyForReview && !player.IsReady) {
					if(status == null)
					{
						status = new TableItemGroup{ Name = "You're the Czar!"};
					}
					shouldPool = false;
					var alert = new UIAlertView(status.Name, "Next round will start when everyone is ready", null, "OK", null);
					alert.Clicked += (sender, e) => {
						ReadyForNextRound();
					};
					alert.Show();
				}
				_isCzar = player.IsCzar;

				if(player.IsCzar){
					status = null;
					Title = "You're the Czar!";
					tGroup = new TableItemGroup{ Name = "Choices so far:"};
					foreach (var oPlayer in game.Players) {
						var card = oPlayer.SelectedWhiteCardId;
						if (string.IsNullOrWhiteSpace (card))
							continue;
						tGroup.Items.Add (card);
						tGroup.ItemIds.Add (card);
					}
					if(game.IsReadyForScoring)
					{
						WhiteCardTable.AllowsSelection = true;
						Title = tGroup.Name = "Select a winner";
					}

					if(game.IsReadyForReview){
						Title = "Round Ended";
						tGroup = new TableItemGroup{ Name = "Next round will start when everyone is ready"};
						status = null;
					}
				}
			}
			if (status != null)
				_whiteCards.Add (status);
			_whiteCards.Add (tGroup);
			WhiteCardTable.ReloadData ();
		}
	}
}

