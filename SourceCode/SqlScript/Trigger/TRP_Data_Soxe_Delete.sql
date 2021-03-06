USE [Synchronization_ETC]
GO
/****** Object:  Trigger [dbo].[TRP_Data_Soxe_Delete]    Script Date: 23/03/2017 1:07:25 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create  TRIGGER [dbo].[TRP_Data_Soxe_Delete]   
ON [dbo].[Data_Soxe]  AFTER  Delete AS

declare @TableName varchar(500);
declare @Action varchar(100)
declare @myxml xml;
SET @TableName = 'Data_Soxe'
SET @Action = 'DELETE'
SET @myxml = (SELECT  *  FROM DELETED FOR XML RAW('Data_Soxe'), ELEMENTS)
  --FOR xml PATH,ROOT)

EXECUTE dbo.sp_Sync_InsertDatatoTrackingTable	@TableName, @Action, @myxml