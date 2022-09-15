CREATE TABLE [dbo].[DeviceSetting]
(
    [GatewayIndex] INT NOT NULL DEFAULT 0,
	[DeviceIndex] INT NOT NULL  DEFAULT 0, 
    [DeviceType] INT NOT NULL DEFAULT 0, 
    [DeviceID] INT NOT NULL DEFAULT 1, 
    [DeviceName] NVARCHAR(50) NOT NULL DEFAULT '', 
    [TempMax] DECIMAL(18, 1) NOT NULL DEFAULT 0, 
    [TempMin] DECIMAL(18, 1) NOT NULL DEFAULT 0, 
    [TempMax1] DECIMAL(18, 1) NOT NULL DEFAULT 0, 
    [TempMin1] DECIMAL(18, 1) NOT NULL DEFAULT 0, 
    [LineIndex] NVARCHAR(50) NOT NULL DEFAULT '', 
    [ElectricIndex] NVARCHAR(50) NULL, 
    [ChillerIndex] NVARCHAR(50) NULL, 
    [CardNo] NVARCHAR(6) NULL, 
    [BoardNo] NVARCHAR(2) NULL, 
    PRIMARY KEY ([GatewayIndex], [DeviceIndex]), 
)
