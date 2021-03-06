USE [Synchronization_ETC]
GO
/****** Object:  Trigger [dbo].[SoatVeVangLai_Insert]    Script Date: 17/03/2017 12:22:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  TRIGGER [dbo].[SoatVeVangLai_Insert]   
ON [dbo].[SoatVeVangLai]  AFTER  INSERT AS

declare @TableName varchar(500);
declare @Action varchar(100)
declare @myxml xml;
SET @TableName = 'SoatVeVangLai'
SET @Action = 'INSERT'
SET @myxml = (SELECT  *  FROM INSERTED  FOR XML RAW('SoatVeVangLai'), ELEMENTS)
  --FOR xml PATH,ROOT)

EXECUTE dbo.sp_Sync_InsertDatatoTrackingTable	@TableName, @Action, @myxml