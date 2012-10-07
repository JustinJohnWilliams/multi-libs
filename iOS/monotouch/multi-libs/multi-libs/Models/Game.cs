using System;

namespace multilibs
{
	public class Game
	{
		public Guid Id { get; set; }
		public String Name { get; set; }
		public bool IsOver { get; set; }
		public Guid winnerId { get; set; }
		public bool IsStarted { get; set; }
		public String CurrentBlackCard { get; set; }
		public bool IsReadyForScoring { get; set; }
		public bool IsReadyForReview { get; set; }

		public Game ()
		{
		}
	}
}

