using NanpaoChillerSystemV2.Configuration;
using NanpaoChillerSystemV2.Enums;
using NanpaoChillerSystemV2.Protocols;
using NModbus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NanpaoChillerSystemV2.Components
{
    public partial class SlaveTCPComponent : Field4Component
    {
        private bool FirstFlag { get; set; } = false;
        private int ChillerNum { get; set; } = 0;
        private int TRNum { get; set; } = 0;
        private int ElectricNum { get; set; } = 0;
        private int FlowNum { get; set; } = 0;
        public SlaveTCPComponent(SlaveSetting slaveSetting, List<AbsProtocol> absProtocols)
        {
            InitializeComponent();
            SlaveSetting = slaveSetting;
            AbsProtocols = absProtocols;
        }

        public SlaveTCPComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        protected override void AfterMyWorkStateChanged(object sender, EventArgs e)
        {
            if (myWorkState)
            {
                IPAddress address = new IPAddress(new byte[] { Convert.ToByte(SlaveSetting.SlaveIP.Split('.')[0]), Convert.ToByte(SlaveSetting.SlaveIP.Split('.')[1]), Convert.ToByte(SlaveSetting.SlaveIP.Split('.')[2]), Convert.ToByte(SlaveSetting.SlaveIP.Split('.')[3]) });
                slaveTcpListener = new TcpListener(address, SlaveSetting.SlavePort);
                slaveTcpListener.Start();//通道打開
                Factory = new ModbusFactory();
                network = Factory.CreateSlaveNetwork(slaveTcpListener);
                network.ListenAsync();//開始側聽使用
                slave = Factory.CreateSlave(1);//設定ID
                network.AddSlave(slave);//開啟通訊 (每個Function 都開到最大 65535 無法修改)
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
                TimeSpan timeSpan = DateTime.Now.Subtract(ReadTime);
                if (timeSpan.TotalSeconds >= 1)
                {
                    ChillerNum = 0;
                    ElectricNum = 0;
                    FlowNum = 0;
                    TRNum = 0;
                    int k = 0;
                    foreach (var item in AbsProtocols)
                    {
                        DeviceType = (DeviceType)item.DeviceSetting.DeviceType;
                        switch (DeviceType)
                        {
                            case DeviceType.DaTongChiller:
                                {
                                    slave.DataStore.CoilInputs.WritePoints(Convert.ToUInt16(0 + (32 * ChillerNum)), item.SlaveFun2.ToArray());
                                    slave.DataStore.InputRegisters.WritePoints(Convert.ToUInt16(0 + (20 * ChillerNum)), item.SlaveFun4.ToArray());
                                    ChillerNum++;
                                }
                                break;
                            case DeviceType.PA330:
                                {
                                    slave.DataStore.InputRegisters.WritePoints(Convert.ToUInt16(300 + (18 * ElectricNum)), item.SlaveFun4.ToArray());
                                    ElectricNum++;
                                }
                                break;
                            case DeviceType.PA3000:
                                {
                                    slave.DataStore.InputRegisters.WritePoints(Convert.ToUInt16(300 + (18 * ElectricNum)), item.SlaveFun4.ToArray());
                                    ElectricNum++;
                                }
                                break;
                            case DeviceType.UF300:
                                {
                                    slave.DataStore.InputRegisters.WritePoints(Convert.ToUInt16(100 + (10 * FlowNum)), item.SlaveFun4.ToArray());
                                    FlowNum++;
                                }
                                break;
                            case DeviceType.TR:
                                {
                                    slave.DataStore.InputRegisters.WritePoints(Convert.ToUInt16(200 + (2 * TRNum)), item.SlaveFun4.ToArray());
                                    if (!FirstFlag)
                                    {
                                        slave.DataStore.HoldingRegisters.WritePoints(Convert.ToUInt16(200 + (4 * TRNum)), item.SlaveFun3.ToArray());
                                    }
                                    TRNum++;
                                }
                                break;
                        }
                    }
                    FirstFlag = true;
                    ushort[] SlaveFun3 = slave.DataStore.HoldingRegisters.ReadPoints(200, 12);
                    var TRProtocol = AbsProtocols.Where(g => g.DeviceSetting.DeviceType == 4).ToList();
                    if (TRProtocol != null)
                    {
                        foreach (var item in TRProtocol)
                        {
                            SenserData senserData = item as SenserData;
                            senserData.TempMax = Convert.ToDecimal(SlaveFun3[k] * 0.1F);
                            senserData.TempMin = Convert.ToDecimal(SlaveFun3[k + 1] * 0.1F);
                            senserData.TempMax1 = Convert.ToDecimal(SlaveFun3[k + 2] * 0.1F);
                            senserData.TempMin1 = Convert.ToDecimal(SlaveFun3[k + 3] * 0.1F);
                            k += 4;
                        }
                    }
                    ReadTime = DateTime.Now;
                }
                else
                {
                    Thread.Sleep(80);
                }
            }
        }
    }
}
