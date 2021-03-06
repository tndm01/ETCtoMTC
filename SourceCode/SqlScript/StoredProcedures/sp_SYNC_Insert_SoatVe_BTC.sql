USE [Synchronization_ETC]
GO
/****** Object:  StoredProcedure [dbo].[sp_SYNC_Insert_SoatVe_BTC]    Script Date: 03/22/2017 13:11:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_SYNC_Insert_SoatVe_BTC]
@ID varchar(20),
@LoaiXe char(2),
@MsLane char(3),
@Login varchar(20),
@Ca char(3),
@Checker varchar(20),
@F0 char(1),
@F1 char(1),
@F2 char(1),
@Soxe_ND nvarchar(20),
@ImageID nvarchar(20),
@SyncEtcMtc int,
@SyncFeBe int
AS
BEGIN
	Insert SoatVe_BTC(ID,Loaixe,MSLane,Login,Ca,Checker,F0,F1,F2,Soxe_ND,ImageID,SyncEtcMtc,SyncFeBe)
	values(@ID,@LoaiXe,@MsLane,@Login
			,@Ca,@Checker,@F0,@F1,@F2,@Soxe_ND,@ImageID
			,@SyncEtcMtc,@SyncFeBe)
END
