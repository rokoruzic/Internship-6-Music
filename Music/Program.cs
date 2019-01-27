using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Dapper;

namespace Music
{
	class Program
	{
		static void Main(string[] args)
		{
			var connectionString =
				"Data Source=DESKTOP-C0JACUB;Initial Catalog=MusicDatabase;Integrated Security=true;MultipleActiveResultSets=true";


			using (var connection = new SqlConnection(connectionString))
			{
				var musicians = connection.Query<Musician>("select * from musicians").ToList();
				var albums = connection.Query<Album>("select * from albums").ToList();
				var songs = connection.Query<Song>("select * from songs").ToList();
				var albumsSongs = connection.Query<AlbumSong>("select * from albumsSongs").ToList();
				foreach (var album in albums)
				{
					foreach (var musician in musicians)
					{
						if (album.MusicianId == musician.MusicianId)
						{
							musician.Albums.Add(album);
							album.Musician = musician;
						}
					}
				}


				foreach (var albumsSong in albumsSongs)
				{
					foreach (var album in albums)
					{
						if (albumsSong.AlbumId == album.AlbumId)
							albumsSong.Album = album;
					}

					foreach (var song in songs)
					{
						if (albumsSong.SongId == song.SongId)
							albumsSong.Song = song;
					}
				}


				foreach (var albumSongs in albumsSongs)
				{
					foreach (var album in albums)
					{
						if (albumSongs.AlbumId == album.AlbumId)
						{
							album.Songs.Add(albumSongs.Song);
						}
					}

					foreach (var song in songs)
						if (albumSongs.SongId == song.SongId)
							song.Albums.Add(albumSongs.Album);
				}



				//#1 linq
				var sortedMusicians = musicians.OrderBy(x => x.Name).ToList();
				Console.WriteLine("Musicians ordered by their name:");
				sortedMusicians.ForEach(x => Console.WriteLine(x.Name));
				Console.Write("\n");

				//#2 linq
				var musiciansByNationality = musicians.Where(x => x.Nationality == "Croatian").ToList();
				Console.WriteLine("Croatian musicians:");
				musiciansByNationality.ForEach(x => Console.WriteLine(x.Name));
				Console.Write("\n");

				//#3 linq
				var groupedByAlbums = albums
						.GroupBy(x => x.DateOfPublish, y => y, (key, g) => new {DateOfPublish = key, Name = g})
						.ToList();
				foreach (var groupedByAlbum in groupedByAlbums)
					{
					Console.WriteLine("Albums that are published in year '{0}':", groupedByAlbum.DateOfPublish.Year);
						foreach (var nameAlbum in groupedByAlbum.Name)
						{
							Console.WriteLine($"Album name:{nameAlbum.Name} Author: {nameAlbum.Musician.Name}");
						}
					}
				Console.Write("\n");

				//#4 linq
				var albumsWithSpecificName = albums.Where(x => x.Name.Contains("Ne")).ToList();
				Console.WriteLine("Albums that contain word 'Ne' are:");
				albumsWithSpecificName.ForEach(x=>Console.WriteLine(x.Name));
				Console.Write("\n");

				//#5 linq
				var albumsWithSongsDurations = albumsSongs.GroupBy(x => x.Album, y => y.Song.DurationInSeconds,
					(key, s) => new {Album = key, DurationSong = s}).ToList();
			
				foreach (var albumWithSongsDurations in albumsWithSongsDurations)
				{
					Console.WriteLine("Duration of album '{0}':",albumWithSongsDurations.Album.Name);
					var albumSongsDuration = 0;
					foreach (var songsDuration in albumWithSongsDurations.DurationSong)
					{
						albumSongsDuration += songsDuration;
					}

					Console.WriteLine($"{albumSongsDuration} seconds.");
				}
				Console.Write("\n");


				//#6 linq
				var albumsWithSpecificSong = albumsSongs.Where(x => x.Song.Name == "Gadith je noob").ToList();
				Console.WriteLine("Albums that include 'Gadith je noob' song:");
				albumsWithSpecificSong.ForEach(x=>Console.WriteLine(x.Album.Name));
				Console.Write("\n");

				//#7 linq
				var albumsOfMusicianAfterGivenDate = albums.Where(x => x.Musician.Name == "Jevric Ekrem")
					.Where(x => x.DateOfPublish.Year > 2005).ToList();
				Console.WriteLine("Songs from Ekrem Jevric's album that are published after 2005");
				albumsOfMusicianAfterGivenDate.ForEach(x=>x.Songs.ForEach(y=>Console.WriteLine(y.Name)));
				



			}
		}

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

		public class AlbumSong
		{
			public int AlbumId { get; set; }
			public int SongId { get; set; }
			public Song Song { get; set; }
			public Album Album { get; set; }
		}
	}
}