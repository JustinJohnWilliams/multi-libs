using System;
using System.Collections.Generic;

namespace multilibs
{
	public class Game
	{
		public string Id { get; set; }
		public String Name { get; set; }
		public bool IsOver { get; set; }
		public string winnerId { get; set; }
		public bool IsStarted { get; set; }
		public String CurrentBlackCard { get; set; }
		public bool IsReadyForScoring { get; set; }
		public bool IsReadyForReview { get; set; }

		public List<string> WhiteCards { get { return Deck ["white"]; } }
		public Dictionary<string, List<string>> Deck { get; set; }

		public Game ()
		{
			Deck = new Dictionary<string, List<string>>();
		}
	}
}

