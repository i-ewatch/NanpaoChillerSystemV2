using NanpaoChillerSystemV2.EF_Modules;
using NanpaoChillerSystemV2.Methods;
using NModbus;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NanpaoChillerSystemV2.Protocols
{
    public class PA3000Protocol : ElectricData
    {
        public PA3000Protocol(GatewaySetting gatewaySetting, DeviceSetting deviceSetting, SqlMethod sqlMethod, List<LineNotifySetting> lineNotifySettings)
        {
            GatewaySetting = gatewaySetting;
            DeviceSetting = deviceSetting;
            SqlMethod = sqlMethod;
            LineNotifySettings = lineNotifySettings;
        }
        public override void DataReader(IModbusMaster master)
        {
            try
            {
                ushort[] data = master.ReadInputRegisters(ID, 4106, 98);
                if (data.Length == 98)
                {
                    int k = 0;
                    RSV = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    STV = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    TRV = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    RV = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    SV = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    TV = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    RA = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    SA = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    TA = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    KW = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    KVAR = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    KVA = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    PFE = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    RV_Angle = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    SV_Angle = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    TV_Angle = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    RA_Angle = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    SA_Angle = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    TA_Angle = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    KWH = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    KVARH = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    KVAH = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    LastReadTime = DateTime.Now;
                    ConnectionFlag = true;
                }
                else
                {
                    ConnectionFlag = false;
                }
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                Log.Error(ex, $"{GatewaySetting.Connect},{DeviceSetting.DeviceName} 通訊錯誤");
                ConnectionFlag = false;
            }
        }
        public override void StateLogCreater(ushort Address, string Message) { }
        public override void ValueWriter(ushort addr, ushort value) { }
        public override void CoilWriter(ushort addr, bool status) { }
    }
}
