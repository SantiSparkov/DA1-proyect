-- DROP SCHEMA dbo;

--CREATE SCHEMA dbo;
-- TaskPanel.dbo.Teams definition

-- Drop table

-- DROP TABLE TaskPanel.dbo.Teams;

CREATE TABLE TaskPanel.dbo.Teams (
	Id int IDENTITY(1,1) NOT NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CreationDate datetime2 NOT NULL,
	TasksDescription nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	MaxAmountOfMembers int NOT NULL,
	TeamLeaderId int NOT NULL,
	CONSTRAINT PK_Teams PRIMARY KEY (Id)
);


-- TaskPanel.dbo.Trashes definition

-- Drop table

-- DROP TABLE TaskPanel.dbo.Trashes;

CREATE TABLE TaskPanel.dbo.Trashes (
	Id int IDENTITY(1,1) NOT NULL,
	UserId int NOT NULL,
	Elements int NOT NULL,
	CONSTRAINT PK_Trashes PRIMARY KEY (Id)
);


-- TaskPanel.dbo.Users definition

-- Drop table

-- DROP TABLE TaskPanel.dbo.Users;

CREATE TABLE TaskPanel.dbo.Users (
	Id int IDENTITY(1,1) NOT NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Email nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Password nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	LastName nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	BirthDate datetime2 NOT NULL,
	IsAdmin bit NOT NULL,
	TrashId int NOT NULL,
	CONSTRAINT PK_Users PRIMARY KEY (Id)
);


-- TaskPanel.dbo.[__EFMigrationsHistory] definition

-- Drop table

-- DROP TABLE TaskPanel.dbo.[__EFMigrationsHistory];

CREATE TABLE TaskPanel.dbo.[__EFMigrationsHistory] (
	MigrationId nvarchar(150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ProductVersion nvarchar(32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY (MigrationId)
);


-- TaskPanel.dbo.Notifications definition

-- Drop table

-- DROP TABLE TaskPanel.dbo.Notifications;

CREATE TABLE TaskPanel.dbo.Notifications (
	Id int IDENTITY(1,1) NOT NULL,
	Message nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	UserId int NOT NULL,
	CONSTRAINT PK_Notifications PRIMARY KEY (Id),
	CONSTRAINT FK_Notifications_Users_UserId FOREIGN KEY (UserId) REFERENCES TaskPanel.dbo.Users(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_Notifications_UserId ON dbo.Notifications (  UserId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- TaskPanel.dbo.Panels definition

-- Drop table

-- DROP TABLE TaskPanel.dbo.Panels;

CREATE TABLE TaskPanel.dbo.Panels (
	Id int IDENTITY(1,1) NOT NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	TeamId int NOT NULL,
	Description nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	IsDeleted bit NOT NULL,
	CreatorId int NOT NULL,
	TrashId int NULL,
	CONSTRAINT PK_Panels PRIMARY KEY (Id),
	CONSTRAINT FK_Panels_Teams_TeamId FOREIGN KEY (TeamId) REFERENCES TaskPanel.dbo.Teams(Id) ON DELETE CASCADE,
	CONSTRAINT FK_Panels_Trashes_TrashId FOREIGN KEY (TrashId) REFERENCES TaskPanel.dbo.Trashes(Id)
);
 CREATE NONCLUSTERED INDEX IX_Panels_TeamId ON dbo.Panels (  TeamId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Panels_TrashId ON dbo.Panels (  TrashId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- TaskPanel.dbo.TeamUser definition

-- Drop table

-- DROP TABLE TaskPanel.dbo.TeamUser;

CREATE TABLE TaskPanel.dbo.TeamUser (
	TeamsId int NOT NULL,
	UsersId int NOT NULL,
	CONSTRAINT PK_TeamUser PRIMARY KEY (TeamsId,UsersId),
	CONSTRAINT FK_TeamUser_Teams_TeamsId FOREIGN KEY (TeamsId) REFERENCES TaskPanel.dbo.Teams(Id) ON DELETE CASCADE,
	CONSTRAINT FK_TeamUser_Users_UsersId FOREIGN KEY (UsersId) REFERENCES TaskPanel.dbo.Users(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_TeamUser_UsersId ON dbo.TeamUser (  UsersId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- TaskPanel.dbo.Epics definition

-- Drop table

-- DROP TABLE TaskPanel.dbo.Epics;

CREATE TABLE TaskPanel.dbo.Epics (
	Id int IDENTITY(1,1) NOT NULL,
	Title nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Priority int NOT NULL,
	Description nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	DueDateTime datetime2 NOT NULL,
	PanelId int NOT NULL,
	IsDeleted bit NOT NULL,
	TrashId int NULL,
	CONSTRAINT PK_Epics PRIMARY KEY (Id),
	CONSTRAINT FK_Epics_Panels_PanelId FOREIGN KEY (PanelId) REFERENCES TaskPanel.dbo.Panels(Id) ON DELETE CASCADE,
	CONSTRAINT FK_Epics_Trashes_TrashId FOREIGN KEY (TrashId) REFERENCES TaskPanel.dbo.Trashes(Id)
);
 CREATE NONCLUSTERED INDEX IX_Epics_PanelId ON dbo.Epics (  PanelId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Epics_TrashId ON dbo.Epics (  TrashId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- TaskPanel.dbo.Tasks definition

-- Drop table

-- DROP TABLE TaskPanel.dbo.Tasks;

CREATE TABLE TaskPanel.dbo.Tasks (
	Id int IDENTITY(1,1) NOT NULL,
	PanelId int NOT NULL,
	Title nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Description nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	DueDate datetime2 NOT NULL,
	Priority int NOT NULL,
	IsDeleted bit NOT NULL,
	EpicId int NULL,
	EstimatioHour int NOT NULL,
	InvertedEstimateHour int NOT NULL,
	TrashId int NULL,
	CONSTRAINT PK_Tasks PRIMARY KEY (Id),
	CONSTRAINT FK_Tasks_Epics_EpicId FOREIGN KEY (EpicId) REFERENCES TaskPanel.dbo.Epics(Id),
	CONSTRAINT FK_Tasks_Panels_PanelId FOREIGN KEY (PanelId) REFERENCES TaskPanel.dbo.Panels(Id) ON DELETE CASCADE,
	CONSTRAINT FK_Tasks_Trashes_TrashId FOREIGN KEY (TrashId) REFERENCES TaskPanel.dbo.Trashes(Id)
);
 CREATE NONCLUSTERED INDEX IX_Tasks_EpicId ON dbo.Tasks (  EpicId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Tasks_PanelId ON dbo.Tasks (  PanelId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Tasks_TrashId ON dbo.Tasks (  TrashId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- TaskPanel.dbo.Comments definition

-- Drop table

-- DROP TABLE TaskPanel.dbo.Comments;

CREATE TABLE TaskPanel.dbo.Comments (
	Id int IDENTITY(1,1) NOT NULL,
	TaskId int NOT NULL,
	Message nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CreatedById int NOT NULL,
	ResolvedById int NULL,
	ResolvedAt datetime2 NULL,
	Status int NOT NULL,
	CONSTRAINT PK_Comments PRIMARY KEY (Id),
	CONSTRAINT FK_Comments_Tasks_TaskId FOREIGN KEY (TaskId) REFERENCES TaskPanel.dbo.Tasks(Id) ON DELETE CASCADE,
	CONSTRAINT FK_Comments_Users_CreatedById FOREIGN KEY (CreatedById) REFERENCES TaskPanel.dbo.Users(Id) ON DELETE CASCADE,
	CONSTRAINT FK_Comments_Users_ResolvedById FOREIGN KEY (ResolvedById) REFERENCES TaskPanel.dbo.Users(Id)
);
 CREATE NONCLUSTERED INDEX IX_Comments_CreatedById ON dbo.Comments (  CreatedById ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Comments_ResolvedById ON dbo.Comments (  ResolvedById ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Comments_TaskId ON dbo.Comments (  TaskId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


INSERT INTO TaskPanel.dbo.Teams (Name,CreationDate,TasksDescription,MaxAmountOfMembers,TeamLeaderId) VALUES
	 (N'Equipo 1','2024-11-20 00:10:05.7832483',N'Este es el primer equipo de prueba',5,1),
	 (N'Equipo 2','2024-11-20 00:10:19.5677750',N'Este es el segundo equipo de pruebas.',2,1),
     (N'Equipo 3','2024-11-20 00:10:19.5677750',N'Este es el tercer equipo de pruebas.',3,1),
     (N'Equipo 4','2024-11-20 00:10:19.5677750',N'Este es el cuarto equipo de pruebas.',2,1),
     (N'Equipo 5','2024-11-20 00:10:19.5677750',N'Este es el quinto equipo de pruebas.',2,1),
     (N'Equipo 6','2024-11-20 00:10:19.5677750',N'Este es el sexto equipo de pruebas.',2,1),
     (N'Equipo 7','2024-11-20 00:10:19.5677750',N'Este es el septimo equipo de pruebas.',2,1),
     (N'Equipo 8','2024-11-20 00:10:19.5677750',N'Este es el octavo equipo de pruebas.',2,1),
     (N'Equipo 9','2024-11-20 00:10:19.5677750',N'Este es el noveno equipo de pruebas.',2,1),
     (N'Equipo 10','2024-11-20 00:10:19.5677750',N'Este es el decimo equipo de pruebas.',2,1);

INSERT INTO TaskPanel.dbo.Trashes (UserId,Elements) VALUES
	 (1,0),
	 (2,0),
	 (3,0),
	 (4,0);

INSERT INTO TaskPanel.dbo.Users (Name,Email,Password,LastName,BirthDate,IsAdmin,TrashId) VALUES
	 (N'Admin',N'admin@admin.com',N'Aa1@',N'User','1990-01-01 00:00:00.0000000',1,1),
	 (N'Francisco',N'francisco.suarez@gmail.com',N'Z0t]2}r3',N'Suarez','1900-01-01 00:00:00.0000000',0,2),
	 (N'Santiago',N'santiago.sparkov@gmail.com',N'SQ4}&Bbi',N'Sparkov','1900-01-01 00:00:00.0000000',0,3),
	 (N'Francisco',N'francisco.sosa@gmail.com',N'$s;>#S+2',N'Sosa','1900-01-01 00:00:00.0000000',1,4);

INSERT INTO TaskPanel.dbo.Panels (Name,TeamId,Description,IsDeleted,CreatorId,TrashId) VALUES
	 (N'Panel 1',1,N'Este es el primer panel de pruebas.',0,1,NULL),
	 (N'Panel 2',2,N'Este es el segundo panel de pruebas.',0,1,NULL),
     (N'Panel 3',1,N'Este es el tercer panel de pruebas.',0,1,NULL),
     (N'Panel 4',2,N'Este es el cuarto panel de pruebas.',0,1,NULL),
     (N'Panel 5',1,N'Este es el quinto panel de pruebas.',0,1,NULL),
     (N'Panel 6',2,N'Este es el sexto panel de pruebas.',0,1,NULL);

INSERT INTO TaskPanel.dbo.TeamUser (TeamsId,UsersId) VALUES
	 (1,1),
     (1,2),
     (1,3),
     (1,4),
	 (2,1),
	 (2,3),
     (3,2),
     (3,3),
     (4,3),
     (4,4),
     (5,4),
     (5,1),
     (6,1),
     (6,3),
     (7,3),
     (7,2),
     (8,3),
     (8,2),
     (9,1),
     (9,3),
     (10,4),
     (10,2);

INSERT INTO TaskPanel.dbo.Epics (Title,Priority,Description,DueDateTime,PanelId,IsDeleted,TrashId) VALUES
	 (N'Epic 1',0,N'Epica de prueba','2024-11-21 00:12:57.7510751',1,0,NULL),
	 (N'Epic 2',0,N'Epica de prueba','2024-11-21 00:13:08.9343602',2,0,NULL),
    (N'Epic 3',0,N'Epica de prueba','2024-11-21 00:13:08.9343602',3,0,NULL);

INSERT INTO TaskPanel.dbo.Tasks (PanelId,Title,Description,DueDate,Priority,IsDeleted,EpicId,EstimatioHour,InvertedEstimateHour,TrashId) VALUES
	 (1,N'Task 1',N'Descripcion tarea 1','2024-12-15 00:00:00.0000000',0,0,NULL,2,2,NULL),
	 (2,N'Task 2',N'Descripcion tarea 2','2024-12-01 00:00:00.0000000',1,0,1,5,3,NULL),
	 (3,N'Task 3',N'Descripcion tarea 3','2024-12-10 00:00:00.0000000',2,0,2,1,1,NULL),
	 (4,N'Task 4',N'Descripcion tarea 4','2024-11-21 00:00:00.0000000',0,0,NULL,1,1,NULL),
	 (5,N'Task 5',N'Descripcion tarea 5','2024-12-15 00:00:00.0000000',0,0,NULL,0,0,NULL),
	 (6,N'Task 6',N'Descripcion tarea 6','2024-12-01 00:00:00.0000000',1,0,1,0,0,NULL),
	 (1,N'Task 7',N'Descripcion tarea 7','2024-12-10 00:00:00.0000000',2,0,3,3,2,NULL),
     (2,N'Task 8',N'Descripcion tarea 8','2024-12-10 00:00:00.0000000',2,0,NULL,0,0,NULL),
     (3,N'Task 9',N'Descripcion tarea 9','2024-12-10 00:00:00.0000000',2,0,1,2,2,NULL),
     (4,N'Task 10',N'Descripcion tarea 10','2024-12-10 00:00:00.0000000',2,0,1,2,1,NULL),
     (5,N'Task 11',N'Descripcion tarea 11','2024-12-10 00:00:00.0000000',2,0,3,1,1,NULL),
     (6,N'Task 12',N'Descripcion tarea 12','2024-12-10 00:00:00.0000000',2,0,1,0,0,NULL),
     (1,N'Task 13',N'Descripcion tarea 13','2024-12-10 00:00:00.0000000',2,0,NULL,1,1,NULL),
     (2,N'Task 14',N'Descripcion tarea 14','2024-12-10 00:00:00.0000000',2,0,1,3,1,NULL),
     (3,N'Task 15',N'Descripcion tarea 15','2024-12-10 00:00:00.0000000',2,0,3,3,2,NULL),
     (4,N'Task 16',N'Descripcion tarea 16','2024-12-10 00:00:00.0000000',2,0,2,2,2,NULL),
     (5,N'Task 17',N'Descripcion tarea 17','2024-12-10 00:00:00.0000000',2,0,2,0,0,NULL),
     (6,N'Task 18',N'Descripcion tarea 18','2024-12-10 00:00:00.0000000',2,0,3,1,1,NULL),
     (1,N'Task 19',N'Descripcion tarea 19','2024-12-10 00:00:00.0000000',2,0,1,1,1,NULL),
     (2,N'Task 20',N'Descripcion tarea 20','2024-12-10 00:00:00.0000000',2,0,1,0,0,NULL);

INSERT INTO TaskPanel.dbo.Comments (TaskId,Message,CreatedById,ResolvedById,ResolvedAt,Status) VALUES
	 (1,N'Comentario test',1,NULL,NULL,1),
     (2,N'Comentario test',1,NULL,NULL,1),
     (3,N'Comentario test',1,NULL,NULL,1),
     (2,N'Comentario test',1,NULL,NULL,1),
     (3,N'Comentario test',1,NULL,NULL,1),
     (1,N'Comentario test',1,NULL,NULL,1),
     (2,N'Comentario test',1,NULL,NULL,1),
     (3,N'Comentario test',1,NULL,NULL,1),
     (1,N'Comentario test',1,NULL,NULL,1),
     (1,N'Comentario test',1,NULL,NULL,1);

INSERT INTO TaskPanel.dbo.Notifications (Message,UserId) VALUES
    (N'Comment has been resolved. Message: asdf',1);