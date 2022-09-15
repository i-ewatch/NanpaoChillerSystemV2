using System;

namespace NanpaoChillerSystemV2.EF_Modules
{
    public class Flowtotal
    {
        public string ttime { get; set; }
        public DateTime ttimen { get; set; }
        public int GatewayIndex { get; set; }
        public int DeviceIndex { get; set; }
        public decimal InputTempAvg { get; set; }
        public decimal OutputTempAvg { get; set; }
        public decimal TempDifferenceAvg { get; set; }
        public decimal FlowAvg { get; set; }
        public decimal RTH { get; set; }
        public decimal FlowStart1 { get; set; }
        public decimal FlowEnd1 { get; set; }
        public decimal FlowStart2 { get; set; }
        public decimal FlowEnd2 { get; set; }
        public decimal FlowTotal { get; set; }
    }
}
