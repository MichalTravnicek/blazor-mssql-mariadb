CREATE DATABASE Application
GO
CREATE DATABASE Test
GO
USE Application
GO
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_SCHEMA = 'dbo'
      AND TABLE_TYPE = 'BASE TABLE'
      AND TABLE_NAME = 'Employee'
)
    BEGIN
        CREATE TABLE Employee (
                                  PKGUID UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Employee
                                      PRIMARY KEY NONCLUSTERED DEFAULT (newsequentialid()),
                                  Id INTEGER NOT NULL IDENTITY(1,1),
                                  Avatar VARCHAR(max) NULL,
                                  Department VARCHAR(max) NULL,
                                  Email VARCHAR(max) NULL,
                                  FirstName VARCHAR(max) NULL,
                                  LastName VARCHAR(max) NULL
        )
    
CREATE UNIQUE CLUSTERED INDEX CIX_Employee ON dbo.Employee(Id)

BEGIN TRANSACTION;
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/autautea.png?size=100x100&set=set1','Factory3','nmorgue0@disqus.com','Nonie','Morgue');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/atquerepudiandaecorporis.png?size=100x100&set=set1','Research and Development','owilliamson2@washingtonpost.com','Or
ville','Williamson');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/liberotemporibusalias.png?size=100x100&set=set1','Product Management','ditzkovici3@phpbb.com','Dwayne','Itzkovici');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/enimsapienteillum.png?size=100x100&set=set1','Research and Development','sjerisch4@craigslist.org','Shelbi','Jerisch
');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/omnisillumnostrum.png?size=100x100&set=set1','Services','jwitcherley5@1und1.de','Jerrilee','Witcherley');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/adipiscisequidolor.png?size=100x100&set=set1','Marketing','ckobpac6@goo.ne.jp','Caritta','Kobpac');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/aliquamquassunt.png?size=100x100&set=set1','Human Resources','qmccague7@facebook.com','Quincey','McCague');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/eaquiserror.png?size=100x100&set=set1','Accounting','acundey8@yahoo.com','Aharon','Cundey');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/dolorconsecteturdeleniti.png?size=100x100&set=set1','Services','nanthill9@posterous.com','Neila','Anthill');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/veroculpadolore.png?size=100x100&set=set1','Business Development','cbroadera@theglobeandmail.com','Cinnamon','Broad
er');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/autveritatisquibusdam.png?size=100x100&set=set1','Business Development','gtampinb@example.com','Godfrey','Tampin');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/utvoluptasdolorum.png?size=100x100&set=set1','Support','oandrellic@t-online.de','Olimpia','Andrelli');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/odioinqui.png?size=100x100&set=set1','Human Resources','gebblesd@fema.gov','Grissel','Ebbles');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/saepedelenitivoluptas.png?size=100x100&set=set1','Sales','bsutherleye@bing.com','Beckie','Sutherley');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/aspernaturautdicta.png?size=100x100&set=set1','Sales','dschohierf@dion.ne.jp','Drusie','Schohier');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/dolornobisratione.png?size=100x100&set=set1','Sales','aheisterg@soup.io','Aida','Heister');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/estnihilsuscipit.png?size=100x100&set=set1','Services','vleakeh@umn.edu','Valentia','Leake');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/abperspiciatismagnam.png?size=100x100&set=set1','Training','ogellatelyi@spotify.com','Octavius','Gellately');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/blanditiisvoluptasrerum.png?size=100x100&set=set1','Product Management','emandyj@webs.com','Erwin','Mandy');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/auteavel.png?size=100x100&set=set1','Engineering','adevenportk@google.fr','Armand','Devenport');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/consequuntureveniethic.png?size=100x100&set=set1','Research and Development','btorell@mit.edu','Bartel','Torel');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/fugitmagnamhic.png?size=100x100&set=set1','Research and Development','aludgatem@ning.com','Auberta','Ludgate');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/voluptatumetet.png?size=100x100&set=set1','Support','csendalln@bizjournals.com','Claudian','Sendall');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/cupiditateestsoluta.png?size=100x100&set=set1','Engineering','sdarrello@auda.org.au','Saw','Darrell');
INSERT INTO Employee VALUES (DEFAULT,'https://robohash.org/quosconsequaturqui.png?size=100x100&set=set1','Research and Development','anarracottp@dedecms.com','Arv','Narracott
');
COMMIT;
END
GO
