USE [dbRestaurantsReview]
GO
/****** Object:  Table [dbo].[tblMember]    Script Date: 2021-10-03 23:06:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblMember](
	[MemberID] [int] IDENTITY(1,1) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Email] [nvarchar](150) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[LastLoginDate] [datetime] NULL,
	[LoginsCount] [int] NOT NULL,
 CONSTRAINT [PK_tblMember] PRIMARY KEY CLUSTERED 
(
	[MemberID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblMemberRole]    Script Date: 2021-10-03 23:06:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblMemberRole](
	[MemberID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_tblMemberRole] PRIMARY KEY CLUSTERED 
(
	[MemberID] ASC,
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblRestaurant]    Script Date: 2021-10-03 23:06:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRestaurant](
	[RestaurantID] [int] IDENTITY(1,1) NOT NULL,
	[OwnerMemberID] [int] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Address] [nvarchar](250) NULL,
	[Description] [nvarchar](2000) NULL,
 CONSTRAINT [PK_tblRestaurant] PRIMARY KEY CLUSTERED 
(
	[RestaurantID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblReview]    Script Date: 2021-10-03 23:06:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblReview](
	[ReviewID] [int] IDENTITY(1,1) NOT NULL,
	[RestaurantID] [int] NOT NULL,
	[ReviewerMemberID] [int] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[RatingStars] [decimal](8, 2) NULL,
	[Comment] [nvarchar](2000) NULL,
 CONSTRAINT [PK_tblReview] PRIMARY KEY CLUSTERED 
(
	[ReviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblReviewReply]    Script Date: 2021-10-03 23:06:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblReviewReply](
	[ReviewID] [int] NOT NULL,
	[ReplyedMemberID] [int] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ReplyText] [nvarchar](2000) NULL,
 CONSTRAINT [PK_tblReviewReply_1] PRIMARY KEY CLUSTERED 
(
	[ReviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblRole]    Script Date: 2021-10-03 23:06:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRole](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](50) NOT NULL,
	[CanReadReviews] [bit] NOT NULL,
	[CanWriteReview] [bit] NOT NULL,
	[CanCreateRestaurant] [bit] NOT NULL,
	[CanEditRestaurant] [bit] NOT NULL,
	[CanReplayToRestaurantReview] [bit] NOT NULL,
	[CanEditMember] [bit] NOT NULL,
	[CanEditRestaurantReview] [bit] NOT NULL,
	[CanEditReplayToRestaurantReview] [bit] NOT NULL,
	[CanEditRole] [bit] NOT NULL,
	[CanListOwnRestaurants] [bit] NOT NULL,
	[CanListAllRestaurants] [bit] NOT NULL,
 CONSTRAINT [PK_tblRole] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tblMember] ON 
GO
INSERT [dbo].[tblMember] ([MemberID], [CreationDate], [FirstName], [LastName], [Email], [Username], [Password], [LastLoginDate], [LoginsCount]) VALUES (6, CAST(N'2021-09-30T21:56:44.947' AS DateTime), N'Thomas', N'Taylor', N'fsda@mfsd.fsd', N'admin', N'admin', CAST(N'2021-10-03T22:25:56.797' AS DateTime), 171)
GO
INSERT [dbo].[tblMember] ([MemberID], [CreationDate], [FirstName], [LastName], [Email], [Username], [Password], [LastLoginDate], [LoginsCount]) VALUES (12, CAST(N'2021-09-30T22:10:36.120' AS DateTime), N'Maria', N'Marisol', N'gsadf@ffsd.as', N'user', N'user', NULL, 0)
GO
INSERT [dbo].[tblMember] ([MemberID], [CreationDate], [FirstName], [LastName], [Email], [Username], [Password], [LastLoginDate], [LoginsCount]) VALUES (14, CAST(N'2021-10-01T11:49:28.010' AS DateTime), N'Barbara', N'Barbados', N'bla@bla.bla', N'ouser', N'ouser', CAST(N'2021-10-03T22:15:42.230' AS DateTime), 268)
GO
INSERT [dbo].[tblMember] ([MemberID], [CreationDate], [FirstName], [LastName], [Email], [Username], [Password], [LastLoginDate], [LoginsCount]) VALUES (15, CAST(N'2021-10-01T18:38:31.777' AS DateTime), N'Arthur', N'King', N'm@m.m', N'muser', N'muser', CAST(N'2021-10-03T20:25:01.857' AS DateTime), 34)
GO
INSERT [dbo].[tblMember] ([MemberID], [CreationDate], [FirstName], [LastName], [Email], [Username], [Password], [LastLoginDate], [LoginsCount]) VALUES (16, CAST(N'2021-10-02T11:59:26.097' AS DateTime), N'Gill', N'Bates', N'gill@bates.com', N'gill', N'gill', CAST(N'2021-10-02T11:59:35.603' AS DateTime), 1)
GO
INSERT [dbo].[tblMember] ([MemberID], [CreationDate], [FirstName], [LastName], [Email], [Username], [Password], [LastLoginDate], [LoginsCount]) VALUES (17, CAST(N'2021-10-03T11:21:40.077' AS DateTime), N'Bronco', N'Billy', N'bb@gmx.net', N'bbbbb', N'bbbbb', CAST(N'2021-10-03T22:35:44.720' AS DateTime), 57)
GO
SET IDENTITY_INSERT [dbo].[tblMember] OFF
GO
INSERT [dbo].[tblMemberRole] ([MemberID], [RoleID]) VALUES (6, 3)
GO
INSERT [dbo].[tblMemberRole] ([MemberID], [RoleID]) VALUES (12, 1)
GO
INSERT [dbo].[tblMemberRole] ([MemberID], [RoleID]) VALUES (14, 2)
GO
INSERT [dbo].[tblMemberRole] ([MemberID], [RoleID]) VALUES (15, 2)
GO
INSERT [dbo].[tblMemberRole] ([MemberID], [RoleID]) VALUES (16, 1)
GO
INSERT [dbo].[tblMemberRole] ([MemberID], [RoleID]) VALUES (17, 1)
GO
SET IDENTITY_INSERT [dbo].[tblRestaurant] ON 
GO
INSERT [dbo].[tblRestaurant] ([RestaurantID], [OwnerMemberID], [CreationDate], [Name], [Address], [Description]) VALUES (1, 14, CAST(N'2021-10-02T11:11:50.430' AS DateTime), N'Yellow Cab Restaurant', N'Some street 123', N'This is the best restaurant ever!')
GO
INSERT [dbo].[tblRestaurant] ([RestaurantID], [OwnerMemberID], [CreationDate], [Name], [Address], [Description]) VALUES (2, 14, CAST(N'2021-10-02T11:11:50.430' AS DateTime), N'Blue Cab Restaurant', N'Silk Road 222', N'This is the best restaurant ever!')
GO
INSERT [dbo].[tblRestaurant] ([RestaurantID], [OwnerMemberID], [CreationDate], [Name], [Address], [Description]) VALUES (3, 14, CAST(N'2021-10-02T11:11:50.430' AS DateTime), N'Silver Cab Restaurant', N'Black street 77', N'This is the best restaurant ever!')
GO
INSERT [dbo].[tblRestaurant] ([RestaurantID], [OwnerMemberID], [CreationDate], [Name], [Address], [Description]) VALUES (4, 14, CAST(N'2021-10-02T22:15:03.740' AS DateTime), N'Restaurant Gold', N'Street 99', N'There is description')
GO
INSERT [dbo].[tblRestaurant] ([RestaurantID], [OwnerMemberID], [CreationDate], [Name], [Address], [Description]) VALUES (5, 14, CAST(N'2021-10-02T22:15:06.167' AS DateTime), N'Restaurant Blue5', N'Nonamestreeet', N'Great staff')
GO
INSERT [dbo].[tblRestaurant] ([RestaurantID], [OwnerMemberID], [CreationDate], [Name], [Address], [Description]) VALUES (7, 14, CAST(N'2021-10-02T23:55:52.997' AS DateTime), N'New Restaurant', N'ulicamone 22', N'Some desc')
GO
INSERT [dbo].[tblRestaurant] ([RestaurantID], [OwnerMemberID], [CreationDate], [Name], [Address], [Description]) VALUES (8, 15, CAST(N'2021-10-03T10:57:24.477' AS DateTime), N'Great Old Oak', N'The Oak Road 8798', N'No oak is served here!')
GO
INSERT [dbo].[tblRestaurant] ([RestaurantID], [OwnerMemberID], [CreationDate], [Name], [Address], [Description]) VALUES (9, 15, CAST(N'2021-10-03T10:57:53.463' AS DateTime), N'Yellow Banana', N'Snake road 1234', N'Some bananas are served here!')
GO
SET IDENTITY_INSERT [dbo].[tblRestaurant] OFF
GO
SET IDENTITY_INSERT [dbo].[tblReview] ON 
GO
INSERT [dbo].[tblReview] ([ReviewID], [RestaurantID], [ReviewerMemberID], [CreationDate], [RatingStars], [Comment]) VALUES (5, 2, 14, CAST(N'2021-10-03T15:07:15.557' AS DateTime), CAST(4.00 AS Decimal(8, 2)), N'Very nice!')
GO
INSERT [dbo].[tblReview] ([ReviewID], [RestaurantID], [ReviewerMemberID], [CreationDate], [RatingStars], [Comment]) VALUES (12, 2, 17, CAST(N'2021-10-03T15:49:37.260' AS DateTime), CAST(5.00 AS Decimal(8, 2)), N'Great')
GO
INSERT [dbo].[tblReview] ([ReviewID], [RestaurantID], [ReviewerMemberID], [CreationDate], [RatingStars], [Comment]) VALUES (13, 2, 17, CAST(N'2021-10-03T15:49:57.667' AS DateTime), CAST(5.00 AS Decimal(8, 2)), N'Nice')
GO
INSERT [dbo].[tblReview] ([ReviewID], [RestaurantID], [ReviewerMemberID], [CreationDate], [RatingStars], [Comment]) VALUES (19, 2, 17, CAST(N'2021-10-03T20:34:24.680' AS DateTime), CAST(1.00 AS Decimal(8, 2)), N'Very bad!')
GO
INSERT [dbo].[tblReview] ([ReviewID], [RestaurantID], [ReviewerMemberID], [CreationDate], [RatingStars], [Comment]) VALUES (35, 8, 17, CAST(N'2021-10-03T22:26:50.950' AS DateTime), CAST(4.00 AS Decimal(8, 2)), N'Nice food!')
GO
INSERT [dbo].[tblReview] ([ReviewID], [RestaurantID], [ReviewerMemberID], [CreationDate], [RatingStars], [Comment]) VALUES (36, 4, 17, CAST(N'2021-10-03T22:29:55.603' AS DateTime), CAST(4.00 AS Decimal(8, 2)), N'Nice place!')
GO
SET IDENTITY_INSERT [dbo].[tblReview] OFF
GO
INSERT [dbo].[tblReviewReply] ([ReviewID], [ReplyedMemberID], [CreationDate], [ReplyText]) VALUES (5, 14, CAST(N'2021-10-03T19:58:38.623' AS DateTime), N'Thank you so much!')
GO
INSERT [dbo].[tblReviewReply] ([ReviewID], [ReplyedMemberID], [CreationDate], [ReplyText]) VALUES (12, 14, CAST(N'2021-10-03T20:03:48.573' AS DateTime), N'Very nice!')
GO
INSERT [dbo].[tblReviewReply] ([ReviewID], [ReplyedMemberID], [CreationDate], [ReplyText]) VALUES (13, 14, CAST(N'2021-10-03T20:08:32.947' AS DateTime), N'Come back again!')
GO
INSERT [dbo].[tblReviewReply] ([ReviewID], [ReplyedMemberID], [CreationDate], [ReplyText]) VALUES (19, 14, CAST(N'2021-10-03T21:03:34.403' AS DateTime), N'Sorry!')
GO
SET IDENTITY_INSERT [dbo].[tblRole] ON 
GO
INSERT [dbo].[tblRole] ([RoleID], [RoleName], [CanReadReviews], [CanWriteReview], [CanCreateRestaurant], [CanEditRestaurant], [CanReplayToRestaurantReview], [CanEditMember], [CanEditRestaurantReview], [CanEditReplayToRestaurantReview], [CanEditRole], [CanListOwnRestaurants], [CanListAllRestaurants]) VALUES (1, N'User', 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1)
GO
INSERT [dbo].[tblRole] ([RoleID], [RoleName], [CanReadReviews], [CanWriteReview], [CanCreateRestaurant], [CanEditRestaurant], [CanReplayToRestaurantReview], [CanEditMember], [CanEditRestaurantReview], [CanEditReplayToRestaurantReview], [CanEditRole], [CanListOwnRestaurants], [CanListAllRestaurants]) VALUES (2, N'Owner', 1, 0, 1, 1, 1, 0, 0, 0, 0, 1, 0)
GO
INSERT [dbo].[tblRole] ([RoleID], [RoleName], [CanReadReviews], [CanWriteReview], [CanCreateRestaurant], [CanEditRestaurant], [CanReplayToRestaurantReview], [CanEditMember], [CanEditRestaurantReview], [CanEditReplayToRestaurantReview], [CanEditRole], [CanListOwnRestaurants], [CanListAllRestaurants]) VALUES (3, N'Admin', 1, 0, 0, 1, 0, 1, 1, 1, 1, 0, 1)
GO
SET IDENTITY_INSERT [dbo].[tblRole] OFF
GO
ALTER TABLE [dbo].[tblMember] ADD  CONSTRAINT [DF_tblMember_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[tblRestaurant] ADD  CONSTRAINT [DF_tblRestaurant_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[tblReview] ADD  CONSTRAINT [DF_tblReview_CreationDate_1]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[tblReviewReply] ADD  CONSTRAINT [DF_tblReviewReply_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[tblRole] ADD  CONSTRAINT [DF_tblRole_CanReadReviews]  DEFAULT ((0)) FOR [CanReadReviews]
GO
ALTER TABLE [dbo].[tblRole] ADD  CONSTRAINT [DF_tblRole_CanEditRole]  DEFAULT ((0)) FOR [CanEditRole]
GO
ALTER TABLE [dbo].[tblRole] ADD  CONSTRAINT [DF_tblRole_CanListOwnRestaurants]  DEFAULT ((0)) FOR [CanListOwnRestaurants]
GO
ALTER TABLE [dbo].[tblRole] ADD  CONSTRAINT [DF_tblRole_CanListAllRestaurants]  DEFAULT ((0)) FOR [CanListAllRestaurants]
GO
ALTER TABLE [dbo].[tblMemberRole]  WITH CHECK ADD  CONSTRAINT [FK_tblMemberRole_tblMember] FOREIGN KEY([MemberID])
REFERENCES [dbo].[tblMember] ([MemberID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblMemberRole] CHECK CONSTRAINT [FK_tblMemberRole_tblMember]
GO
ALTER TABLE [dbo].[tblMemberRole]  WITH CHECK ADD  CONSTRAINT [FK_tblMemberRole_tblRole] FOREIGN KEY([RoleID])
REFERENCES [dbo].[tblRole] ([RoleID])
GO
ALTER TABLE [dbo].[tblMemberRole] CHECK CONSTRAINT [FK_tblMemberRole_tblRole]
GO
ALTER TABLE [dbo].[tblRestaurant]  WITH CHECK ADD  CONSTRAINT [FK_tblRestaurant_tblMember] FOREIGN KEY([OwnerMemberID])
REFERENCES [dbo].[tblMember] ([MemberID])
GO
ALTER TABLE [dbo].[tblRestaurant] CHECK CONSTRAINT [FK_tblRestaurant_tblMember]
GO
ALTER TABLE [dbo].[tblReview]  WITH CHECK ADD  CONSTRAINT [FK_tblReview_tblMember] FOREIGN KEY([ReviewerMemberID])
REFERENCES [dbo].[tblMember] ([MemberID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblReview] CHECK CONSTRAINT [FK_tblReview_tblMember]
GO
ALTER TABLE [dbo].[tblReview]  WITH CHECK ADD  CONSTRAINT [FK_tblReview_tblRestaurant] FOREIGN KEY([RestaurantID])
REFERENCES [dbo].[tblRestaurant] ([RestaurantID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblReview] CHECK CONSTRAINT [FK_tblReview_tblRestaurant]
GO
ALTER TABLE [dbo].[tblReviewReply]  WITH CHECK ADD  CONSTRAINT [FK_tblReviewReply_tblMember] FOREIGN KEY([ReplyedMemberID])
REFERENCES [dbo].[tblMember] ([MemberID])
GO
ALTER TABLE [dbo].[tblReviewReply] CHECK CONSTRAINT [FK_tblReviewReply_tblMember]
GO
ALTER TABLE [dbo].[tblReviewReply]  WITH CHECK ADD  CONSTRAINT [FK_tblReviewReply_tblReview] FOREIGN KEY([ReviewID])
REFERENCES [dbo].[tblReview] ([ReviewID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblReviewReply] CHECK CONSTRAINT [FK_tblReviewReply_tblReview]
GO
