namespace NanpaoChillerSystemV2.Protocols
{
    public abstract class SenserData : AbsProtocol
    {
        public decimal Temp
        {
            get { return temp; }
            set
            {
                if (temp != value)
                {
                    temp = value;
                    if (value > TempMax & TempMax != 0 & tempIndex != 2)
                    {
                        SqlMethod.InsertTRState(DeviceSetting, $"設備名稱:{DeviceSetting.DeviceName},環境溫度過高警報");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r環境溫度過高警報");
                        tempIndex = 2;
                    }
                    else if (value < TempMin & TempMin != 0 & tempIndex != 1)
                    {
                        SqlMethod.InsertTRState(DeviceSetting, $"設備名稱:{DeviceSetting.DeviceName},環境溫度過低警報");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r環境溫度過低警報");
                        tempIndex = 1;
                    }
                    else if(tempIndex != 0)
                    {
                        SqlMethod.InsertTRState(DeviceSetting, $"設備名稱:{DeviceSetting.DeviceName},環境溫度恢復正常");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r環境溫度恢復正常");
                        tempIndex = 0;
                    }
                }
            }
        }
        private int tempIndex { get; set; } = 0;
        public decimal temp { get; set; }
        public decimal Temp1 {
            get { return temp1; }
            set
            {
                if (temp1 != value)
                {
                    temp1 = value;
                    if (value > TempMax1 & TempMax1 != 0 & tempIndex1!=2)
                    {
                        SqlMethod.InsertTRState(DeviceSetting, $"設備名稱:{DeviceSetting.DeviceName},變壓器溫度過高警報");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r變壓器溫度過高警報");
                        tempIndex1 = 2;
                    }
                    else if (value < TempMin1 & TempMin1 != 0 & tempIndex1 != 1)
                    {
                        SqlMethod.InsertTRState(DeviceSetting, $"設備名稱:{DeviceSetting.DeviceName},變壓器溫度過低警報");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r變壓器溫度過低警報");
                        tempIndex1 = 1;
                    }
                    else if (tempIndex1 != 0)
                    {
                        SqlMethod.InsertTRState(DeviceSetting, $"設備名稱:{DeviceSetting.DeviceName},變壓器溫度恢復正常");
                        AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r變壓器溫度恢復正常");
                        tempIndex1 = 0;
                    }
                }
            }
        }
        private int tempIndex1 { get; set; } = 0;
        public decimal temp1 { get; set; }

        public decimal temp_max { get; set; } = 0;
        public decimal TempMax
        {
            get { return temp_max; }
            set
            {
                if (value != temp_max)
                {
                    temp_max = value;
                    SqlMethod.UpdateDeviceTemp(DeviceSetting, value, 0);
                }
            }
        }
        public decimal temp_min { get; set; } = 0;
        public decimal TempMin
        {
            get { return temp_min; }
            set
            {
                if (value != temp_min)
                {
                    temp_min = value;
                    SqlMethod.UpdateDeviceTemp(DeviceSetting, value, 1);
                }
            }
        }

        public decimal temp_max1 { get; set; } = 0;
        public decimal TempMax1
        {
            get { return temp_max1; }
            set
            {
                if (value != temp_max1)
                {
                    temp_max1 = value;
                    SqlMethod.UpdateDeviceTemp(DeviceSetting, value, 2);
                }
            }
        }
        public decimal temp_min1 { get; set; } = 0;
        public decimal TempMin1
        {
            get { return temp_min1; }
            set
            {
                if (value != temp_min1)
                {
                    temp_min1 = value;
                    SqlMethod.UpdateDeviceTemp(DeviceSetting, value, 3);
                }
            }
        }
    }
}
