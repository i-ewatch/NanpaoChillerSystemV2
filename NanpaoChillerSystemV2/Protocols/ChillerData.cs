using System;

namespace NanpaoChillerSystemV2.Protocols
{
    public abstract class ChillerData : AbsProtocol
    {
        #region 綜合狀態
        /// <summary>
        /// 運轉狀態
        /// </summary>
        public bool Operating_Status
        {
            get
            {
                var flag = false;
                if (CHP_RUN_ST || CWP_RUN_ST || CH1_RUN_ST || CH2_RUN_ST)
                {
                    flag = true;
                }
                return flag;
            }
        }
        public bool Alarm_Status
        {
            get
            {
                var flag = false;
                if (CW_TEMP_LOW || CHW_FLOW_FLT || CW_FLOW_FLT || POWER_BACK_FLT || FROZEN_SWITCH_FLT || DELTA_PRESS_FLT || POWER_BREAK_FLT
                                || CH1_HIGHPRESS_FLT || CH1_LOWPRESS_FLT || CH1_OVERHEAT_FLT || CH1_OVERLOAD_FLT || CH1_OVERTIME_FLT
                                || CH2_HIGHPRESS_FLT || CH2_LOWPRESS_FLT || CH2_OVERHEAT_FLT || CH2_OVERLOAD_FLT || CH2_OVERTIME_FLT
                                || CH_WATEROUT_FLT || CH_WATERIN_FLT || CHT_DELTA_FLT || CWT_DELTA_FLT || ERR_RECOVER_ST
                                || CH1_ERR_ST || CH2_ERR_ST)
                {
                    flag = true;
                }
                return flag;
            }
        }
        #endregion
        #region 水溫顯示
        /// <summary>
        /// 冰水出水溫
        /// </summary>
        public decimal CHW_OUT_TEMP { get; protected set; } = 0;
        /// <summary>
        /// 冰水入水溫
        /// </summary>
        public decimal CHW_IN_TEMP { get; protected set; } = 0;
        public decimal CHW_TEMP_Difference
        {
            get
            {
                var data = CHW_IN_TEMP - CHW_OUT_TEMP;
                if (data < 0)
                {
                    data = 0;
                }
                return data;
            }
        }
        /// <summary>
        /// 冷卻水出水溫
        /// </summary>
        public decimal CW_OUT_TEMP { get; protected set; } = 0;
        /// <summary>
        /// 冷卻水入水溫
        /// </summary>
        public decimal CW_IN_TEMP { get; protected set; } = 0;
        public decimal CW_TEMP_Difference
        {
            get
            {
                var data = CW_OUT_TEMP - CW_IN_TEMP;
                if (data < 0)
                {
                    data = 0;
                }
                return data;
            }
        }
        #endregion
        #region 冷媒壓力
        /// <summary>
        /// 機1高壓壓力
        /// </summary>
        public decimal CH1_PRESS_HIGH { get; protected set; }
        /// <summary>
        /// 機1低壓壓力
        /// </summary>
        public decimal CH1_PRESS_LOW { get; protected set; }
        /// <summary>
        /// 機2高壓壓力
        /// </summary>
        public decimal CH2_PRESS_HIGH { get; protected set; }
        /// <summary>
        /// 機2低壓壓力
        /// </summary>
        public decimal CH2_PRESS_LOW { get; protected set; }
        #endregion
        #region 壓縮機資訊
        /// <summary>
        /// 機1容調顯示值
        /// </summary>
        public ushort CH1_RATE { get; protected set; }
        /// <summary>
        /// 機2容調顯示值
        /// </summary>
        public ushort CH2_RATE { get; protected set; }
        /// <summary>
        /// 機1停機保護倒數
        /// </summary>
        public ushort CH1_STOP_COUNT { get; protected set; }
        /// <summary>
        /// 機2停機保護倒數
        /// </summary>
        public ushort CH2_STOP_COUNT { get; protected set; }
        /// <summary>
        /// 冰水泵卸載到數
        /// </summary>
        public ushort CHP_STOP_COUNT { get; protected set; }
        /// <summary>
        /// 機1運轉時數
        /// </summary>
        public uint CH1_RUN_HOUR { get; protected set; }
        /// <summary>
        /// 機2運轉時數
        /// </summary>
        public uint CH2_RUN_HOUR { get; protected set; }
        /// <summary>
        /// 機1啟動次數
        /// </summary>
        public uint CH1_RUN_TIME { get; protected set; }
        /// <summary>
        /// 機2啟動次數
        /// </summary>
        public uint CH2_RUN_TIME { get; protected set; }
        #endregion
        #region 壓縮機資訊(控制)
        /// <summary>
        /// 主機啟動鍵(0停1動)
        /// </summary>
        public bool CH_START_ST
        {
            get { return ch_start_st; }
            set
            {
                if (ch_start_st != value)
                {
                    ch_start_st = value;
                    CoilWriter(2248, value);
                }
            }
        }
        protected bool ch_start_st;
        /// <summary>
        /// 機1容調限制鍵(0停1動)
        /// </summary>
        public bool CH1_RATE_LIMIT
        {
            get { return ch1_rate_limit; }
            set
            {
                if (ch1_rate_limit != value)
                {
                    ch1_rate_limit = value;
                    CoilWriter(2569, value);
                }
            }
        }
        protected bool ch1_rate_limit;
        /// <summary>
        /// 機1容調切換鍵(0停1動)
        /// </summary>
        public bool CH1_RATE_SWITCH
        {
            get { return ch1_rate_swtich; }
            set
            {
                if (ch1_rate_swtich != value)
                {
                    ch1_rate_swtich = value;
                    CoilWriter(2570, value);
                }
            }
        }
        protected bool ch1_rate_swtich;
        /// <summary>
        /// 機2容調限制鍵(0停1動)
        /// </summary>
        public bool CH2_RATE_LIMIT
        {
            get { return ch2_rate_limit; }
            set
            {
                if (ch2_rate_limit != value)
                {
                    ch2_rate_limit = value;
                    CoilWriter(2571, value);
                }
            }
        }
        protected bool ch2_rate_limit;
        /// <summary>
        /// 機2容調切換鍵(0停1動)
        /// </summary>
        public bool CH2_RATE_SWITCH
        {
            get { return ch2_rate_switch; }
            set
            {
                if (ch2_rate_switch != value)
                {
                    ch2_rate_switch = value;
                    CoilWriter(2572, value);
                }
            }
        }
        protected bool ch2_rate_switch;
        /// <summary>
        /// 機1選擇鍵(0停1動)
        /// </summary>
        public bool CH1_SELECT_ST
        {
            get { return ch1_select_st; }
            set
            {
                if (ch1_select_st != value)
                {
                    ch1_select_st = value;
                    CoilWriter(2749, value);
                }
            }
        }
        protected bool ch1_select_st;
        /// <summary>
        /// 機2選擇鍵(0停1動)
        /// </summary>
        public bool CH2_SELECT_ST
        {
            get { return ch2_select_st; }
            set
            {
                if (ch2_select_st != value)
                {
                    ch2_select_st = value;
                    CoilWriter(2750, value);
                }
            }
        }
        protected bool ch2_select_st;
        #endregion
        #region 水溫設定
        /// <summary>
        /// 第一次冰水出水溫度設定
        /// </summary>
        public decimal NO1_CHWOUT_SET
        {
            get { return no1_chwout_set; }
            set
            {
                if (no1_chwout_set != value)
                {
                    no1_chwout_set = value;
                    ushort val = Convert.ToUInt16(value * 10);
                    ValueWriter(6096, val);
                }
            }
        }
        protected decimal no1_chwout_set;
        /// <summary>
        /// 第一次冰水出水溫度範圍
        /// </summary>
        public decimal NO1_CHWOUT_RANGE
        {
            get { return no1_chwout_range; }
            set
            {
                if (no1_chwout_range != value)
                {
                    no1_chwout_range = value;
                    ushort val = Convert.ToUInt16(value * 10);
                    ValueWriter(6097, val);
                }
            }
        }
        protected decimal no1_chwout_range;
        /// <summary>
        /// 第二次冰水出水溫度設定
        /// </summary>
        public decimal NO2_CHWOUT_SET
        {
            get { return no2_chwout_set; }
            set
            {
                if (no2_chwout_set != value)
                {
                    no2_chwout_set = value;
                    ushort val = Convert.ToUInt16(value * 10);
                    ValueWriter(6098, val);
                }
            }
        }
        protected decimal no2_chwout_set;
        /// <summary>
        /// 第二次冰水出水溫度範圍
        /// </summary>
        public decimal NO2_CHWOUT_RANGE
        {
            get { return no2_chwout_range; }
            set
            {
                if (no2_chwout_range != value)
                {
                    no2_chwout_range = value;
                    ushort val = Convert.ToUInt16(value * 10);
                    ValueWriter(6099, val);
                }
            }
        }
        protected decimal no2_chwout_range;
        /// <summary>
        /// 第一次冰水入水溫度設定
        /// </summary>
        public decimal NO1_CHWIN_SET
        {
            get { return no1_chwin_set; }
            set
            {
                if (no1_chwin_set != value)
                {
                    no1_chwin_set = value;
                    ushort val = Convert.ToUInt16(value * 10);
                    ValueWriter(6100, val);
                }
            }
        }
        protected decimal no1_chwin_set;
        /// <summary>
        /// 第一次冰水入水溫度範圍
        /// </summary>
        public decimal NO1_CHWIN_RANGE
        {
            get { return no1_chwin_range; }
            set
            {
                if (no1_chwin_range != value)
                {
                    no1_chwin_range = value;
                    ushort val = Convert.ToUInt16(value * 10);
                    ValueWriter(6101, val);
                }
            }
        }
        protected decimal no1_chwin_range;
        /// <summary>
        /// 第二次冰水入水溫度設定
        /// </summary>
        public decimal NO2_CHWIN_SET
        {
            get { return no2_chwin_set; }
            set
            {
                if (no2_chwin_set != value)
                {
                    no2_chwin_set = value;
                    ushort val = Convert.ToUInt16(value * 10);
                    ValueWriter(6102, val);
                }
            }
        }
        protected decimal no2_chwin_set;
        /// <summary>
        /// 第二次冰水入水溫度範圍
        /// </summary>
        public decimal NO2_CHWIN_RANGE
        {
            get { return no2_chwin_range; }
            set
            {
                if (no2_chwin_range != value)
                {
                    no2_chwin_range = value;
                    ushort val = Convert.ToUInt16(value * 10);
                    ValueWriter(6103, val);
                }
            }
        }
        protected decimal no2_chwin_range;
        /// <summary>
        /// 出入水溫控選擇(0出水1入水)
        /// </summary>
        public bool WATER_SELECT_INOUT
        {
            get { return water_select_inout; }
            set
            {
                if (water_select_inout != value)
                {
                    water_select_inout = value;
                    CoilWriter(2798, value);
                }
            }
        }
        protected bool water_select_inout;
        #endregion
        #region 預約啟動 (不使用)
        /// <summary>
        /// 預約啟動時間1
        /// </summary>
        public ushort STARTTIME1
        {
            get { return starttime1; }
            set
            {
                if (starttime1 != value)
                {
                    starttime1 = value;
                    ValueWriter(6146, value);
                }
            }
        }
        protected ushort starttime1;
        /// <summary>
        /// 預約結束時間1
        /// </summary>
        public ushort STOPTIME1
        {
            get { return stoptime1; }
            set
            {
                if (stoptime1 != value)
                {
                    stoptime1 = value;
                    ValueWriter(6147, value);
                }
            }
        }
        protected ushort stoptime1;
        /// <summary>
        /// 預約啟動時間2
        /// </summary>
        public ushort STARTTIME2
        {
            get { return starttime2; }
            set
            {
                if (starttime2 != value)
                {
                    starttime2 = value;
                    ValueWriter(6148, value);
                }
            }
        }
        protected ushort starttime2;
        /// <summary>
        /// 預約結束時間2
        /// </summary>
        public ushort STOPTIME2
        {
            get { return stoptime2; }
            set
            {
                if (stoptime2 != value)
                {
                    stoptime2 = value;
                    ValueWriter(6149, value);
                }
            }
        }
        protected ushort stoptime2;
        /// <summary>
        /// 預約啟動時間3
        /// </summary>
        public ushort STARTTIME3
        {
            get { return starttime3; }
            set
            {
                if (starttime3 != value)
                {
                    starttime3 = value;
                    ValueWriter(6150, value);
                }
            }
        }
        protected ushort starttime3;
        /// <summary>
        /// 預約結束時間3
        /// </summary>
        public ushort STOPTIME3
        {
            get { return stoptime3; }
            set
            {
                if (stoptime3 != value)
                {
                    stoptime3 = value;
                    ValueWriter(6151, value);
                }
            }
        }
        protected ushort stoptime3;
        /// <summary>
        /// 預約啟動時間4
        /// </summary>
        public ushort STARTTIME4
        {
            get { return starttime4; }
            set
            {
                if (starttime4 != value)
                {
                    starttime4 = value;
                    ValueWriter(6152, value);
                }
            }
        }
        protected ushort starttime4;
        /// <summary>
        /// 預約結束時間4
        /// </summary>
        public ushort STOPTIME4
        {
            get { return stoptime4; }
            set
            {
                if (stoptime4 != value)
                {
                    stoptime4 = value;
                    ValueWriter(6153, value);
                }
            }
        }
        protected ushort stoptime4;
        /// <summary>
        /// 預約啟動時間5
        /// </summary>
        public ushort STARTTIME5
        {
            get { return starttime5; }
            set
            {
                if (starttime5 != value)
                {
                    starttime5 = value;
                    ValueWriter(6154, value);
                }
            }
        }
        protected ushort starttime5;
        /// <summary>
        /// 預約結束時間5
        /// </summary>
        public ushort STOPTIME5
        {
            get { return stoptime5; }
            set
            {
                if (stoptime5 != value)
                {
                    stoptime5 = value;
                    ValueWriter(6155, value);
                }
            }
        }
        protected ushort stoptime5;
        /// <summary>
        /// 預約啟動時間6
        /// </summary>
        public ushort STARTTIME6
        {
            get { return starttime6; }
            set
            {
                if (starttime6 != value)
                {
                    starttime6 = value;
                    ValueWriter(6156, value);
                }
            }
        }
        protected ushort starttime6;
        /// <summary>
        /// 預約結束時間6
        /// </summary>
        public ushort STOPTIME6
        {
            get { return stoptime6; }
            set
            {
                if (stoptime6 != value)
                {
                    stoptime6 = value;
                    ValueWriter(6157, value);
                }
            }
        }
        protected ushort stoptime6;
        /// <summary>
        /// 預約啟動時間7
        /// </summary>
        public ushort STARTTIME7
        {
            get { return starttime7; }
            set
            {
                if (starttime7 != value)
                {
                    starttime7 = value;
                    ValueWriter(6158, value);
                }
            }
        }
        protected ushort starttime7;
        /// <summary>
        /// 預約結束時間7
        /// </summary>
        public ushort STOPTIME7
        {
            get { return stoptime7; }
            set
            {
                if (stoptime7 != value)
                {
                    stoptime7 = value;
                    ValueWriter(6159, value);
                }
            }
        }
        protected ushort stoptime7;
        /// <summary>
        /// 預約啟動鍵(0停1動)
        /// </summary>
        public bool RESERVE_TIME_SWITCH
        {
            get { return reserve_time_switch; }
            set
            {
                if (reserve_time_switch != value)
                {
                    reserve_time_switch = value;
                    CoilWriter(2753, value);
                }
            }
        }
        protected bool reserve_time_switch;
        #endregion
        #region 系統警報
        /// <summary>
        /// 冰水水溫過低警報(0正常1異常)
        /// </summary>
        public bool CW_TEMP_LOW
        {
            get { return cw_temp_low; }
            protected set
            {
                if (cw_temp_low != value)
                {
                    cw_temp_low = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2848, $"冰水溫度過低警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r冰水溫度過低警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool cw_temp_low;
        /// <summary>
        /// 冰水流動開關警報
        /// </summary>
        public bool CHW_FLOW_FLT
        {
            get { return chw_flow_flt; }
            protected set
            {
                if (chw_flow_flt != value)
                {
                    chw_flow_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2849, $"冰水流動開關警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r冰水流動開關警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool chw_flow_flt;
        /// <summary>
        /// 冷卻水流動開關警報
        /// </summary>
        public bool CW_FLOW_FLT
        {
            get { return cw_flow_flt; }
            protected set
            {
                if (cw_flow_flt != value)
                {
                    cw_flow_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2850, $"冷卻水流動開關警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r冷卻水流動開關警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool cw_flow_flt;
        /// <summary>
        /// 電源逆向警報
        /// </summary>
        public bool POWER_BACK_FLT
        {
            get { return power_back_flt; }
            protected set
            {
                if (power_back_flt != value)
                {
                    power_back_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2851, $"電源逆向警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r電源逆向警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool power_back_flt;
        /// <summary>
        /// 防凍開關警報
        /// </summary>
        public bool FROZEN_SWITCH_FLT
        {
            get { return frozen_swtich_flt; }
            protected set
            {
                if (frozen_swtich_flt != value)
                {
                    frozen_swtich_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2852, $"防凍開關警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r防凍開關警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool frozen_swtich_flt;
        /// <summary>
        /// 壓差開關警報
        /// </summary>
        public bool DELTA_PRESS_FLT
        {
            get { return delta_press_flt; }
            protected set
            {
                if (delta_press_flt != value)
                {
                    delta_press_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2853, $"壓差開關警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r壓差開關警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool delta_press_flt;
        /// <summary>
        /// 停斷電警報
        /// </summary>
        public bool POWER_BREAK_FLT
        {
            get { return power_break_flt; }
            protected set
            {
                if (power_break_flt != value)
                {
                    power_break_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2854, $"停斷電警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r停斷電警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool power_break_flt;
        /// <summary>
        /// 機1高壓警報
        /// </summary>
        public bool CH1_HIGHPRESS_FLT
        {
            get { return ch1_highpress_flt; }
            protected set
            {
                if (ch1_highpress_flt != value)
                {
                    ch1_highpress_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2855, $"機1高壓警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r機1高壓警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool ch1_highpress_flt;
        /// <summary>
        /// 機1低壓警報
        /// </summary>
        public bool CH1_LOWPRESS_FLT
        {
            get { return ch1_lowpress_flt; }
            protected set
            {
                if (ch1_lowpress_flt != value)
                {
                    ch1_lowpress_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2856, $"機1低壓警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r機1低壓警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool ch1_lowpress_flt;
        /// <summary>
        /// 機1過熱警報
        /// </summary>
        public bool CH1_OVERHEAT_FLT
        {
            get { return ch1_overheat_flt; }
            protected set
            {
                if (ch1_overheat_flt != value)
                {
                    ch1_overheat_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2857, $"機1過熱警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r機1過熱警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool ch1_overheat_flt;
        /// <summary>
        /// 機1過載警報
        /// </summary>
        public bool CH1_OVERLOAD_FLT
        {
            get { return ch1_overload_flt; }
            protected set
            {
                if (ch1_overload_flt != value)
                {
                    ch1_overload_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2858, $"機1過載警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r機1過載警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool ch1_overload_flt;
        /// <summary>
        /// 機1油位警報
        /// </summary>
        public bool CH1_OILLEVEL_FLT
        {
            get { return ch1_overlevel_flt; }
            protected set
            {
                if (ch1_overlevel_flt != value)
                {
                    ch1_overlevel_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2859, $"機1油位警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r機1油位警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool ch1_overlevel_flt;
        /// <summary>
        /// 機1超時運轉警報
        /// </summary>
        public bool CH1_OVERTIME_FLT
        {
            get { return ch1_overtime_flt; }
            protected set
            {
                if (ch1_overtime_flt != value)
                {
                    ch1_overtime_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2860, $"機1超時運轉警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r機1超時運轉警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool ch1_overtime_flt;
        /// <summary>
        /// 機2高壓警報
        /// </summary>
        public bool CH2_HIGHPRESS_FLT
        {
            get { return ch2_highpress_flt; }
            protected set
            {
                if (ch2_highpress_flt != value)
                {
                    ch2_highpress_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2861, $"機2高壓警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r機2高壓警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool ch2_highpress_flt;
        /// <summary>
        /// 機2低壓警報
        /// </summary>
        public bool CH2_LOWPRESS_FLT
        {
            get { return ch2_lowpress_flt; }
            protected set
            {
                if (ch2_lowpress_flt != value)
                {
                    ch2_lowpress_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2862, $"機2低壓警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r機2低壓警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool ch2_lowpress_flt;
        /// <summary>
        /// 機2過熱警報
        /// </summary>
        public bool CH2_OVERHEAT_FLT
        {
            get { return ch2_overheat_flt; }
            protected set
            {
                if (ch2_overheat_flt != value)
                {
                    ch2_overheat_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2863, $"機2過熱警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r機2過熱警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool ch2_overheat_flt;
        /// <summary>
        /// 機2過載警報
        /// </summary>
        public bool CH2_OVERLOAD_FLT
        {
            get { return ch2_overload_flt; }
            protected set
            {
                if (ch2_overload_flt != value)
                {
                    ch2_overload_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2864, $"機2過載警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r機2過載警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool ch2_overload_flt;
        /// <summary>
        /// 機2油位警報
        /// </summary>
        public bool CH2_OILLEVEL_FLT
        {
            get { return ch2_overlevel_flt; }
            protected set
            {
                if (ch2_overlevel_flt != value)
                {
                    ch2_overlevel_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2865, $"機2油位警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r機2油位警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool ch2_overlevel_flt;
        /// <summary>
        /// 機2超時運轉警報
        /// </summary>
        public bool CH2_OVERTIME_FLT
        {
            get { return ch2_overtime_flt; }
            protected set
            {
                if (ch2_overtime_flt != value)
                {
                    ch2_overtime_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2866, $"機2超時運轉警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r機2超時運轉警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool ch2_overtime_flt;
        /// <summary>
        /// 冰水出水警報
        /// </summary>
        public bool CH_WATEROUT_FLT
        {
            get { return ch_waterout_flt; }
            protected set
            {
                if (ch_waterout_flt != value)
                {
                    ch_waterout_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2867, $"冰水出水警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r冰水出水警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool ch_waterout_flt;
        /// <summary>
        /// 冰水入水警報
        /// </summary>
        public bool CH_WATERIN_FLT
        {
            get { return chw_waterin_flt; }
            protected set
            {
                if (chw_waterin_flt != value)
                {
                    chw_waterin_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2868, $"冰水入水警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r冰水入水警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool chw_waterin_flt;
        /// <summary>
        /// 冰水溫差過大警報
        /// </summary>
        public bool CHT_DELTA_FLT
        {
            get { return cht_delta_flt; }
            protected set
            {
                if (cht_delta_flt != value)
                {
                    cht_delta_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2869, $"冰水溫差過大警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r冰水溫差過大警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool cht_delta_flt;
        /// <summary>
        /// 冷卻水溫差過大警報
        /// </summary>
        public bool CWT_DELTA_FLT
        {
            get { return cwt_delta_flt; }
            protected set
            {
                if (cwt_delta_flt != value)
                {
                    cwt_delta_flt = value;
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2870, $"冷卻水溫差過大警報[{(value ? "異常觸發" : "恢復正常")}]");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r冷卻水溫差過大警報[{(value ? "異常觸發" : "恢復正常")}]");
                    }
                }
            }
        }
        protected bool cwt_delta_flt;
        /// <summary>
        /// 異常復歸(0停1動)
        /// </summary>
        public bool ERR_RECOVER_ST
        {
            get { return err_recover_st; }
            set
            {
                if (err_recover_st != value)
                {
                    err_recover_st = value;
                    CoilWriter(2448, value);
                    if (FirstReadFlag)
                    {
                        SqlMethod.InserterChillerState(DeviceSetting,2448, $"\r設備名稱:{DeviceSetting.DeviceName}\r異常復歸[{(value ? "啟動" : "停止")}]");
                    }
                }
            }
        }
        protected bool err_recover_st;
        #endregion
        #region DO輸出
        /// <summary>
        /// 冰水泵(0停1動)
        /// </summary>
        public bool CHP_RUN_ST { get; protected set; }
        /// <summary>
        /// 冷卻水泵(0停1動)
        /// </summary>
        public bool CWP_RUN_ST { get; protected set; }
        /// <summary>
        /// 壓縮機1(0停1動)
        /// </summary>
        public bool CH1_RUN_ST { get; protected set; }
        /// <summary>
        /// 壓縮機2(0停1動)
        /// </summary>
        public bool CH2_RUN_ST { get; protected set; }
        /// <summary>
        /// 壓縮機1異常燈(0正1異)
        /// </summary>
        public bool CH1_ERR_ST { get; protected set; }
        /// <summary>
        /// 壓縮機2異常燈(0正1異)
        /// </summary>
        public bool CH2_ERR_ST { get; protected set; }
        #endregion
    }
}
