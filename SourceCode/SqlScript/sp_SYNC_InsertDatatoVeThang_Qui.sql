USE [Synchronization_ETC]
GO
/****** Object:  StoredProcedure [dbo].[sp_Sync_InsertDatatoSoatVeThang_Qui]    Script Date: 20/03/2017 9:56:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_SYNC_InsertDatatoVeThang_Qui]
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
 insert into VeThang_Qui values (
	@TID,
	@GIOBAN,
	@NGAYBAN,
	@NGAYBD,
	@NGAYKT,
	@SOXE,
	@KH,
	@DCKH,
	@GIAVE,
	@MSLOAIVE,
	@LOGIN,
	@Ca,
	@MSloaixe,
	@HaTai,
	@SoDangKiem,
	@Expired,
	@ChuyenKhoan,
	@MSTram)
END