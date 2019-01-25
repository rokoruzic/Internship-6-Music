BEGIN TRANSACTION
CREATE TABLE Musicians(
	MusicianId INT IDENTITY (1,1) PRIMARY KEY,
	Name nvarchar(100),
	Nationality nvarchar(50)
)

CREATE TABLE Albums(
	AlbumId INT IDENTITY (1,1) PRIMARY KEY,
	Name nvarchar(50) NOT NULL,
	DateOfPublish datetime2	NOT NULL,
	MusicianId INT  NOT NULL
	)

CREATE TABLE Songs(
	SongId INT IDENTITY (1,1) PRIMARY KEY,
	Name nvarchar(50) NOT NULL,
	DurationInSeconds int NOT NULL
)

CREATE TABLE AlbumsSongs(
	AlbumId INT FOREIGN KEY REFERENCES Albums(AlbumId),
	SongId INT FOREIGN KEY REFERENCES Songs(SongId)
)
ALTER TABLE Albums
	ADD FOREIGN KEY(MusicianId) REFERENCES Musicians(MusicianId)



INSERT INTO Musicians

VALUES
(
    N'Ljubo iz Siska', N'Serbian'),
	(N'Ekrem Jevric', N'Albanian'),
	(N'Ljubomir bibi Štiber', N'Croatian'
)

INSERT INTO Albums

VALUES
(
    N'Brat Slobo',DATEADD(year, -1,GETDATE()), 1),
	(N'Zlocesti decki',DATEADD(year, -3,GETDATE()), 1),
	(N'Kuca poso',DATEADD(year,-10,GETDATE()),2),
	(N'Švabo Šmit',DATEADD(year,-5,GETDATE()),2),
	(N'Ne mijenjajte me',DATEADD(year,-3,GETDATE()),3),
	(N'Splite moj',DATEADD(year,-2,GETDATE()),3
)	

INSERT INTO Songs

VALUES
(
    N'Lijepa macka',320),
	(N'Ružni pas',460),
	(N'Banana Brain',200),
	(N'Nedostaješ',150),
	(N'Kad te mrma drma',180),
	(N'Kamen i palma',100),
	(N'Kike i Mavro',265),
	(N'Horse with no name',190),
	(N'Twist in my sobriety',292),
	(N'E moj šnicle zagrebacki',200),
	(N'One of us',250),
	(N'Viski',200),
	(N'Gadith je noob',291),
	(N'Balada o najvecem prou, Mnyz-u',800),
	(N'Bohemian rhapsody',612),
	(N'Heart of glass',192),
	(N'Ceca u Parizu',201),
	(N'Krek kuca',509),
	(N'Wow je zakon',300),
	(N'Šta cu ja u Grckoj brate',20),
	(N'Pjesmica o Tontantenu',250),
	(N'E moj šnicle beogradski',321),
	(N'Total eclipse of the heart',152),
	(N'Wonderful life',301
)

INSERT INTO AlbumsSongs

VALUES
(
    1,1), (1,2), (1,5), (1,10), (1,15),
	(2,5), (2,9), (2,12), (2,3), (2,4),
	(3,24), (3,20), (3,13), (3,14), (3,7),
	(4,15), (4,16), (4,6), (4,18), (4, 11),
	(5,5), (5,19), (5,13), (5,12), (5,21),
	(6,9), (6,10), (6,23), (6,17), (6,8
)
COMMIT

 