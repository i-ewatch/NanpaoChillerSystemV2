namespace NanpaoChillerSystemV2.EF_Modules
{
    public class DeviceSetting
    {
        public int GatewayIndex { get; set; }
        public int DeviceIndex { get; set; }
        public int DeviceType { get; set; }
        public int DeviceID { get; set; }
        public string DeviceName { get; set; }
        public decimal TempMax { get; set; }
        public decimal TempMin { get; set; }
        public decimal TempMax1 { get; set; }
        public decimal TempMin1 { get; set; }
        public string LineIndex { get; set; }
        public string ElectricIndex { get; set; }
        public string ChillerIndex { get; set; }
        public string CardNo { get; set; }
        public string BoardNo { get; set; }
        public override string ToString()
        {
            return DeviceName;
        }
    }
}
