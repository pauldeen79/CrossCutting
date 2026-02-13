--****** Object:  Table [dbo].[Catalog] ******
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Catalog](
	[Id] int IDENTITY(1, 1) NOT NULL,
	[Name] varchar(64) NOT NULL,
	[DateCreated] datetime NOT NULL,
	[DateLastModified] datetime NOT NULL,
	[DateSynchronized] datetime NOT NULL,
	[DriveSerialNumber] varchar(9) NOT NULL,
	[DriveTypeCodeType] varchar(3) NOT NULL,
	[DriveTypeCode] varchar(3) NOT NULL,
	[DriveTotalSize] int NOT NULL,
	[DriveFreeSpace] int NOT NULL,
	[Recursive] bit NOT NULL,
	[Sorted] bit NOT NULL,
	[StartDirectory] varchar(512) NOT NULL,
	[ExtraField1] varchar(64) NULL,
	[ExtraField2] varchar(64) NULL,
	[ExtraField3] varchar(64) NULL,
	[ExtraField4] varchar(64) NULL,
	[ExtraField5] varchar(64) NULL,
	[ExtraField6] varchar(64) NULL,
	[ExtraField7] varchar(64) NULL,
	[ExtraField8] varchar(64) NULL,
	[ExtraField9] varchar(64) NULL,
	[ExtraField10] varchar(64) NULL,
	[ExtraField11] varchar(64) NULL,
	[ExtraField12] varchar(64) NULL,
	[ExtraField13] varchar(64) NULL,
	[ExtraField14] varchar(64) NULL,
	[ExtraField15] varchar(64) NULL,
	[ExtraField16] varchar(64) NULL,
 CONSTRAINT [PK_Catalog] PRIMARY KEY CLUSTERED
(
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_Catalog_Name] ON [dbo].[Catalog]
(
	[Name] ASC
) ON [PRIMARY]
GO
ALTER TABLE [Catalog] ADD CONSTRAINT [DF_DriveTypeCodeType] DEFAULT ('CDT') FOR [DriveTypeCodeType]
GO
--****** Object:  Table [dbo].[File] ******
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[File](
	[Id] int IDENTITY(1, 1) NOT NULL,
	[Path] varchar(512) NOT NULL,
	[FileName] varchar(128) NOT NULL,
	[Extension] varchar(12) NOT NULL,
	[Date] datetime NOT NULL,
	[Size] bigint NOT NULL,
	[CatalogId] int NOT NULL,
	[ExtraField1] varchar(64) NULL,
	[ExtraField2] varchar(64) NULL,
	[ExtraField3] varchar(64) NULL,
	[ExtraField4] varchar(64) NULL,
	[ExtraField5] varchar(64) NULL,
	[ExtraField6] varchar(64) NULL,
	[ExtraField7] varchar(64) NULL,
	[ExtraField8] varchar(64) NULL,
	[ExtraField9] varchar(64) NULL,
	[ExtraField10] varchar(64) NULL,
	[ExtraField11] varchar(64) NULL,
	[ExtraField12] varchar(64) NULL,
	[ExtraField13] varchar(64) NULL,
	[ExtraField14] varchar(64) NULL,
	[ExtraField15] varchar(64) NULL,
	[ExtraField16] varchar(64) NULL,
 CONSTRAINT [PK_File] PRIMARY KEY CLUSTERED
(
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_File_Path] ON [dbo].[File]
(
	[Path] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_File_FileName] ON [dbo].[File]
(
	[Path] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_File_Extension] ON [dbo].[File]
(
	[Path] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_File_Date] ON [dbo].[File]
(
	[Path] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_File_CatalogId] ON [dbo].[File]
(
	[Path] ASC
) ON [PRIMARY]
GO
--****** Object:  Table [dbo].[Code] ******
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Code](
	[Code] varchar(3) NOT NULL,
	[CodeType] varchar(3) NOT NULL,
	[Description] varchar(512) NULL,
 CONSTRAINT [PK_Code] PRIMARY KEY CLUSTERED
(
	[CodeType] ASC,
	[Code] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
--****** Object:  Table [dbo].[ExtraField] ******
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ExtraField](
	[EntityName] varchar(64) NOT NULL,
	[Name] varchar(64) NOT NULL,
	[Description] varchar(512) NULL,
	[FieldNumber] tinyint NULL,
	[FieldType] varchar(64) NOT NULL,
 CONSTRAINT [PK_ExtraField] PRIMARY KEY CLUSTERED
(
	[EntityName] ASC,
	[Name] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
--****** Object:  ForeignKey [Catalog_DriveType] ******
ALTER TABLE [dbo].[Catalog]  WITH CHECK ADD  CONSTRAINT [Catalog_DriveType] FOREIGN KEY([DriveTypeCodeType],[DriveTypeCode])
REFERENCES [dbo].[Code] ([CodeType],[Code])
ON UPDATE NO ACTION
ON DELETE NO ACTION
GO
ALTER TABLE [dbo].[Catalog] CHECK CONSTRAINT [Catalog_DriveType]
GO
--****** Object:  ForeignKey [File_Catalog] ******
ALTER TABLE [dbo].[File]  WITH CHECK ADD  CONSTRAINT [File_Catalog] FOREIGN KEY([CatalogId])
REFERENCES [dbo].[Catalog] ([Id])
ON UPDATE NO ACTION
ON DELETE NO ACTION
GO
ALTER TABLE [dbo].[File] CHECK CONSTRAINT [File_Catalog]
GO
--****** Object:  StoredProcedure [dbo].[InsertCatalog] ******
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertCatalog]
	@Name varchar(64),
	@DateCreated datetime,
	@DateLastModified datetime,
	@DateSynchronized datetime,
	@DriveSerialNumber varchar(9),
	@DriveTypeCodeType varchar(3),
	@DriveTypeCode varchar(3),
	@DriveTotalSize int,
	@DriveFreeSpace int,
	@Recursive bit,
	@Sorted bit,
	@StartDirectory varchar(512),
	@ExtraField1 varchar(64),
	@ExtraField2 varchar(64),
	@ExtraField3 varchar(64),
	@ExtraField4 varchar(64),
	@ExtraField5 varchar(64),
	@ExtraField6 varchar(64),
	@ExtraField7 varchar(64),
	@ExtraField8 varchar(64),
	@ExtraField9 varchar(64),
	@ExtraField10 varchar(64),
	@ExtraField11 varchar(64),
	@ExtraField12 varchar(64),
	@ExtraField13 varchar(64),
	@ExtraField14 varchar(64),
	@ExtraField15 varchar(64),
	@ExtraField16 varchar(64)
AS
BEGIN

INSERT INTO [Catalog] ([Name], [DateCreated], [DateLastModified], [DateSynchronized], [DriveSerialNumber], [DriveTypeCodeType], [DriveTypeCode], [DriveTotalSize], [DriveFreeSpace], [Recursive], [Sorted], [StartDirectory], [ExtraField1], [ExtraField2], [ExtraField3], [ExtraField4], [ExtraField5], [ExtraField6], [ExtraField7], [ExtraField8], [ExtraField9], [ExtraField10], [ExtraField11], [ExtraField12], [ExtraField13], [ExtraField14], [ExtraField15], [ExtraField16]) OUTPUT INSERTED.[Id], INSERTED.[Name], INSERTED.[DateCreated], INSERTED.[DateLastModified], INSERTED.[DateSynchronized], INSERTED.[DriveSerialNumber], INSERTED.[DriveTypeCodeType], INSERTED.[DriveTypeCode], INSERTED.[DriveTotalSize], INSERTED.[DriveFreeSpace], INSERTED.[Recursive], INSERTED.[Sorted], INSERTED.[StartDirectory], INSERTED.[ExtraField1], INSERTED.[ExtraField2], INSERTED.[ExtraField3], INSERTED.[ExtraField4], INSERTED.[ExtraField5], INSERTED.[ExtraField6], INSERTED.[ExtraField7], INSERTED.[ExtraField8], INSERTED.[ExtraField9], INSERTED.[ExtraField10], INSERTED.[ExtraField11], INSERTED.[ExtraField12], INSERTED.[ExtraField13], INSERTED.[ExtraField14], INSERTED.[ExtraField15], INSERTED.[ExtraField16] VALUES (@Name, @DateCreated, @DateLastModified, @DateSynchronized, @DriveSerialNumber, @DriveTypeCodeType, @DriveTypeCode, @DriveTotalSize, @DriveFreeSpace, @Recursive, @Sorted, @StartDirectory, @ExtraField1, @ExtraField2, @ExtraField3, @ExtraField4, @ExtraField5, @ExtraField6, @ExtraField7, @ExtraField8, @ExtraField9, @ExtraField10, @ExtraField11, @ExtraField12, @ExtraField13, @ExtraField14, @ExtraField15, @ExtraField16)

END
GO
--****** Object:  StoredProcedure [dbo].[UpdateCatalog] ******
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateCatalog]
	@Name varchar(64),
	@DateCreated datetime,
	@DateLastModified datetime,
	@DateSynchronized datetime,
	@DriveSerialNumber varchar(9),
	@DriveTypeCodeType varchar(3),
	@DriveTypeCode varchar(3),
	@DriveTotalSize int,
	@DriveFreeSpace int,
	@Recursive bit,
	@Sorted bit,
	@StartDirectory varchar(512),
	@ExtraField1 varchar(64),
	@ExtraField2 varchar(64),
	@ExtraField3 varchar(64),
	@ExtraField4 varchar(64),
	@ExtraField5 varchar(64),
	@ExtraField6 varchar(64),
	@ExtraField7 varchar(64),
	@ExtraField8 varchar(64),
	@ExtraField9 varchar(64),
	@ExtraField10 varchar(64),
	@ExtraField11 varchar(64),
	@ExtraField12 varchar(64),
	@ExtraField13 varchar(64),
	@ExtraField14 varchar(64),
	@ExtraField15 varchar(64),
	@ExtraField16 varchar(64),
	@IdOriginal int,
	@NameOriginal varchar(64),
	@DateCreatedOriginal datetime,
	@DateLastModifiedOriginal datetime,
	@DateSynchronizedOriginal datetime,
	@DriveSerialNumberOriginal varchar(9),
	@DriveTypeCodeTypeOriginal varchar(3),
	@DriveTypeCodeOriginal varchar(3),
	@DriveTotalSizeOriginal int,
	@DriveFreeSpaceOriginal int,
	@RecursiveOriginal bit,
	@SortedOriginal bit,
	@StartDirectoryOriginal varchar(512),
	@ExtraField1Original varchar(64),
	@ExtraField2Original varchar(64),
	@ExtraField3Original varchar(64),
	@ExtraField4Original varchar(64),
	@ExtraField5Original varchar(64),
	@ExtraField6Original varchar(64),
	@ExtraField7Original varchar(64),
	@ExtraField8Original varchar(64),
	@ExtraField9Original varchar(64),
	@ExtraField10Original varchar(64),
	@ExtraField11Original varchar(64),
	@ExtraField12Original varchar(64),
	@ExtraField13Original varchar(64),
	@ExtraField14Original varchar(64),
	@ExtraField15Original varchar(64),
	@ExtraField16Original varchar(64)
AS
BEGIN

UPDATE [Catalog] SET [Name] = @Name, [DateCreated] = @DateCreated, [DateLastModified] = @DateLastModified, [DateSynchronized] = @DateSynchronized, [DriveSerialNumber] = @DriveSerialNumber, [DriveTypeCodeType] = @DriveTypeCodeType, [DriveTypeCode] = @DriveTypeCode, [DriveTotalSize] = @DriveTotalSize, [DriveFreeSpace] = @DriveFreeSpace, [Recursive] = @Recursive, [Sorted] = @Sorted, [StartDirectory] = @StartDirectory, [ExtraField1] = @ExtraField1, [ExtraField2] = @ExtraField2, [ExtraField3] = @ExtraField3, [ExtraField4] = @ExtraField4, [ExtraField5] = @ExtraField5, [ExtraField6] = @ExtraField6, [ExtraField7] = @ExtraField7, [ExtraField8] = @ExtraField8, [ExtraField9] = @ExtraField9, [ExtraField10] = @ExtraField10, [ExtraField11] = @ExtraField11, [ExtraField12] = @ExtraField12, [ExtraField13] = @ExtraField13, [ExtraField14] = @ExtraField14, [ExtraField15] = @ExtraField15, [ExtraField16] = @ExtraField16 OUTPUT INSERTED.[Id], INSERTED.[Name], INSERTED.[DateCreated], INSERTED.[DateLastModified], INSERTED.[DateSynchronized], INSERTED.[DriveSerialNumber], INSERTED.[DriveTypeCodeType], INSERTED.[DriveTypeCode], INSERTED.[DriveTotalSize], INSERTED.[DriveFreeSpace], INSERTED.[Recursive], INSERTED.[Sorted], INSERTED.[StartDirectory], INSERTED.[ExtraField1], INSERTED.[ExtraField2], INSERTED.[ExtraField3], INSERTED.[ExtraField4], INSERTED.[ExtraField5], INSERTED.[ExtraField6], INSERTED.[ExtraField7], INSERTED.[ExtraField8], INSERTED.[ExtraField9], INSERTED.[ExtraField10], INSERTED.[ExtraField11], INSERTED.[ExtraField12], INSERTED.[ExtraField13], INSERTED.[ExtraField14], INSERTED.[ExtraField15], INSERTED.[ExtraField16] WHERE [Id] = @IdOriginal AND ((@NameOriginal IS NOT NULL AND [Name] = @NameOriginal) OR (@NameOriginal IS NULL AND [Name] IS NULL)) AND ((@DateCreatedOriginal IS NOT NULL AND [DateCreated] = @DateCreatedOriginal) OR (@DateCreatedOriginal IS NULL AND [DateCreated] IS NULL)) AND ((@DateLastModifiedOriginal IS NOT NULL AND [DateLastModified] = @DateLastModifiedOriginal) OR (@DateLastModifiedOriginal IS NULL AND [DateLastModified] IS NULL)) AND ((@DateSynchronizedOriginal IS NOT NULL AND [DateSynchronized] = @DateSynchronizedOriginal) OR (@DateSynchronizedOriginal IS NULL AND [DateSynchronized] IS NULL)) AND ((@DriveSerialNumberOriginal IS NOT NULL AND [DriveSerialNumber] = @DriveSerialNumberOriginal) OR (@DriveSerialNumberOriginal IS NULL AND [DriveSerialNumber] IS NULL)) AND ((@DriveTypeCodeTypeOriginal IS NOT NULL AND [DriveTypeCodeType] = @DriveTypeCodeTypeOriginal) OR (@DriveTypeCodeTypeOriginal IS NULL AND [DriveTypeCodeType] IS NULL)) AND ((@DriveTypeCodeOriginal IS NOT NULL AND [DriveTypeCode] = @DriveTypeCodeOriginal) OR (@DriveTypeCodeOriginal IS NULL AND [DriveTypeCode] IS NULL)) AND ((@DriveTotalSizeOriginal IS NOT NULL AND [DriveTotalSize] = @DriveTotalSizeOriginal) OR (@DriveTotalSizeOriginal IS NULL AND [DriveTotalSize] IS NULL)) AND ((@DriveFreeSpaceOriginal IS NOT NULL AND [DriveFreeSpace] = @DriveFreeSpaceOriginal) OR (@DriveFreeSpaceOriginal IS NULL AND [DriveFreeSpace] IS NULL)) AND ((@RecursiveOriginal IS NOT NULL AND [Recursive] = @RecursiveOriginal) OR (@RecursiveOriginal IS NULL AND [Recursive] IS NULL)) AND ((@SortedOriginal IS NOT NULL AND [Sorted] = @SortedOriginal) OR (@SortedOriginal IS NULL AND [Sorted] IS NULL)) AND ((@StartDirectoryOriginal IS NOT NULL AND [StartDirectory] = @StartDirectoryOriginal) OR (@StartDirectoryOriginal IS NULL AND [StartDirectory] IS NULL)) AND ((@ExtraField1Original IS NOT NULL AND [ExtraField1] = @ExtraField1Original) OR (@ExtraField1Original IS NULL AND [ExtraField1] IS NULL)) AND ((@ExtraField2Original IS NOT NULL AND [ExtraField2] = @ExtraField2Original) OR (@ExtraField2Original IS NULL AND [ExtraField2] IS NULL)) AND ((@ExtraField3Original IS NOT NULL AND [ExtraField3] = @ExtraField3Original) OR (@ExtraField3Original IS NULL AND [ExtraField3] IS NULL)) AND ((@ExtraField4Original IS NOT NULL AND [ExtraField4] = @ExtraField4Original) OR (@ExtraField4Original IS NULL AND [ExtraField4] IS NULL)) AND ((@ExtraField5Original IS NOT NULL AND [ExtraField5] = @ExtraField5Original) OR (@ExtraField5Original IS NULL AND [ExtraField5] IS NULL)) AND ((@ExtraField6Original IS NOT NULL AND [ExtraField6] = @ExtraField6Original) OR (@ExtraField6Original IS NULL AND [ExtraField6] IS NULL)) AND ((@ExtraField7Original IS NOT NULL AND [ExtraField7] = @ExtraField7Original) OR (@ExtraField7Original IS NULL AND [ExtraField7] IS NULL)) AND ((@ExtraField8Original IS NOT NULL AND [ExtraField8] = @ExtraField8Original) OR (@ExtraField8Original IS NULL AND [ExtraField8] IS NULL)) AND ((@ExtraField9Original IS NOT NULL AND [ExtraField9] = @ExtraField9Original) OR (@ExtraField9Original IS NULL AND [ExtraField9] IS NULL)) AND ((@ExtraField10Original IS NOT NULL AND [ExtraField10] = @ExtraField10Original) OR (@ExtraField10Original IS NULL AND [ExtraField10] IS NULL)) AND ((@ExtraField11Original IS NOT NULL AND [ExtraField11] = @ExtraField11Original) OR (@ExtraField11Original IS NULL AND [ExtraField11] IS NULL)) AND ((@ExtraField12Original IS NOT NULL AND [ExtraField12] = @ExtraField12Original) OR (@ExtraField12Original IS NULL AND [ExtraField12] IS NULL)) AND ((@ExtraField13Original IS NOT NULL AND [ExtraField13] = @ExtraField13Original) OR (@ExtraField13Original IS NULL AND [ExtraField13] IS NULL)) AND ((@ExtraField14Original IS NOT NULL AND [ExtraField14] = @ExtraField14Original) OR (@ExtraField14Original IS NULL AND [ExtraField14] IS NULL)) AND ((@ExtraField15Original IS NOT NULL AND [ExtraField15] = @ExtraField15Original) OR (@ExtraField15Original IS NULL AND [ExtraField15] IS NULL)) AND ((@ExtraField16Original IS NOT NULL AND [ExtraField16] = @ExtraField16Original) OR (@ExtraField16Original IS NULL AND [ExtraField16] IS NULL))

END
GO
--****** Object:  StoredProcedure [dbo].[DeleteCatalog] ******
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteCatalog]
	@Id int,
	@Name varchar(64),
	@DateCreated datetime,
	@DateLastModified datetime,
	@DateSynchronized datetime,
	@DriveSerialNumber varchar(9),
	@DriveTypeCodeType varchar(3),
	@DriveTypeCode varchar(3),
	@DriveTotalSize int,
	@DriveFreeSpace int,
	@Recursive bit,
	@Sorted bit,
	@StartDirectory varchar(512),
	@ExtraField1 varchar(64),
	@ExtraField2 varchar(64),
	@ExtraField3 varchar(64),
	@ExtraField4 varchar(64),
	@ExtraField5 varchar(64),
	@ExtraField6 varchar(64),
	@ExtraField7 varchar(64),
	@ExtraField8 varchar(64),
	@ExtraField9 varchar(64),
	@ExtraField10 varchar(64),
	@ExtraField11 varchar(64),
	@ExtraField12 varchar(64),
	@ExtraField13 varchar(64),
	@ExtraField14 varchar(64),
	@ExtraField15 varchar(64),
	@ExtraField16 varchar(64)
AS
BEGIN

DELETE FROM [Catalog] WHERE [Id] = @Id AND ((@Name IS NOT NULL AND [Name] = @Name) OR (@Name IS NULL AND [Name] IS NULL)) AND ((@DateCreated IS NOT NULL AND [DateCreated] = @DateCreated) OR (@DateCreated IS NULL AND [DateCreated] IS NULL)) AND ((@DateLastModified IS NOT NULL AND [DateLastModified] = @DateLastModified) OR (@DateLastModified IS NULL AND [DateLastModified] IS NULL)) AND ((@DateSynchronized IS NOT NULL AND [DateSynchronized] = @DateSynchronized) OR (@DateSynchronized IS NULL AND [DateSynchronized] IS NULL)) AND ((@DriveSerialNumber IS NOT NULL AND [DriveSerialNumber] = @DriveSerialNumber) OR (@DriveSerialNumber IS NULL AND [DriveSerialNumber] IS NULL)) AND ((@DriveTypeCodeType IS NOT NULL AND [DriveTypeCodeType] = @DriveTypeCodeType) OR (@DriveTypeCodeType IS NULL AND [DriveTypeCodeType] IS NULL)) AND ((@DriveTypeCode IS NOT NULL AND [DriveTypeCode] = @DriveTypeCode) OR (@DriveTypeCode IS NULL AND [DriveTypeCode] IS NULL)) AND ((@DriveTotalSize IS NOT NULL AND [DriveTotalSize] = @DriveTotalSize) OR (@DriveTotalSize IS NULL AND [DriveTotalSize] IS NULL)) AND ((@DriveFreeSpace IS NOT NULL AND [DriveFreeSpace] = @DriveFreeSpace) OR (@DriveFreeSpace IS NULL AND [DriveFreeSpace] IS NULL)) AND ((@Recursive IS NOT NULL AND [Recursive] = @Recursive) OR (@Recursive IS NULL AND [Recursive] IS NULL)) AND ((@Sorted IS NOT NULL AND [Sorted] = @Sorted) OR (@Sorted IS NULL AND [Sorted] IS NULL)) AND ((@StartDirectory IS NOT NULL AND [StartDirectory] = @StartDirectory) OR (@StartDirectory IS NULL AND [StartDirectory] IS NULL)) AND ((@ExtraField1 IS NOT NULL AND [ExtraField1] = @ExtraField1) OR (@ExtraField1 IS NULL AND [ExtraField1] IS NULL)) AND ((@ExtraField2 IS NOT NULL AND [ExtraField2] = @ExtraField2) OR (@ExtraField2 IS NULL AND [ExtraField2] IS NULL)) AND ((@ExtraField3 IS NOT NULL AND [ExtraField3] = @ExtraField3) OR (@ExtraField3 IS NULL AND [ExtraField3] IS NULL)) AND ((@ExtraField4 IS NOT NULL AND [ExtraField4] = @ExtraField4) OR (@ExtraField4 IS NULL AND [ExtraField4] IS NULL)) AND ((@ExtraField5 IS NOT NULL AND [ExtraField5] = @ExtraField5) OR (@ExtraField5 IS NULL AND [ExtraField5] IS NULL)) AND ((@ExtraField6 IS NOT NULL AND [ExtraField6] = @ExtraField6) OR (@ExtraField6 IS NULL AND [ExtraField6] IS NULL)) AND ((@ExtraField7 IS NOT NULL AND [ExtraField7] = @ExtraField7) OR (@ExtraField7 IS NULL AND [ExtraField7] IS NULL)) AND ((@ExtraField8 IS NOT NULL AND [ExtraField8] = @ExtraField8) OR (@ExtraField8 IS NULL AND [ExtraField8] IS NULL)) AND ((@ExtraField9 IS NOT NULL AND [ExtraField9] = @ExtraField9) OR (@ExtraField9 IS NULL AND [ExtraField9] IS NULL)) AND ((@ExtraField10 IS NOT NULL AND [ExtraField10] = @ExtraField10) OR (@ExtraField10 IS NULL AND [ExtraField10] IS NULL)) AND ((@ExtraField11 IS NOT NULL AND [ExtraField11] = @ExtraField11) OR (@ExtraField11 IS NULL AND [ExtraField11] IS NULL)) AND ((@ExtraField12 IS NOT NULL AND [ExtraField12] = @ExtraField12) OR (@ExtraField12 IS NULL AND [ExtraField12] IS NULL)) AND ((@ExtraField13 IS NOT NULL AND [ExtraField13] = @ExtraField13) OR (@ExtraField13 IS NULL AND [ExtraField13] IS NULL)) AND ((@ExtraField14 IS NOT NULL AND [ExtraField14] = @ExtraField14) OR (@ExtraField14 IS NULL AND [ExtraField14] IS NULL)) AND ((@ExtraField15 IS NOT NULL AND [ExtraField15] = @ExtraField15) OR (@ExtraField15 IS NULL AND [ExtraField15] IS NULL)) AND ((@ExtraField16 IS NOT NULL AND [ExtraField16] = @ExtraField16) OR (@ExtraField16 IS NULL AND [ExtraField16] IS NULL))

END
GO
--****** Object:  StoredProcedure [dbo].[InsertFile] ******
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertFile]
	@Path varchar(512),
	@FileName varchar(128),
	@Extension varchar(12),
	@Date datetime,
	@Size bigint,
	@CatalogId int,
	@ExtraField1 varchar(64),
	@ExtraField2 varchar(64),
	@ExtraField3 varchar(64),
	@ExtraField4 varchar(64),
	@ExtraField5 varchar(64),
	@ExtraField6 varchar(64),
	@ExtraField7 varchar(64),
	@ExtraField8 varchar(64),
	@ExtraField9 varchar(64),
	@ExtraField10 varchar(64),
	@ExtraField11 varchar(64),
	@ExtraField12 varchar(64),
	@ExtraField13 varchar(64),
	@ExtraField14 varchar(64),
	@ExtraField15 varchar(64),
	@ExtraField16 varchar(64)
AS
BEGIN

INSERT INTO [File] ([Path], [FileName], [Extension], [Date], [Size], [CatalogId], [ExtraField1], [ExtraField2], [ExtraField3], [ExtraField4], [ExtraField5], [ExtraField6], [ExtraField7], [ExtraField8], [ExtraField9], [ExtraField10], [ExtraField11], [ExtraField12], [ExtraField13], [ExtraField14], [ExtraField15], [ExtraField16]) OUTPUT INSERTED.[Id], INSERTED.[Path], INSERTED.[FileName], INSERTED.[Extension], INSERTED.[Date], INSERTED.[Size], INSERTED.[CatalogId], INSERTED.[ExtraField1], INSERTED.[ExtraField2], INSERTED.[ExtraField3], INSERTED.[ExtraField4], INSERTED.[ExtraField5], INSERTED.[ExtraField6], INSERTED.[ExtraField7], INSERTED.[ExtraField8], INSERTED.[ExtraField9], INSERTED.[ExtraField10], INSERTED.[ExtraField11], INSERTED.[ExtraField12], INSERTED.[ExtraField13], INSERTED.[ExtraField14], INSERTED.[ExtraField15], INSERTED.[ExtraField16] VALUES (@Path, @FileName, @Extension, @Date, @Size, @CatalogId, @ExtraField1, @ExtraField2, @ExtraField3, @ExtraField4, @ExtraField5, @ExtraField6, @ExtraField7, @ExtraField8, @ExtraField9, @ExtraField10, @ExtraField11, @ExtraField12, @ExtraField13, @ExtraField14, @ExtraField15, @ExtraField16)

END
GO
--****** Object:  StoredProcedure [dbo].[UpdateFile] ******
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateFile]
	@Path varchar(512),
	@FileName varchar(128),
	@Extension varchar(12),
	@Date datetime,
	@Size bigint,
	@CatalogId int,
	@ExtraField1 varchar(64),
	@ExtraField2 varchar(64),
	@ExtraField3 varchar(64),
	@ExtraField4 varchar(64),
	@ExtraField5 varchar(64),
	@ExtraField6 varchar(64),
	@ExtraField7 varchar(64),
	@ExtraField8 varchar(64),
	@ExtraField9 varchar(64),
	@ExtraField10 varchar(64),
	@ExtraField11 varchar(64),
	@ExtraField12 varchar(64),
	@ExtraField13 varchar(64),
	@ExtraField14 varchar(64),
	@ExtraField15 varchar(64),
	@ExtraField16 varchar(64),
	@IdOriginal int,
	@PathOriginal varchar(512),
	@FileNameOriginal varchar(128),
	@ExtensionOriginal varchar(12),
	@DateOriginal datetime,
	@SizeOriginal bigint,
	@CatalogIdOriginal int,
	@ExtraField1Original varchar(64),
	@ExtraField2Original varchar(64),
	@ExtraField3Original varchar(64),
	@ExtraField4Original varchar(64),
	@ExtraField5Original varchar(64),
	@ExtraField6Original varchar(64),
	@ExtraField7Original varchar(64),
	@ExtraField8Original varchar(64),
	@ExtraField9Original varchar(64),
	@ExtraField10Original varchar(64),
	@ExtraField11Original varchar(64),
	@ExtraField12Original varchar(64),
	@ExtraField13Original varchar(64),
	@ExtraField14Original varchar(64),
	@ExtraField15Original varchar(64),
	@ExtraField16Original varchar(64)
AS
BEGIN

UPDATE [File] SET [Path] = @Path, [FileName] = @FileName, [Extension] = @Extension, [Date] = @Date, [Size] = @Size, [CatalogId] = @CatalogId, [ExtraField1] = @ExtraField1, [ExtraField2] = @ExtraField2, [ExtraField3] = @ExtraField3, [ExtraField4] = @ExtraField4, [ExtraField5] = @ExtraField5, [ExtraField6] = @ExtraField6, [ExtraField7] = @ExtraField7, [ExtraField8] = @ExtraField8, [ExtraField9] = @ExtraField9, [ExtraField10] = @ExtraField10, [ExtraField11] = @ExtraField11, [ExtraField12] = @ExtraField12, [ExtraField13] = @ExtraField13, [ExtraField14] = @ExtraField14, [ExtraField15] = @ExtraField15, [ExtraField16] = @ExtraField16 OUTPUT INSERTED.[Id], INSERTED.[Path], INSERTED.[FileName], INSERTED.[Extension], INSERTED.[Date], INSERTED.[Size], INSERTED.[CatalogId], INSERTED.[ExtraField1], INSERTED.[ExtraField2], INSERTED.[ExtraField3], INSERTED.[ExtraField4], INSERTED.[ExtraField5], INSERTED.[ExtraField6], INSERTED.[ExtraField7], INSERTED.[ExtraField8], INSERTED.[ExtraField9], INSERTED.[ExtraField10], INSERTED.[ExtraField11], INSERTED.[ExtraField12], INSERTED.[ExtraField13], INSERTED.[ExtraField14], INSERTED.[ExtraField15], INSERTED.[ExtraField16] WHERE [Id] = @IdOriginal AND ((@PathOriginal IS NOT NULL AND [Path] = @PathOriginal) OR (@PathOriginal IS NULL AND [Path] IS NULL)) AND ((@FileNameOriginal IS NOT NULL AND [FileName] = @FileNameOriginal) OR (@FileNameOriginal IS NULL AND [FileName] IS NULL)) AND ((@ExtensionOriginal IS NOT NULL AND [Extension] = @ExtensionOriginal) OR (@ExtensionOriginal IS NULL AND [Extension] IS NULL)) AND ((@DateOriginal IS NOT NULL AND [Date] = @DateOriginal) OR (@DateOriginal IS NULL AND [Date] IS NULL)) AND ((@SizeOriginal IS NOT NULL AND [Size] = @SizeOriginal) OR (@SizeOriginal IS NULL AND [Size] IS NULL)) AND ((@CatalogIdOriginal IS NOT NULL AND [CatalogId] = @CatalogIdOriginal) OR (@CatalogIdOriginal IS NULL AND [CatalogId] IS NULL)) AND ((@ExtraField1Original IS NOT NULL AND [ExtraField1] = @ExtraField1Original) OR (@ExtraField1Original IS NULL AND [ExtraField1] IS NULL)) AND ((@ExtraField2Original IS NOT NULL AND [ExtraField2] = @ExtraField2Original) OR (@ExtraField2Original IS NULL AND [ExtraField2] IS NULL)) AND ((@ExtraField3Original IS NOT NULL AND [ExtraField3] = @ExtraField3Original) OR (@ExtraField3Original IS NULL AND [ExtraField3] IS NULL)) AND ((@ExtraField4Original IS NOT NULL AND [ExtraField4] = @ExtraField4Original) OR (@ExtraField4Original IS NULL AND [ExtraField4] IS NULL)) AND ((@ExtraField5Original IS NOT NULL AND [ExtraField5] = @ExtraField5Original) OR (@ExtraField5Original IS NULL AND [ExtraField5] IS NULL)) AND ((@ExtraField6Original IS NOT NULL AND [ExtraField6] = @ExtraField6Original) OR (@ExtraField6Original IS NULL AND [ExtraField6] IS NULL)) AND ((@ExtraField7Original IS NOT NULL AND [ExtraField7] = @ExtraField7Original) OR (@ExtraField7Original IS NULL AND [ExtraField7] IS NULL)) AND ((@ExtraField8Original IS NOT NULL AND [ExtraField8] = @ExtraField8Original) OR (@ExtraField8Original IS NULL AND [ExtraField8] IS NULL)) AND ((@ExtraField9Original IS NOT NULL AND [ExtraField9] = @ExtraField9Original) OR (@ExtraField9Original IS NULL AND [ExtraField9] IS NULL)) AND ((@ExtraField10Original IS NOT NULL AND [ExtraField10] = @ExtraField10Original) OR (@ExtraField10Original IS NULL AND [ExtraField10] IS NULL)) AND ((@ExtraField11Original IS NOT NULL AND [ExtraField11] = @ExtraField11Original) OR (@ExtraField11Original IS NULL AND [ExtraField11] IS NULL)) AND ((@ExtraField12Original IS NOT NULL AND [ExtraField12] = @ExtraField12Original) OR (@ExtraField12Original IS NULL AND [ExtraField12] IS NULL)) AND ((@ExtraField13Original IS NOT NULL AND [ExtraField13] = @ExtraField13Original) OR (@ExtraField13Original IS NULL AND [ExtraField13] IS NULL)) AND ((@ExtraField14Original IS NOT NULL AND [ExtraField14] = @ExtraField14Original) OR (@ExtraField14Original IS NULL AND [ExtraField14] IS NULL)) AND ((@ExtraField15Original IS NOT NULL AND [ExtraField15] = @ExtraField15Original) OR (@ExtraField15Original IS NULL AND [ExtraField15] IS NULL)) AND ((@ExtraField16Original IS NOT NULL AND [ExtraField16] = @ExtraField16Original) OR (@ExtraField16Original IS NULL AND [ExtraField16] IS NULL))

END
GO
--****** Object:  StoredProcedure [dbo].[DeleteFile] ******
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteFile]
	@Id int,
	@Path varchar(512),
	@FileName varchar(128),
	@Extension varchar(12),
	@Date datetime,
	@Size bigint,
	@CatalogId int,
	@ExtraField1 varchar(64),
	@ExtraField2 varchar(64),
	@ExtraField3 varchar(64),
	@ExtraField4 varchar(64),
	@ExtraField5 varchar(64),
	@ExtraField6 varchar(64),
	@ExtraField7 varchar(64),
	@ExtraField8 varchar(64),
	@ExtraField9 varchar(64),
	@ExtraField10 varchar(64),
	@ExtraField11 varchar(64),
	@ExtraField12 varchar(64),
	@ExtraField13 varchar(64),
	@ExtraField14 varchar(64),
	@ExtraField15 varchar(64),
	@ExtraField16 varchar(64)
AS
BEGIN

DELETE FROM [File] WHERE [Id] = @Id AND ((@Path IS NOT NULL AND [Path] = @Path) OR (@Path IS NULL AND [Path] IS NULL)) AND ((@FileName IS NOT NULL AND [FileName] = @FileName) OR (@FileName IS NULL AND [FileName] IS NULL)) AND ((@Extension IS NOT NULL AND [Extension] = @Extension) OR (@Extension IS NULL AND [Extension] IS NULL)) AND ((@Date IS NOT NULL AND [Date] = @Date) OR (@Date IS NULL AND [Date] IS NULL)) AND ((@Size IS NOT NULL AND [Size] = @Size) OR (@Size IS NULL AND [Size] IS NULL)) AND ((@CatalogId IS NOT NULL AND [CatalogId] = @CatalogId) OR (@CatalogId IS NULL AND [CatalogId] IS NULL)) AND ((@ExtraField1 IS NOT NULL AND [ExtraField1] = @ExtraField1) OR (@ExtraField1 IS NULL AND [ExtraField1] IS NULL)) AND ((@ExtraField2 IS NOT NULL AND [ExtraField2] = @ExtraField2) OR (@ExtraField2 IS NULL AND [ExtraField2] IS NULL)) AND ((@ExtraField3 IS NOT NULL AND [ExtraField3] = @ExtraField3) OR (@ExtraField3 IS NULL AND [ExtraField3] IS NULL)) AND ((@ExtraField4 IS NOT NULL AND [ExtraField4] = @ExtraField4) OR (@ExtraField4 IS NULL AND [ExtraField4] IS NULL)) AND ((@ExtraField5 IS NOT NULL AND [ExtraField5] = @ExtraField5) OR (@ExtraField5 IS NULL AND [ExtraField5] IS NULL)) AND ((@ExtraField6 IS NOT NULL AND [ExtraField6] = @ExtraField6) OR (@ExtraField6 IS NULL AND [ExtraField6] IS NULL)) AND ((@ExtraField7 IS NOT NULL AND [ExtraField7] = @ExtraField7) OR (@ExtraField7 IS NULL AND [ExtraField7] IS NULL)) AND ((@ExtraField8 IS NOT NULL AND [ExtraField8] = @ExtraField8) OR (@ExtraField8 IS NULL AND [ExtraField8] IS NULL)) AND ((@ExtraField9 IS NOT NULL AND [ExtraField9] = @ExtraField9) OR (@ExtraField9 IS NULL AND [ExtraField9] IS NULL)) AND ((@ExtraField10 IS NOT NULL AND [ExtraField10] = @ExtraField10) OR (@ExtraField10 IS NULL AND [ExtraField10] IS NULL)) AND ((@ExtraField11 IS NOT NULL AND [ExtraField11] = @ExtraField11) OR (@ExtraField11 IS NULL AND [ExtraField11] IS NULL)) AND ((@ExtraField12 IS NOT NULL AND [ExtraField12] = @ExtraField12) OR (@ExtraField12 IS NULL AND [ExtraField12] IS NULL)) AND ((@ExtraField13 IS NOT NULL AND [ExtraField13] = @ExtraField13) OR (@ExtraField13 IS NULL AND [ExtraField13] IS NULL)) AND ((@ExtraField14 IS NOT NULL AND [ExtraField14] = @ExtraField14) OR (@ExtraField14 IS NULL AND [ExtraField14] IS NULL)) AND ((@ExtraField15 IS NOT NULL AND [ExtraField15] = @ExtraField15) OR (@ExtraField15 IS NULL AND [ExtraField15] IS NULL)) AND ((@ExtraField16 IS NOT NULL AND [ExtraField16] = @ExtraField16) OR (@ExtraField16 IS NULL AND [ExtraField16] IS NULL))

END
GO
--****** Object:  StoredProcedure [dbo].[InsertCode] ******
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertCode]
	@Code varchar(3),
	@CodeType varchar(3),
	@Description varchar(512)
AS
BEGIN

INSERT INTO [Code] ([Code], [CodeType], [Description]) OUTPUT INSERTED.[Code], INSERTED.[CodeType], INSERTED.[Description] VALUES (@Code, @CodeType, @Description)

END
GO
--****** Object:  StoredProcedure [dbo].[UpdateCode] ******
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateCode]
	@Code varchar(3),
	@CodeType varchar(3),
	@Description varchar(512),
	@CodeOriginal varchar(3),
	@CodeTypeOriginal varchar(3),
	@DescriptionOriginal varchar(512)
AS
BEGIN

UPDATE [Code] SET [Code] = @Code, [CodeType] = @CodeType, [Description] = @Description OUTPUT INSERTED.[Code], INSERTED.[CodeType], INSERTED.[Description] WHERE ((@CodeOriginal IS NOT NULL AND [Code] = @CodeOriginal) OR (@CodeOriginal IS NULL AND [Code] IS NULL)) AND ((@CodeTypeOriginal IS NOT NULL AND [CodeType] = @CodeTypeOriginal) OR (@CodeTypeOriginal IS NULL AND [CodeType] IS NULL)) AND ((@DescriptionOriginal IS NOT NULL AND [Description] = @DescriptionOriginal) OR (@DescriptionOriginal IS NULL AND [Description] IS NULL))

END
GO
--****** Object:  StoredProcedure [dbo].[DeleteCode] ******
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteCode]
	@Code varchar(3),
	@CodeType varchar(3),
	@Description varchar(512)
AS
BEGIN

DELETE FROM [Code] WHERE ((@Code IS NOT NULL AND [Code] = @Code) OR (@Code IS NULL AND [Code] IS NULL)) AND ((@CodeType IS NOT NULL AND [CodeType] = @CodeType) OR (@CodeType IS NULL AND [CodeType] IS NULL)) AND ((@Description IS NOT NULL AND [Description] = @Description) OR (@Description IS NULL AND [Description] IS NULL))

END
GO
--****** Object:  StoredProcedure [dbo].[InsertExtraField] ******
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertExtraField]
	@EntityName varchar(64),
	@Name varchar(64),
	@Description varchar(512),
	@FieldNumber tinyint,
	@FieldType varchar(64)
AS
BEGIN

INSERT INTO [ExtraField] ([EntityName], [Name], [Description], [FieldNumber], [FieldType]) OUTPUT INSERTED.[EntityName], INSERTED.[Name], INSERTED.[Description], INSERTED.[FieldNumber], INSERTED.[FieldType] VALUES (@EntityName, @Name, @Description, @FieldNumber, @FieldType)

END
GO
--****** Object:  StoredProcedure [dbo].[UpdateExtraField] ******
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateExtraField]
	@EntityName varchar(64),
	@Name varchar(64),
	@Description varchar(512),
	@FieldNumber tinyint,
	@FieldType varchar(64),
	@EntityNameOriginal varchar(64),
	@NameOriginal varchar(64),
	@DescriptionOriginal varchar(512),
	@FieldNumberOriginal tinyint,
	@FieldTypeOriginal varchar(64)
AS
BEGIN

UPDATE [ExtraField] SET [EntityName] = @EntityName, [Name] = @Name, [Description] = @Description, [FieldNumber] = @FieldNumber, [FieldType] = @FieldType OUTPUT INSERTED.[EntityName], INSERTED.[Name], INSERTED.[Description], INSERTED.[FieldNumber], INSERTED.[FieldType] WHERE ((@EntityNameOriginal IS NOT NULL AND [EntityName] = @EntityNameOriginal) OR (@EntityNameOriginal IS NULL AND [EntityName] IS NULL)) AND ((@NameOriginal IS NOT NULL AND [Name] = @NameOriginal) OR (@NameOriginal IS NULL AND [Name] IS NULL)) AND ((@DescriptionOriginal IS NOT NULL AND [Description] = @DescriptionOriginal) OR (@DescriptionOriginal IS NULL AND [Description] IS NULL)) AND ((@FieldNumberOriginal IS NOT NULL AND [FieldNumber] = @FieldNumberOriginal) OR (@FieldNumberOriginal IS NULL AND [FieldNumber] IS NULL)) AND ((@FieldTypeOriginal IS NOT NULL AND [FieldType] = @FieldTypeOriginal) OR (@FieldTypeOriginal IS NULL AND [FieldType] IS NULL))

END
GO
--****** Object:  StoredProcedure [dbo].[DeleteExtraField] ******
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteExtraField]
	@EntityName varchar(64),
	@Name varchar(64),
	@Description varchar(512),
	@FieldNumber tinyint,
	@FieldType varchar(64)
AS
BEGIN

DELETE FROM [ExtraField] WHERE ((@EntityName IS NOT NULL AND [EntityName] = @EntityName) OR (@EntityName IS NULL AND [EntityName] IS NULL)) AND ((@Name IS NOT NULL AND [Name] = @Name) OR (@Name IS NULL AND [Name] IS NULL)) AND ((@Description IS NOT NULL AND [Description] = @Description) OR (@Description IS NULL AND [Description] IS NULL)) AND ((@FieldNumber IS NOT NULL AND [FieldNumber] = @FieldNumber) OR (@FieldNumber IS NULL AND [FieldNumber] IS NULL)) AND ((@FieldType IS NOT NULL AND [FieldType] = @FieldType) OR (@FieldType IS NULL AND [FieldType] IS NULL))

END
GO
