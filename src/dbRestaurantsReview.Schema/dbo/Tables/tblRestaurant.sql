CREATE TABLE [dbo].[tblRestaurant] (
    [RestaurantID]  INT             IDENTITY (1, 1) NOT NULL,
    [OwnerMemberID] INT             NOT NULL,
    [CreationDate]  DATETIME        CONSTRAINT [DF_tblRestaurant_CreationDate] DEFAULT (getdate()) NOT NULL,
    [Name]          NVARCHAR (250)  NOT NULL,
    [Address]       NVARCHAR (250)  NULL,
    [Description]   NVARCHAR (2000) NULL,
    CONSTRAINT [PK_tblRestaurant] PRIMARY KEY CLUSTERED ([RestaurantID] ASC),
    CONSTRAINT [FK_tblRestaurant_tblMember] FOREIGN KEY ([OwnerMemberID]) REFERENCES [dbo].[tblMember] ([MemberID])
);

