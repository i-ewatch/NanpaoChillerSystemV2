namespace NanpaoChillerSystemV2.Protocols
{
    public abstract class FlowData : AbsProtocol
    {
        /// <summary>
        /// 順間流量
        /// </summary>
        public decimal Flow { get; set; }
        /// <summary>
        /// 累積流量
        /// </summary>
        public decimal FlowTotal { get; set; }
        /// <summary>
        /// 回水溫度
        /// </summary>
        public decimal InputTemp { get; set; } = 0;
        /// <summary>
        /// 出水溫度
        /// </summary>
        public decimal OutputTemp { get; set; } = 0;
        /// <summary>
        /// 溫差
        /// </summary>
        public decimal TempDifference
        {
            get
            {
                var data = InputTemp - OutputTemp;
                if (data < 0)
                {
                    data = 0;
                }
                return data;
            }
        }
    }
}
