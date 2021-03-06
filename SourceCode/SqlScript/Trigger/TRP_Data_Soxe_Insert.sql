USE [Synchronization_ETC]
GO
/****** Object:  Trigger [dbo].[TRP_Data_Soxe_Insert]    Script Date: 24/03/2017 10:06:23 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER  TRIGGER [dbo].[TRP_Data_Soxe_Insert]   
ON [dbo].[Data_Soxe]  AFTER  INSERT AS

declare @TableName varchar(500);
declare @Action varchar(100)
declare @myxml xml;
SET @TableName = 'Data_Soxe'
SET @Action = 'INSERT'
SET @myxml = (SELECT  *  FROM INSERTED  FOR XML RAW('Data_Soxe'), ELEMENTS)
  --FOR xml PATH,ROOT)

EXECUTE dbo.sp_Sync_InsertDatatoTrackingTable	@TableName, @Action, @myxml