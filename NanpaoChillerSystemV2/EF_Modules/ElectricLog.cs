using System;

namespace NanpaoChillerSystemV2.EF_Modules
{
    public class ElectricLog
    {
        public string ttime { get; set; }
        public Nullable<System.DateTime> ttimen { get; set; }
        public int GatewayIndex { get; set; }
        public int DeviceIndex { get; set; }
        public double trv { get; set; }
        public double tsv { get; set; }
        public double ttv { get; set; }
        public double tri { get; set; }
        public double tsi { get; set; }
        public double tti { get; set; }
        public double tpre { get; set; }
        public double tkw { get; set; }
        public double tkwh { get; set; }
    }
}
