USE [Synchronization_ETC]
GO

/****** Object:  StoredProcedure [dbo].[sp_SYNC_DeleteTRP_tblCheckObuAccount_RFID]    Script Date: 03/23/2017 11:16:02 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_SYNC_DeleteVeThangQui]
@TID varchar(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
DELETE FROM VeThang_Qui
WHERE TID = @TID
END

GO

