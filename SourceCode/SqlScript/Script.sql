--SQL Table 
--Cac table này tao ở cả 2 database MTC và ETC
-----------------
-- Phiếu xuất vé lượt/vé tháng quý// Đối với vé lượt, MSLoaiVe < 20
CREATE TABLE [dbo].[CT_PhieuXuat](
 [MaPX] [int] NOT NULL,
 [MSLOAIVE] [char](2) NOT NULL,
 [SERIAL_FROM] [varchar](20) NOT NULL,
 [SERIAL_TO] [varchar](20) NOT NULL,
 [SOLUONG] [int] NOT NULL,
 [Editable] [char](1) NOT NULL,
 CONSTRAINT [PK_CT_PhieuXuat] PRIMARY KEY CLUSTERED 
(
 [MaPX] ASC,
 [SERIAL_FROM] ASC,
 [SERIAL_TO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
----------------------------------------
-- MTC Tables
-- [Active table] - Dữ liệu vé lượt
----------------------------------------
CREATE TABLE [dbo].[Active](
	[TID] [varchar](20) NOT NULL,
	[LOGIN] [varchar](50) NULL,
 CONSTRAINT [PK_Active] PRIMARY KEY CLUSTERED 
(
	[TID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
-----------------------------------------
-- Thông tin vé tháng quí
CREATE TABLE [dbo].[VeThang_Qui](
 [TID] [varchar](20) NOT NULL,
 [GIOBAN] [int] NOT NULL,
 [NGAYBAN] [datetime] NOT NULL,
 [NGAYBD] [datetime] NOT NULL,
 [NGAYKT] [datetime] NOT NULL,
 [SOXE] [varchar](15) NULL,
 [KH] [varchar](100) NULL,
 [DCKH] [varchar](200) NOT NULL,
 [GIAVE] [int] NOT NULL,
 [MSLOAIVE] [char](2) NOT NULL,
 [LOGIN] [varchar](20) NOT NULL,
 [Ca] [char](3) NOT NULL,
 [MSloaixe] [char](2) NOT NULL,
 [HaTai] [int] NULL,
 [SoDangKiem] [varchar](20) NULL,
 [Expired] [bit] NULL,
 [ChuyenKhoan] [int] NULL CONSTRAINT [DF_VeThang_Qui_ChuyenKhoan]  DEFAULT ((0)),
 [MSTram] [char](1) NULL CONSTRAINT [DF_VeThang_Qui_MSTram]  DEFAULT ((1)),
 CONSTRAINT [PK_VeThang_Qui] PRIMARY KEY CLUSTERED 
(
 [TID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
-----------------------------------------------

-----------------------------------------
--Data_Soxe table - Dữ liệu số xe

CREATE TABLE [dbo].[Data_Soxe](
 [SoXe] [varchar](15) NOT NULL,
 [MSLoaiVe] [varchar](2) NOT NULL,
 [GiaVe] [numeric](18, 0) NOT NULL,
 [NgayDangKy] [datetime] NULL,
 [SoDangkiem] [varchar](30) NULL,
 [Taitrong] [varchar](50) NULL,
 [ENABLED] [tinyint] NULL,
 [Ghichu] [varchar](255) NULL,
 [Login] [varchar](20) NULL,
 [NgayNhap] [datetime] NULL,
 [F0] [char](1) NULL,
 [F1] [char](1) NULL,
 [F2] [char](1) NULL,
 [GhiChu_F1] [varchar](50) NULL CONSTRAINT [DF_Data_Soxe_XeVuotTram]  DEFAULT ((0)),
 [MSTram] [char](1) NULL DEFAULT ((1)),
 CONSTRAINT [PK_Data_Soxe] PRIMARY KEY CLUSTERED 
(
 [SoXe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-----------------------------
--Table Nhan Vien - Dữ liệu nhân viên
CREATE TABLE [dbo].[NhanVien](
 [MSNV] [varchar](20) NOT NULL,
 [MSTO] [char](3) NOT NULL CONSTRAINT [DF_NhanVien_MSTO]  DEFAULT (''),
 [HONV] [varchar](100) NOT NULL CONSTRAINT [DF_NhanVien_HONV]  DEFAULT (''),
 [TENNV] [varchar](100) NOT NULL CONSTRAINT [DF_NhanVien_TENNV]  DEFAULT (''),
 [TEN_SEARCH] [varchar](20) NOT NULL,
 [DIACHI] [varchar](250) NULL CONSTRAINT [DF_NhanVien_DIACHI]  DEFAULT (''),
 [DIENTHOAI] [varchar](15) NULL CONSTRAINT [DF_NhanVien_DIENTHOAI]  DEFAULT (''),
 [GHICHU] [varchar](250) NULL CONSTRAINT [DF_NhanVien_GHICHU]  DEFAULT (''),
 [MSTram] [char](1) NOT NULL CONSTRAINT [DF_NhanVien_MSTram]  DEFAULT ((3)),
 CONSTRAINT [PK_NhanVien] PRIMARY KEY CLUSTERED 
(
 [MSNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

------------------------------------------------
-- Store Insert vé lượt vào bảng Acitve
ALTER PROCEDURE [dbo].[AddBlocTicket_Active]
@SerialRoot as varchar(20),
@SoLuong as int,
@Login as varchar(20),
@Type as varchar(1)=1,
@Result smallint=1 Output
 AS
DECLARE @sTID varchar(20)
DECLARE @Serial as varchar(20)
DECLARE @SoBatDau as int
declare @iCount as int
declare @sTemp as varchar(20)
select @Serial=left(@SerialRoot,LEN(@SerialRoot) - 7)
select @SoBatDau=convert(int, right(@SerialRoot,7))
select @iCount=@SoBatDau
set @Type = SUBSTRING(@SerialRoot,len(@SerialRoot)-8,1)
while @iCount < @SoBatDau + @SoLuong
    BEGIN
    select @sTemp=right(('0000000'+ convert(varchar(20),@iCount)),7)
    select @sTID= @Serial + @sTemp        
    if(@Type = '1')    
  begin
   DELETE from Active where tid = @sTID
   INSERT INTO dbo.Active(TID,Login) VALUES(@sTID,@Login) 
  end     
 else     
     INSERT INTO dbo.Active_VeThang_Qui(TID,Login) VALUES(@sTID,@Login)
  select @iCount=@iCount+1
    End    
Return
--------------------------------------------------------
-- Dư liệu soát vé lượt bên ETC

CREATE TABLE [dbo].[SoatVeVangLai](
 [TID] [varchar](20) NOT NULL,
 [MSLANE] [char](3) NOT NULL,
 [GIAVE] [int] NOT NULL,
 [GIOSOAT] [int] NOT NULL,
 [NGAYSOAT] [datetime] NOT NULL,
 [Login] [varchar](20) NOT NULL,
 [Ca] [char](3) NULL,
 [MSLoaive] [char](2) NULL,
 [MSloaixe] [char](2) NULL,
 [Checker] [varchar](20) NULL,
 [SoXe_ND] [varchar](20) NULL,
 [F0] [char](1) NULL,
 [F1] [char](1) NULL,
 [F2] [char](1) NULL,
 [ImageID] [nvarchar](20) NULL,
 [SyncEtcMtc] [int] NOT NULL,
 [SyncFeBe] [int] NOT NULL,
 [EtagID] [varchar](24) NULL
) ON [PRIMARY]

GO
---------------------------------------------
-- Dữ liệu soát vé tháng quý bên ETC
CREATE TABLE [dbo].[SoatVeThang_Qui](
 [TID] [varchar](20) NOT NULL,
 [GIOSOAT] [int] NOT NULL,
 [NGAYSOAT] [datetime] NOT NULL,
 [MSLANE] [char](3) NOT NULL,
 [Login] [varchar](20) NOT NULL,
 [Ca] [char](3) NULL,
 [MSLoaive] [char](2) NULL,
 [MSloaixe] [char](2) NULL,
 [Checker] [varchar](20) NULL,
 [SoXe_ND] [varchar](20) NULL,
 [F0] [char](1) NULL,
 [F1] [char](1) NULL,
 [F2] [char](1) NULL,
 [ImageID] [nvarchar](20) NULL,
 [SyncEtcMtc] [int] NOT NULL,
 [SyncFeBe] [int] NOT NULL,
 [EtagID] [varchar](24) NULL
) ON [PRIMARY]

GO
-----------------------------------------------------
-- Dữ liệu soát vé ưu tiên Bộ Tài Chính bên ETC
CREATE TABLE [dbo].[SoatVe_BTC](
 [ID] [varchar](20) NOT NULL,
 [Loaixe] [char](2) NULL,
 [MSLane] [char](3) NOT NULL,
 [Login] [varchar](20) NULL,
 [Ngayqua] [datetime] NULL,
 [Gioqua] [int] NULL,
 [Ca] [char](3) NULL,
 [Checker] [varchar](20) NOT NULL,
 [F0] [char](1) NOT NULL,
 [F1] [char](1) NOT NULL,
 [F2] [char](1) NOT NULL,
 [Soxe_ND] [nvarchar](20) NULL,
 [ImageID] [nvarchar](20) NULL,
 [IsConsider] [int] NULL,
 [SyncEtcMtc] [int] NOT NULL,
 [SyncFeBe] [int] NOT NULL,
 [EtagID] [varchar](24) NULL
) ON [PRIMARY]

GO

-----------------------------------------------------------
-- Dữ liệu soát vé ưu tiên bên ETC
CREATE TABLE [dbo].[ForceOpen](
 [NGAYMO] [datetime] NOT NULL,
 [GIOMO] [int] NOT NULL,
 [LOGIN] [varchar](20) NOT NULL,
 [MSLANE] [char](3) NOT NULL,
 [NGAYDONG] [datetime] NOT NULL,
 [GIODONG] [int] NOT NULL,
 [Ca] [char](3) NOT NULL,
 [TID] [varchar](20) NOT NULL,
 [Reason] [smallint] NULL,
 [Checker] [varchar](20) NULL,
 [SoXe_ND] [varchar](20) NULL,
 [F0] [char](1) NULL,
 [F1] [char](1) NULL,
 [F2] [char](1) NULL,
 [ImageID] [nvarchar](20) NULL,
 [IsConsider] [int] NULL,
 [Note] [varchar](200) NULL,
 [FP] [int] NULL,
 [SyncEtcMtc] [int] NOT NULL,
 [SyncFeBe] [int] NOT NULL,
 [EtagID] [varchar](24) NULL
) ON [PRIMARY]
--------------------------------------------------------------------

-- Dữ liệu soát vé Etag bên ETC
GO
CREATE TABLE [dbo].[TRP_tblCheckObuAccount_RFID](
 [CheckObuAccountID] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
 [ObuID] [nvarchar](50) NOT NULL,
 [PrepaidAccountID] [nvarchar](20) NULL,
 [CheckDate] [datetime] NOT NULL,
 [CheckTime] [int] NULL,
 [BeginBalance] [int] NOT NULL,
 [ChargeAmount] [int] NOT NULL,
 [Balance] [int] NULL,
 [VehicleClassID] [int] NULL,
 [LoginID] [nvarchar](20) NOT NULL,
 [LaneID] [nvarchar](20) NOT NULL,
 [ShiftID] [nvarchar](20) NOT NULL,
 [StationID] [nvarchar](20) NOT NULL,
 [RegisPlateNumber] [nvarchar](12) NULL,
 [PlateType] [smallint] NULL,
 [RecogPlateNumber] [nvarchar](12) NULL,
 [IsIntelligentVerified] [bit] NULL,
 [IntelVerifyResult] [smallint] NULL,
 [ImageID] [nvarchar](20) NULL,
 [ImageStatus] [int] NULL,
 [PeriodTicket] [int] NULL,
 [Checker] [nvarchar](20) NULL,
 [SupervisionStatus] [char](1) NULL,
 [PreSupervisionStatus] [char](1) NULL,
 [F0] [char](1) NULL,
 [F1] [char](1) NULL,
 [F2] [char](1) NULL,
 [SyncStatus] [smallint] NULL,
 [ModifyDate] [datetime] NULL,
 [SyncReturnMsg] [nvarchar](500) NULL,
 [FP] [int] NULL,
 [FC] [int] NULL,
 [TransactionStatus] [int] NULL,
 [TicketID] [nvarchar](20) NULL,
 [CheckInDate] [datetime] NULL,
 [CommitDate] [datetime] NULL,
 [ETCStatus] [char](1) NULL,
 [FeeType] [char](3) NULL,
 [FeeChargeType] [int] NULL,
 [FeeChargeInfo] [nvarchar](100) NULL,
 [Notes] [nvarchar](500) NULL,
 [SyncEtcMtc] [int] NOT NULL,
 [SyncFeBe] [int] NOT NULL,
 [IsOnlineCheck] [int] NOT NULL,
 [TID] [varchar](48) NULL,
 CONSTRAINT [PK_TRP_tblCheckObuAccount_RFID] PRIMARY KEY CLUSTERED 
(
 [CheckObuAccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO