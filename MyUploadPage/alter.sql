USE [UploadDb02]
GO

ALTER TABLE [dbo].[Docs]
ADD Data varbinary(max) FILESTREAM NULL
GO