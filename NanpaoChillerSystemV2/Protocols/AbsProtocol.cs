using LineNotifyLibrary;
using MathLibrary;
using NanpaoChillerSystemV2.EF_Modules;
using NanpaoChillerSystemV2.Methods;
using NanpaoChillerSystemV2.Modules;
using NModbus;
using System;
using System.Collections.Generic;

namespace NanpaoChillerSystemV2.Protocols
{
    public abstract class AbsProtocol
    {
        public List<ushort> SlaveFun3 { get; set; } = new List<ushort>();
        public List<ushort> SlaveFun4 { get; set; } = new List<ushort>();
        public List<bool> SlaveFun1 { get; set; } = new List<bool>();
        public List<bool> SlaveFun2 { get; set; } = new List<bool>();
        /// <summary>
        /// 資料庫方法
        /// </summary>
        public SqlMethod SqlMethod { get; set; }
        /// <summary>
        /// 通道資訊
        /// </summary>
        public GatewaySetting GatewaySetting { get; set; }
        /// <summary>
        /// 設備資訊
        /// </summary>
        public DeviceSetting DeviceSetting { get; set; }
        /// <summary>
        /// Line推播資訊
        /// </summary>
        public List<LineNotifySetting> LineNotifySettings { get; set; }
        /// <summary>
        /// 通道編碼
        /// </summary>
        public int GatewayIndex { get; set; }
        /// <summary>
        /// 設備編碼
        /// </summary>
        public int DeviceIndex { get; set; }
        /// <summary>
        /// 設備站號
        /// </summary>
        public byte ID { get; set; }
        /// <summary>
        /// 數學運算函式實體化
        /// </summary>
        protected MathClass Calculate = new MathClass();
        /// <summary>
        /// 連線旗標
        /// </summary>
        public bool ConnectionFlag { get; protected set; } = false;
        /// <summary>
        /// 第一次讀取旗標
        /// </summary>
        protected bool FirstReadFlag { get; set; } = false;
        /// <summary>
        /// 最後讀取時間
        /// </summary>
        public DateTime LastReadTime { get; protected set; } = Convert.ToDateTime("1900/01/01 00:00:00");
        /// <summary>
        /// 狀態寫入
        /// </summary>
        public List<WriteCoilObject> WriteCoils { get; set; } = new List<WriteCoilObject>();
        /// <summary>
        /// 數值寫入
        /// </summary>
        public List<WriteValueObject> WriteValues { get; set; } = new List<WriteValueObject>();
        /// <summary>
        /// 寫入狀態旗標
        /// </summary>
        public bool WriteFlag
        {
            get
            {
                if (WriteCoils.Count > 0 || WriteValues.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 資料讀取方法
        /// </summary>
        public abstract void DataReader(IModbusMaster master);
        /// <summary>
        /// 狀態紀錄
        /// </summary>
        /// <param name="actionName">動作/寫入的名稱</param>
        /// <param name="status">當前狀態</param>
        /// <param name="statusString">紀錄的字串</param>
        public abstract void StateLogCreater(ushort Address, string Message);
        /// <summary>
        /// 寫入狀態
        /// </summary>
        public abstract void CoilWriter(ushort addr, bool status);
        /// <summary>
        /// 寫入數值
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="value"></param>
        public abstract void ValueWriter(ushort addr, ushort value);
        /// <summary>
        /// 告警發送
        /// </summary>
        public void AlarmNotifySender(string message)
        {
            foreach (var item in LineNotifySettings)
            {
                if (item.SendFlag && item.Token != "" && DeviceSetting.LineIndex.Contains($"{item.LineIndex}"))
                {
                    using (LineNotifyClass lineNotifyClass = new LineNotifyClass(item.Token))
                    {
                        lineNotifyClass.LineNotifyFunction(message);
                    }
                }
            }
        }
    }
}
