USE [Synchronization_ETC]
GO
/****** Object:  StoredProcedure [dbo].[sp_SYNC_InsertDataSoxeTable]    Script Date: 17/03/2017 12:09:57 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_SYNC_InsertDataSoxeTable] 
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
	INSERT INTO [dbo].[Data_Soxe]
         ( [SoXe] ,
	[MSLoaiVe] ,
	[GiaVe] ,
	[NgayDangKy] ,
	[SoDangkiem] ,
	[Taitrong] ,
	[ENABLED] ,
	[Ghichu],
	[Login],
	[NgayNhap] ,
	[F0] ,
	[F1] ,
	[F2],
	[GhiChu_F1],
	[MSTram] )
     VALUES
           (@SOXE,
	@MSLOAIVE ,
	@GIAVE ,
	@NGAYDANGKY ,
	@SODANGKIEM ,
	@TRONGTAI ,
	@ENABLED ,
	@GHICHU ,
	@LOGIN ,
	@NGAYNHAP ,
	@F0 ,
	@F1 ,
	@F2 ,
	@GHICHU_F1 ,
	@MSTRAM )
END
