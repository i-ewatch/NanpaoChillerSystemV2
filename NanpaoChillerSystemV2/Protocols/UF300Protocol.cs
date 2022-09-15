using NanpaoChillerSystemV2.EF_Modules;
using NanpaoChillerSystemV2.Methods;
using NModbus;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NanpaoChillerSystemV2.Protocols
{
    public class UF300Protocol : FlowData
    {
        public UF300Protocol(GatewaySetting gatewaySetting, DeviceSetting deviceSetting, SqlMethod sqlMethod, List<LineNotifySetting> lineNotifySettings)
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
                ushort[] A1 = master.ReadHoldingRegisters(ID, 0, 2);
                Thread.Sleep(10);
                ushort[] A2 = master.ReadHoldingRegisters(ID, 8, 4);
                Thread.Sleep(10);
                ushort[] A3 = master.ReadHoldingRegisters(ID, 32, 4);
                if (A1.Length == 2 || A2.Length == 2 || A3.Length == 4)
                {
                    SlaveFun4 = new List<ushort>();
                    SlaveFun4.AddRange(A1);
                    SlaveFun4.AddRange(A2);
                    SlaveFun4.AddRange(A3);
                    Flow =Convert.ToDecimal( Math.Round(Calculate.work16to754(A1[1], A1[0]), 2));
                    FlowTotal = Convert.ToDecimal(Math.Round(Convert.ToDouble(Calculate.work16to10(A2[1], A2[0])), 2) + Math.Round(Calculate.work16to754(A2[3], A2[2]), 2));
                    OutputTemp = Convert.ToDecimal(Math.Round(Calculate.work16to754(A3[1], A3[0]), 2));
                    InputTemp = Convert.ToDecimal(Math.Round(Calculate.work16to754(A3[3], A3[2]), 2));
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
