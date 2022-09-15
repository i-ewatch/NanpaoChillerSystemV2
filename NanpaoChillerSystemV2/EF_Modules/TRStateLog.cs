using System;

namespace NanpaoChillerSystemV2.EF_Modules
{
    public class TRStateLog
    {
        public string ttime { get; set; }
        public DateTime ttimen { get; set; }
        public int GatewayIndex { get; set; }
        public int DeviceIndex { get; set; }
        public string Message { get; set; }
    }
}
