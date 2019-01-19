-- Script Date: 16/01/2019 11:22 ص  - ErikEJ.SqlCeScripting version 3.5.2.80
CREATE TABLE [User] (
  [Id] int IDENTITY (3,1) NOT NULL
, [UserName] nvarchar(50) NULL
, [FirstName] nvarchar(50) NULL
, [LastName] nvarchar(50) NULL
, [Password] nvarchar(50) NULL
);
GO
CREATE TABLE [Rx] (
  [Id] int IDENTITY (2,1) NOT NULL
, [IMG] ntext NULL
, [CreatedDate] datetime NULL
);
GO
CREATE TABLE [Medicin] (
  [Id] int IDENTITY (3,1) NOT NULL
, [MidicinName] nvarchar(50) NULL
);
GO
CREATE TABLE [Juice] (
  [Id] int IDENTITY (3,1) NOT NULL
, [JuiceName] nvarchar(50) NULL
);
GO
CREATE TABLE [Doctor] (
  [Id] int IDENTITY (3,1) NOT NULL
, [DoctorName] nvarchar(50) NULL
);
GO
CREATE TABLE [Patient] (
  [Id] int IDENTITY (2,1) NOT NULL
, [PatientName] nvarchar(50) NULL
, [DOB] datetime NULL
, [Address] nvarchar(1000) NULL
, [Phone] nvarchar(50) NULL
, [PatientIMG] ntext NULL
, [PatientIdenficationIMG] ntext NULL
, [DoctorId] int NULL
);
GO
CREATE TABLE [PatientVisit] (
  [Id] int IDENTITY (2,1) NOT NULL
, [UserId] int NULL
, [PatientId] int NULL
, [DoctorId] int NULL
, [MedicinId] int NULL
, [JuiceId] int NULL
, [Dose] float NULL
, [Notes] ntext NULL
, [RxId] int NULL
, [DayWeek] int NULL
, [VisitDate] datetime NULL
);
GO
SET IDENTITY_INSERT [User] ON;
GO
INSERT INTO [User] ([Id],[UserName],[FirstName],[LastName],[Password]) VALUES (
1,N'salama',N'Ahmad',N'Salama',N'123456');
GO
INSERT INTO [User] ([Id],[UserName],[FirstName],[LastName],[Password]) VALUES (
2,N'assem',N'Assem',N'El-Sawy',N'123456');
GO
SET IDENTITY_INSERT [User] OFF;
GO
SET IDENTITY_INSERT [Rx] OFF;
GO
SET IDENTITY_INSERT [Medicin] ON;
GO
INSERT INTO [Medicin] ([Id],[MidicinName]) VALUES (
1,N'Methadone');
GO
INSERT INTO [Medicin] ([Id],[MidicinName]) VALUES (
2,N'Antinal');
GO
SET IDENTITY_INSERT [Medicin] OFF;
GO
SET IDENTITY_INSERT [Juice] ON;
GO
INSERT INTO [Juice] ([Id],[JuiceName]) VALUES (
1,N'Orange');
GO
INSERT INTO [Juice] ([Id],[JuiceName]) VALUES (
2,N'Apple');
GO
SET IDENTITY_INSERT [Juice] OFF;
GO
SET IDENTITY_INSERT [Doctor] ON;
GO
INSERT INTO [Doctor] ([Id],[DoctorName]) VALUES (
1,N'Smith');
GO
INSERT INTO [Doctor] ([Id],[DoctorName]) VALUES (
2,N'John');
GO
SET IDENTITY_INSERT [Doctor] OFF;
GO
SET IDENTITY_INSERT [Patient] ON;
GO
INSERT INTO [Patient] ([Id],[PatientName],[DOB],[Address],[Phone],[PatientIMG],[PatientIdenficationIMG],[DoctorId]) VALUES (
1,N'Andrew',{ts '2018-12-12 00:00:00.000'},N'41 manial street',N'125486',N'2569874',NULL,1);
GO
SET IDENTITY_INSERT [Patient] OFF;
GO
SET IDENTITY_INSERT [PatientVisit] ON;
GO
INSERT INTO [PatientVisit] ([Id],[UserId],[PatientId],[DoctorId],[MedicinId],[JuiceId],[Dose],[Notes],[RxId],[DayWeek],[VisitDate]) VALUES (
1,1,1,1,1,1,20.5,N'nothing',NULL,NULL,{ts '2018-12-29 00:00:00.000'});
GO
SET IDENTITY_INSERT [PatientVisit] OFF;
GO
ALTER TABLE [User] ADD CONSTRAINT [PK__tmp_ms_x__3214EC0742827A95] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Rx] ADD CONSTRAINT [PK__tmp_ms_x__3214EC076546B532] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Medicin] ADD CONSTRAINT [PK__Medicin__3214EC07D9E1954E] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Juice] ADD CONSTRAINT [PK__Juice__3214EC07D85AB6FB] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Doctor] ADD CONSTRAINT [PK__Doctor__3214EC072904C5F2] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Patient] ADD CONSTRAINT [PK__tmp_ms_x__3214EC07E4B71D18] PRIMARY KEY ([Id]);
GO
ALTER TABLE [PatientVisit] ADD CONSTRAINT [PK__tmp_ms_x__3214EC07D56FB483] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Patient] ADD CONSTRAINT [FK_Patient_ToDoctor] FOREIGN KEY ([DoctorId]) REFERENCES [Doctor]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [PatientVisit] ADD CONSTRAINT [FK_PatientVisit_ToDoctor] FOREIGN KEY ([DoctorId]) REFERENCES [Doctor]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [PatientVisit] ADD CONSTRAINT [FK_PatientVisit_ToJuice] FOREIGN KEY ([JuiceId]) REFERENCES [Juice]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [PatientVisit] ADD CONSTRAINT [FK_PatientVisit_ToMedicin] FOREIGN KEY ([MedicinId]) REFERENCES [Medicin]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [PatientVisit] ADD CONSTRAINT [FK_PatientVisit_ToPatient] FOREIGN KEY ([PatientId]) REFERENCES [Patient]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [PatientVisit] ADD CONSTRAINT [FK_PatientVisit_ToRx] FOREIGN KEY ([RxId]) REFERENCES [Rx]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [PatientVisit] ADD CONSTRAINT [FK_PatientVisit_ToUser] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

