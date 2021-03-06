USE [Synchronization_ETC]
GO
/****** Object:  Trigger [dbo].[SoatVeThang_Qui_Insert]    Script Date: 17/03/2017 12:24:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   TRIGGER [dbo].[SoatVeThang_Qui_Insert]   
ON [dbo].[SoatVeThang_Qui]  AFTER  INSERT AS

declare @TableName varchar(500);
declare @Action varchar(100)
declare @myxml xml;
SET @TableName = 'SoatVeThang_Qui'
SET @Action = 'INSERT'
SET @myxml = (SELECT  *  FROM INSERTED  FOR XML RAW('SoatVeThang_Qui'), ELEMENTS)
  --FOR xml PATH,ROOT)

EXECUTE dbo.sp_Sync_InsertDatatoTrackingTable	@TableName, @Action, @myxml