USE [Synchronization_ETC]
GO

/****** Object:  StoredProcedure [dbo].[sp_SYNC_UpDateStatusImageTrackingData]    Script Date: 03/27/2017 9:41:29 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_SYNC_UpDateStatusImageTrackingData] 
	-- Add the parameters for the stored procedure here
	@TrackingId bigint,
	@SyncStatus int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE SYNC_TrackingImageData
	SET SyncStatus = @SyncStatus
	WHERE TrackingID = @TrackingId

END

GO

