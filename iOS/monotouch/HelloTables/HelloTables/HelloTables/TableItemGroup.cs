using System;
using System.Linq;
using System.Collections.Generic;

namespace HelloTables
{
	public class TableItemGroup
	{
		public string Name { get; set; }
		public string Footer { get; set; }
		public List<string> Items { get; set; }
		
		public TableItemGroup ()
		{
			Items = new List<string>();
		}
	}
}

