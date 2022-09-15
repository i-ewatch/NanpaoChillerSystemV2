CREATE TABLE [dbo].[GatewaySetting]
(
	[GatewayIndex] INT NOT NULL PRIMARY KEY DEFAULT 0, 
    [GatewayType] INT NOT NULL DEFAULT 0, 
    [Connect] NVARCHAR(20) NOT NULL DEFAULT 'COM1,9600', 
    [GatewayName] NVARCHAR(50) NOT NULL DEFAULT ''
)