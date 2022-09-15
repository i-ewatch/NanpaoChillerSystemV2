using Dapper;
using NanpaoChillerSystemV2.Configuration;
using NanpaoChillerSystemV2.EF_Modules;
using NanpaoChillerSystemV2.Protocols;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;

namespace NanpaoChillerSystemV2.Methods
{
    public class SqlMethod
    {
        /// <summary>
        /// 冰機資料庫連接物件
        /// </summary>
        protected SqlConnectionStringBuilder Chillerscsb { get; set; } = null;
        public SqlMethod(SystemSetting systemSetting)
        {
            Chillerscsb = new SqlConnectionStringBuilder
            {
                DataSource = systemSetting.ServerIp,
                InitialCatalog = systemSetting.Database,
                UserID = systemSetting.UserId,
                Password = systemSetting.UserPwd
            };
        }
        #region 新增設定資訊
        /// <summary>
        /// 新增通道資訊
        /// </summary>
        /// <param name="gatewaySettings"></param>
        /// <returns></returns>
        public bool Inserter_GatewaySetting(List<GatewaySetting> gatewaySettings)
        {
            try
            {
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string sql = "DELETE FROM GatewaySetting ";
                    conn.Execute(sql);
                    foreach (var item in gatewaySettings)
                    {
                        string Isql = "INSERT INTO GatewaySetting (GatewayIndex,GatewayType,Connect,GatewayName) VALUES (@GatewayIndex,@GatewayType,@Connect,@GatewayName)";
                        conn.Execute(Isql, item);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "新增通道資訊失敗");
                return false;
            }
        }
        /// <summary>
        /// 新增設備資訊
        /// </summary>
        /// <param name="deviceSettings"></param>
        /// <returns></returns>
        public bool Inserter_DeviceSetting(List<DeviceSetting> deviceSettings)
        {
            try
            {
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string sql = "DELETE FROM DeviceSetting";
                    conn.Execute(sql);
                    foreach (var item in deviceSettings)
                    {
                        string Isql = "INSERT INTO DeviceSetting (GatewayIndex,DeviceIndex,DeviceType,DeviceID,DeviceName,TempMax,TempMin,TempMax1,TempMin1,LineIndex,ElectricIndex,ChillerIndex,CardNo,BoardNo) VALUES (@GatewayIndex,@DeviceIndex,@DeviceType,@DeviceID,@DeviceName,@TempMax,@TempMin,@TempMax1,@TempMin1,@LineIndex,@ElectricIndex,@ChillerIndex,@CardNo,@BoardNo)";
                        conn.Execute(Isql, item);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "新增設備資訊失敗");
                return false;
            }
        }
        /// <summary>
        /// 新增LINE推播資訊
        /// </summary>
        /// <param name="gatewaySettings"></param>
        /// <returns></returns>
        public bool Inserter_LineNotifySetting(List<LineNotifySetting> lineNotifySettings)
        {
            try
            {
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string sql = "DELETE FROM LineNotifySetting";
                    conn.Execute(sql);
                    foreach (var item in lineNotifySettings)
                    {
                        string Isql = "INSERT INTO LineNotifySetting (LineIndex,SendFlag,Token) VALUES (@LineIndex,@SendFlag,@Token)";
                        conn.Execute(Isql, item);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "新增LINE推播資訊失敗");
                return false;
            }
        }
        #endregion
        #region 查詢資訊
        public List<GatewaySetting> Search_GatewaySetting()
        {
            List<GatewaySetting> settings = null;
            try
            {
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string sql = "Select * FROM GatewaySetting";
                    settings = conn.Query<GatewaySetting>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "查詢通訊通道資訊錯誤");
            }
            return settings;
        }
        /// <summary>
        /// 查詢設備資訊
        /// </summary>
        /// <returns></returns>
        public List<DeviceSetting> Search_DeviceSetting()
        {
            List<DeviceSetting> settings = null;
            try
            {
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string sql = "Select * FROM DeviceSetting";
                    settings = conn.Query<DeviceSetting>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "查詢設備資訊錯誤");
            }
            return settings;
        }

        /// <summary>
        /// 查詢Line推播資訊
        /// </summary>
        /// <returns></returns>
        public List<LineNotifySetting> Search_LineNotifySetting()
        {
            List<LineNotifySetting> settings = null;
            try
            {
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string sql = "Select * FROM LineNotifySetting";
                    settings = conn.Query<LineNotifySetting>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "查詢Line推播資訊錯誤");
            }
            return settings;
        }

        #endregion
        #region 建立資料庫與資料表
        /// <summary>
        /// 建立資料庫與資料表
        /// </summary>
        public void CreateDataBase()
        {
            try
            {
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string databaseSql = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name='NanpaoLog_{DateTime.Now.Year}')  CREATE DATABASE [NanpaoLog_{DateTime.Now.Year}]";
                    string electricforwebSql = $"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ElectricLog') CREATE TABLE [ElectricLog] ( " +
                     $" [ttime] NVARCHAR(14) NOT NULL" +
                     $",[ttimen] DATETIME NOT NULL" +
                     $",[GatewayIndex]INT NOT NULL" +
                     $",[DeviceIndex] INT NOT NULL" +
                     $",[trv] DECIMAL(18, 2) NOT NULL DEFAULT 0" +
                     $",[tsv] DECIMAL(18, 2) NOT NULL DEFAULT 0" +
                     $",[ttv] DECIMAL(18, 2) NOT NULL DEFAULT 0" +
                     $",[tri] DECIMAL(18, 2) NOT NULL DEFAULT 0" +
                     $",[tsi] DECIMAL(18, 2) NOT NULL DEFAULT 0" +
                     $",[tti] DECIMAL(18, 2) NOT NULL DEFAULT 0" +
                     $",[tpre] DECIMAL(18, 2) NOT NULL DEFAULT 0" +
                     $",[tkw] DECIMAL(18, 2) NOT NULL DEFAULT 0" +
                     $",[tkwh] DECIMAL(18, 2) NOT NULL DEFAULT 0 " +
                     $",CONSTRAINT [PK_ElectricLog] PRIMARY KEY ([ttime], [GatewayIndex],[DeviceIndex]))";
                    string electrictableSql = $"USE [NanpaoLog_{DateTime.Now.Year}] IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ElectricLog') CREATE TABLE [ElectricLog] ( " +
                       $" [ttime] NVARCHAR(14) NOT NULL" +
                       $",[ttimen] DATETIME NOT NULL" +
                       $",[GatewayIndex]INT NOT NULL" +
                       $",[DeviceIndex] INT NOT NULL" +
                       $",[trv] DECIMAL(18, 2) NOT NULL DEFAULT 0" +
                       $",[tsv] DECIMAL(18, 2) NOT NULL DEFAULT 0" +
                       $",[ttv] DECIMAL(18, 2) NOT NULL DEFAULT 0" +
                       $",[tri] DECIMAL(18, 2) NOT NULL DEFAULT 0" +
                       $",[tsi] DECIMAL(18, 2) NOT NULL DEFAULT 0" +
                       $",[tti] DECIMAL(18, 2) NOT NULL DEFAULT 0" +
                       $",[tpre] DECIMAL(18, 2) NOT NULL DEFAULT 0" +
                       $",[tkw] DECIMAL(18, 2) NOT NULL DEFAULT 0" +
                       $",[tkwh] DECIMAL(18, 2) NOT NULL DEFAULT 0 " +
                       $",CONSTRAINT [PK_ElectricLog] PRIMARY KEY ([ttime], [GatewayIndex],[DeviceIndex]))";
                    string kwhTotaltableSql = $"USE [NanpaoLog_{DateTime.Now.Year}] IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ElectricTotal') CREATE TABLE [ElectricTotal](" +
                       "[ttime] CHAR (10)  NOT NULL," +
                       "[ttimen]       DATETIME NOT NULL," +
                       "[GatewayIndex]INT NOT NULL," +
                       "[DeviceIndex] INT NOT NULL," +
                       "[KwhStart1]    DECIMAL(18, 2) DEFAULT((0)) NOT NULL," +
                       "[KwhEnd1]      DECIMAL(18, 2) DEFAULT((0)) NOT NULL," +
                       "[KwhStart2]    DECIMAL(18, 2) DEFAULT((0)) NOT NULL," +
                       "[KwhEnd2]      DECIMAL(18, 2) DEFAULT((0)) NOT NULL," +
                       "[KwhTotal]     DECIMAL(18, 2) DEFAULT((0)) NOT NULL" +
                       ",CONSTRAINT [PK_ElectricTotal] PRIMARY KEY ([ttime],  [GatewayIndex],[DeviceIndex]))";
                    string flowforwebSql = $"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='FlowLog') CREATE TABLE [FlowLog] ( " +
                         $" [ttime] NVARCHAR(14) NOT NULL" +
                         $",[ttimen] DATETIME NOT NULL" +
                         $",[GatewayIndex]INT NOT NULL" +
                         $",[DeviceIndex] INT NOT NULL" +
                         $",[Flow] DECIMAL(18,2) NOT NULL DEFAULT 0" +
                         $",[FlowTotal] DECIMAL(18,2) NOT NULL DEFAULT 0" +
                         $",[InputTemp] DECIMAL(18,2) NOT NULL DEFAULT 0" +
                         $",[OutputTemp] DECIMAL(18,2) NOT NULL DEFAULT 0" +
                         $",[TempDifference] DECIMAL(18,2) NOT NULL DEFAULT 0" +
                         $",[SwitchFlag] BIT NOT NULL DEFAULT 0" +
                         $",CONSTRAINT [PK_FlowLog] PRIMARY KEY ([ttime],  [GatewayIndex],[DeviceIndex]))";
                    string flowtableSql = $"USE [NanpaoLog_{DateTime.Now.Year}] IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='FlowLog') CREATE TABLE [FlowLog] ( " +
                           $" [ttime] NVARCHAR(14) NOT NULL" +
                           $",[ttimen] DATETIME NOT NULL" +
                           $",[GatewayIndex]INT NOT NULL" +
                           $",[DeviceIndex] INT NOT NULL" +
                           $",[Flow] DECIMAL(18,2) NOT NULL DEFAULT 0" +
                           $",[FlowTotal] DECIMAL(18,2) NOT NULL DEFAULT 0" +
                           $",[InputTemp] DECIMAL(18,2) NOT NULL DEFAULT 0" +
                           $",[OutputTemp] DECIMAL(18,2) NOT NULL DEFAULT 0" +
                           $",[TempDifference] DECIMAL(18,2) NOT NULL DEFAULT 0" +
                           $",[SwitchFlag] BIT NOT NULL DEFAULT 0" +
                           $",CONSTRAINT [PK_FlowLog] PRIMARY KEY ([ttime],  [GatewayIndex],[DeviceIndex]))";
                    string FlowTotaltableSql = $"USE [NanpaoLog_{DateTime.Now.Year}] IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Flowtotal') CREATE TABLE [Flowtotal](" +
                          "[ttime] CHAR (10)  NOT NULL," +
                          "[ttimen]       DATETIME NOT NULL," +
                          "[GatewayIndex]INT NOT NULL," +
                          "[DeviceIndex] INT NOT NULL," +
                          "[InputTempAvg] DECIMAL(18, 2) DEFAULT((0)) NOT NULL," +
                          "[OutputTempAvg] DECIMAL(18, 2) DEFAULT((0)) NOT NULL," +
                          "[TempDifferenceAvg] DECIMAL(18, 2) DEFAULT((0)) NOT NULL," +
                          "[FlowAvg] DECIMAL(18, 2) DEFAULT((0)) NOT NULL," +
                          "[RTH] DECIMAL(18, 2) DEFAULT((0)) NOT NULL," +
                          "[FlowStart1]    DECIMAL(18, 2) DEFAULT((0)) NOT NULL," +
                          "[FlowEnd1]      DECIMAL(18, 2) DEFAULT((0)) NOT NULL," +
                          "[FlowStart2]    DECIMAL(18, 2) DEFAULT((0)) NOT NULL," +
                          "[FlowEnd2]      DECIMAL(18, 2) DEFAULT((0)) NOT NULL," +
                          "[FlowTotal]     DECIMAL(18, 2) DEFAULT((0)) NOT NULL" +
                          ",CONSTRAINT [PK_Flowtotal] PRIMARY KEY ([ttime], [GatewayIndex],[DeviceIndex]))";
                    string chillerforwebSql = $"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ChillerLog') CREATE TABLE [dbo].[ChillerLog](" +
                      "[ttime]          CHAR(14)       NOT NULL DEFAULT ''," +
                      "[ttimen]         DATETIME NOT NULL DEFAULT getdate()," +
                      "[GatewayIndex]INT NOT NULL," +
                      "[DeviceIndex]         INT NOT NULL DEFAULT 0," +
                      "[CHW_OUT_TEMP]   DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                      "[CHW_IN_TEMP]    DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                      "[CHW_TEMP_Difference] DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                      "[CW_OUT_TEMP]    DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                      "[CW_IN_TEMP]     DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                      "[CW_TEMP_Difference] DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                      "[CH1_PRESS_HIGH] DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                      "[CH1_PRESS_LOW]  DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                      "[CH2_PRESS_HIGH] DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                      "[CH2_PRESS_LOW]  DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                      "[CH1_RATE]       DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                      "[CH2_RATE]       DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                      "[CH1_STOP_COUNT] DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                      "[CH2_STOP_COUNT] DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                      "[CHP_STOP_COUNT] DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                      "[CH1_RUN_HOUR]   DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                      "[CH2_RUN_HOUR]   DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                      "[CH1_RUN_TIME]   DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                      "[CH2_RUN_TIME]   DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                      "CONSTRAINT[PK_ChillerLog] PRIMARY KEY([ttime],[GatewayIndex],[DeviceIndex]))";
                    string chillertableSql = $"USE [NanpaoLog_{DateTime.Now.Year}] IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ChillerLog') CREATE TABLE [dbo].[ChillerLog](" +
                                              "[ttime]          CHAR(14)       NOT NULL DEFAULT ''," +
                                              "[ttimen]         DATETIME NOT NULL DEFAULT getdate()," +
                                              "[GatewayIndex]INT NOT NULL," +
                                              "[DeviceIndex]         INT NOT NULL DEFAULT 0," +
                                              "[CHW_OUT_TEMP]   DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                                              "[CHW_IN_TEMP]    DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                                              "[CHW_TEMP_Difference] DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                                              "[CW_OUT_TEMP]    DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                                              "[CW_IN_TEMP]     DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                                              "[CW_TEMP_Difference] DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                                              "[CH1_PRESS_HIGH] DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                                              "[CH1_PRESS_LOW]  DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                                              "[CH2_PRESS_HIGH] DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                                              "[CH2_PRESS_LOW]  DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                                              "[CH1_RATE]       DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                                              "[CH2_RATE]       DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                                              "[CH1_STOP_COUNT] DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                                              "[CH2_STOP_COUNT] DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                                              "[CHP_STOP_COUNT] DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                                              "[CH1_RUN_HOUR]   DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                                              "[CH2_RUN_HOUR]   DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                                              "[CH1_RUN_TIME]   DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                                              "[CH2_RUN_TIME]   DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                                              "CONSTRAINT[PK_ChillerLog] PRIMARY KEY([ttime],[GatewayIndex],[DeviceIndex]))";
                    string ChillerstatetableSql = $"USE [NanpaoLog_{DateTime.Now.Year}] IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ChillerStateLog') CREATE TABLE [dbo].[ChillerStateLog] (" +
                                                  "[ttime]   CHAR(17)      NOT NULL DEFAULT ''," +
                                                  "[ttimen]  DATETIME NOT NULL DEFAULT getdate()," +
                                                  "[GatewayIndex]INT NOT NULL," +
                                                  "[DeviceIndex]         INT NOT NULL DEFAULT 0," +
                                                  "[Address] INT NOT NULL DEFAULT 0," +
                                                  "[Message] NVARCHAR(150) CONSTRAINT[DF_StateLog_Message] DEFAULT('') NOT NULL," +
                                                  "CONSTRAINT[PK_ChillerStateLog] PRIMARY KEY CLUSTERED([GatewayIndex],[DeviceIndex], [ttime], [Address]))";
                    string TRforwebLogSql = $"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TRLog') CREATE TABLE [dbo].[TRLog] (" +
                     "[ttime]   CHAR(14)      NOT NULL DEFAULT ''," +
                     "[ttimen]  DATETIME NOT NULL DEFAULT getdate()," +
                     "[GatewayIndex]INT NOT NULL," +
                     "[DeviceIndex]         INT NOT NULL DEFAULT 0," +
                     "[Temp] DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                     "[Temp1] DECIMAL(18, 2) NOT NULL DEFAULT 0" +
                     "CONSTRAINT[PK_TRSLog] PRIMARY KEY CLUSTERED([GatewayIndex],[DeviceIndex], [ttime]))";
                    string TRLogSql = $"USE [NanpaoLog_{DateTime.Now.Year}] IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TRLog') CREATE TABLE [dbo].[TRLog] (" +
                                             "[ttime]   CHAR(14)      NOT NULL DEFAULT ''," +
                                             "[ttimen]  DATETIME NOT NULL DEFAULT getdate()," +
                                             "[GatewayIndex]INT NOT NULL," +
                                             "[DeviceIndex]         INT NOT NULL DEFAULT 0," +
                                             "[Temp] DECIMAL(18, 2) NOT NULL DEFAULT 0," +
                                             "[Temp1] DECIMAL(18, 2) NOT NULL DEFAULT 0" +
                                             "CONSTRAINT[PK_TRSLog] PRIMARY KEY CLUSTERED([GatewayIndex],[DeviceIndex], [ttime]))";
                    string TRStatetableSql = $"USE [NanpaoLog_{DateTime.Now.Year}] IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TRStateLog') CREATE TABLE [dbo].[TRStateLog] (" +
                                             "[ttime]   CHAR(17)      NOT NULL DEFAULT ''," +
                                             "[ttimen]  DATETIME NOT NULL DEFAULT getdate()," +
                                             "[GatewayIndex] INT NOT NULL DEFAULT 0," +
                                             "[DeviceIndex]         INT NOT NULL DEFAULT 0," +
                                             "[Message] NVARCHAR(150) NOT NULL DEFAULT '' " +
                                             "CONSTRAINT[PK_TRStateLog] PRIMARY KEY CLUSTERED([GatewayIndex],[DeviceIndex],[ttime]))";
                    conn.Execute(databaseSql);
                    Thread.Sleep(10);
                    conn.Execute(electricforwebSql);
                    Thread.Sleep(10);
                    conn.Execute(electrictableSql);
                    Thread.Sleep(10);
                    conn.Execute(kwhTotaltableSql);
                    Thread.Sleep(10);
                    conn.Execute(flowforwebSql);
                    Thread.Sleep(10);
                    conn.Execute(flowtableSql);
                    Thread.Sleep(10);
                    conn.Execute(FlowTotaltableSql);
                    Thread.Sleep(10);
                    conn.Execute(chillerforwebSql);
                    Thread.Sleep(10);
                    conn.Execute(chillertableSql);
                    Thread.Sleep(10);
                    conn.Execute(ChillerstatetableSql);
                    Thread.Sleep(10);
                    conn.Execute(TRforwebLogSql);
                    Thread.Sleep(10);
                    conn.Execute(TRLogSql);
                    Thread.Sleep(10);
                    conn.Execute(TRStatetableSql);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "資料庫建立失敗");
            }
        }
        #endregion

        #region 更新溫度設定值
        public bool UpdateDeviceTemp(DeviceSetting deviceSetting, decimal value, int Max_Min)
        {
            try
            {
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string sql = "";
                    switch (Max_Min)
                    {
                        case 0:
                            {
                                sql = "UPDATE DeviceSetting SET TempMax = @value WHERE GatewayIndex = @GatewayIndex AND DeviceIndex = @DeviceIndex";
                            }
                            break;
                        case 1:
                            {
                                sql = "UPDATE DeviceSetting SET TempMin = @value WHERE GatewayIndex = @GatewayIndex AND DeviceIndex = @DeviceIndex";
                            }
                            break;
                        case 2:
                            {
                                sql = "UPDATE DeviceSetting SET TempMax1 = @value WHERE GatewayIndex = @GatewayIndex AND DeviceIndex = @DeviceIndex";
                            }
                            break;
                        case 3:
                            {
                                sql = "UPDATE DeviceSetting SET TempMin1 = @value WHERE GatewayIndex = @GatewayIndex AND DeviceIndex = @DeviceIndex";
                            }
                            break;
                    }
                    conn.Execute(sql, new { value = value, GatewayIndex = deviceSetting.GatewayIndex, DeviceIndex = deviceSetting.DeviceIndex });
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"更新溫度設定值失敗 {deviceSetting.DeviceName}");
                return false;
            }
        }
        #endregion
        #region 更新變壓器警報
        public void InsertTRState(DeviceSetting setting, string Message)
        {
            try
            {
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string NowTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    var exsit = conn.ExecuteScalar<bool>($"USE [NanpaoLog_{DateTime.Now.Year}] SELECT TOP 1 1 FROM TRStateLog WHERE ttime = @NowTime AND DeviceIndex = @DeviceIndex AND GatewayIndex = @GatewayIndex", new { NowTime, DeviceIndex = setting.DeviceIndex, GatewayIndex = setting.GatewayIndex });
                    if (!exsit)
                    {
                        string sql = $"USE [NanpaoLog_{DateTime.Now.Year}] INSERT INTO TRStateLog (ttime, ttimen,GatewayIndex, DeviceIndex, Message) VALUES (@ttime, @ttimen,@GatewayIndex ,@DeviceIndex, @Message)";
                        TRStateLog tRStateLog = new TRStateLog()
                        {
                            ttime = NowTime,
                            ttimen = DateTime.Now,
                            GatewayIndex = setting.GatewayIndex,
                            DeviceIndex = setting.DeviceIndex,
                            Message = Message
                        };
                        conn.Execute(sql, tRStateLog);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "更新變壓器警報失敗");
            }
        }
        #endregion
        #region 更新變壓器溫度LOG
        /// <summary>
        /// 更新變壓器溫度LOG
        /// </summary>
        /// <param name="senserData"></param>
        public void InserterTRLog(SenserData senserData)
        {
            try
            {
                DateTime NowTimen = Convert.ToDateTime($"{DateTime.Now:yyyy/MM/dd HH:mm}:00");
                string NowTime = $"{DateTime.Now:yyyyMMddHHmm}00";
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string sql = "";
                    var exsitforweb = conn.ExecuteScalar<bool>($"SELECT TOP 1 1 FROM TRLog WHERE  GatewayIndex=@GatewayIndex AND DeviceIndex = @DeviceIndex", new { GatewayIndex = senserData.GatewayIndex, DeviceIndex = senserData.DeviceIndex });
                    if (exsitforweb)
                    {
                        sql = $"UPDATE [TRLog] SET ttime=@ttime,ttimen=@ttimen,Temp=@Temp,Temp1=@Temp1  WHERE  GatewayIndex=@GatewayIndex AND DeviceIndex=@DeviceIndex ";
                    }
                    else
                    {
                        sql = $"INSERT INTO [dbo].[TRLog] ([ttime], [ttimen], [GatewayIndex] , [DeviceIndex],[Temp] ,[Temp1])" +
                          $" VALUES(@ttime, @ttimen, @GatewayIndex, @DeviceIndex, @Temp, @Temp1)";
                    }
                    TRLog tRLog = new TRLog
                    {
                        ttime = NowTime,
                        ttimen = NowTimen,
                        GatewayIndex = senserData.GatewayIndex,
                        DeviceIndex = senserData.DeviceIndex,
                        Temp = senserData.Temp,
                        Temp1 = senserData.Temp1
                    };
                    conn.Execute(sql, tRLog);
                    var exsit = conn.ExecuteScalar<bool>($"USE [NanpaoLog_{DateTime.Now.Year}] SELECT TOP 1 1 FROM TRLog WHERE ttime = @NowTime AND GatewayIndex=@GatewayIndex AND DeviceIndex = @DeviceIndex", new { NowTime, GatewayIndex = senserData.GatewayIndex, DeviceIndex = senserData.DeviceIndex });
                    if (!exsit)
                    {
                        sql = $"USE [NanpaoLog_{DateTime.Now.Year}] INSERT INTO [dbo].[TRLog] ([ttime], [ttimen], [GatewayIndex] , [DeviceIndex],[Temp] ,[Temp1])" +
                           $" VALUES(@ttime, @ttimen, @GatewayIndex, @DeviceIndex, @Temp, @Temp1)";

                        conn.Execute(sql, tRLog);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "TRLog錯誤");
            }
        }
        #endregion

        #region 更新冰機異常狀態
        public void InserterChillerState(DeviceSetting deviceSetting, ushort Address, string Message)
        {
            using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
            {
                string NowTime = $"{DateTime.Now:yyyyMMddHHmmssfff}";
                var exsit = conn.ExecuteScalar<bool>($"USE [NanpaoLog_{DateTime.Now.Year}] SELECT TOP 1 1 FROM ChillerStateLog WHERE ttime = @NowTime AND DeviceIndex = @DeviceIndex AND GatewayIndex=@GatewayIndex", new { NowTime, DeviceIndex = deviceSetting.DeviceIndex, GatewayIndex = deviceSetting.GatewayIndex });
                if (!exsit)
                {
                    string sql = $"USE [NanpaoLog_{DateTime.Now.Year}] INSERT INTO ChillerStateLog (ttime, ttimen,GatewayIndex, DeviceIndex, Address, Message) VALUES (@ttime, @ttimen,@GatewayIndex, @DeviceIndex, @Address, @Message)";
                    ChillerStateLog stateLog = new ChillerStateLog()
                    {
                        ttime = NowTime,
                        ttimen = DateTime.Now,
                        DeviceIndex = deviceSetting.DeviceIndex,
                        Address = Address,
                        Message = Message
                    };
                    conn.Execute(sql, stateLog);
                }
            }
        }
        #endregion
        #region 更新冰機LOG
        public void InserterChillerLog(ChillerData chillerData)
        {
            try
            {
                DateTime NowTimen = Convert.ToDateTime($"{DateTime.Now:yyyy/MM/dd HH:mm}:00");
                string NowTime = $"{DateTime.Now:yyyyMMddHHmm}00";
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string sql = "";
                    var exsitforweb = conn.ExecuteScalar<bool>($"SELECT TOP 1 1 FROM ChillerLog WHERE  GatewayIndex=@GatewayIndex AND DeviceIndex = @DeviceIndex", new { GatewayIndex = chillerData.GatewayIndex, DeviceIndex = chillerData.DeviceIndex });
                    if (exsitforweb)
                    {
                        sql = $"UPDATE [dbo].[ChillerLog] SET [ttime]=@ttime, [ttimen]=@ttimen,[CHW_OUT_TEMP]=@CHW_OUT_TEMP ,[CHW_IN_TEMP]=@CHW_IN_TEMP,[CHW_TEMP_Difference]=@CHW_TEMP_Difference ,[CW_OUT_TEMP]=@CW_OUT_TEMP ,[CW_IN_TEMP]=@CW_IN_TEMP,[CW_TEMP_Difference]=@CW_TEMP_Difference ,[CH1_PRESS_HIGH]=@CH1_PRESS_HIGH," +
                                   "[CH1_PRESS_LOW]=@CH1_PRESS_LOW ,[CH2_PRESS_HIGH]=@CH2_PRESS_HIGH ,[CH2_PRESS_LOW]=@CH2_PRESS_LOW ,[CH1_RATE]=@CH1_RATE ,[CH2_RATE]=@CH2_RATE ,[CH1_STOP_COUNT] =@CH1_STOP_COUNT,[CH2_STOP_COUNT]=@CH2_STOP_COUNT ,[CHP_STOP_COUNT]=@CHP_STOP_COUNT ,[CH1_RUN_HOUR]=@CH1_RUN_HOUR," +
                                   "[CH2_RUN_HOUR]=@CH2_RUN_HOUR ,[CH1_RUN_TIME]=@CH1_RUN_TIME ,[CH2_RUN_TIME]=@CH2_RUN_TIME WHERE [GatewayIndex]=@GatewayIndex AND [DeviceIndex]=@DeviceIndex";
                    }
                    else
                    {
                        sql = $"INSERT INTO [dbo].[ChillerLog] ([ttime], [ttimen], [GatewayIndex] , [DeviceIndex],[CHW_OUT_TEMP] ,[CHW_IN_TEMP],[CHW_TEMP_Difference] ,[CW_OUT_TEMP] ,[CW_IN_TEMP],[CW_TEMP_Difference] ,[CH1_PRESS_HIGH]," +
                                    "[CH1_PRESS_LOW] ,[CH2_PRESS_HIGH] ,[CH2_PRESS_LOW] ,[CH1_RATE] ,[CH2_RATE] ,[CH1_STOP_COUNT] ,[CH2_STOP_COUNT] ,[CHP_STOP_COUNT] ,[CH1_RUN_HOUR]," +
                                    "[CH2_RUN_HOUR] ,[CH1_RUN_TIME] ,[CH2_RUN_TIME])" +
                                    "VALUES (@ttime, @ttimen,@GatewayIndex, @DeviceIndex, @CHW_OUT_TEMP, @CHW_IN_TEMP,@CHW_TEMP_Difference, @CW_OUT_TEMP, @CW_IN_TEMP,@CW_TEMP_Difference, @CH1_PRESS_HIGH," +
                                    "@CH1_PRESS_LOW, @CH2_PRESS_HIGH, @CH2_PRESS_LOW, @CH1_RATE, @CH2_RATE, @CH1_STOP_COUNT, @CH2_STOP_COUNT, @CHP_STOP_COUNT, @CH1_RUN_HOUR," +
                                    "@CH2_RUN_HOUR, @CH1_RUN_TIME, @CH2_RUN_TIME)";
                    }
                    ChillerLog chillerLog = new ChillerLog()
                    {
                        ttime = NowTime,
                        ttimen = NowTimen,
                        GatewayIndex = chillerData.GatewayIndex,
                        DeviceIndex = chillerData.DeviceIndex,
                        CHW_OUT_TEMP = chillerData.CHW_OUT_TEMP,
                        CHW_IN_TEMP = chillerData.CHW_IN_TEMP,
                        CHW_TEMP_Difference = chillerData.CHW_TEMP_Difference,
                        CW_OUT_TEMP = chillerData.CW_OUT_TEMP,
                        CW_IN_TEMP = chillerData.CW_IN_TEMP,
                        CW_TEMP_Difference = chillerData.CW_TEMP_Difference,
                        CH1_PRESS_HIGH = chillerData.CH1_PRESS_HIGH,
                        CH1_PRESS_LOW = chillerData.CH1_PRESS_LOW,
                        CH2_PRESS_HIGH = chillerData.CH2_PRESS_HIGH,
                        CH2_PRESS_LOW = chillerData.CH2_PRESS_LOW,
                        CH1_RATE = chillerData.CH1_RATE,
                        CH2_RATE = chillerData.CH2_RATE,
                        CH1_STOP_COUNT = chillerData.CH1_STOP_COUNT,
                        CH2_STOP_COUNT = chillerData.CH2_STOP_COUNT,
                        CHP_STOP_COUNT = chillerData.CHP_STOP_COUNT,
                        CH1_RUN_HOUR = chillerData.CH1_RUN_HOUR,
                        CH2_RUN_HOUR = chillerData.CH2_RUN_HOUR,
                        CH1_RUN_TIME = chillerData.CH1_RUN_TIME,
                        CH2_RUN_TIME = chillerData.CH2_RUN_TIME
                    };
                    conn.Execute(sql, chillerLog);
                    var exsit = conn.ExecuteScalar<bool>($"USE [NanpaoLog_{DateTime.Now.Year}] SELECT TOP 1 1 FROM ChillerLog WHERE ttime = @NowTime AND GatewayIndex=@GatewayIndex AND DeviceIndex = @DeviceIndex", new { NowTime, GatewayIndex = chillerData.GatewayIndex, DeviceIndex = chillerData.DeviceIndex });
                    if (!exsit)
                    {
                        sql = $"USE [NanpaoLog_{DateTime.Now.Year}] INSERT INTO [dbo].[ChillerLog] ([ttime], [ttimen], [GatewayIndex] , [DeviceIndex],[CHW_OUT_TEMP] ,[CHW_IN_TEMP],[CHW_TEMP_Difference] ,[CW_OUT_TEMP] ,[CW_IN_TEMP],[CW_TEMP_Difference] ,[CH1_PRESS_HIGH]," +
                                    "[CH1_PRESS_LOW] ,[CH2_PRESS_HIGH] ,[CH2_PRESS_LOW] ,[CH1_RATE] ,[CH2_RATE] ,[CH1_STOP_COUNT] ,[CH2_STOP_COUNT] ,[CHP_STOP_COUNT] ,[CH1_RUN_HOUR]," +
                                    "[CH2_RUN_HOUR] ,[CH1_RUN_TIME] ,[CH2_RUN_TIME])" +
                                    "VALUES (@ttime, @ttimen,@GatewayIndex, @DeviceIndex, @CHW_OUT_TEMP, @CHW_IN_TEMP,@CHW_TEMP_Difference, @CW_OUT_TEMP, @CW_IN_TEMP,@CW_TEMP_Difference, @CH1_PRESS_HIGH," +
                                    "@CH1_PRESS_LOW, @CH2_PRESS_HIGH, @CH2_PRESS_LOW, @CH1_RATE, @CH2_RATE, @CH1_STOP_COUNT, @CH2_STOP_COUNT, @CHP_STOP_COUNT, @CH1_RUN_HOUR," +
                                    "@CH2_RUN_HOUR, @CH1_RUN_TIME, @CH2_RUN_TIME)";
                        conn.Execute(sql, chillerLog);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Chillerlog錯誤");
            }
        }
        #endregion

        #region 更新電表LOG
        public bool InsertElectric(ElectricData data)
        {
            try
            {
                string timestr = DateTime.Now.ToString("yyyyMMddHHmm00");
                DateTime dateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:00"));
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string sql = "";
                    var exsitforweb = conn.ExecuteScalar<bool>($"SELECT TOP 1 1 FROM [ElectricLog] WHERE  DeviceIndex = @DeviceIndex AND GatewayIndex = @GatewayIndex", new { DeviceIndex = data.DeviceSetting.DeviceIndex, GatewayIndex = data.DeviceSetting.GatewayIndex });
                    if (exsitforweb)
                    {
                        sql = $"UPDATE [ElectricLog]  SET ttime=@ttime,ttimen=@ttimen,trv=@trv,tsv=@tsv,ttv=@ttv,tri=@tri,tsi=@tsi,tti=@tti,tpre=@tpre,tkw=@tkw,tkwh=@tkwh WHERE [GatewayIndex]=@GatewayIndex AND [DeviceIndex]=@DeviceIndex";
                    }
                    else
                    {
                        sql = $"INSERT INTO [ElectricLog] (ttime,ttimen,GatewayIndex,DeviceIndex,trv,tsv,ttv,tri,tsi,tti,tpre,tkw,tkwh) VALUES (@ttime,@ttimen,@GatewayIndex,@DeviceIndex,@trv,@tsv,@ttv,@tri,@tsi,@tti,@tpre,@tkw,@tkwh )";
                    }
                    conn.Execute(sql, new { ttime = timestr, ttimen = dateTime, GatewayIndex = data.DeviceSetting.GatewayIndex, DeviceIndex = data.DeviceSetting.DeviceIndex, trv = data.RV, tsv = data.SV, ttv = data.TV, tri = data.RA, tsi = data.SA, tti = data.TA, tpre = data.PFE, tkw = data.KW, tkwh = data.KWH });
                    var exsit = conn.ExecuteScalar<bool>($"USE [NanpaoLog_{DateTime.Now.Year}]  SELECT TOP 1 1 FROM [ElectricLog] WHERE ttime = @timestr AND DeviceIndex = @DeviceIndex AND GatewayIndex = @GatewayIndex", new { timestr, DeviceIndex = data.DeviceSetting.DeviceIndex, GatewayIndex = data.DeviceSetting.GatewayIndex });
                    if (!exsit)
                    {
                        sql = $"USE [NanpaoLog_{DateTime.Now.Year}] INSERT INTO [ElectricLog] (ttime,ttimen,GatewayIndex,DeviceIndex,trv,tsv,ttv,tri,tsi,tti,tpre,tkw,tkwh) VALUES (@ttime,@ttimen,@GatewayIndex,@DeviceIndex,@trv,@tsv,@ttv,@tri,@tsi,@tti,@tpre,@tkw,@tkwh )";
                        conn.Execute(sql, new { ttime = timestr, ttimen = dateTime, GatewayIndex = data.DeviceSetting.GatewayIndex, DeviceIndex = data.DeviceSetting.DeviceIndex, trv = data.RV, tsv = data.SV, ttv = data.TV, tri = data.RA, tsi = data.SA, tti = data.TA, tpre = data.PFE, tkw = data.KW, tkwh = data.KWH });
                    }
                    return exsit;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"更新電表LOG失敗 {data.DeviceSetting.DeviceName}");
                return false;
            }
        }
        #endregion
        #region 更新累積電量
        /// <summary>
        /// 更新累積電量
        /// </summary>
        /// <param name="protocol"></param>
        public void InsertElectricTotal(ElectricData protocol)
        {
            string timestr = DateTime.Now.ToString("yyyyMMddHH0000");
            DateTime dateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:00:00"));
            try
            {
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    ElectricTotal electricTotal;

                    string selectsql = $"USE [NanpaoLog_{DateTime.Now.Year}] SELECT * FROM [ElectricTotal] WHERE GatewayIndex=@GatewayIndex AND DeviceIndex = @DeviceIndex AND ttime = @nowdate";
                    electricTotal = conn.QuerySingleOrDefault<ElectricTotal>(selectsql, new { GatewayIndex = protocol.GatewayIndex, DeviceIndex = protocol.DeviceIndex, nowdate = timestr.Substring(0, 10) });

                    if (electricTotal != null)
                    {
                        if (electricTotal.KwhStart1 == 0 && electricTotal.KwhEnd1 == 0 && electricTotal.KwhStart2 == 0 && electricTotal.KwhEnd2 == 0)
                        {
                            electricTotal.KwhStart1 = Math.Round(Convert.ToDecimal(protocol.KWH), 2);
                            electricTotal.KwhEnd1 = Math.Round(Convert.ToDecimal(protocol.KWH), 2);
                        }
                        else if (electricTotal.KwhEnd1 <= Math.Round(Convert.ToDecimal(protocol.KWH), 2) && electricTotal.KwhStart2 == 0)
                        {
                            electricTotal.KwhEnd1 = Math.Round(Convert.ToDecimal(protocol.KWH), 2);
                        }
                        else if (electricTotal.KwhStart2 == 0 && electricTotal.KwhEnd1 > Math.Round(Convert.ToDecimal(protocol.KWH), 2))
                        {
                            electricTotal.KwhStart2 = Math.Round(Convert.ToDecimal(protocol.KWH), 2);
                            electricTotal.KwhEnd2 = Math.Round(Convert.ToDecimal(protocol.KWH), 2);
                        }
                        else if (electricTotal.KwhStart2 != 0 && electricTotal.KwhEnd2 <= Math.Round(Convert.ToDecimal(protocol.KWH), 2))
                        {
                            electricTotal.KwhEnd2 = Math.Round(Convert.ToDecimal(protocol.KWH), 2);
                        }
                        electricTotal.KwhTotal = (electricTotal.KwhEnd1 - electricTotal.KwhStart1) + (electricTotal.KwhEnd2 - electricTotal.KwhStart2);
                        string updatesql = $"USE [NanpaoLog_{DateTime.Now.Year}] UPDATE ElectricTotal SET KwhStart1=@KwhStart1,KwhEnd1=@KwhEnd1,KwhStart2=@KwhStart2,KwhEnd2=@KwhEnd2,KwhTotal=@KwhTotal WHERE GatewayIndex=@GatewayIndex AND DeviceIndex = @DeviceIndex AND ttime = @ttime";
                        conn.Execute(updatesql, electricTotal);
                    }
                    else
                    {
                        electricTotal = new ElectricTotal()
                        {
                            ttime = timestr.Substring(0, 10),
                            ttimen = dateTime,
                            GatewayIndex = protocol.GatewayIndex,
                            DeviceIndex = protocol.DeviceIndex,
                            KwhStart1 = Convert.ToDecimal(protocol.KWH),
                            KwhEnd1 = Convert.ToDecimal(protocol.KWH),
                            KwhStart2 = 0,
                            KwhEnd2 = 0,
                            KwhTotal = 0,
                        };
                        string insertsql = $"USE [NanpaoLog_{DateTime.Now.Year}] INSERT INTO ElectricTotal (ttime,ttimen,GatewayIndex,DeviceIndex,KwhStart1,KwhEnd1,KwhStart2,KwhEnd2,KwhTotal) VALUES (@ttime,@ttimen,@GatewayIndex,@DeviceIndex,@KwhStart1,@KwhEnd1,@KwhStart2,@KwhEnd2,@KwhTotal)";
                        conn.Execute(insertsql, electricTotal);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"更新累積電量失敗 {protocol.DeviceSetting.DeviceName}");
            }
        }
        #endregion

        #region 更新流量計Log
        /// <summary>
        /// 更新液體流量計
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="chillerProtocol"></param>
        public void InsertFlow(FlowData protocol, ChillerProtocol chillerProtocol)
        {
            string timestr = DateTime.Now.ToString("yyyyMMddHHmm00");
            DateTime dateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:00"));
            try
            {
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string sql = "";
                    var exsitforweb = conn.ExecuteScalar<bool>($"SELECT TOP 1 1 FROM [FlowLog] WHERE GatewayIndex=@GatewayIndex AND DeviceIndex = @DeviceIndex", new { GatewayIndex = protocol.GatewayIndex, DeviceIndex = protocol.DeviceIndex });
                    if (exsitforweb)
                    {
                        sql = $"UPDATE [FlowLog] SET ttime=@ttime,ttimen=@ttimen,Flow=@Flow,FlowTotal=@FlowTotal,InputTemp=@InputTemp,OutputTemp=@OutputTemp,TempDifference=@TempDifference,SwitchFlag=@SwitchFlag WHERE GatewayIndex=@GatewayIndex AND DeviceIndex=@DeviceIndex";
                    }
                    else
                    {
                        sql = $"INSERT INTO [FlowLog] (ttime,ttimen,GatewayIndex,DeviceIndex,Flow,FlowTotal,InputTemp,OutputTemp,TempDifference,SwitchFlag) VALUES (@ttime,@ttimen,@GatewayIndex,@DeviceIndex,@Flow,@FlowTotal,@InputTemp,@OutputTemp,@TempDifference,@SwitchFlag)";
                    }
                    if (chillerProtocol != null)
                    {
                        int switchFlag = 0;
                        if (chillerProtocol.CHP_RUN_ST || chillerProtocol.CWP_RUN_ST || chillerProtocol.CH1_RUN_ST || chillerProtocol.CH2_RUN_ST)
                        {
                            switchFlag = 1;
                        }
                        conn.Execute(sql, new { ttime = timestr, ttimen = dateTime, GatewayIndex = protocol.GatewayIndex, DeviceIndex = protocol.DeviceIndex, protocol.Flow, protocol.FlowTotal, InputTemp = Math.Round(chillerProtocol.CHW_IN_TEMP, 2), OutputTemp = Math.Round(chillerProtocol.CHW_OUT_TEMP, 2), TempDifference = Math.Round(chillerProtocol.CHW_TEMP_Difference, 2), SwitchFlag = switchFlag });
                    }
                    else
                    {
                        conn.Execute(sql, new { ttime = timestr, ttimen = dateTime, GatewayIndex = protocol.GatewayIndex, DeviceIndex = protocol.DeviceIndex, protocol.Flow, protocol.FlowTotal, protocol.InputTemp, protocol.OutputTemp, protocol.TempDifference, SwitchFlag = 1 });
                    }
                    var exsit = conn.ExecuteScalar<bool>($"USE [NanpaoLog_{DateTime.Now.Year}]  SELECT TOP 1 1 FROM [FlowLog] WHERE ttime = @timestr AND GatewayIndex=@GatewayIndex AND DeviceIndex = @DeviceIndex", new { timestr, GatewayIndex = protocol.GatewayIndex, DeviceIndex = protocol.DeviceIndex });
                    if (!exsit)
                    {
                        int switchFlag = 0;
                        sql = $"USE [NanpaoLog_{DateTime.Now.Year}] INSERT INTO [FlowLog] (ttime,ttimen,GatewayIndex,DeviceIndex,Flow,FlowTotal,InputTemp,OutputTemp,TempDifference,SwitchFlag) VALUES (@ttime,@ttimen,@GatewayIndex,@DeviceIndex,@Flow,@FlowTotal,@InputTemp,@OutputTemp,@TempDifference,@SwitchFlag)";
                        if (chillerProtocol != null)
                        {
                            if (chillerProtocol.CHP_RUN_ST || chillerProtocol.CWP_RUN_ST || chillerProtocol.CH1_RUN_ST || chillerProtocol.CH2_RUN_ST)
                            {
                                switchFlag = 1;
                            }
                            conn.Execute(sql, new { ttime = timestr, ttimen = dateTime, GatewayIndex = protocol.GatewayIndex, DeviceIndex = protocol.DeviceIndex, protocol.Flow, protocol.FlowTotal, InputTemp = Math.Round(chillerProtocol.CHW_IN_TEMP, 2), OutputTemp = Math.Round(chillerProtocol.CHW_OUT_TEMP, 2), TempDifference = Math.Round(chillerProtocol.CHW_TEMP_Difference, 2), SwitchFlag = switchFlag });
                        }
                        else
                        {
                            conn.Execute(sql, new { ttime = timestr, ttimen = dateTime, GatewayIndex = protocol.GatewayIndex, DeviceIndex = protocol.DeviceIndex, protocol.Flow, protocol.FlowTotal, protocol.InputTemp, protocol.OutputTemp, protocol.TempDifference, SwitchFlag = 1 });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"更新液體流量計(有冰機)失敗 {protocol.DeviceSetting.DeviceName}");
            }
        }
        public void InsertFlow(FlowData protocol)
        {
            string timestr = DateTime.Now.ToString("yyyyMMddHHmm00");
            DateTime dateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:00"));
            try
            {
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string sql = "";
                    var exsitforweb = conn.ExecuteScalar<bool>($"SELECT TOP 1 1 FROM [FlowLog] WHERE GatewayIndex = @GatewayIndex AND DeviceIndex = @DeviceIndex", new { GatewayIndex = protocol.GatewayIndex, DeviceIndex = protocol.DeviceIndex });
                    if (exsitforweb)
                    {
                        sql = $"UPDATE [FlowLog] SET ttime=@ttime,ttimen=@ttimen,Flow=@Flow,FlowTotal=@FlowTotal,InputTemp=@InputTemp,OutputTemp=@OutputTemp,TempDifference=@TempDifference WHERE GatewayIndex=@GatewayIndex AND DeviceIndex=@DeviceIndex";
                    }
                    else
                    {
                        sql = $"INSERT INTO [FlowLog] (ttime,ttimen,GatewayIndex,DeviceIndex,Flow,FlowTotal,InputTemp,OutputTemp,TempDifference) VALUES (@ttime,@ttimen,@GatewayIndex,@DeviceIndex,@Flow,@FlowTotal,@InputTemp,@OutputTemp,@TempDifference)";
                    }
                    conn.Execute(sql, new { ttime = timestr, ttimen = dateTime, GatewayIndex = protocol.GatewayIndex, DeviceIndex = protocol.DeviceIndex, protocol.Flow, protocol.FlowTotal, protocol.InputTemp, protocol.OutputTemp, protocol.TempDifference });
                    var exsit = conn.ExecuteScalar<bool>($"USE [NanpaoLog_{DateTime.Now.Year}]  SELECT TOP 1 1 FROM [FlowLog] WHERE ttime = @timestr AND GatewayIndex = @GatewayIndex AND DeviceIndex = @DeviceIndex", new { timestr, GatewayIndex = protocol.GatewayIndex, DeviceIndex = protocol.DeviceIndex });
                    if (!exsit)
                    {
                        sql = $"USE [NanpaoLog_{DateTime.Now.Year}] INSERT INTO [FlowLog] (ttime,ttimen,GatewayIndex,DeviceIndex,Flow,FlowTotal,InputTemp,OutputTemp,TempDifference) VALUES (@ttime,@ttimen,@GatewayIndex,@DeviceIndex,@Flow,@FlowTotal,@InputTemp,@OutputTemp,@TempDifference)";
                        conn.Execute(sql, new { ttime = timestr, ttimen = dateTime, GatewayIndex = protocol.GatewayIndex, DeviceIndex = protocol.DeviceIndex, protocol.Flow, protocol.FlowTotal, protocol.InputTemp, protocol.OutputTemp, protocol.TempDifference });
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"更新液體流量計(無冰機)失敗 {protocol.DeviceSetting.DeviceName}");
            }
        }
        #endregion
        #region 更新累積流量
        /// <summary>
        /// 累積流量
        /// </summary>
        /// <param name="flowSetting"></param>
        public void InsertFlowTotal(FlowData protocol)
        {
            string timestr = DateTime.Now.ToString("yyyyMMddHH");
            try
            {
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    Flowtotal setting;
                    string selectsql = $"USE [NanpaoLog_{DateTime.Now.Year}] SELECT * FROM [Flowtotal] WHERE GatewayIndex = @GatewayIndex AND DeviceIndex = @DeviceIndex AND ttime = @nowdate";
                    setting = conn.QuerySingleOrDefault<Flowtotal>(selectsql, new { GatewayIndex = protocol.GatewayIndex, DeviceIndex = protocol.DeviceIndex, nowdate = timestr });
                    string Totalsql = "";
                    if (setting == null)
                    {
                        setting = new Flowtotal();
                        setting.ttime = DateTime.Now.ToString("yyyyMMddHH");
                        setting.ttimen = DateTime.Now;
                        setting.GatewayIndex = protocol.GatewayIndex;
                        setting.DeviceIndex = protocol.DeviceIndex;
                        Totalsql = $"USE [NanpaoLog_{DateTime.Now.Year}] INSERT INTO  [Flowtotal] (ttime,ttimen,GatewayIndex,DeviceIndex,InputTempAvg,OutputTempAvg,TempDifferenceAvg,RTH,FlowAvg,FlowStart1,FlowEnd1,FlowStart2,FlowEnd2,FlowTotal) VALUES (@ttime,@ttimen,@GatewayIndex,@DeviceIndex,@InputTempAvg,@OutputTempAvg,@TempDifferenceAvg,@RTH,@FlowAvg,@FlowStart1,@FlowEnd1,@FlowStart2,@FlowEnd2,@FlowTotal)";
                    }
                    else
                    {
                        setting.ttimen = DateTime.Now;
                        Totalsql = $"USE [NanpaoLog_{DateTime.Now.Year}] UPDATE [Flowtotal] SET ttimen=@ttimen,InputTempAvg=@InputTempAvg,OutputTempAvg=@OutputTempAvg,TempDifferenceAvg=@TempDifferenceAvg,RTH=@RTH,FlowAvg=@FlowAvg,FlowStart1=@FlowStart1,FlowEnd1=@FlowEnd1,FlowStart2=@FlowStart2,FlowEnd2=@FlowEnd2,FlowTotal=@FlowTotal WHERE  GatewayIndex=@GatewayIndex AND DeviceIndex=@DeviceIndex AND ttime = @ttime";
                    }
                    string sql = $"USE [NanpaoLog_{DateTime.Now.Year}] Select * FROM FlowLog Where ttime Like '{timestr}%' AND DeviceIndex = {protocol.DeviceIndex} AND GatewayIndex = {protocol.GatewayIndex}";
                    List<FlowLog> logs = conn.Query<FlowLog>(sql).ToList();
                    if (protocol.DeviceSetting.DeviceType == 3)//液體流量
                    {
                        decimal Index = 0;
                        decimal Flow = 0;
                        decimal InputTemp = 0;
                        decimal OutputTemp = 0;
                        decimal TempDifference = 0;
                        foreach (var logitem in logs)
                        {
                            if (logitem.SwitchFlag)
                            {
                                Index++;
                                Flow += logitem.Flow;
                                InputTemp += logitem.InputTemp;
                                OutputTemp += logitem.OutputTemp;
                                TempDifference += logitem.TempDifference;
                            }
                        }
                        if (Index != 0)
                        {
                            setting.FlowAvg = Math.Round(Convert.ToDecimal(Flow / Index), 2);
                            setting.InputTempAvg = Math.Round(Convert.ToDecimal(InputTemp / Index), 2);
                            setting.OutputTempAvg = Math.Round(Convert.ToDecimal(OutputTemp / Index), 2);
                            setting.TempDifferenceAvg = Math.Round(Convert.ToDecimal(TempDifference / Index), 2);
                            decimal T = (setting.InputTempAvg - setting.OutputTempAvg) > 0 ? (setting.InputTempAvg - setting.OutputTempAvg) : 0;
                            var rth = Math.Round(setting.FlowAvg * T * Convert.ToDecimal(4.2) / 3600 * 1000 / Convert.ToDecimal(3.516), 2);
                            if (rth > 0)
                            {
                                setting.RTH = rth;
                            }
                        }
                    }
                    bool RefershFlag = false;
                    bool RefershFlag1 = false;
                    decimal RefershData = 0;
                    foreach (var item in logs)
                    {
                        if (item.FlowTotal < RefershData && !RefershFlag && !RefershFlag1)
                        {
                            RefershData = item.FlowTotal;
                            RefershFlag = true;
                        }
                        else if (item.FlowTotal < RefershData && RefershFlag && !RefershFlag1)
                        {
                            RefershData = item.FlowTotal;
                            RefershFlag1 = true;
                        }
                        else
                        {
                            RefershData = item.FlowTotal;
                        }
                        if (setting.FlowStart1 == 0 && setting.FlowEnd1 == 0 && setting.FlowStart2 == 0 && setting.FlowEnd2 == 0)
                        {
                            setting.FlowStart1 = Math.Round(Convert.ToDecimal(item.FlowTotal), 2);
                            setting.FlowEnd1 = Math.Round(Convert.ToDecimal(item.FlowTotal), 2);
                        }
                        else if (setting.FlowEnd1 <= Math.Round(Convert.ToDecimal(item.FlowTotal), 2) && setting.FlowStart2 == 0 && !RefershFlag)
                        {
                            setting.FlowEnd1 = Math.Round(Convert.ToDecimal(item.FlowTotal), 2);
                        }
                        else if (setting.FlowStart2 == 0 && setting.FlowEnd1 > Math.Round(Convert.ToDecimal(item.FlowTotal), 2) && RefershFlag && !RefershFlag1)
                        {
                            setting.FlowStart2 = Math.Round(Convert.ToDecimal(item.FlowTotal), 2);
                            setting.FlowEnd2 = Math.Round(Convert.ToDecimal(item.FlowTotal), 2);
                        }
                        else if (setting.FlowStart2 != 0 && setting.FlowEnd2 <= Math.Round(Convert.ToDecimal(item.FlowTotal), 2) && !RefershFlag1)
                        {
                            setting.FlowEnd2 = Math.Round(Convert.ToDecimal(item.FlowTotal), 2);
                        }
                    }
                    if (!RefershFlag || !RefershFlag1)
                    {
                        setting.FlowTotal = (setting.FlowEnd1 - setting.FlowStart1) + (setting.FlowEnd2 - setting.FlowStart2);
                        conn.Execute(Totalsql, setting);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "累積流量失敗");
            }
        }
        #endregion

        #region 查詢冰機資訊
        /// <summary>
        /// 查詢冰機資訊
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public List<ChillerLog> SearchChiller(DeviceSetting setting, string StartTime, string EndTime)
        {
            List<ChillerLog> logs = null;
            try
            {
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string sql = $"USE [NanpaoLog_{StartTime.Substring(0, 4)}] SELECT * FROM ChillerLog WHERE (ttime >= @StartTIme AND ttime <= @EndTime) AND DeviceIndex = @DeviceIndex AND GatewayIndex = @GatewayIndex";
                    logs = conn.Query<ChillerLog>(sql, new { StartTime, EndTime, setting.DeviceIndex, setting.GatewayIndex }).ToList();
                    return logs;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "查詢冰機資訊失敗");
                return logs;
            }
        }
        /// <summary>
        /// 查詢冰機狀態失敗
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public List<ChillerStateLog> SearchChillerState(DeviceSetting setting, string StartTime, string EndTime)
        {
            List<ChillerStateLog> logs = null;
            try
            {
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string sql = $"USE [NanpaoLog_{StartTime.Substring(0, 4)}] SELECT * FROM ChillerStateLog WHERE (ttime >= @StartTIme AND ttime <= @EndTime) AND DeviceIndex = @DeviceIndex AND GatewayIndex = @GatewayIndex";
                    logs = conn.Query<ChillerStateLog>(sql, new { StartTime, EndTime, setting.DeviceIndex, setting.GatewayIndex }).ToList();
                    return logs;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "查詢冰機狀態失敗");
                return logs;
            }
        }
        #endregion
        #region 查詢電表資訊
        /// <summary>
        /// 查詢電表歷史資訊
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public List<ElectricLog> SearchElectric(DeviceSetting setting, string StartTime, string EndTime)
        {
            List<ElectricLog> logs = null;
            try
            {
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string sql = $"USE [NanpaoLog_{StartTime.Substring(0, 4)}] SELECT * FROM ElectricLog WHERE (ttime >= @StartTIme AND ttime <= @EndTime) AND DeviceIndex = @DeviceIndex AND GatewayIndex = @GatewayIndex";
                    logs = conn.Query<ElectricLog>(sql, new { StartTime, EndTime, setting.DeviceIndex, setting.GatewayIndex }).ToList();
                    return logs;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "查詢電表資訊失敗");
                return logs;
            }
        }
        /// <summary>
        /// 查詢電表累積資訊
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public List<ElectricTotal> SearchElectricTotal(DeviceSetting setting, string StartTime, string EndTime, int Day_Month)
        {
            List<ElectricTotal> logs = null;
            try
            {
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string sql = "";
                    switch (Day_Month)
                    {
                        case 0://每日
                            {
                                sql = $"USE [NanpaoLog_{StartTime.Substring(0, 4)}] SELECT MIN(LEFT(ttime,8)) AS ttime," +
                                $" MIN(ttimen) AS ttimen," +
                                $" MIN(DeviceIndex) AS DeviceIndex," +
                                $" MIN(GatewayIndex) AS GatewayIndex," +
                                $" MIN(KwhStart1) AS KwhStart1," +
                                $" MIN(KwhEnd1) AS KwhEnd1," +
                                $" MIN(KwhStart2) AS KwhStart2," +
                                $" MIN(KwhEnd2) AS KwhEnd2," +
                                $" SUM(KwhTotal) AS KwhTotal " +
                                $" FROM ElectricTotal WHERE (ttime >= @StartTIme AND ttime <= @EndTime) AND DeviceIndex = @DeviceIndex AND GatewayIndex = @GatewayIndex  GROUP BY LEFT(ttime,8)";
                            }
                            break;
                        case 1://每月
                            {
                                sql = $"USE [NanpaoLog_{StartTime.Substring(0, 4)}] SELECT MIN(LEFT(ttime,6)) AS ttime," +
                                $" MIN(ttimen) AS ttimen," +
                                $" MIN(DeviceIndex) AS DeviceIndex," +
                                $" MIN(GatewayIndex) AS GatewayIndex," +
                                $" MIN(KwhStart1) AS KwhStart1," +
                                $" MIN(KwhEnd1) AS KwhEnd1," +
                                $" MIN(KwhStart2) AS KwhStart2," +
                                $" MIN(KwhEnd2) AS KwhEnd2," +
                                $" SUM(KwhTotal) AS KwhTotal " +
                                $" FROM ElectricTotal WHERE (ttime >= @StartTIme AND ttime <= @EndTime) AND DeviceIndex = @DeviceIndex AND GatewayIndex = @GatewayIndex  GROUP BY LEFT(ttime,6)";
                            }
                            break;
                    }
                    logs = conn.Query<ElectricTotal>(sql, new { StartTime, EndTime, setting.DeviceIndex, setting.GatewayIndex }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "查詢電表累積資訊失敗");
            }
            return logs;
        }
        #endregion
        #region 查詢流量計資訊
        /// <summary>
        /// 查詢流量計資訊
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public List<FlowLog> SearchFlow(DeviceSetting setting, string StartTime, string EndTime)
        {
            List<FlowLog> logs = null;
            try
            {
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string sql = $"USE [NanpaoLog_{StartTime.Substring(0, 4)}] SELECT * FROM FlowLog WHERE (ttime >= @StartTIme AND ttime <= @EndTime) AND DeviceIndex = @DeviceIndex AND GatewayIndex = @GatewayIndex";
                    logs = conn.Query<FlowLog>(sql, new { StartTime, EndTime, setting.DeviceIndex, setting.GatewayIndex }).ToList();
                    return logs;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "查詢流量計資訊失敗");
                return logs;
            }
        }
        /// <summary>
        /// 查詢流量計累積資訊
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public List<Flowtotal> SearchFlowTotal(DeviceSetting setting, string StartTime, string EndTime, int Day_Month)
        {
            List<Flowtotal> logs = null;
            try
            {
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string sql = "";
                    switch (Day_Month)
                    {
                        case 0://每日
                            {
                                sql = $"USE [NanpaoLog_{StartTime.Substring(0, 4)}] SELECT MIN(LEFT(ttime,8)) AS ttime," +
                                $" MIN(ttimen) AS ttimen," +
                                $" MIN(GatewayIndex) AS GatewayIndex," +
                                $" MIN(DeviceIndex) AS DeviceIndex," +
                                $" MIN(InputTempAvg) AS InputTempAvg," +
                                $" MIN(OutputTempAvg) AS OutputTempAvg," +
                                $" MIN(TempDifferenceAvg) AS TempDifferenceAvg," +
                                $" MIN(FlowAvg) AS FlowAvg," +
                                $" SUM(RTH) AS RTH," +
                                $" MIN(FlowStart1) AS FlowStart1," +
                                $" MIN(FlowEnd1) AS FlowEnd1," +
                                $" MIN(FlowStart2) AS FlowStart2," +
                                $" MIN(FlowEnd2) AS FlowEnd2," +
                                $" SUM(FlowTotal) AS FlowTotal " +
                                $"FROM FlowTotal WHERE (ttime >= @StartTIme AND ttime <= @EndTime) AND DeviceIndex = @DeviceIndex AND GatewayIndex = @GatewayIndex GROUP BY LEFT(ttime,8)";
                            }
                            break;
                        case 1://每月
                            {
                                sql = $"USE [NanpaoLog_{StartTime.Substring(0, 4)}] SELECT MIN(LEFT(ttime,6)) AS ttime," +
                                $" MIN(ttimen) AS ttimen," +
                                $" MIN(GatewayIndex) AS GatewayIndex," +
                                $" MIN(DeviceIndex) AS DeviceIndex," +
                                $" MIN(InputTempAvg) AS InputTempAvg," +
                                $" MIN(OutputTempAvg) AS OutputTempAvg," +
                                $" MIN(TempDifferenceAvg) AS TempDifferenceAvg," +
                                $" MIN(FlowAvg) AS FlowAvg," +
                                $" SUM(RTH) AS RTH," +
                                $" MIN(FlowStart1) AS FlowStart1," +
                                $" MIN(FlowEnd1) AS FlowEnd1," +
                                $" MIN(FlowStart2) AS FlowStart2," +
                                $" MIN(FlowEnd2) AS FlowEnd2," +
                                $" SUM(FlowTotal) AS FlowTotal " +
                                $"FROM FlowTotal WHERE (ttime >= @StartTIme AND ttime <= @EndTime) AND  DeviceIndex = @DeviceIndex AND GatewayIndex = @GatewayIndex GROUP BY LEFT(ttime,6)";
                            }
                            break;
                        case 2://每時
                            {
                                sql = $"USE [NanpaoLog_{StartTime.Substring(0, 4)}] SELECT MIN(LEFT(ttime,10)) AS ttime," +
                               $" MIN(ttimen) AS ttimen," +
                               $" MIN(GatewayIndex) AS GatewayIndex," +
                               $" MIN(DeviceIndex) AS DeviceIndex," +
                               $" MIN(InputTempAvg) AS InputTempAvg," +
                               $" MIN(OutputTempAvg) AS OutputTempAvg," +
                               $" MIN(TempDifferenceAvg) AS TempDifferenceAvg," +
                               $" MIN(FlowAvg) AS FlowAvg," +
                               $" SUM(RTH) AS RTH," +
                               $" MIN(FlowStart1) AS FlowStart1," +
                               $" MIN(FlowEnd1) AS FlowEnd1," +
                               $" MIN(FlowStart2) AS FlowStart2," +
                               $" MIN(FlowEnd2) AS FlowEnd2," +
                               $" SUM(FlowTotal) AS FlowTotal " +
                               $"FROM FlowTotal WHERE (ttime >= @StartTIme AND ttime <= @EndTime) AND DeviceIndex = @DeviceIndex AND GatewayIndex = @GatewayIndex GROUP BY LEFT(ttime,10)";
                            }
                            break;
                    }
                    logs = conn.Query<Flowtotal>(sql, new { StartTime, EndTime, setting.DeviceIndex, setting.GatewayIndex }).ToList();
                    return logs;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "查詢流量計累積資訊失敗");
                return logs;
            }
        }
        #endregion
        #region 查詢變壓器資訊
        /// <summary>
        /// 查詢變壓器資訊
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public List<TRLog> SearchTR(DeviceSetting setting, string StartTime, string EndTime)
        {
            List<TRLog> logs = null;
            try
            {
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string sql = $"USE [NanpaoLog_{StartTime.Substring(0, 4)}] SELECT * FROM TRLog WHERE (ttime >= @StartTIme AND ttime <= @EndTime) AND DeviceIndex = @DeviceIndex AND GatewayIndex = @GatewayIndex";
                    logs = conn.Query<TRLog>(sql, new { StartTime, EndTime, setting.DeviceIndex, setting.GatewayIndex }).ToList();
                    return logs;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "查詢變壓器資訊失敗");
                return logs;
            }
        }
        public List<TRStateLog> SearchTRState(DeviceSetting setting, string StartTime, string EndTime)
        {
            List<TRStateLog> logs = null;
            try
            {
                using (var conn = new SqlConnection(Chillerscsb.ConnectionString))
                {
                    string sql = $"USE [NanpaoLog_{StartTime.Substring(0, 4)}] SELECT * FROM TRStateLog WHERE (ttime >= @StartTIme AND ttime <= @EndTime) AND DeviceIndex = @DeviceIndex AND GatewayIndex = @GatewayIndex";
                    logs = conn.Query<TRStateLog>(sql, new { StartTime, EndTime, setting.DeviceIndex, setting.GatewayIndex }).ToList();
                    return logs;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "查詢變壓器狀態失敗");
                return logs;
            }
        }
        #endregion
    }
}
