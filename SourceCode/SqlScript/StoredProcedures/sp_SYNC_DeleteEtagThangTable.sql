USE [Synchronization_ETC]
GO
/****** Object:  StoredProcedure [dbo].[sp_SYNC_DeleteEtagThangTable]    Script Date: 22/03/2017 5:57:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[sp_SYNC_DeleteEtagThangTable]
	-- Add the parameters for the stored procedure here
	@MaETag varchar(50)
AS
BEGIN
	Delete EtagThang 
	where MaETag = @MaETag
	end