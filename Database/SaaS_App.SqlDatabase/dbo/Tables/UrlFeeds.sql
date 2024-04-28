CREATE TABLE [dbo].[UrlFeeds]
(
	[Id] int IDENTITY(1,1) NOT NULL,
	[Url] VARCHAR(MAX) NOT NULL,
	[AccountId] int NOT NULL,
	CONSTRAINT [PK_UrlFeed] PRIMARY KEY ([Id] ASC), 
    CONSTRAINT [FK_UrlFeed_Account] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Accounts]([Id])
)
