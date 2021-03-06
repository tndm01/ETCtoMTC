USE [Synchronization_ETC]
GO
/****** Object:  StoredProcedure [dbo].[sp_SYNC_UpdateDataSoxeTable]    Script Date: 22/03/2017 5:57:39 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_SYNC_UpdateDataSoxeTable] 
	-- Add the parameters for the stored procedure here
	
	@SOXE varchar(15),
	@MSLOAIVE char(2),
	@GIAVE numeric(18,0),
	@NGAYDANGKY datetime,
	@SODANGKIEM varchar(30),
	@TRONGTAI varchar(50),
	@ENABLED tinyint,
	@GHICHU varchar(250),
	@LOGIN varchar(20),
	@NGAYNHAP datetime,
	@F0 char(1),
	@F1 char(1),
	@F2 char(1),
	@GHICHU_F1 varchar(50),
	@MSTRAM char(1)

AS
BEGIN
	UPDATE Data_Soxe SET  SoXe=@SOXE,
							MSLoaiVe=@MSLOAIVE,
							GiaVe=@GIAVE,
							NgayDangKy=@NGAYDANGKY,
							SoDangkiem=@SODANGKIEM,
							Taitrong=@TRONGTAI,
							ENABLED=@ENABLED,
							Ghichu=@GHICHU,
							Login=@LOGIN,
							NgayNhap=@NGAYNHAP,
							F0=@F0,
							F1=@F1,
							F2=@F2,
							GhiChu_F1=@GHICHU_F1,
							MSTram=@MSTRAM
							where SoXe=@SOXE
END
