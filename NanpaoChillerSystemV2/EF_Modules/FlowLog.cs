using System;

namespace NanpaoChillerSystemV2.EF_Modules
{
    public class FlowLog
    {
        public string ttime { get; set; }
        public DateTime ttimen { get; set; }
        public int GatewayIndex { get; set; }
        public int DeviceIndex { get; set; }
        public decimal Flow { get; set; }
        public decimal FlowTotal { get; set; }
        public decimal TempDifference { get; set; }
        public decimal InputTemp { get; set; }
        public decimal OutputTemp { get; set; }
        public bool SwitchFlag { get; set; }
    }
}
