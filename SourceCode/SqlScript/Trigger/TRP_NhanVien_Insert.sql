USE [Synchronization_ETC]
GO
/****** Object:  Trigger [dbo].[TRP_NhanVien_Insert]    Script Date: 22/03/2017 11:39:32 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  TRIGGER [dbo].[TRP_NhanVien_Insert]   
ON [dbo].[NhanVien]  AFTER  INSERT AS

declare @TableName varchar(500);
declare @Action varchar(100)
declare @myxml xml;
SET @TableName = 'NhanVien'
SET @Action = 'INSERT'
SET @myxml = (SELECT  *  FROM INSERTED  FOR XML RAW('NhanVien'), ELEMENTS)
  --FOR xml PATH,ROOT)

EXECUTE dbo.sp_Sync_InsertDatatoTrackingTable	@TableName, @Action, @myxml