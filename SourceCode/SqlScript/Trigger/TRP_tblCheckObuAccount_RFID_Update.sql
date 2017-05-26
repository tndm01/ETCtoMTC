USE [Synchronization_ETC]
GO

/****** Object:  Trigger [dbo].[TRP_tblCheckObuAccount_RFID_Update]    Script Date: 03/17/2017 11:28:43 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE  TRIGGER [dbo].[TRP_tblCheckObuAccount_RFID_Update]   
ON [dbo].[TRP_tblCheckObuAccount_RFID]  AFTER  UPDATE AS

declare @TableName varchar(500);
declare @Action varchar(100)
declare @myxml xml;
SET @TableName = 'TRP_tblCheckObuAccount_RFID'
SET @Action = 'UPDATE'
SET @myxml = (SELECT  *  FROM INSERTED  FOR XML RAW('TRP_tblCheckObuAccount_RFID'), ELEMENTS)
  --FOR xml PATH,ROOT)

EXECUTE dbo.sp_Sync_InsertDatatoTrackingTable	@TableName, @Action, @myxml
GO

