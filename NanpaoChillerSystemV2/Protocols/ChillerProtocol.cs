using NanpaoChillerSystemV2.EF_Modules;
using NanpaoChillerSystemV2.Methods;
using NanpaoChillerSystemV2.Modules;
using NModbus;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NanpaoChillerSystemV2.Protocols
{
    public class ChillerProtocol : ChillerData
    {
        public ChillerProtocol(GatewaySetting gatewaySetting, DeviceSetting deviceSetting, SqlMethod sqlMethod, List<LineNotifySetting> lineNotifySettings)
        {
            GatewaySetting = gatewaySetting;
            DeviceSetting = deviceSetting;
            SqlMethod = sqlMethod;
            LineNotifySettings = lineNotifySettings;
        }
        public override void CoilWriter(ushort addr, bool status)
        {
            WriteCoilObject writeCoil = new WriteCoilObject()
            {
                Address = addr,
                State = status
            };
            WriteCoils.Add(writeCoil);
        }

        public override void DataReader(IModbusMaster master)
        {
            try
            {
                int k = 0;
                ushort[] data = master.ReadHoldingRegisters(ID, 4396, 4);//水溫讀取
                Thread.Sleep(10);
                ushort[] data1 = master.ReadHoldingRegisters(ID, 4406, 4);//冷媒壓力讀取
                Thread.Sleep(10);
                ushort[] data2 = master.ReadHoldingRegisters(ID, 4417, 2);//壓縮機容調顯示值讀取
                Thread.Sleep(10);
                ushort[] data3 = master.ReadHoldingRegisters(ID, 4997, 2);//停機倒數讀取
                Thread.Sleep(10);
                ushort[] data4 = master.ReadHoldingRegisters(ID, 5006, 1);//冰水泵倒數讀取
                Thread.Sleep(10);
                ushort[] data5 = master.ReadHoldingRegisters(ID, 6296, 4);//冰機運轉時數讀取
                Thread.Sleep(10);
                ushort[] data6 = master.ReadHoldingRegisters(ID, 6306, 4);//冰機運轉次數讀取
                Thread.Sleep(10);
                bool[] state = master.ReadCoils(ID, 2248, 1);//主機啟動鍵讀取(僅啟動執行一次)
                Thread.Sleep(10);
                bool[] state1 = master.ReadCoils(ID, 2569, 4);//容調值切換及限制狀態(僅啟動執行一次)
                Thread.Sleep(10);
                bool[] state2 = master.ReadCoils(ID, 2749, 2);//選擇鍵讀取(僅啟動執行一次)
                Thread.Sleep(10);
                #region 水溫及溫度範圍設定(僅啟動執行一次)
                //ushort[] data7 = master.ReadHoldingRegisters(ID, 6096, 8);//讀取設定範圍及設定水溫
                //bool[] state3 = master.ReadCoils(ID, 2798, 1);//讀取設定選擇
                #endregion
                #region 預約時間設定讀取(僅啟動執行一次)
                //ushort[] data8 = master.ReadHoldingRegisters(ID, 6146, 14);//讀取設定預約啟動
                //bool[] state4 = master.ReadCoils(ID, 2753, 1);//讀取設定選擇
                #endregion
                bool[] state5 = master.ReadCoils(ID, 2848, 23);//讀取系統異常狀態
                Thread.Sleep(10);
                bool[] state6 = master.ReadCoils(ID, 2448, 1);//異常復歸(僅啟動執行一次)
                Thread.Sleep(10);
                bool[] state7 = master.ReadCoils(ID, 1280, 2);//讀取Pump DO狀態
                Thread.Sleep(10);
                bool[] state8 = master.ReadCoils(ID, 1288, 4);//讀取壓縮機DO狀態
                //ushort[] data9 = master.ReadHoldingRegisters(ID, 4696, 51);//讀取冰機電力資訊

                CHW_OUT_TEMP = Convert.ToDecimal(data[0] * 0.1F);
                CHW_IN_TEMP = Convert.ToDecimal(data[1] * 0.1F);
                CW_OUT_TEMP = Convert.ToDecimal(data[2] * 0.1F);
                CW_IN_TEMP = Convert.ToDecimal(data[3] * 0.1F);
                CH1_PRESS_HIGH = Convert.ToDecimal(data1[0] * 0.1F);
                CH1_PRESS_LOW = Convert.ToDecimal(data1[1] * 0.1F);
                CH2_PRESS_HIGH = Convert.ToDecimal(data1[2] * 0.1F);
                CH2_PRESS_LOW = Convert.ToDecimal(data1[3] * 0.1F);
                CH1_RATE = data2[0];
                CH2_RATE = data2[1];
                CH1_STOP_COUNT = data3[0];
                CH2_STOP_COUNT = data3[1];
                CHP_STOP_COUNT = data4[0];
                CH1_RUN_HOUR = Convert.ToUInt32(Calculate.work16to10(data5[k + 1], data5[k])); k += 2;
                CH2_RUN_HOUR = Convert.ToUInt32(Calculate.work16to10(data5[k + 1], data5[k]));
                k = 0;
                CH1_RUN_TIME = Convert.ToUInt32(Calculate.work16to10(data6[k + 1], data6[k])); k += 2;
                CH2_RUN_TIME = Convert.ToUInt32(Calculate.work16to10(data6[k + 1], data6[k]));

                CHP_RUN_ST = state7[0];
                CWP_RUN_ST = state7[1];
                CH1_RUN_ST = state8[0];
                CH2_RUN_ST = state8[1];
                CH1_ERR_ST = state8[2];
                CH2_ERR_ST = state8[3];
                if (FirstReadFlag)
                {
                    CH_START_ST = state[0];
                    CH1_RATE_LIMIT = state1[0];
                    CH1_RATE_SWITCH = state1[1];
                    CH2_RATE_LIMIT = state1[2];
                    CH2_RATE_SWITCH = state1[3];
                    CH1_SELECT_ST = state2[0];
                    CH2_SELECT_ST = state2[1];
                    //NO1_CHWOUT_SET = data7[0] * 0.1F;
                    //NO1_CHWOUT_RANGE = data7[1] * 0.1F;
                    //NO2_CHWOUT_SET = data7[2] * 0.1F;
                    //NO2_CHWOUT_RANGE = data7[3] * 0.1F;
                    //NO1_CHWIN_SET = data7[4] * 0.1F;
                    //NO1_CHWIN_RANGE = data7[5] * 0.1F;
                    //NO2_CHWIN_SET = data7[6] * 0.1F;
                    //NO2_CHWIN_RANGE = data7[7] * 0.1F;
                    //WATER_SELECT_INOUT = state3[0];
                    //STARTTIME1 = data8[0];
                    //STOPTIME1 = data8[1];
                    //STARTTIME2 = data8[2];
                    //STOPTIME2 = data8[3];
                    //STARTTIME3 = data8[4];
                    //STOPTIME3 = data8[5];
                    //STARTTIME4 = data8[6];
                    //STOPTIME4 = data8[7];
                    //STARTTIME5 = data8[8];
                    //STOPTIME5 = data8[9];
                    //STARTTIME6 = data8[10];
                    //STOPTIME6 = data8[11];
                    //STARTTIME7 = data8[12];
                    //STOPTIME7 = data8[13];
                    //RESERVE_TIME_SWITCH = state4[0];
                    CW_TEMP_LOW = state5[0];
                    CHW_FLOW_FLT = state5[1];
                    CW_FLOW_FLT = state5[2];
                    POWER_BACK_FLT = state5[3];
                    FROZEN_SWITCH_FLT = state5[4];
                    DELTA_PRESS_FLT = state5[5];
                    POWER_BREAK_FLT = state5[6];
                    CH1_HIGHPRESS_FLT = state5[7];
                    CH1_LOWPRESS_FLT = state5[8];
                    CH1_OVERHEAT_FLT = state5[9];
                    CH1_OVERLOAD_FLT = state5[10];
                    CH1_OILLEVEL_FLT = state5[11];
                    CH1_OVERTIME_FLT = state5[12];
                    CH2_HIGHPRESS_FLT = state5[13];
                    CH2_LOWPRESS_FLT = state5[14];
                    CH2_OVERHEAT_FLT = state5[15];
                    CH2_OVERLOAD_FLT = state5[16];
                    CH2_OILLEVEL_FLT = state5[17];
                    CH2_OVERTIME_FLT = state5[18];
                    CH_WATEROUT_FLT = state5[19];
                    CH_WATERIN_FLT = state5[20];
                    CHT_DELTA_FLT = state5[21];
                    CWT_DELTA_FLT = state5[22];
                    ERR_RECOVER_ST = state6[0];
                }
                else
                {
                    ch_start_st = state[0];
                    ch1_rate_limit = state1[0];
                    ch1_rate_swtich = state1[1];
                    ch2_rate_limit = state1[2];
                    ch2_rate_switch = state1[3];
                    ch1_select_st = state2[0];
                    ch2_select_st = state2[1];
                    //no1_chwout_set = data7[0] * 0.1F;
                    //no1_chwout_range = data7[1] * 0.1F;
                    //no2_chwout_set = data7[2] * 0.1F;
                    //no2_chwout_range = data7[3] * 0.1F;
                    //no1_chwin_set = data7[4] * 0.1F;
                    //no1_chwin_range = data7[5] * 0.1F;
                    //no2_chwin_set = data7[6] * 0.1F;
                    //no2_chwin_range = data7[7] * 0.1F;
                    //water_select_inout = state3[0];
                    //starttime1 = data8[0];
                    //stoptime1 = data8[1];
                    //starttime2 = data8[2];
                    //stoptime2 = data8[3];
                    //starttime3 = data8[4];
                    //stoptime3 = data8[5];
                    //starttime4 = data8[6];
                    //stoptime4 = data8[7];
                    //starttime5 = data8[8];
                    //stoptime5 = data8[9];
                    //starttime6 = data8[10];
                    //stoptime6 = data8[11];
                    //starttime7 = data8[12];
                    //stoptime7 = data8[13];
                    //reserve_time_switch = state4[0];
                    cw_temp_low = state5[0];
                    chw_flow_flt = state5[1];
                    cw_flow_flt = state5[2];
                    power_back_flt = state5[3];
                    frozen_swtich_flt = state5[4];
                    delta_press_flt = state5[5];
                    power_break_flt = state5[6];
                    ch1_highpress_flt = state5[7];
                    ch1_lowpress_flt = state5[8];
                    ch1_overheat_flt = state5[9];
                    ch1_overload_flt = state5[10];
                    ch1_overlevel_flt = state5[11];
                    ch1_overtime_flt = state5[12];
                    ch2_highpress_flt = state5[13];
                    ch2_lowpress_flt = state5[14];
                    ch2_overheat_flt = state5[15];
                    ch2_overload_flt = state5[16];
                    ch2_overlevel_flt= state5[17];
                    ch2_overtime_flt = state5[18];
                    ch_waterout_flt = state5[19];
                    chw_waterin_flt = state5[20];
                    cht_delta_flt = state5[21];
                    cwt_delta_flt = state5[22];
                    err_recover_st = state6[0];
                    FirstReadFlag = true;
                }
                SlaveFun4 = new List<ushort>();
                SlaveFun4.AddRange(data);
                SlaveFun4.AddRange(data1);
                SlaveFun4.AddRange(data2);
                SlaveFun4.AddRange(data3);
                SlaveFun4.AddRange(data4);
                SlaveFun4.AddRange(data5);
                SlaveFun4.AddRange(data6);
                SlaveFun2 = new List<bool>();
                SlaveFun2.AddRange(state5);
                SlaveFun2.AddRange(state6);
                SlaveFun2.AddRange(state7);
                SlaveFun2.AddRange(state8);
                SlaveFun2.Add(Operating_Status);
                SlaveFun2.Add(Alarm_Status);
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

        public override void StateLogCreater(ushort Address, string Message)
        {
           
        }

        public override void ValueWriter(ushort addr, ushort value)
        {
            WriteValueObject writeValue = new WriteValueObject()
            {
                Address = addr,
                Value = value
            };
            WriteValues.Add(writeValue);
        }
    }
}
