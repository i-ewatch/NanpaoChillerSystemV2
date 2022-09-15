namespace NanpaoChillerSystemV2.EF_Modules
{
    public class ChillerStateLog
    {
        public string ttime { get; set; }
        public System.DateTime ttimen { get; set; }
        public int GatewayIndex { get; set; }
        public int DeviceIndex { get; set; }
        public int Address { get; set; }
        public string Message { get; set; }
    }
}
