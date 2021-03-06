USE [Synchronization_ETC]
GO
/****** Object:  StoredProcedure [dbo].[sp_SYNC_UpdateNhanvienTable]    Script Date: 22/03/2017 5:58:03 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_SYNC_UpdateNhanvienTable] 
	-- Add the parameters for the stored procedure here
	
	@MSNV varchar(20),
	@MSTO char(3),
	@HONV varchar(100),
	@TENNV varchar(100),
	@TEN_SEARCH varchar(20),
	@DIACHI varchar(20),
	@DIENTHOAI varchar(15),
	@GHICHU varchar(250),
	@MSTram char(1)
AS
BEGIN
	UPDATE NhanVien SET MSNV=@MSNV,
						MSTO=@MSTO,
						HONV=@HONV,
						TENNV=@TENNV,
						TEN_SEARCH=@TEN_SEARCH,
						DIACHI=@DIACHI,
						DIENTHOAI=@DIENTHOAI,
						GHICHU=@GHICHU,
						MSTram=@MSTram
		Where MSNV=@MSNV
END
