USE [Synchronization_ETC]
GO

/****** Object:  Table [dbo].[PhieuXuat]    Script Date: 23/03/2017 9:36:03 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PhieuXuat](
	[MaPX] [int] NOT NULL,
	[MaPTX] [char](3) NOT NULL,
	[NGAYXUAT] [datetime] NOT NULL,
	[NGUOINHAN] [varchar](20) NULL,
	[NGUOIGIAO] [varchar](20) NULL,
	[TINHTRANG] [char](1) NULL,
	[COSUA] [char](1) NULL,
	[SoPhieu] [smallint] NULL,
	[Nam] [smallint] NULL,
	[Ca] [varchar](3) NULL,
	[Thang] [smallint] NULL,
	[MSTram] [char](1) NULL,
 CONSTRAINT [PK_PhieuXuat] PRIMARY KEY CLUSTERED 
(
	[MaPX] ASC,
	[MaPTX] ASC,
	[NGAYXUAT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


