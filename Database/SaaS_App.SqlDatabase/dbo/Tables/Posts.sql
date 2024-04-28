CREATE TABLE [dbo].[Posts]
(
	[Id] int IDENTITY(1,1) NOT NULL,
	[Title] VARCHAR(MAX) NOT NULL,
	[Url] VARCHAR(MAX) NOT NULL,
	[UrlFeedId] int NOT NULL
	CONSTRAINT [PK_Post] PRIMARY KEY ([Id] ASC), 
    CONSTRAINT [FK_Post_UrlFeed] FOREIGN KEY ([UrlFeedId]) REFERENCES [dbo].[UrlFeeds]([Id])
)
