USE [Synchronization_ETC]
GO
/****** Object:  Trigger [dbo].[TRP_EtagThang_Update]    Script Date: 23/03/2017 1:08:28 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  TRIGGER [dbo].[TRP_EtagThang_Update]   
ON [dbo].[EtagThang]  AFTER  update AS

declare @TableName varchar(500);
declare @Action varchar(100)
declare @myxml xml;
SET @TableName = 'EtagThang'
SET @Action = 'UPDATE'
SET @myxml = (SELECT  *  FROM INSERTED  FOR XML RAW('EtagThang'), ELEMENTS)
  --FOR xml PATH,ROOT)

EXECUTE dbo.sp_Sync_InsertDatatoTrackingTable	@TableName, @Action, @myxml