USE [Synchronization_ETC]
GO
/****** Object:  StoredProcedure [dbo].[sp_SYNC_DeleteNhanvienTable]    Script Date: 22/03/2017 5:57:26 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_SYNC_DeleteNhanvienTable] 
	-- Add the parameters for the stored procedure here
	
	@MSNV varchar(20)
AS
BEGIN
	Delete NhanVien 
	where MSNV = @MSNV
END
