CREATE TABLE [dbo].[UserLoginHistory]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] INT NOT NULL,
	[LoginTime] [datetime] NOT NULL,
	CONSTRAINT [PK_UserLoginStory] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_UserLoginHistory_User] FOREIGN KEY([UserId]) REFERENCES [dbo].[User] ([Id])
)
