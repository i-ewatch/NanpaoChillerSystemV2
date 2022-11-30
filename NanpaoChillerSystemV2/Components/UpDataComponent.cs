using NanpaoChillerSystemV2.Configuration;
using NanpaoChillerSystemV2.Enums;
using NanpaoChillerSystemV2.Methods;
using NanpaoChillerSystemV2.Protocols;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace NanpaoChillerSystemV2.Components
{
    public partial class UpDataComponent : Field4Component
    {
        private int MinuteIndex = -1;
        private bool FirstTime = false;
        private bool CheckDataBase
        {
            get
            {
                bool Flag = false;
                if (DateTime.Now.ToString("HHmm") == "2359" || !FirstTime)
                {
                    Flag = true;
                }
                return Flag;
            }
        }
        public UpDataComponent(List<AbsProtocol> absProtocols, SqlMethod sqlMethod, UpToEwatchSetting upToEwatchSetting)
        {
            InitializeComponent();
            AbsProtocols = absProtocols;
            SqlMethod = sqlMethod;
            UpToEwatchSetting = upToEwatchSetting;
            UptoEwatchMethod = new UptoEwatchMethod();
        }

        public UpDataComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        protected override void AfterMyWorkStateChanged(object sender, EventArgs e)
        {
            if (myWorkState)
            {
                ReadThread = new Thread(Analysis);
                ReadThread.Start();
            }
            else
            {
                if (ReadThread != null)
                {
                    ReadThread.Abort();
                }
            }
        }
        private void Analysis()
        {
            while (myWorkState)
            {
                if (DateTime.Now.Minute != MinuteIndex)
                {
                    if (CheckDataBase)
                    {
                        SqlMethod.CreateDataBase();
                        FirstTime = true;
                    }
                    foreach (var item in AbsProtocols)
                    {
                        try
                        {
                            if (item.ConnectionFlag)
                            {
                                DeviceType = (DeviceType)item.DeviceSetting.DeviceType;
                                switch (DeviceType)
                                {
                                    case DeviceType.DaTongChiller:
                                        {
                                            ChillerData data = item as ChillerData;
                                            SqlMethod.InserterChillerLog(data);
                                        }
                                        break;
                                    case DeviceType.PA330:
                                        {
                                            ElectricData data = item as ElectricData;
                                            SqlMethod.InsertElectric(data);
                                            SqlMethod.InsertElectricTotal(data);
                                            if (UpToEwatchSetting.SendFlag & !string.IsNullOrEmpty(data.DeviceSetting.CardNo) & !string.IsNullOrEmpty(data.DeviceSetting.BoardNo))
                                            {
                                                UptoEwatchMethod.scan_adioco(data.DeviceSetting.CardNo, data.DeviceSetting.BoardNo,
                                                new int[8], new byte[8], new byte[8],
                                                Convert.ToSingle(data.RSV), Convert.ToSingle(data.STV), Convert.ToSingle(data.TRV), Convert.ToSingle(data.RA), Convert.ToSingle(data.SA), Convert.ToSingle(data.TA), Convert.ToSingle(data.KW), Convert.ToSingle(data.KWH), Convert.ToSingle(data.PFE), Convert.ToSingle(data.KVAR), Convert.ToSingle(data.KVARH), UpToEwatchSetting.IP, UpToEwatchSetting.Port);
                                            }
                                        }
                                        break;
                                    case DeviceType.PA3000:
                                        {
                                            ElectricData data = item as ElectricData;
                                            SqlMethod.InsertElectric(data);
                                            SqlMethod.InsertElectricTotal(data);
                                            if (UpToEwatchSetting.SendFlag & !string.IsNullOrEmpty(data.DeviceSetting.CardNo) & !string.IsNullOrEmpty(data.DeviceSetting.BoardNo))
                                            {
                                                UptoEwatchMethod.scan_adioco(data.DeviceSetting.CardNo, data.DeviceSetting.BoardNo,
                                                new int[8], new byte[8], new byte[8],
                                                Convert.ToSingle(data.RSV), Convert.ToSingle(data.STV), Convert.ToSingle(data.TRV), Convert.ToSingle(data.RA), Convert.ToSingle(data.SA), Convert.ToSingle(data.TA), Convert.ToSingle(data.KW), Convert.ToSingle(data.KWH), Convert.ToSingle(data.PFE), Convert.ToSingle(data.KVAR), Convert.ToSingle(data.KVARH), UpToEwatchSetting.IP, UpToEwatchSetting.Port);
                                            }
                                        }
                                        break;
                                    case DeviceType.UF300:
                                        {
                                            FlowData data = item as FlowData;
                                            if (string.IsNullOrEmpty(data.DeviceSetting.ChillerIndex))
                                            {
                                                SqlMethod.InsertFlow(data);
                                            }
                                            else
                                            {
                                                ChillerProtocol chiller = AbsProtocols.SingleOrDefault(g => g.GatewayIndex == data.GatewayIndex & g.DeviceIndex == Convert.ToInt32(data.DeviceSetting.ChillerIndex)) as ChillerProtocol;
                                                SqlMethod.InsertFlow(data, chiller);
                                            }
                                            SqlMethod.InsertFlowTotal(data);
                                        }
                                        break;
                                    case DeviceType.TR:
                                        {
                                            SenserData data = item as SenserData;
                                            SqlMethod.InserterTRLog(data);
                                        }
                                        break;
                                }
                                MinuteIndex = DateTime.Now.Minute;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex, $"上傳或更新資料庫失敗");
                        }
                    }
                }
                else
                {
                    Thread.Sleep(80);
                }
            }
        }
    }
}
