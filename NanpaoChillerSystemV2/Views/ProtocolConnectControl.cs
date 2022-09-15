using NanpaoChillerSystemV2.EF_Modules;
using NanpaoChillerSystemV2.Protocols;
using System.Collections.Generic;
using System.Drawing;

namespace NanpaoChillerSystemV2.Views
{
    public partial class ProtocolConnectControl : Field4Control
    {
        private List<AbsProtocol> AbsProtocols { get; set; }
        private List<DeviceSetting> DeviceSettings { get; set; }
        private List<DeviceControl> DeviceControls { get; set; } = new List<DeviceControl>();
        private int ViewIndex { get; set; }
        public ProtocolConnectControl(List<AbsProtocol> absProtocols, List<DeviceSetting> deviceSettings)
        {
            InitializeComponent();
            AbsProtocols = absProtocols;
            DeviceSettings = deviceSettings;
            foreach (var item in deviceSettings)
            {
                DeviceControl control = new DeviceControl(absProtocols, item) { Location = new Point(5 + 280 * (ViewIndex % 4), 5 + 80 * (ViewIndex / 4)) };
                DeviceControls.Add(control);
                xtraScrollableControl1.Controls.Add(control);
                ViewIndex++;
            }
        }
        public override void TextChange()
        {
            foreach (var item in DeviceControls)
            {
                item.TextChange();
            }
        }
    }
}
