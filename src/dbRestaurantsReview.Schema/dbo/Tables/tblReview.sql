CREATE TABLE [dbo].[tblReview] (
    [ReviewID]         INT             IDENTITY (1, 1) NOT NULL,
    [RestaurantID]     INT             NOT NULL,
    [ReviewerMemberID] INT             NOT NULL,
    [CreationDate]     DATETIME        CONSTRAINT [DF_tblReview_CreationDate_1] DEFAULT (getdate()) NOT NULL,
    [RatingStars]      DECIMAL (8, 2)  NULL,
    [Comment]          NVARCHAR (2000) NULL,
    CONSTRAINT [PK_tblReview] PRIMARY KEY CLUSTERED ([ReviewID] ASC),
    CONSTRAINT [FK_tblReview_tblMember] FOREIGN KEY ([ReviewerMemberID]) REFERENCES [dbo].[tblMember] ([MemberID]) ON DELETE CASCADE,
    CONSTRAINT [FK_tblReview_tblRestaurant] FOREIGN KEY ([RestaurantID]) REFERENCES [dbo].[tblRestaurant] ([RestaurantID]) ON DELETE CASCADE
);

