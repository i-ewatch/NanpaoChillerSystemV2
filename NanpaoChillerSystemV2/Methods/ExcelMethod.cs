using NanpaoChillerSystemV2.EF_Modules;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace NanpaoChillerSystemV2.Methods
{
    public class ExcelMethod
    {
        /// <summary>
        /// 開啟檔案
        /// </summary>
        private OpenFileDialog openFileDialog { get; set; }
        /// <summary>
        /// 載入檔案
        /// </summary>
        private XSSFWorkbook xworkbook { get; set; }
        /// <summary>
        /// 分頁數量
        /// </summary>
        private int SheetIndex { get; set; } = 0;
        /// <summary>
        /// 檔案名稱
        /// </summary>
        private string FileName { get; set; }
        /// <summary>
        /// 設備讀取資訊
        /// </summary>
        public List<GatewaySetting> GatewaySettings { get; set; }
        /// <summary>
        /// Slave讀取資訊
        /// </summary>
        public List<DeviceSetting> DeviceSettings { get; set; }
        /// <summary>
        /// Line推播資訊
        /// </summary>
        public List<LineNotifySetting> LineNotifySettings { get; set; }
        public SqlMethod SqlMethod { get; set; }
        public ExcelMethod(SqlMethod sqlMethod)
        {
            SqlMethod = sqlMethod;
        }
        public bool Excel_Load()
        {
            GatewaySettings = new List<GatewaySetting>();
            DeviceSettings = new List<DeviceSetting>();
            LineNotifySettings = new List<LineNotifySetting>();
            try
            {
                openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "*.Xlsx| *.xlsx";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FileName = openFileDialog.FileName.Split('\\')[openFileDialog.FileName.Split('\\').Length - 1];
                    using (FileStream file = new FileStream($"{openFileDialog.FileName}", FileMode.Open, FileAccess.Read))
                    {
                        xworkbook = new XSSFWorkbook(file);//Excel檔案載入
                    }
                    SheetIndex = xworkbook.NumberOfSheets;//取得分頁數量
                    for (int Sheetnum = 0; Sheetnum < SheetIndex; Sheetnum++)
                    {
                        string SheetName = xworkbook.GetSheetName(Sheetnum).Trim();
                        var data = xworkbook.GetSheetAt(Sheetnum);//載入分頁資訊
                        switch (SheetName)
                        {
                            case "GatewaySetting":
                                {
                                    for (int Rownum = 1; Rownum < data.LastRowNum + 1; Rownum++)
                                    {
                                        IRow row = data.GetRow(Rownum);
                                        if (row != null)
                                        {
                                            ICell GatewayIndex = row.GetCell(0);
                                            ICell GatewayType = row.GetCell(1);
                                            ICell Connect = row.GetCell(2);
                                            ICell GatewayName = row.GetCell(3);
                                            GatewaySetting setting = new GatewaySetting();
                                            if (GatewayIndex != null)
                                            {
                                                setting.GatewayIndex = Convert.ToInt32(GatewayIndex.ToString());
                                            }
                                            if (GatewayType != null)
                                            {
                                                setting.GatewayType = Convert.ToInt32(GatewayType.ToString());
                                            }
                                            if (Connect != null)
                                            {
                                                setting.Connect = Connect.ToString();
                                            }
                                            if (GatewayName != null)
                                            {
                                                setting.GatewayName = GatewayName.ToString();
                                            }
                                            GatewaySettings.Add(setting);
                                        }
                                    }
                                }
                                break;
                            case "DeviceSetting":
                                {
                                    for (int Rownum = 1; Rownum < data.LastRowNum + 1; Rownum++)
                                    {
                                        IRow row = data.GetRow(Rownum);
                                        if (row != null)
                                        {
                                            ICell GatewayIndex = row.GetCell(0);
                                            ICell DeviceIndex = row.GetCell(1);
                                            ICell DeviceType = row.GetCell(2);
                                            ICell DeviceID = row.GetCell(3);
                                            ICell DeviceName = row.GetCell(4);
                                            ICell TempMax = row.GetCell(5);
                                            ICell TempMin = row.GetCell(6);
                                            ICell TempMax1 = row.GetCell(7);
                                            ICell TempMin1 = row.GetCell(8);
                                            ICell LineIndex = row.GetCell(9);
                                            ICell ElectricIndex = row.GetCell(10);
                                            ICell ChillerIndex = row.GetCell(11);
                                            ICell CardNo = row.GetCell(12);
                                            ICell BoardNo = row.GetCell(13);
                                            DeviceSetting setting = new DeviceSetting();
                                            if (GatewayIndex != null)
                                            {
                                                setting.GatewayIndex = Convert.ToInt32(GatewayIndex.ToString());
                                            }
                                            if (DeviceIndex != null)
                                            {
                                                setting.DeviceIndex = Convert.ToInt32(DeviceIndex.ToString());
                                            }
                                            if (DeviceType != null)
                                            {
                                                setting.DeviceType = Convert.ToInt32(DeviceType.ToString());
                                            }
                                            if (DeviceID != null)
                                            {
                                                setting.DeviceID = Convert.ToInt32(DeviceID.ToString());
                                            }
                                            if (DeviceName != null)
                                            {
                                                setting.DeviceName = DeviceName.ToString();
                                            }
                                            if (TempMax != null)
                                            {
                                                setting.TempMax = Convert.ToDecimal(TempMax.ToString());
                                            }
                                            if (TempMin != null)
                                            {
                                                setting.TempMin = Convert.ToDecimal(TempMin.ToString());
                                            }
                                            if (TempMax1 != null)
                                            {
                                                setting.TempMax1 = Convert.ToDecimal(TempMax1.ToString());
                                            }
                                            if (TempMin1 != null)
                                            {
                                                setting.TempMin1 = Convert.ToDecimal(TempMin1.ToString());
                                            }
                                            if (LineIndex != null)
                                            {
                                                setting.LineIndex = LineIndex.ToString();
                                            }
                                            else
                                            {
                                                setting.LineIndex = "";
                                            }
                                            if (ElectricIndex != null)
                                            {
                                                setting.ElectricIndex = ElectricIndex.ToString();
                                            }
                                            else
                                            {
                                                setting.ElectricIndex = "";
                                            }
                                            if (ChillerIndex != null)
                                            {
                                                setting.ChillerIndex = ChillerIndex.ToString();
                                            }
                                            else
                                            {
                                                setting.ChillerIndex = "";
                                            }
                                            if (CardNo != null)
                                            {
                                                setting.CardNo = CardNo.ToString();
                                            }
                                            else
                                            {
                                                setting.CardNo = "";
                                            }
                                            if (BoardNo != null)
                                            {
                                                setting.BoardNo = BoardNo.ToString();
                                            }
                                            else
                                            {
                                                setting.BoardNo = "";
                                            }
                                            DeviceSettings.Add(setting);
                                        }
                                    }
                                }
                                break;
                            case "LineNotifySetting":
                                {
                                    for (int Rownum = 1; Rownum < data.LastRowNum + 1; Rownum++)
                                    {
                                        IRow row = data.GetRow(Rownum);
                                        if (row != null)
                                        {
                                            ICell LineIndex = row.GetCell(0);
                                            ICell SendFlag = row.GetCell(1);
                                            ICell Token = row.GetCell(2);
                                            LineNotifySetting setting = new LineNotifySetting();
                                            if (LineIndex != null)
                                            {
                                                setting.LineIndex = Convert.ToInt32(LineIndex.ToString());
                                            }
                                            if (SendFlag != null)
                                            {
                                                setting.SendFlag = Convert.ToBoolean(Convert.ToInt32(SendFlag.ToString()));
                                            }
                                            if (Token != null)
                                            {
                                                setting.Token = Token.ToString();
                                            }
                                            LineNotifySettings.Add(setting);
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    bool gateway = false;
                    bool device = false;
                    bool line = false;
                    bool Flag = false;
                    if (GatewaySettings.Count > 0)
                    {
                        gateway = SqlMethod.Inserter_GatewaySetting(GatewaySettings);
                    }
                    if (DeviceSettings.Count > 0)
                    {
                        device = SqlMethod.Inserter_DeviceSetting(DeviceSettings);
                    }
                    if (LineNotifySettings.Count > 0)
                    {
                        line = SqlMethod.Inserter_LineNotifySetting(LineNotifySettings);
                    }
                    if ((gateway && device) || line)
                    {
                        Flag = true;
                    }
                    return Flag;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) { Log.Error(ex, $"資料匯入失敗  檔案名稱 : {FileName}"); return false; }
        }
    }
}
