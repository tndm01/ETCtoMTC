USE [Synchronization_ETC]
GO
/****** Object:  Trigger [dbo].[TRP_EtagThang_Delete]    Script Date: 23/03/2017 1:08:06 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  TRIGGER [dbo].[TRP_EtagThang_Delete]   
ON [dbo].[EtagThang]  AFTER  delete AS

declare @TableName varchar(500);
declare @Action varchar(100)
declare @myxml xml;
SET @TableName = 'EtagThang'
SET @Action = 'DELETE'
SET @myxml = (SELECT  *  FROM DELETED  FOR XML RAW('EtagThang'), ELEMENTS)
  --FOR xml PATH,ROOT)

EXECUTE dbo.sp_Sync_InsertDatatoTrackingTable	@TableName, @Action, @myxml