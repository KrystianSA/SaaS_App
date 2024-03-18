CREATE TABLE [dbo].[Users]
(
	[Id] INT IDENTITY (1, 1) NOT NULL, 
    [Email] NCHAR(100) NOT NULL, 
    [HashedPassword] NCHAR(200) NOT NULL, 
    [RegisterDate] DATETIMEOFFSET NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO

CREATE unique INDEX [UQ_Users_Email] ON [dbo].[Users] (Email)
