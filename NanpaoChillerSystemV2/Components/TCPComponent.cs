using NanpaoChillerSystemV2.EF_Modules;
using NanpaoChillerSystemV2.Enums;
using NanpaoChillerSystemV2.Methods;
using NanpaoChillerSystemV2.Protocols;
using NModbus;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;

namespace NanpaoChillerSystemV2.Components
{
    public partial class TCPComponent : Field4Component
    {
        public TCPComponent(GatewaySetting gatewaySetting, List<DeviceSetting> deviceSettings, SqlMethod sqlMethod, List<LineNotifySetting> lineNotifySettings)
        {
            InitializeComponent();
            GatewaySetting = gatewaySetting;
            DeviceSettings = deviceSettings;
            SqlMethod = sqlMethod;
            LineNotifySettings = lineNotifySettings;
        }

        public TCPComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        protected override void AfterMyWorkStateChanged(object sender, EventArgs e)
        {
            if (myWorkState)
            {
                foreach (var item in DeviceSettings)
                {
                    DeviceType = (DeviceType)item.DeviceType;
                    switch (DeviceType)
                    {
                        case DeviceType.DaTongChiller:
                            {
                                ChillerProtocol protocol = new ChillerProtocol(GatewaySetting, item, SqlMethod, LineNotifySettings) { DeviceIndex = item.DeviceIndex, GatewayIndex = item.GatewayIndex, ID = (byte)item.DeviceID };
                                AbsProtocols.Add(protocol);
                            }
                            break;
                        case DeviceType.PA330:
                            {
                                PA330Protocol protocol = new PA330Protocol(GatewaySetting, item, SqlMethod, LineNotifySettings) { DeviceIndex = item.DeviceIndex, GatewayIndex = item.GatewayIndex, ID = (byte)item.DeviceID };
                                AbsProtocols.Add(protocol);
                            }
                            break;
                        case DeviceType.PA3000:
                            {
                                PA3000Protocol protocol = new PA3000Protocol(GatewaySetting, item, SqlMethod, LineNotifySettings) { DeviceIndex = item.DeviceIndex, GatewayIndex = item.GatewayIndex, ID = (byte)item.DeviceID };
                                AbsProtocols.Add(protocol);
                            }
                            break;
                        case DeviceType.UF300:
                            {
                                UF300Protocol protocol = new UF300Protocol(GatewaySetting, item, SqlMethod, LineNotifySettings) { DeviceIndex = item.DeviceIndex, GatewayIndex = item.GatewayIndex, ID = (byte)item.DeviceID };
                                AbsProtocols.Add(protocol);
                            }
                            break;
                        case DeviceType.TR:
                            {
                                TRProtocol protocol = new TRProtocol(GatewaySetting, item, SqlMethod, LineNotifySettings) { DeviceIndex = item.DeviceIndex, GatewayIndex = item.GatewayIndex, ID = (byte)item.DeviceID };
                                AbsProtocols.Add(protocol);
                            }
                            break;
                    }
                }
                Factory = new ModbusFactory();
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
        protected void Analysis()
        {
            while (myWorkState)
            {
                TimeSpan timeSpan = DateTime.Now.Subtract(ReadTime);
                if (timeSpan.TotalSeconds >= 1)
                {
                    foreach (var item in AbsProtocols)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(GatewaySetting.Connect))
                            {
                                using (TcpClient client = new TcpClient(Location, Port))
                                {
                                    master = Factory.CreateMaster(client);//建立TCP通訊
                                    master.Transport.Retries = 0;
                                    master.Transport.ReadTimeout = 2000;
                                    master.Transport.WriteTimeout = 2000;
                                    item.DataReader(master);
                                    Thread.Sleep(10);
                                };
                            }
                            ReadTime = DateTime.Now;
                        }
                        catch (ThreadAbortException) { }
                        catch (Exception ex)
                        {
                            ReadTime = DateTime.Now;
                            Log.Error(ex, $"通訊失敗 IP:{Location} Port:{Port} ");
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
