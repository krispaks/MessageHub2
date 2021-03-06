USE [MessageHubDB]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 4/2/2015 9:29:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedDate] [datetime] NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Comments]    Script Date: 4/2/2015 9:29:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MessageId] [int] NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedDate] [datetime] NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Messages]    Script Date: 4/2/2015 9:29:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Messages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](250) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[SubCategoryId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedDate] [datetime] NULL,
 CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([Id], [ParentId], [Name], [Description], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedDate]) VALUES (1, NULL, N'TestParent', N'Test', 1, CAST(0x0000A41A00000000 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Categories] ([Id], [ParentId], [Name], [Description], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedDate]) VALUES (2, 1, N'TestChild', N'TestChild', 1, CAST(0x0000A41A00000000 AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Categories] OFF
SET IDENTITY_INSERT [dbo].[Messages] ON 

INSERT [dbo].[Messages] ([Id], [Title], [Content], [SubCategoryId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedDate]) VALUES (1, N'Test', N'Test', 2, 1, CAST(0x0000A4590136E813 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Messages] ([Id], [Title], [Content], [SubCategoryId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedDate]) VALUES (2, N'Test', N'Test', 2, 1, CAST(0x0000A45901679DD4 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Messages] ([Id], [Title], [Content], [SubCategoryId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedDate]) VALUES (3, N'Test', N'Test', 2, 1, CAST(0x0000A45B017BFAFD AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Messages] ([Id], [Title], [Content], [SubCategoryId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedDate]) VALUES (1002, N'Test', N'Test', 2, 1, CAST(0x0000A45D017A3956 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Messages] ([Id], [Title], [Content], [SubCategoryId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedDate]) VALUES (1003, N'Test', N'Test', 2, 1, CAST(0x0000A460016BD5DD AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Messages] ([Id], [Title], [Content], [SubCategoryId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedDate]) VALUES (1004, N'Test', N'Test', 2, 1, CAST(0x0000A46001854BAE AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Messages] ([Id], [Title], [Content], [SubCategoryId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedDate]) VALUES (1005, N'Test', N'Test', 2, 1, CAST(0x0000A464015F540D AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Messages] ([Id], [Title], [Content], [SubCategoryId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedDate]) VALUES (1006, N'Test', N'Test', 2, 1, CAST(0x0000A464015FA87A AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Messages] ([Id], [Title], [Content], [SubCategoryId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedDate]) VALUES (1007, N'Test', N'Test', 2, 1, CAST(0x0000A464015FC8DD AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Messages] ([Id], [Title], [Content], [SubCategoryId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedDate]) VALUES (1008, N'Test', N'Test', 2, 1, CAST(0x0000A464016675A2 AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Messages] OFF
ALTER TABLE [dbo].[Categories]  WITH CHECK ADD  CONSTRAINT [FK_Categories_Categories] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Categories] ([Id])
GO
ALTER TABLE [dbo].[Categories] CHECK CONSTRAINT [FK_Categories_Categories]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Messages] FOREIGN KEY([MessageId])
REFERENCES [dbo].[Messages] ([Id])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Messages]
GO
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_Categories] FOREIGN KEY([SubCategoryId])
REFERENCES [dbo].[Categories] ([Id])
GO
ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_Categories]
GO
