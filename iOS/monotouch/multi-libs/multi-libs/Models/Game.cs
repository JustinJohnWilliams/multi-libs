using System;
using System.Collections.Generic;

namespace multilibs
{
	public class Game
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public List<Player> Players { get; set; }
		public bool IsOver { get; set; }
		public string WinnerId { get; set; }
		public string WinningCardId { get; set; }
		public bool IsStarted { get; set; }
		public Deck Deck { get; set; }
		public string CurrentBlackCard { get; set; }
		public bool IsReadyForScoring { get; set; }
		public bool IsReadyForReview { get; set; }
		public int PointsToWin { get; set; }

		public Game ()
		{
			Deck = new Deck();
			Players = new List<Player>();
		}
	}

	public class Player
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public bool IsReady { get; set; }
		public string SelectedWhiteCardId { get; set; }
		public int AwesomePoints { get; set; }
		public bool IsCzar { get; set; }
		public List<string> Cards { get; set; }

		public Player()
		{
			Cards = new List<string>();
		}
	}
	
	public class Deck
	{
		public List<string> Black { get; set; }
		public List<string> White { get; set; }

		public Deck()
		{
			Black = new List<string>();
			White = new List<string>();
		}
	}
}

