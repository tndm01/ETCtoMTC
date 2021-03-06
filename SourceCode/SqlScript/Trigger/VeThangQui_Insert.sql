USE [Synchronization_ETC]
GO
/****** Object:  Trigger [dbo].[VeThang_Qui_Insert]    Script Date: 22/03/2017 10:15:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   TRIGGER [dbo].[VeThang_Qui_Insert]   
ON [dbo].[VeThang_Qui]  AFTER  INSERT AS

declare @TableName varchar(500);
declare @Action varchar(100)
declare @myxml xml;
SET @TableName = 'VeThang_Qui'
SET @Action = 'INSERT'
SET @myxml = (SELECT  *  FROM INSERTED  FOR XML RAW('VeThang_Qui'), ELEMENTS)
  --FOR xml PATH,ROOT)

EXECUTE dbo.sp_Sync_InsertDatatoTrackingTable	@TableName, @Action, @myxml