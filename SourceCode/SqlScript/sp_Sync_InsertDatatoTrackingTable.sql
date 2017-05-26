-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE sp_Sync_InsertDatatoTrackingTable 
	-- Add the parameters for the stored procedure here
	@tableName varchar(500),
	@Action varchar(100),
	@TrackingData xml
AS
BEGIN
	INSERT INTO [dbo].[SYNC_DataTracking]
           ([TableSource]
           ,[Action]
           ,[DataTracking])
     VALUES
           (@tableName
           ,@Action
           ,@TrackingData)
END
GO
