CREATE TABLE [dbo].[tblMemberRole] (
    [MemberID] INT NOT NULL,
    [RoleID]   INT NOT NULL,
    CONSTRAINT [PK_tblMemberRole] PRIMARY KEY CLUSTERED ([MemberID] ASC, [RoleID] ASC),
    CONSTRAINT [FK_tblMemberRole_tblMember] FOREIGN KEY ([MemberID]) REFERENCES [dbo].[tblMember] ([MemberID]) ON DELETE CASCADE,
    CONSTRAINT [FK_tblMemberRole_tblRole] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[tblRole] ([RoleID])
);

