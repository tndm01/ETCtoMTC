USE [Synchronization_ETC]
GO

/****** Object:  Table [dbo].[LoaiVe]    Script Date: 23/03/2017 9:37:00 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LoaiVe](
	[MSLOAIVE] [char](2) NOT NULL,
	[TENLOAIVE] [varchar](200) NULL,
	[GIAVE] [int] NOT NULL,
	[DIENGIAI] [varchar](250) NOT NULL,
	[LOAIVE] [nchar](2) NOT NULL,
	[LoaiUuTien] [smallint] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


