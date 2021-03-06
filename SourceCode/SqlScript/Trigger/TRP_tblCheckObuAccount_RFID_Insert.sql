

/****** Object:  Trigger [dbo].[TRP_tblCheckObuAccount_RFID_Insert]    Script Date: 3/14/2017 2:52:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
cREATE  TRIGGER [dbo].[TRP_tblCheckObuAccount_RFID_Insert]   
ON [dbo].[TRP_tblCheckObuAccount_RFID]  AFTER  INSERT AS

declare @TableName varchar(500);
declare @Action varchar(100)
declare @myxml xml;
SET @TableName = 'TRP_tblCheckObuAccount_RFID'
SET @Action = 'INSERT'
SET @myxml = (SELECT  *  FROM INSERTED  FOR XML RAW('TRP_tblCheckObuAccount_RFID'), ELEMENTS)
  --FOR xml PATH,ROOT)

EXECUTE dbo.sp_Sync_InsertDatatoTrackingTable	@TableName, @Action, @myxml