USE [Synchronization_ETC]
GO
/****** Object:  StoredProcedure [dbo].[sp_SYNC_InsertDatatoSoatVeVangLai]    Script Date: 22/03/2017 1:48:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_SYNC_InsertDatatoSoatVeVangLai]
	@TID	varchar(20),
	@MSLANE	char(3),
	@GIAVE	int	,
	@GIOSOAT	int	,
	@NGAYSOAT	datetime,
	@Login	varchar(20)	,
	@Ca	char(3),
	@MSLoaive	char(2)	,
	@MSloaixe	char(2),
	@Checker	varchar(20)	,
	@SoXe_ND	varchar(20)	,
	@F0	char(1)	,
	@F1	char(1)	,
	@F2	char(1)	,
	@ImageID	nvarchar(20)	,
	@SyncEtcMtc	int	,
	@SyncFeBe	int,
	@EtagID	varchar(24),
	@MSTRAM char(1),
	@KHUHOI char (1)

as
BEGIN
 insert into SoatVeVangLai values (
	@TID,
	@MSLANE,
	@GIAVE,
	@GIOSOAT,
	@NGAYSOAT,
	@Login,
	@Ca,
	@MSLoaive,
	@MSloaixe,
	@Checker,
	@SoXe_ND,
	@F0,
	@F1,
	@F2,
	@ImageID,
	@SyncEtcMtc,
	@SyncFeBe,
	@EtagID,
	@MSTRAM,
	@KHUHOI
)
END