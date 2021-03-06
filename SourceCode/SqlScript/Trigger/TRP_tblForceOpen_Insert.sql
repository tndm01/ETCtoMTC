USE [Synchronization_ETC]
GO
/****** Object:  Trigger [dbo].[TRP_tblForceOpen_Insert]    Script Date: 03/17/2017 12:27:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER  TRIGGER [dbo].[TRP_tblForceOpen_Insert]   
ON [dbo].[ForceOpen]  AFTER  INSERT AS
declare @TableName varchar(500);
declare @Action varchar(100)
declare @myxml xml;
SET @TableName = 'ForceOpen'
SET @Action = 'INSERT'
SET @myxml = (SELECT  *  FROM INSERTED  FOR XML RAW('ForceOpen'), ELEMENTS)
  --FOR xml PATH,ROOT)

EXECUTE dbo.sp_Sync_InsertDatatoTrackingTable	@TableName, @Action, @myxml