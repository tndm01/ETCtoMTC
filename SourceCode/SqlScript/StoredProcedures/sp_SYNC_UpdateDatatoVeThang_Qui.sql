USE [Synchronization_ETC]
GO
/****** Object:  StoredProcedure [dbo].[sp_SYNC_InsertDatatoVeThang_Qui]    Script Date: 23/03/2017 11:26:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_SYNC_UpdateDatatoVeThang_Qui]
 @TID varchar(20),
 @GIOBAN int,
 @NGAYBAN datetime,
 @NGAYBD datetime,
 @NGAYKT datetime,
 @SOXE varchar(15),
 @KH varchar(100),
 @DCKH varchar(200),
 @GIAVE int,
 @MSLOAIVE char(2),
 @LOGIN varchar(20),
 @Ca char(3),
 @MSloaixe char(2),
 @HaTai int,
 @SoDangKiem varchar(20),
 @Expired bit,
 @ChuyenKhoan int,
 @MSTram char(1)
 
as
BEGIN
update VeThang_Qui 
set
	TID=@TID,
	GIOBAN=@GIOBAN,
	NGAYBAN=@NGAYBAN,
	NGAYBD=@NGAYBD,
	NGAYKT=@NGAYKT,
	SOXE=@SOXE,
	KH=@KH,
	DCKH=@DCKH,
	GIAVE=@GIAVE,
	MSLOAIVE=@MSLOAIVE,
	VeThang_Qui.LOGIN=@LOGIN,
	Ca=@Ca,
	MSloaixe=@MSloaixe,
	HaTai=@HaTai,
	SoDangKiem=@SoDangKiem,
	Expired=@Expired,
	ChuyenKhoan=@ChuyenKhoan,
	MSTram=@MSTram
where
TID=@TID
END