CREATE TABLE [dbo].[LineNotifySetting]
(
	[LineIndex] INT NOT NULL  PRIMARY KEY DEFAULT 0, 
    [SendFlag] BIT NOT NULL DEFAULT 0 ,
    [Token] NVARCHAR(50) NOT NULL DEFAULT '' 
)
