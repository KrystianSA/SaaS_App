CREATE TABLE [dbo].[AccountUser]
(
	[Id] INT identity(1,1) NOT NULL, 
    [UserId] INT NOT NULL, 
    [AccountId] INT NOT NULL, 
    CONSTRAINT [PK_AccountUser] PRIMARY KEY clustered ([Id] ASC), 
    CONSTRAINT [FK_AccountUser_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_AccountUser_Accounts] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Accounts]([Id]) ON DELETE CASCADE
)

GO
CREATE INDEX [IX_AccountUser_AccountId] ON [dbo].[AccountUser] ([AccountId])

GO
CREATE INDEX [IX_AccountUser_UserId] ON [dbo].[AccountUser] ([UserId])