CREATE TABLE [dbo].[Tokens]
(
	[Id] int IDENTITY(1,1) NOT NULL,
	[UserId] varchar(36) NOT NULL,
	[Token] VARCHAR(128) NOT NULL unique,
	[Token_Expiry] DateTime not null,
	CONSTRAINT [PK_UserId_Token] PRIMARY KEY ([UserId],[Token] ASC)
)
