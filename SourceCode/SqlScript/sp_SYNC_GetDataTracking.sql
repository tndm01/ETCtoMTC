USE [ETC_RFID_TN]
GO
/****** Object:  StoredProcedure [dbo].[sp_SYNC_GetDataTracking]    Script Date: 3/15/2017 9:38:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_SYNC_GetDataTracking] 
	-- Add the parameters for the stored procedure here
	@TableName varchar(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT  top 10 * from SYNC_DataTracking where [TableSource] = @TableName and SyncStatus = 0
END
