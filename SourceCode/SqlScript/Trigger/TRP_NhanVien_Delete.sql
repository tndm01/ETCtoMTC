USE [Synchronization_ETC]
GO
/****** Object:  Trigger [dbo].[TRP_NhanVien_Delete]    Script Date: 23/03/2017 1:08:44 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create  TRIGGER [dbo].[TRP_NhanVien_Delete]   
ON [dbo].[NhanVien]  AFTER  delete AS

declare @TableName varchar(500);
declare @Action varchar(100)
declare @myxml xml;
SET @TableName = 'NhanVien'
SET @Action = 'DELETE'
SET @myxml = (SELECT  *  FROM DELETED  FOR XML RAW('NhanVien'), ELEMENTS)
  --FOR xml PATH,ROOT)

EXECUTE dbo.sp_Sync_InsertDatatoTrackingTable	@TableName, @Action, @myxml