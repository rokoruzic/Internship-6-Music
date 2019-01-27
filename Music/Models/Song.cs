using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseConnection.Models
{
	public class Song
	{
		public int SongId { get; set; }
		public string Name { get; set; }
		public int DurationInSeconds { get; set; }
		public List<Album> Albums { get; set; }

		public Song()
		{
			Albums = new List<Album>();
		}
	}
}
