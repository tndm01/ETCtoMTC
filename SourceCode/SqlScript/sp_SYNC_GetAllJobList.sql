USE [ETC_RFID_TN]
GO
/****** Object:  StoredProcedure [dbo].[sp_SYNC_GetAllJobList]    Script Date: 3/19/2017 10:22:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_SYNC_GetAllJobList]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM SYNC_JobList WHERE IsUsed = 1
END
