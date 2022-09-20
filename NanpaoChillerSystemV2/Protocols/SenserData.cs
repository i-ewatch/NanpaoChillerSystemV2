using System;

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
                    if (value > TempMax & TempMax != 0) { tempIndex = 2; }
                    else if (value < TempMin & TempMin != 0) { tempIndex = 1; }
                    else if (TempMax >= value & value >= TempMin & TempMax != 0 & TempMin != 0) { tempIndex = 0; }
                }
            }
        }
        private DateTime tempDate { get; set; } = new DateTime();
        private int tempFlag { get; set; } = 0;
        private int tempIndex
        {
            get { return _tempIndex; }
            set
            {
                if (value != _tempIndex)
                {
                    tempDate = DateTime.Now;
                    _tempIndex = value;
                }
                else
                {
                    TimeSpan timeSpan = DateTime.Now.Subtract(tempDate);
                    if (timeSpan.TotalSeconds > 10 && tempFlag != value)
                    {
                        switch (value)
                        {
                            case 0:
                                {
                                    SqlMethod.InsertTRState(DeviceSetting, $"設備名稱:{DeviceSetting.DeviceName},環境溫度恢復正常");
                                    AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r環境溫度恢復正常");
                                    tempFlag = 0;
                                }
                                break;
                            case 1:
                                {
                                    SqlMethod.InsertTRState(DeviceSetting, $"設備名稱:{DeviceSetting.DeviceName},環境溫度過低警報");
                                    AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r環境溫度過低警報");
                                    tempFlag = 1;
                                }
                                break;
                            case 2:
                                {
                                    SqlMethod.InsertTRState(DeviceSetting, $"設備名稱:{DeviceSetting.DeviceName},環境溫度過高警報");
                                    AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r環境溫度過高警報");
                                    tempFlag = 2;
                                }
                                break;
                        }
                    }
                }
            }
        }
        private int _tempIndex { get; set; } = 0;
        public decimal temp { get; set; }
        public decimal Temp1
        {
            get { return temp1; }
            set
            {
                if (temp1 != value)
                {
                    if (value > TempMax1 & TempMax1 != 0) { tempIndex1 = 2; }
                    else if (value < TempMin1 & TempMin1 != 0) { tempIndex1 = 1; }
                    else if (TempMax1 >= value & value >= TempMin1 & TempMax1 != 0 & TempMin1 != 0) { tempIndex1 = 0; }
                    temp1 = value;
                }
            }
        }
        private DateTime tempDate1 { get; set; } = new DateTime();
        private int tempFlag1 { get; set; } = 0;
        private int tempIndex1
        {
            get { return _tempIndex1; }
            set
            {
                if (value != _tempIndex1)
                {
                    tempDate1 = DateTime.Now;
                    _tempIndex1 = value;
                }
                else
                {
                    TimeSpan timeSpan = DateTime.Now.Subtract(tempDate1);
                    if (timeSpan.TotalSeconds > 10 && tempFlag1 != value)
                    {
                        switch (value)
                        {
                            case 0:
                                {
                                    SqlMethod.InsertTRState(DeviceSetting, $"設備名稱:{DeviceSetting.DeviceName},變壓器溫度恢復正常");
                                    AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r變壓器溫度恢復正常");
                                    tempFlag1 = 0;
                                }
                                break;
                            case 1:
                                {
                                    SqlMethod.InsertTRState(DeviceSetting, $"設備名稱:{DeviceSetting.DeviceName},變壓器溫度過低警報");
                                    AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r變壓器溫度過低警報");
                                    tempFlag1 = 1;
                                }
                                break;
                            case 2:
                                {
                                    SqlMethod.InsertTRState(DeviceSetting, $"設備名稱:{DeviceSetting.DeviceName},變壓器溫度過高警報");
                                    AlarmNotifySender($"\r設備名稱:{DeviceSetting.DeviceName}\r變壓器溫度過高警報");
                                    tempFlag1 = 2;
                                }
                                break;
                        }
                    }
                }
            }
        }
        #region 溫度上下限
        private int _tempIndex1 { get; set; } = 0;
        public decimal temp1 { get; set; }

        public decimal temp_max { get; set; } = 0;
        public decimal TempMax
        {
            get { return temp_max; }
            set
            {
                if (value != temp_max & value != 0)
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
                if (value != temp_min & value != 0)
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
                if (value != temp_max1 & value != 0)
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
                if (value != temp_min1 & value != 0)
                {
                    temp_min1 = value;
                    SqlMethod.UpdateDeviceTemp(DeviceSetting, value, 3);
                }
            }
        }
        #endregion
    }
}
