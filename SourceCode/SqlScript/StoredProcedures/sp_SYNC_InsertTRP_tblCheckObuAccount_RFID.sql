USE [Synchronization_ETC]
GO

/****** Object:  StoredProcedure [dbo].[sp_SYNC_InsertTRP_tblCheckObuAccount_RFID]    Script Date: 03/23/2017 4:39:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_SYNC_InsertTRP_tblCheckObuAccount_RFID]
@ImageID nvarchar(20), 
@CheckDate datetime, 
@ObuID nvarchar(50), 
@ChargeAmount int, 
@RecogPlateNumber nvarchar(12),
@LoginID nvarchar(20),
@ShiftID nvarchar(20),
@LaneID nvarchar(20),
@TicketID nvarchar(20), 
@RegisPlateNumber nvarchar(12)
 
as
BEGIN
INSERT INTO TRP_tblCheckObuAccount_RFID
(
ImageID, 
CheckDate, 
ObuID, 
ChargeAmount, 
RecogPlateNumber, 
LoginID, 
ShiftID, 
LaneID, 
TicketID, 
RegisPlateNumber,
BeginBalance,
StationID,
SyncEtcMtc,
SyncFeBe,
IsOnlineCheck
)
VALUES
(
@ImageID,
@CheckDate,
@ObuID,
@ChargeAmount,
@RecogPlateNumber,
@LoginID,
@ShiftID,
@LaneID,
@TicketID,
@RegisPlateNumber,
0,
'',
0,
0,
0
)
END
GO

