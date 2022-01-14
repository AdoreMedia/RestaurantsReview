CREATE TABLE [dbo].[tblReviewReply] (
    [ReviewID]        INT             NOT NULL,
    [ReplyedMemberID] INT             NOT NULL,
    [CreationDate]    DATETIME        CONSTRAINT [DF_tblReviewReply_CreationDate] DEFAULT (getdate()) NOT NULL,
    [ReplyText]       NVARCHAR (2000) NULL,
    CONSTRAINT [PK_tblReviewReply_1] PRIMARY KEY CLUSTERED ([ReviewID] ASC),
    CONSTRAINT [FK_tblReviewReply_tblMember] FOREIGN KEY ([ReplyedMemberID]) REFERENCES [dbo].[tblMember] ([MemberID]),
    CONSTRAINT [FK_tblReviewReply_tblReview] FOREIGN KEY ([ReviewID]) REFERENCES [dbo].[tblReview] ([ReviewID]) ON DELETE CASCADE
);





