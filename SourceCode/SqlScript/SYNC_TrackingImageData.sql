
/****** Object:  Table [dbo].[SYNC_TrackingImageData]    Script Date: 3/17/2017 11:14:44 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SYNC_TrackingImageData](
	[TrackingID] [bigint] IDENTITY(1,1) NOT NULL,
	[ImageID] [varchar](20) NOT NULL,
	[LaneID] [varchar](20) NOT NULL,
	[SyncStatus] [int] NULL,
 CONSTRAINT [PK_SYNC_TrackingImageData] PRIMARY KEY CLUSTERED 
(
	[TrackingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[SYNC_TrackingImageData] ADD  CONSTRAINT [DF_SYNC_TrackingImageData_SyncStatus]  DEFAULT ((0)) FOR [SyncStatus]
GO


