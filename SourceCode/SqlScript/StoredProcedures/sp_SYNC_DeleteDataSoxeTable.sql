USE [Synchronization_ETC]
GO
/****** Object:  StoredProcedure [dbo].[sp_SYNC_DeleteDataSoxeTable]    Script Date: 22/03/2017 5:56:38 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[sp_SYNC_DeleteDataSoxeTable] 
	-- Add the parameters for the stored procedure here
	
	@SOXE varchar(15)	

AS
BEGIN
	Delete Data_Soxe 
	where SoXe=@SOXE
	
END