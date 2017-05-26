USE [Synchronization_ETC]
GO

/****** Object:  Table [dbo].[EtagThang]    Script Date: 17/03/2017 1:22:45 CH ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[EtagThang](
	[MaETag] [varchar](50) NULL,
	[GiaVe] [varchar](50) NULL,
	[MaLoaiVe] [varchar](50) NULL,
	[SoXe] [varchar](50) NULL,
	[NgayBatDau] [datetime] NULL,
	[NgayKetThuc] [datetime] NULL,
	[MaNhanVien] [varchar](50) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


