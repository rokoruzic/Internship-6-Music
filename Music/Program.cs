using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using DatabaseConnection.Models;

namespace DatabaseConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString =
                "Data Source=DESKTOP-H2HCQOK;Initial Catalog=MusicDatabase;Integrated Security=true;MultipleActiveResultSets=true";
            using (var connection = new SqlConnection(connectionString))
            {
                var musicians = connection.Query<Musician>("select * from musicians").ToList();
                var albums = connection.Query<Album>("select * from albums").ToList();
                var songs = connection.Query<Song>("select * from songs").ToList();
                var albumsSongs = connection.Query<AlbumSong>("select * from albumsSongs").ToList();

                foreach (var album in albums)
                {
                    var musician = musicians.FirstOrDefault(x => x.MusicianId == album.MusicianId);
                    musician.Albums.Add(album);
                    album.Musician = musician;
                }

                foreach (var albumsSong in albumsSongs)
                {
                    var album = albums.FirstOrDefault(x => x.AlbumId == albumsSong.AlbumId);
                    var song = songs.FirstOrDefault(x => x.SongId == albumsSong.SongId);
                    song.Albums.Add(album);
                    album.Songs.Add(song);
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
                    .GroupBy(x => x.DateOfPublish, y => y, (key, g) => new {DateOfPublish = key, Name = g}).ToList();
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
                albumsWithSpecificName.ForEach(x => Console.WriteLine(x.Name));
                Console.Write("\n");

                //#5 linq
                var albumsWithSongsDuration =
                    albums.GroupBy(x => x.Songs, y => y.Name, (key, s) => new {song = key, album = s}).ToList();
                foreach (var albumWithSongsDuration in albumsWithSongsDuration)
                {
                    foreach (var albumName in albumWithSongsDuration.album)
                    {
                        Console.WriteLine("Duration of album '{0}':", albumName);
                        var albumSongsDuration = 0;
                        foreach (var songsDuration in albumWithSongsDuration.song)
                        {
                            albumSongsDuration += songsDuration.DurationInSeconds;
                        }

                        Console.WriteLine($"{albumSongsDuration} seconds.");
                    }
                }

                Console.Write("\n");

                //#6 linq
                Console.WriteLine("Albums that include 'Gadith je noob' song:");
                var songsWithSpecificName = songs.Where(x => x.Name == "Gadith je noob").ToList();
                songsWithSpecificName.ForEach(x => x.Albums.ForEach(y => Console.WriteLine(y.Name)));
                Console.Write("\n");

                //#7 linq
                var albumsOfMusicianAfterGivenDate = albums.Where(x => x.Musician.Name == "Jevric Ekrem")
                    .Where(x => x.DateOfPublish.Year > 2005).ToList();
                Console.WriteLine("Songs from Ekrem Jevric's album that are published after 2005");
                albumsOfMusicianAfterGivenDate.ForEach(x => x.Songs.ForEach(y => Console.WriteLine(y.Name)));
            }
        }
    }
}