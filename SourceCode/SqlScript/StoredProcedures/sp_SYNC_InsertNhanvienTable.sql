USE [Synchronization_ETC]
GO
/****** Object:  StoredProcedure [dbo].[sp_SYNC_InsertNhanvienTable]    Script Date: 17/03/2017 12:09:21 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_SYNC_InsertNhanvienTable] 
	-- Add the parameters for the stored procedure here
	
	@MSNV varchar(20),
	@MSTO char(3),
	@HONV varchar(100),
	@TENNV varchar(100),
	@TEN_SEARCH varchar(20),
	@DIACHI varchar(20),
	@DIENTHOAI varchar(15),
	@GHICHU varchar(250),
	@MSTram char(1)
AS
BEGIN
	INSERT INTO [dbo].[NhanVien]
         (  [MSNV] 
			,[MSTO], 
			[HONV],
			[TENNV], 
			[TEN_SEARCH],
			[DIACHI], 
			[DIENTHOAI] ,
			[GHICHU] ,
			[MSTram])
     VALUES
           (@MSNV ,
			@MSTO ,
			@HONV ,
			@TENNV ,
			@TEN_SEARCH ,
			@DIACHI ,
			@DIENTHOAI ,
			@GHICHU ,
			@MSTram )
END
