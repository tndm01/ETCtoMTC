USE [Synchronization_ETC]
GO
/****** Object:  Trigger [dbo].[VeThang_Qui_Delete]    Script Date: 24/03/2017 12:05:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   TRIGGER [dbo].[VeThang_Qui_Update]   
ON [dbo].[VeThang_Qui]  AFTER  Update AS

declare @TableName varchar(500);
declare @Action varchar(100)
declare @myxml xml;
SET @TableName = 'VeThang_Qui'
SET @Action = 'UPDATE'
SET @myxml = (SELECT  *  FROM inserted  FOR XML RAW('VeThang_Qui'), ELEMENTS)
  --FOR xml PATH,ROOT)

EXECUTE dbo.sp_Sync_InsertDatatoTrackingTable	@TableName, @Action, @myxml