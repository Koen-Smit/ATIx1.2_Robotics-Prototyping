CREATE TABLE [dbo].[User](
	[Name] [nvarchar](50) NOT NULL,
	[Age] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
)

CREATE TABLE [dbo].[Battery](
	[Percentage] [int] NOT NULL,
)