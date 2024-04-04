CREATE TABLE [dbo].[Tokens]
(
	[Id] int IDENTITY(1,1) NOT NULL,
	[RecipientId] int NOT NULL,
	[HashedToken] VARCHAR(128) NOT NULL unique,
	[Token_Expiry] DateTime not null,
	CONSTRAINT [PK_UserId_Token] PRIMARY KEY ([Id],[HashedToken] ASC), 
    CONSTRAINT [FK_Tokens_AccountUser] FOREIGN KEY ([RecipientId]) REFERENCES [dbo].[AccountUser]([Id])
)
