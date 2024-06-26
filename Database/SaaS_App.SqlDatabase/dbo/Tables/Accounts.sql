﻿CREATE TABLE [dbo].[Accounts]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
    [Name] NCHAR(100) NOT NULL, 
    [CreateDate] DATETIMEOFFSET NOT NULL, 
    [IsActive] Bit NOT NULL, 
    CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED ([Id] ASC) 
)
