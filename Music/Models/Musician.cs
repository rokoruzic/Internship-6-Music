﻿using System.Collections.Generic;

namespace DatabaseConnection.Models
{
	public class Musician
	{
		public int MusicianId { get; set; }
		public string Name { get; set; }
		public string Nationality { get; set; }
		public List<Album> Albums { get; set; }

		public Musician()
		{
			Albums = new List<Album>();
		}
	}
}
