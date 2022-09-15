using System;

namespace NanpaoChillerSystemV2.EF_Modules
{
    public class TRLog
    {
        public string ttime { get; set; }
        public DateTime ttimen { get; set; }
        public int GatewayIndex { get; set; }
        public int DeviceIndex { get; set; }
        /// <summary>
        /// 環境溫度
        /// </summary>
        public decimal Temp { get; set; }
        /// <summary>
        /// 變壓器溫度
        /// </summary>
        public decimal Temp1 { get; set; }
    }
}
