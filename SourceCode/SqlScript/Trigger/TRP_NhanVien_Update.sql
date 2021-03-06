USE [Synchronization_ETC]
GO
/****** Object:  Trigger [dbo].[TRP_NhanVien_Update]    Script Date: 23/03/2017 1:09:24 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  TRIGGER [dbo].[TRP_NhanVien_Update]   
ON [dbo].[NhanVien]  AFTER  UPDATE AS

declare @TableName varchar(500);
declare @Action varchar(100)
declare @myxml xml;
SET @TableName = 'NhanVien'
SET @Action = 'UPDATE'
SET @myxml = (SELECT  *  FROM INSERTED  FOR XML RAW('NhanVien'), ELEMENTS)
  --FOR xml PATH,ROOT)

EXECUTE dbo.sp_Sync_InsertDatatoTrackingTable	@TableName, @Action, @myxml