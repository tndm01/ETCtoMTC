USE [Synchronization_ETC]
GO
/****** Object:  Trigger [dbo].[TRP_tblSoatVe_BTC_Insert]    Script Date: 03/19/2017 20:29:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[TRP_tblSoatVe_BTC_Insert]   
ON [dbo].[SoatVe_BTC]  AFTER  INSERT AS

declare @TableName varchar(500);
declare @Action varchar(100)
declare @myxml xml;
SET @TableName = 'SoatVe_BTC'
SET @Action = 'INSERT'
SET @myxml = (SELECT  *  FROM INSERTED  FOR XML RAW('SoatVe_BTC'), ELEMENTS)
  --FOR xml PATH,ROOT)

EXECUTE dbo.sp_Sync_InsertDatatoTrackingTable	@TableName, @Action, @myxml