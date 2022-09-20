using NanpaoChillerSystemV2.EF_Modules;
using NanpaoChillerSystemV2.Methods;
using NModbus;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NanpaoChillerSystemV2.Protocols
{
    public class PA330Protocol : ElectricData
    {
        public PA330Protocol(GatewaySetting gatewaySetting, DeviceSetting deviceSetting, SqlMethod sqlMethod, List<LineNotifySetting> lineNotifySettings)
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
                ushort[] data = master.ReadInputRegisters(ID, 4096, 72);
                if (data.Length == 72)
                {
                    SlaveFun4 = new List<ushort>();
                    int k = 0;
                    SlaveFun4.Add(data[k]);
                    SlaveFun4.Add(data[k + 1]);
                    RSV = Convert.ToDecimal(Math.Round(Calculate.work16to754(data[k + 1], data[k]), 2)); k += 2;
                    SlaveFun4.Add(data[k]);
                    SlaveFun4.Add(data[k + 1]);
                    STV = Convert.ToDecimal(Math.Round(Calculate.work16to754(data[k + 1], data[k]), 2)); k += 2;
                    SlaveFun4.Add(data[k]);
                    SlaveFun4.Add(data[k + 1]);
                    TRV = Convert.ToDecimal(Math.Round(Calculate.work16to754(data[k + 1], data[k]), 2)); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;            // Vln AVG
                    RV = Convert.ToDecimal(Math.Round(Calculate.work16to754(data[k + 1], data[k]), 2)); k += 2;
                    SV = Convert.ToDecimal(Math.Round(Calculate.work16to754(data[k + 1], data[k]), 2)); k += 2;
                    TV = Convert.ToDecimal(Math.Round(Calculate.work16to754(data[k + 1], data[k]), 2)); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;            // Vll AVG
                    SlaveFun4.Add(data[k]);
                    SlaveFun4.Add(data[k + 1]);
                    RA = Convert.ToDecimal(Math.Round(Calculate.work16to754(data[k + 1], data[k]), 2)); k += 2;
                    SlaveFun4.Add(data[k]);
                    SlaveFun4.Add(data[k + 1]);
                    SA = Convert.ToDecimal(Math.Round(Calculate.work16to754(data[k + 1], data[k]), 2)); k += 2;
                    SlaveFun4.Add(data[k]);
                    SlaveFun4.Add(data[k + 1]);
                    TA = Convert.ToDecimal(Math.Round(Calculate.work16to754(data[k + 1], data[k]), 2)); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;            // I AVG
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;            // HZ
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;            // KW A
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;            // KW B
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;            // KW C
                    KW = Convert.ToDecimal(Math.Round(Calculate.work16to754(data[k + 1], data[k]), 2)); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;            // KVAR A
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;            // KVAR B
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;            // KVAR C
                    KVAR = Convert.ToDecimal(Math.Round(Calculate.work16to754(data[k + 1], data[k]), 2)); k += 2;
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;            // KVA A
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;            // KVA B
                    _ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;            // KVA C
                    KVA = Convert.ToDecimal(Math.Round(Calculate.work16to754(data[k + 1], data[k]), 2)); k += 2;
                    SlaveFun4.Add(data[k]);
                    SlaveFun4.Add(data[k + 1]);
                    PFE = Convert.ToDecimal(Math.Round(Calculate.work16to754(data[k + 1], data[k]), 2)); k += 2;
                    SlaveFun4.Add(data[32]);
                    SlaveFun4.Add(data[33]);
                    SlaveFun4.Add(data[k]);
                    SlaveFun4.Add(data[k + 1]);
                    KWH = Convert.ToDecimal(Math.Round(Calculate.work16to754(data[k + 1], data[k]), 2)); k += 2;
                    KVARH = Convert.ToDecimal(Math.Round(Calculate.work16to754(data[k + 1], data[k]), 2)); k += 2;
                    //KVAH = Convert.ToDecimal(Math.Round(Calculate.work16to754(data[k + 1], data[k]), 2)); k += 2;
                    //_ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    //_ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    //_ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    //_ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    //_ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    //_ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k])); k += 2;
                    //_ = Convert.ToDecimal(Calculate.work16to754(data[k + 1], data[k]));
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
