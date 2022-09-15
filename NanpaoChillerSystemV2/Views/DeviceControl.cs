using NanpaoChillerSystemV2.EF_Modules;
using NanpaoChillerSystemV2.Enums;
using NanpaoChillerSystemV2.Protocols;
using System.Collections.Generic;
using System.Linq;

namespace NanpaoChillerSystemV2.Views
{
    public partial class DeviceControl : DevExpress.XtraEditors.XtraUserControl
    {
        private List<AbsProtocol> AbsProtocols { get; set; }
        private DeviceSetting DeviceSetting { get; set; }
        private DeviceType DeviceType { get; set; }
        public DeviceControl(List<AbsProtocol> absProtocols, DeviceSetting deviceSetting)
        {
            InitializeComponent();
            AbsProtocols = absProtocols;
            DeviceSetting = deviceSetting;
            DeviceType = (DeviceType)DeviceSetting.DeviceType;
            gcl_Device.Text = DeviceSetting.DeviceName;
        }
        public void TextChange()
        {
            var absprotocol = AbsProtocols.SingleOrDefault(g => g.DeviceIndex == DeviceSetting.DeviceIndex & g.GatewayIndex == DeviceSetting.GatewayIndex);
            if (absprotocol != null)
            {
                if (absprotocol.ConnectionFlag)
                {
                    switch (DeviceType)
                    {
                        case DeviceType.DaTongChiller:
                            {
                                ChillerData data = absprotocol as ChillerData;
                                lbl_Time.Text = $"{data.LastReadTime:yyyy-MM-dd HH:mm:ss}";
                            }
                            break;
                        case DeviceType.PA330:
                            {
                                ElectricData data = absprotocol as ElectricData;
                                lbl_Time.Text = $"{data.LastReadTime:yyyy-MM-dd HH:mm:ss}";
                            }
                            break;
                        case DeviceType.PA3000:
                            {
                                ElectricData data = absprotocol as ElectricData;
                                lbl_Time.Text = $"{data.LastReadTime:yyyy-MM-dd HH:mm:ss}";
                            }
                            break;
                        case DeviceType.UF300:
                            {
                                FlowData data = absprotocol as FlowData;
                                lbl_Time.Text = $"{data.LastReadTime:yyyy-MM-dd HH:mm:ss}";
                            }
                            break;
                        case DeviceType.TR:
                            {
                                SenserData data = absprotocol as SenserData;
                                lbl_Time.Text = $"{data.LastReadTime:yyyy-MM-dd HH:mm:ss}";
                            }
                            break;
                    }
                    stateIndicatorComponent1.StateIndex = 3;
                }
                else
                {
                    stateIndicatorComponent1.StateIndex = 1;
                }
            }
        }
    }
}
