USE [Synchronization_ETC]
GO
/****** Object:  StoredProcedure [dbo].[sp_SYNC_UpdateEtagThangTable]    Script Date: 22/03/2017 5:57:51 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_SYNC_UpdateEtagThangTable]
	-- Add the parameters for the stored procedure here
	@MaETag varchar(50),
	@GiaVe varchar(50),
	@MaLoaiVe varchar(50),
	@SoXe varchar(50),
	@NgayBatDau datetime,
	@NgayKetThuc datetime,
	@MaNhanVien varchar(50)	
AS
BEGIN
	UPDATE EtagThang SET MaETag = @MaETag,
						GiaVe = @GiaVe,
						MaLoaiVe= @MaLoaiVe,
						SoXe=@SoXe,
						NgayBatDau=@NgayBatDau,
						NgayKetThuc=@NgayKetThuc,
						MaNhanVien=@MaNhanVien
		Where MaETag=@MaETag
END