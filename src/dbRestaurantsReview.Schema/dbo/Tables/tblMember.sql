CREATE TABLE [dbo].[tblMember] (
    [MemberID]      INT            IDENTITY (1, 1) NOT NULL,
    [CreationDate]  DATETIME       CONSTRAINT [DF_tblMember_CreationDate] DEFAULT (getdate()) NOT NULL,
    [FirstName]     NVARCHAR (50)  NULL,
    [LastName]      NVARCHAR (50)  NULL,
    [Email]         NVARCHAR (150) NOT NULL,
    [Username]      NVARCHAR (100) NOT NULL,
    [Password]      NVARCHAR (100) NOT NULL,
    [LastLoginDate] DATETIME       NULL,
    [LoginsCount]   INT            NOT NULL,
    CONSTRAINT [PK_tblMember] PRIMARY KEY CLUSTERED ([MemberID] ASC)
);

