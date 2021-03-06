USE [Synchronization_ETC]
GO
/****** Object:  Trigger [dbo].[CT_PhieuXuat_Insert]    Script Date: 23/03/2017 9:37:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Batch submitted through debugger: SQLQuery60.sql|7|0|C:\Users\WINUIT\AppData\Local\Temp\~vsDB16.sql
ALTER   TRIGGER [dbo].[CT_PhieuXuat_Insert]   
ON [dbo].[CT_PhieuXuat]  AFTER  INSERT AS
--Giave, soluong, vé đầu, vé cuối, mã nhân viên nhận, ngày tạo, Ca xuất, Ma NV xuất
DECLARE @GIAVE int
DECLARE @SOLUONG INT
DECLARE @SERIAL_FROM VARCHAR(20)
DECLARE @SERIAL_TO VARCHAR(20)
DECLARE @Editable CHAR(1)
DECLARE @MaPX int
DECLARE @MSLoaive char(2)
DECLARE @MaNvNhan varchar(20)
DECLARE @NgayXuat Datetime
DECLARE @Ca varchar(3)
DECLARE @MaNvXuat varchar(20)

declare @TableName varchar(500)
declare @Action varchar(100)
declare @myxml xml;
SET @TableName = 'CT_PhieuXuat'
SET @Action = 'INSERT'

Select @MaPX = MaPX , @MSLoaive = MSLoaiVe,@SERIAL_FROM = SERIAL_FROM ,@SERIAL_TO = SERIAL_TO,@SOLUONG = SOLUONG from inserted
if(@MSLoaive <20)
	begin
select @MaNvNhan = NGUOINHAN, @MaNvXuat = NGUOIGIAO,@NgayXuat = NGAYXUAT,@Ca = Ca
from PhieuXuat 
where MaPX = @MaPX

select @GIAVE = GIAVE from LoaiVe where MSLoaive = @MSLoaive
--SET @myxml = (SELECT  *  FROM INSERTED  FOR XML RAW('CT_PhieuXuat'), ELEMENTS)
  --FOR xml PATH,ROOT)
SET @myxml = (SELECT GIAVE =@GIAVE,SOLUONG = @SOLUONG,SERIAL_FROM = @SERIAL_FROM,SERIAL_TO = @SERIAL_TO,MAPX= @MaPX,MSLOAIVE =@MSLoaive,NVNHAN = @MaNvNhan,NGAYXUAT = @NgayXuat,CA =@Ca, NVXUAT = @MaNvXuat   FOR XML RAW('CT_PhieuXuat'), ELEMENTS)
EXECUTE dbo.sp_Sync_InsertDatatoTrackingTable	@TableName, @Action, @myxml
end