using NanpaoChillerSystemV2.Configuration;
using NanpaoChillerSystemV2.EF_Modules;
using NanpaoChillerSystemV2.Enums;
using NanpaoChillerSystemV2.Methods;
using NanpaoChillerSystemV2.Protocols;
using NModbus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;

namespace NanpaoChillerSystemV2.Components
{
    public class Field4Component : Component
    {
        /// <summary>
        /// 上傳小E資訊
        /// </summary>
        public UpToEwatchSetting UpToEwatchSetting { get; set; }
        /// <summary>
        /// 資料庫方法
        /// </summary>
        public SqlMethod SqlMethod { get; set; }
        /// <summary>
        /// 小E上傳
        /// </summary>
        public UptoEwatchMethod UptoEwatchMethod { get; set; } = new UptoEwatchMethod();
        /// <summary>
        /// Slave資訊
        /// </summary>
        public SlaveSetting SlaveSetting { get; set; }
        /// <summary>
        /// 通道資訊
        /// </summary>
        public GatewaySetting GatewaySetting { get; set; }
        /// <summary>
        /// LINE推播
        /// </summary>
        public List<LineNotifySetting> LineNotifySettings { get; set; }
        /// <summary>
        /// 設備資訊
        /// </summary>
        public List<DeviceSetting> DeviceSettings { get; set; }
        /// <summary>
        /// 通訊資訊
        /// </summary>
        public List<AbsProtocol> AbsProtocols { get; set; } = new List<AbsProtocol>();
        /// <summary>
        /// 設備類型
        /// </summary>
        public DeviceType DeviceType { get; set; }
        /// <summary>
        /// 通訊 COM/IP
        /// </summary>
        public string Location
        {
            get
            {
                if (GatewaySetting != null)
                {
                    string data = GatewaySetting.Connect.Split(',')[0];
                    return data;
                }
                else
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// 通訊 Port/Rate
        /// </summary>
        public int Port
        {
            get
            {
                if (GatewaySetting != null)
                {
                    int data = Convert.ToInt32(GatewaySetting.Connect.Split(',')[1]);
                    return data;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 通訊執行緒
        /// </summary>
        public Thread ReadThread { get; set; }
        /// <summary>
        /// 最後讀取時間
        /// </summary>
        public DateTime ReadTime { get; set; }
        #region Nmodbus物件
        /// <summary>
        /// 通訊建置類別(通用)
        /// </summary>
        public ModbusFactory Factory { get; set; }

        #region Master
        /// <summary>
        /// 通訊物件
        /// </summary>
        public IModbusMaster master { get; set; }
        #endregion

        #region Slave
        /// <summary>
        /// Slave物件 (若要多個Slaver請不要加入在這Field4Component，請在SlaveComponent內加入)
        /// </summary>
        public IModbusSlave slave;
        /// <summary>
        /// 總Slave物件 (List類型，可以加入多個 IModbusSlave物件)
        /// </summary>
        public IModbusSlaveNetwork network;
        /// <summary>
        /// IP連線通訊
        /// </summary>
        public TcpListener slaveTcpListener;
        #endregion

        #endregion
        #region 初始設定
        public Field4Component()
        {
            OnMyWorkStateChanged += new MyWorkStateChanged(AfterMyWorkStateChanged);
        }
        /// <summary>
        /// 系統工作路徑
        /// </summary>
        protected readonly string WorkPath = AppDomain.CurrentDomain.BaseDirectory;
        public delegate void MyWorkStateChanged(object sender, EventArgs e);
        public event MyWorkStateChanged OnMyWorkStateChanged;
        /// <summary>
        /// 通訊功能啟動判斷旗標
        /// </summary>
        protected bool myWorkState;
        /// <summary>
        /// 通訊功能啟動旗標
        /// </summary>
        public bool MyWorkState
        {
            get { return myWorkState; }
            set
            {
                if (value != myWorkState)
                {
                    myWorkState = value;
                    WhenMyWorkStateChange();
                }
            }
        }
        /// <summary>
        /// 執行續工作狀態改變觸發事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void AfterMyWorkStateChanged(object sender, EventArgs e) { }
        protected void WhenMyWorkStateChange()
        {
            OnMyWorkStateChanged?.Invoke(this, null);
        }
        #endregion
    }
}
