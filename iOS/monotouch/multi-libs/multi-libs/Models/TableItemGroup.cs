using System;
using System.Linq;
using System.Collections.Generic;

namespace multilibs
{
	public class TableItemGroup
	{
		public string Name { get; set; }
		public string Footer { get; set; }
		public List<string> Items { get; set; }
		public List<string> ItemIds { get; set; }

		public TableItemGroup ()
		{
			Items = new List<string>();
			ItemIds = new List<string>();
		}
	}
}

