using NanpaoChillerSystemV2.EF_Modules;
using NanpaoChillerSystemV2.Methods;
using NModbus;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NanpaoChillerSystemV2.Protocols
{
    public class TRProtocol : SenserData
    {
        public TRProtocol(GatewaySetting gatewaySetting, DeviceSetting deviceSetting, SqlMethod sqlMethod, List<LineNotifySetting> lineNotifySettings)
        {
            GatewaySetting = gatewaySetting;
            DeviceSetting = deviceSetting;
            SqlMethod = sqlMethod;
            LineNotifySettings = lineNotifySettings;
            temp_max = deviceSetting.TempMax;
            temp_min = deviceSetting.TempMin;
            temp_max1 = deviceSetting.TempMax1;
            temp_min1 = deviceSetting.TempMin1;
            SlaveFun3 = new List<ushort>();
            SlaveFun3.Add(Convert.ToUInt16(TempMax * 10));
            SlaveFun3.Add(Convert.ToUInt16(TempMin * 10));
            SlaveFun3.Add(Convert.ToUInt16(TempMax1 * 10));
            SlaveFun3.Add(Convert.ToUInt16(TempMin1 * 10));
        }
        public override void DataReader(IModbusMaster master)
        {
            try
            {
                ushort[] A1 = master.ReadHoldingRegisters(ID, 4096, 2);
                SlaveFun4 = new List<ushort>();
                SlaveFun4.AddRange(A1);
                if (FirstReadFlag)
                {
                    temp = Convert.ToDecimal(A1[0] * 0.1F);
                    temp1 = Convert.ToDecimal(A1[1] * 0.1F);
                    FirstReadFlag = true;
                }
                else
                {
                    Temp = Convert.ToDecimal(A1[0] * 0.1F);
                    Temp1 = Convert.ToDecimal(A1[1] * 0.1F);
                }
                LastReadTime = DateTime.Now;
                ConnectionFlag = true;
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                Log.Error(ex, $"{GatewaySetting.Connect},{DeviceSetting.DeviceName} 通訊錯誤");
                ConnectionFlag = false;
            }
        }
        public override void StateLogCreater(ushort Address, string Message) { }
        public override void CoilWriter(ushort addr, bool status) { }
        public override void ValueWriter(ushort addr, ushort value) { }
    }
}
