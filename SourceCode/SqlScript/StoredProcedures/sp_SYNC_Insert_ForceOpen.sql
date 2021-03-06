USE [Synchronization_ETC]
GO
/****** Object:  StoredProcedure [dbo].[sp_SYNC_Insert_ForceOpen]    Script Date: 03/21/2017 09:59:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_SYNC_Insert_ForceOpen]
@NgayMo datetime,
@GioMo int,
@Ngaydong datetime,
@Giodong int,
@Login varchar(20),
@MsLane char(3),
@Ca char(3),
@Tid varchar(20),
@Reason smallint,
@Checker varchar(20),
@Soxe_ND varchar(20),
@F2 char(1),
@ImageID varchar(20),
@SyncEtcMtc int,
@SyncFeBe int
AS
BEGIN
	Insert into ForceOpen (NGAYMO, GIOMO,NGAYDONG,GIODONG, LOGIN, MSLANE,Ca,TID,Reason
	,Checker,SoXe_ND,F2,ImageID,SyncEtcMtc,SyncFeBe)
	values(@NgayMo,@GioMo,@Ngaydong,@Giodong,@Login,@MsLane,@Ca,@Tid,@Reason,@Checker,@Soxe_ND,
		   @F2,@ImageID,@SyncEtcMtc,@SyncFeBe)
END
