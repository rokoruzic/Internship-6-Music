using System;
using System.Collections.Generic;
using System.Text;
using DatabaseConnection.Models;

namespace DatabaseConnection.Models
{
	public class Album
	{
		public int MusicianId { get; set; }
		public int AlbumId { get; set; }
		public string Name { get; set; }
		public DateTime DateOfPublish { get; set; }
		public Musician Musician { get; set; }
		public List<Song> Songs { get; set; }

		public Album()
		{
			Songs = new List<Song>();
		}
	}
}
