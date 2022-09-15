namespace NanpaoChillerSystemV2.EF_Modules
{
    public class ChillerLog
    {
        public string ttime { get; set; }
        public System.DateTime ttimen { get; set; }
        public int GatewayIndex { get; set; }
        public int DeviceIndex { get; set; }
        public decimal CHW_OUT_TEMP { get; set; }
        public decimal CHW_IN_TEMP { get; set; }
        public decimal CHW_TEMP_Difference { get; set; }
        public decimal CW_OUT_TEMP { get; set; }
        public decimal CW_IN_TEMP { get; set; }
        public decimal CW_TEMP_Difference { get; set; }
        public decimal CH1_PRESS_HIGH { get; set; }
        public decimal CH1_PRESS_LOW { get; set; }
        public decimal CH2_PRESS_HIGH { get; set; }
        public decimal CH2_PRESS_LOW { get; set; }
        public decimal CH1_RATE { get; set; }
        public decimal CH2_RATE { get; set; }
        public decimal CH1_STOP_COUNT { get; set; }
        public decimal CH2_STOP_COUNT { get; set; }
        public decimal CHP_STOP_COUNT { get; set; }
        public decimal CH1_RUN_HOUR { get; set; }
        public decimal CH2_RUN_HOUR { get; set; }
        public decimal CH1_RUN_TIME { get; set; }
        public decimal CH2_RUN_TIME { get; set; }
    }
}
