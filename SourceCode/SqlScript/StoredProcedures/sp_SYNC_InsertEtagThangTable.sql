USE [Synchronization_ETC]
GO
/****** Object:  StoredProcedure [dbo].[sp_SYNC_InsertEtagThangTable]    Script Date: 23/03/2017 2:06:36 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_SYNC_InsertEtagThangTable] 
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
	INSERT INTO [dbo].EtagThang
         (  [MaETag] ,
	[GiaVe] ,
	[MaLoaiVe] ,
	[SoXe] ,
	[NgayBatDau] ,
	[NgayKetThuc] ,
	[MaNhanVien] )
     VALUES
           (@MaETag ,
	@GiaVe ,
	@MaLoaiVe ,
	@SoXe ,
	@NgayBatDau ,
	@NgayKetThuc ,
	@MaNhanVien )
END
