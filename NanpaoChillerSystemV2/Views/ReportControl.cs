using DevExpress.Data;
using DevExpress.XtraBars.Docking2010.Customization;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraSplashScreen;
using NanpaoChillerSystemV2.EF_Modules;
using NanpaoChillerSystemV2.Enums;
using NanpaoChillerSystemV2.Methods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace NanpaoChillerSystemV2.Views
{
    public partial class ReportControl : Field4Control
    {
        /// <summary>
        /// 資料庫方法
        /// </summary>
        SqlMethod SqlMethod { get; set; }
        /// <summary>
        /// 設備資訊
        /// </summary>
        List<DeviceSetting> DeviceSettings { get; set; }
        /// <summary>
        /// 開始時間
        /// </summary>
        private DateTime StartTime { get; set; }
        /// <summary>
        /// 結束時間
        /// </summary>
        private DateTime EndTime { get; set; }
        /// <summary>
        /// Loading物件繼承
        /// </summary>
        private IOverlaySplashScreenHandle handle = null;
        /// 關閉Loading視窗
        /// </summary>
        /// <param name="handle"></param>
        private void CloseProgressPanel(IOverlaySplashScreenHandle handle)
        {
            if (handle != null)
                SplashScreenManager.CloseOverlayForm(handle);
        }
        public ReportControl(List<DeviceSetting> deviceSettings, SqlMethod sqlMethod)
        {
            InitializeComponent();
            DeviceSettings = deviceSettings;
            SqlMethod = sqlMethod;
            foreach (var item in deviceSettings)
            {
                DeviceType deviceType = (DeviceType)item.DeviceType;
                switch (deviceType)
                {
                    case DeviceType.DaTongChiller:
                        {
                            cbet_Chiller_Device.Properties.Items.Add(item);
                        }
                        break;
                    case DeviceType.PA330:
                    case DeviceType.PA3000:
                        {
                            cbet_Electric_Device.Properties.Items.Add(item);
                        }
                        break;
                    case DeviceType.UF300:
                        {
                            cbet_Flow_Device.Properties.Items.Add(item);
                            cbet_Year_Device.Properties.Items.Add(item);
                            cbet_Month_Device.Properties.Items.Add(item);
                        }
                        break;
                    case DeviceType.TR:
                        {
                            cbet_TR_Device.Properties.Items.Add(item);
                        }
                        break;
                }
            }
            #region 冰機數值下拉選單
            cbet_Chiller_Device.SelectedIndexChanged += (s, e) =>
            {
                if (cbet_Chiller_Value.Properties.Items.Count > 0)
                {
                    cbet_Chiller_Value.Properties.Items.Clear();
                }
                cbet_Chiller_Value.Properties.Items.Add("冰水溫度");
                cbet_Chiller_Value.Properties.Items.Add("冷卻水溫度");
                cbet_Chiller_Value.Properties.Items.Add("壓縮機容調顯示值");
                cbet_Chiller_Value.Properties.Items.Add("壓縮機高壓");
                cbet_Chiller_Value.Properties.Items.Add("壓縮機低壓");
                cbet_Chiller_Value.Properties.Items.Add("狀態");
            };
            #endregion
            #region 電表數值下拉選單
            cbet_Electric_Device.SelectedIndexChanged += (s, e) =>
            {
                if (cbet_Electric_Value.Properties.Items.Count > 0)
                {
                    cbet_Electric_Value.Properties.Items.Clear();
                }
                cbet_Electric_Value.Properties.Items.Add("三相電壓");
                cbet_Electric_Value.Properties.Items.Add("三相電流");
                cbet_Electric_Value.Properties.Items.Add("功率因數");
                cbet_Electric_Value.Properties.Items.Add("瞬間需量");
                cbet_Electric_Value.Properties.Items.Add("累積量");
            };
            #endregion
            #region 流量計下拉選單
            cbet_Flow_Device.SelectedIndexChanged += (s, e) =>
            {
                if (cbet_Flow_Value.Properties.Items.Count > 0)
                {
                    cbet_Flow_Value.Properties.Items.Clear();
                }
                cbet_Flow_Value.Properties.Items.Add("瞬間流量");
                cbet_Flow_Value.Properties.Items.Add("溫度");
                cbet_Flow_Value.Properties.Items.Add("累積流量");
            };
            #endregion
            #region 變壓器下拉選單
            cbet_TR_Device.SelectedIndexChanged += (s, e) =>
            {
                if (cbet_TR_Value.Properties.Items.Count > 0)
                {
                    cbet_TR_Value.Properties.Items.Clear();
                }
                cbet_TR_Value.Properties.Items.Add("溫度");
                cbet_TR_Value.Properties.Items.Add("狀態");
            };
            #endregion

            #region 冰機按鈕
            btn_Chiller_Search.Click += (s, e) =>
            {
                if (Check_Time(det_Chiller_Start, det_Chiller_End) & Check_Device(cbet_Chiller_Device, cbet_Chiller_Value))
                {
                    handle = SplashScreenManager.ShowOverlayForm(FindForm());
                    if (gvw_Chiller.Columns.Count > 0)
                    {
                        gvw_Chiller.Columns.Clear();
                    }
                    if (ccl_Chiller.Series.Count > 0)
                    {
                        ccl_Chiller.Series.Clear();
                    }

                    List<ChillerLog> logs = new List<ChillerLog>();
                    List<ChillerStateLog> stateLogs = new List<ChillerStateLog>();
                    DeviceSetting device = cbet_Chiller_Device.Properties.Items[cbet_Chiller_Device.SelectedIndex] as DeviceSetting;
                    if (cbet_Chiller_Value.SelectedIndex != 5)
                    {
                        layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        logs = SqlMethod.SearchChiller(device, StartTime.ToString("yyyyMMddHHmmss"), EndTime.ToString("yyyyMMddHHmmss"));
                        gcl_Chiller.DataSource = logs;
                        for (int i = 0; i < gvw_Chiller.Columns.Count(); i++)
                        {
                            gvw_Chiller.Columns[i].Visible = false;
                            gvw_Chiller.Columns[i].OptionsColumn.AllowEdit = false;
                        }
                        ccl_Chiller.DataSource = logs;
                        ccl_Chiller.Legend.Direction = LegendDirection.TopToBottom;//曲線圖線條說明的排序
                        ccl_Chiller.CrosshairOptions.CrosshairLabelMode = CrosshairLabelMode.ShowCommonForAllSeries; //顯示全部線條內容
                        ccl_Chiller.CrosshairOptions.LinesMode = CrosshairLinesMode.Auto;//自動獲取點上面的數值
                        ccl_Chiller.CrosshairOptions.GroupHeaderTextOptions.Font = new System.Drawing.Font("微軟正黑體", 12);
                        ccl_Chiller.CrosshairOptions.ShowArgumentLabels = true;//是否顯示Y軸垂直線
                        ccl_Chiller.SideBySideEqualBarWidth = false;//線條是否需要相等寬度
                    }
                    else
                    {
                        layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        stateLogs = SqlMethod.SearchChillerState(device, StartTime.ToString("yyyyMMddHHmmss"), EndTime.ToString("yyyyMMddHHmmss"));
                        gcl_Chiller.DataSource = stateLogs;
                    }

                    ChillerReportType type = (ChillerReportType)cbet_Chiller_Value.SelectedIndex;
                    switch (type)
                    {
                        case ChillerReportType.CHW_TEMP:
                            {
                                #region 報表
                                gvw_Chiller.Columns["CHW_TEMP_Difference"].Caption = "冰水溫差";
                                gvw_Chiller.Columns["CHW_TEMP_Difference"].Visible = true;
                                gvw_Chiller.Columns["CHW_IN_TEMP"].Caption = "冰水入水溫度";
                                gvw_Chiller.Columns["CHW_IN_TEMP"].Visible = true;
                                gvw_Chiller.Columns["CHW_OUT_TEMP"].Caption = "冰水出水溫度";
                                gvw_Chiller.Columns["CHW_OUT_TEMP"].Visible = true;
                                gvw_Chiller.Columns["ttimen"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm";
                                gvw_Chiller.Columns["ttimen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                gvw_Chiller.Columns["ttimen"].Caption = "時間";
                                gvw_Chiller.Columns["ttimen"].Visible = true;
                                #endregion

                                #region 曲線圖
                                Series Value = new Series($"{device.DeviceName}-冰水出水溫度", ViewType.Line);
                                Value.ArgumentDataMember = "ttimen";
                                Value.ValueDataMembers.AddRange(new string[] { "CHW_OUT_TEMP" });
                                Value.CrosshairLabelPattern = "{S} {V:0.##} \xb0" + "C";
                                Series Value2 = new Series($"{device.DeviceName}-冰水入水溫度", ViewType.Line);
                                Value2.ArgumentDataMember = "ttimen";
                                Value2.ValueDataMembers.AddRange(new string[] { "CHW_IN_TEMP" });
                                Value2.CrosshairLabelPattern = "{S} {V:0.##} \xb0" + "C";
                                ccl_Chiller.Series.AddRange(Value);
                                ccl_Chiller.Series.AddRange(Value2);
                                #endregion
                            }
                            break;
                        case ChillerReportType.CW_TEMP:
                            {
                                #region 報表
                                gvw_Chiller.Columns["CW_IN_TEMP"].Caption = "冷卻水入水溫度";
                                gvw_Chiller.Columns["CW_IN_TEMP"].Visible = true;
                                gvw_Chiller.Columns["CW_OUT_TEMP"].Caption = "冷卻水出水溫度";
                                gvw_Chiller.Columns["CW_OUT_TEMP"].Visible = true;
                                gvw_Chiller.Columns["ttimen"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm";
                                gvw_Chiller.Columns["ttimen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                gvw_Chiller.Columns["ttimen"].Caption = "時間";
                                gvw_Chiller.Columns["ttimen"].Visible = true;
                                #endregion

                                #region 曲線圖
                                Series Value = new Series($"{device.DeviceName}-冷卻水出水溫度", ViewType.Line);
                                Value.ArgumentDataMember = "ttimen";
                                Value.ValueDataMembers.AddRange(new string[] { "CW_OUT_TEMP" });
                                Value.CrosshairLabelPattern = "{S} {V:0.##} \xb0" + "C";
                                Series Value2 = new Series($"{device.DeviceName}-冷卻水入水溫度", ViewType.Line);
                                Value2.ArgumentDataMember = "ttimen";
                                Value2.ValueDataMembers.AddRange(new string[] { "CW_IN_TEMP" });
                                Value2.CrosshairLabelPattern = "{S} {V:0.##} \xb0" + "C";
                                ccl_Chiller.Series.AddRange(Value);
                                ccl_Chiller.Series.AddRange(Value2);
                                #endregion
                            }
                            break;
                        case ChillerReportType.CH_RATE:
                            {
                                #region 報表
                                gvw_Chiller.Columns["CH2_RATE"].Caption = "2機壓縮機容調顯示值";
                                gvw_Chiller.Columns["CH2_RATE"].Visible = true;
                                gvw_Chiller.Columns["CH1_RATE"].Caption = "1機壓縮機容調顯示值";
                                gvw_Chiller.Columns["CH1_RATE"].Visible = true;
                                gvw_Chiller.Columns["ttimen"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm";
                                gvw_Chiller.Columns["ttimen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                gvw_Chiller.Columns["ttimen"].Caption = "時間";
                                gvw_Chiller.Columns["ttimen"].Visible = true;
                                #endregion

                                #region 曲線圖
                                Series Value = new Series($"{device.DeviceName}-1機壓縮機容調顯示值", ViewType.Line);
                                Value.ArgumentDataMember = "ttimen";
                                Value.ValueDataMembers.AddRange(new string[] { "CH1_RATE" });
                                Value.CrosshairLabelPattern = "{S} {V:0.##}" + "%";
                                Series Value2 = new Series($"{device.DeviceName}-2機壓縮機容調顯示值", ViewType.Line);
                                Value2.ArgumentDataMember = "ttimen";
                                Value2.ValueDataMembers.AddRange(new string[] { "CH2_RATE" });
                                Value2.CrosshairLabelPattern = "{S} {V:0.##}" + "%";
                                ccl_Chiller.Series.AddRange(Value);
                                ccl_Chiller.Series.AddRange(Value2);
                                #endregion
                            }
                            break;
                        case ChillerReportType.CH_PRESS_HIGH:
                            {
                                #region 報表
                                gvw_Chiller.Columns["CH2_PRESS_HIGH"].Caption = "2機壓縮機高壓";
                                gvw_Chiller.Columns["CH2_PRESS_HIGH"].Visible = true;
                                gvw_Chiller.Columns["CH1_PRESS_HIGH"].Caption = "1機壓縮機高壓";
                                gvw_Chiller.Columns["CH1_PRESS_HIGH"].Visible = true;
                                gvw_Chiller.Columns["ttimen"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm";
                                gvw_Chiller.Columns["ttimen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                gvw_Chiller.Columns["ttimen"].Caption = "時間";
                                gvw_Chiller.Columns["ttimen"].Visible = true;
                                #endregion

                                #region 曲線圖
                                Series Value = new Series($"{device.DeviceName}-1機壓縮機高壓", ViewType.Line);
                                Value.ArgumentDataMember = "ttimen";
                                Value.ValueDataMembers.AddRange(new string[] { "CH1_PRESS_HIGH" });
                                Value.CrosshairLabelPattern = "{S} {V:0.##} KG/CM" + "\xb2";
                                Series Value2 = new Series($"{device.DeviceName}-2機壓縮機高壓", ViewType.Line);
                                Value2.ArgumentDataMember = "ttimen";
                                Value2.ValueDataMembers.AddRange(new string[] { "CH2_PRESS_HIGH" });
                                Value2.CrosshairLabelPattern = "{S} {V:0.##} KG/CM" + "\xb2";
                                ccl_Chiller.Series.AddRange(Value);
                                ccl_Chiller.Series.AddRange(Value2);
                                #endregion
                            }
                            break;
                        case ChillerReportType.CH_PRESS_LOW:
                            {
                                #region 報表
                                gvw_Chiller.Columns["CH2_PRESS_LOW"].Caption = "2機壓縮機低壓";
                                gvw_Chiller.Columns["CH2_PRESS_LOW"].Visible = true;
                                gvw_Chiller.Columns["CH1_PRESS_LOW"].Caption = "1機壓縮機低壓";
                                gvw_Chiller.Columns["CH1_PRESS_LOW"].Visible = true;
                                gvw_Chiller.Columns["ttimen"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm";
                                gvw_Chiller.Columns["ttimen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                gvw_Chiller.Columns["ttimen"].Caption = "時間";
                                gvw_Chiller.Columns["ttimen"].Visible = true;
                                #endregion

                                #region 曲線圖
                                Series Value = new Series($"{device.DeviceName}-1機壓縮機低壓", ViewType.Line);
                                Value.ArgumentDataMember = "ttimen";
                                Value.ValueDataMembers.AddRange(new string[] { "CH1_PRESS_LOW" });
                                Value.CrosshairLabelPattern = "{S} {V:0.##} KG/CM" + "\xb2";
                                Series Value2 = new Series($"{device.DeviceName}-2機壓縮機低壓", ViewType.Line);
                                Value2.ArgumentDataMember = "ttimen";
                                Value2.ValueDataMembers.AddRange(new string[] { "CH2_PRESS_LOW" });
                                Value2.CrosshairLabelPattern = "{S} {V:0.##} KG/CM" + "\xb2";
                                ccl_Chiller.Series.AddRange(Value);
                                ccl_Chiller.Series.AddRange(Value2);
                                #endregion
                            }
                            break;
                        case ChillerReportType.State:
                            {
                                #region 報表
                                gvw_Chiller.Columns[0].Visible = false;
                                gvw_Chiller.Columns[1].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm:sss";
                                gvw_Chiller.Columns[1].Caption = "時間";
                                gvw_Chiller.Columns[1].BestFit();
                                gvw_Chiller.Columns[2].Visible = false;
                                gvw_Chiller.Columns[3].Visible = false;
                                gvw_Chiller.Columns[4].Visible = false;
                                gvw_Chiller.Columns[2].Caption = $"訊息";
                                gvw_Chiller.Columns[2].OptionsColumn.AllowEdit = false;
                                #endregion
                            }
                            break;
                    }
                    if (ccl_Chiller.DataSource != null)
                    {
                        XYDiagram diagram = (XYDiagram)ccl_Chiller.Diagram;
                        if (diagram != null)
                        {
                            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Minute; // 顯示設定
                            diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Minute; // 刻度設定
                            diagram.AxisX.Label.TextPattern = "{A:yyyy-MM-dd HH:mm}";//X軸顯示
                            diagram.AxisX.WholeRange.SideMarginsValue = 0;//不需要邊寬
                        }
                    }
                    CloseProgressPanel(handle);
                }
                else
                {
                    FlyoutAction action = new FlyoutAction();
                    action.Caption = "冰機查詢報表錯誤";
                    action.Description = "請確認每一個條件是否選取完畢(不可跨年份)";
                    action.Commands.Add(FlyoutCommand.OK);
                    FlyoutDialog.Show(FindForm(), action);
                }
            };
            btn_Chiller_Export.Click += (s, e) =>
            {
                if (gvw_Chiller.DataSource != null)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Xlsx|*xlsx";
                    saveFileDialog.Title = "Export Data";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        gvw_Chiller.ExportToXlsx($"{saveFileDialog.FileName}.xlsx");
                    }
                }
                else
                {
                    FlyoutAction action = new FlyoutAction();
                    action.Caption = "冰機報表資訊-匯出報表錯誤";
                    action.Description = "請查詢報表在進行匯出動作";
                    action.Commands.Add(FlyoutCommand.OK);
                    FlyoutDialog.Show(FindForm(), action);
                }
            };
            #endregion
            #region 電表按鈕
            btn_Electric_Search.Click += (s, e) =>
            {
                if (Check_Time(det_Electric_Start, det_Electric_End) & Check_Device(cbet_Electric_Device, cbet_Electric_Value))
                {
                    handle = SplashScreenManager.ShowOverlayForm(FindForm());
                    if (gvw_Electric.Columns.Count > 0)
                    {
                        gvw_Electric.Columns.Clear();
                    }
                    if (ccl_Electric.Series.Count > 0)
                    {
                        ccl_Electric.Series.Clear();
                    }
                    DeviceSetting device = cbet_Electric_Device.Properties.Items[cbet_Electric_Device.SelectedIndex] as DeviceSetting;
                    List<ElectricLog> logs = new List<ElectricLog>();
                    List<ElectricTotal> electricTotals = new List<ElectricTotal>();
                    if (cbet_Electric_Value.SelectedIndex != 4)
                    {
                        logs = SqlMethod.SearchElectric(device, StartTime.ToString("yyyyMMddHHmmss"), EndTime.ToString("yyyyMMddHHmmss"));
                        ccl_Electric.DataSource = logs;
                        gcl_Electric.DataSource = logs;
                    }
                    else
                    {
                        electricTotals = SqlMethod.SearchElectricTotal(device, StartTime.ToString("yyyyMMddHHmmss"), EndTime.ToString("yyyyMMddHHmmss"), 0);
                        ccl_Electric.DataSource = electricTotals;
                        gcl_Electric.DataSource = electricTotals;
                    }
                    for (int i = 0; i < gvw_Electric.Columns.Count(); i++)
                    {
                        gvw_Electric.Columns[i].Visible = false;
                        gvw_Electric.Columns[i].OptionsColumn.AllowEdit = false;
                    }
                    ccl_Electric.Legend.Direction = LegendDirection.TopToBottom;//曲線圖線條說明的排序
                    ccl_Electric.CrosshairOptions.CrosshairLabelMode = CrosshairLabelMode.ShowCommonForAllSeries; //顯示全部線條內容
                    ccl_Electric.CrosshairOptions.LinesMode = CrosshairLinesMode.Auto;//自動獲取點上面的數值
                    ccl_Electric.CrosshairOptions.GroupHeaderTextOptions.Font = new System.Drawing.Font("微軟正黑體", 12);
                    ccl_Electric.CrosshairOptions.ShowArgumentLabels = true;//是否顯示Y軸垂直線
                    ccl_Electric.SideBySideEqualBarWidth = false;//線條是否需要相等寬度                  
                    ElectricReportType type = (ElectricReportType)cbet_Electric_Value.SelectedIndex;
                    switch (type)
                    {
                        case ElectricReportType.Voltage:
                            {
                                #region 報表
                                gvw_Electric.Columns["ttv"].Caption = "T相電壓";
                                gvw_Electric.Columns["ttv"].Visible = true;
                                gvw_Electric.Columns["tsv"].Caption = "S相電壓";
                                gvw_Electric.Columns["tsv"].Visible = true;
                                gvw_Electric.Columns["trv"].Caption = "R相電壓";
                                gvw_Electric.Columns["trv"].Visible = true;
                                gvw_Electric.Columns["ttimen"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm";
                                gvw_Electric.Columns["ttimen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                gvw_Electric.Columns["ttimen"].Caption = "時間";
                                gvw_Electric.Columns["ttimen"].Visible = true;
                                #endregion
                                #region 曲線圖
                                Series Value = new Series($"{device.DeviceName}-R相電壓", ViewType.Line);
                                Value.ArgumentDataMember = "ttimen";
                                Value.ValueDataMembers.AddRange(new string[] { "trv" });
                                Value.CrosshairLabelPattern = "{S} {V:0.##} V";
                                Series Value2 = new Series($"{device.DeviceName}-S相電壓", ViewType.Line);
                                Value2.ArgumentDataMember = "ttimen";
                                Value2.ValueDataMembers.AddRange(new string[] { "tsv" });
                                Value2.CrosshairLabelPattern = "{S} {V:0.##} V";
                                Series Value3 = new Series($"{device.DeviceName}-T相電壓", ViewType.Line);
                                Value3.ArgumentDataMember = "ttimen";
                                Value3.ValueDataMembers.AddRange(new string[] { "ttv" });
                                Value3.CrosshairLabelPattern = "{S} {V:0.##} V";
                                ccl_Electric.Series.AddRange(Value);
                                ccl_Electric.Series.AddRange(Value2);
                                ccl_Electric.Series.AddRange(Value3);
                                #endregion
                            }
                            break;
                        case ElectricReportType.Current:
                            {
                                #region 報表
                                gvw_Electric.Columns["tti"].Caption = "T相電流";
                                gvw_Electric.Columns["tti"].Visible = true;
                                gvw_Electric.Columns["tsi"].Caption = "S相電流";
                                gvw_Electric.Columns["tsi"].Visible = true;
                                gvw_Electric.Columns["tri"].Caption = "R相電流";
                                gvw_Electric.Columns["tri"].Visible = true;
                                gvw_Electric.Columns["ttimen"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm";
                                gvw_Electric.Columns["ttimen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                gvw_Electric.Columns["ttimen"].Caption = "時間";
                                gvw_Electric.Columns["ttimen"].Visible = true;
                                #endregion
                                #region 曲線圖
                                Series Value = new Series($"{device.DeviceName}-R相電流", ViewType.Line);
                                Value.ArgumentDataMember = "ttimen";
                                Value.ValueDataMembers.AddRange(new string[] { "tri" });
                                Value.CrosshairLabelPattern = "{S} {V:0.##} A";
                                Series Value2 = new Series($"{device.DeviceName}-S相電流", ViewType.Line);
                                Value2.ArgumentDataMember = "ttimen";
                                Value2.ValueDataMembers.AddRange(new string[] { "tsi" });
                                Value2.CrosshairLabelPattern = "{S} {V:0.##} A";
                                Series Value3 = new Series($"{device.DeviceName}-T相電流", ViewType.Line);
                                Value3.ArgumentDataMember = "ttimen";
                                Value3.ValueDataMembers.AddRange(new string[] { "tti" });
                                Value3.CrosshairLabelPattern = "{S} {V:0.##} A";
                                ccl_Electric.Series.AddRange(Value);
                                ccl_Electric.Series.AddRange(Value2);
                                ccl_Electric.Series.AddRange(Value3);
                                #endregion
                            }
                            break;
                        case ElectricReportType.PF:
                            {
                                #region 報表
                                gvw_Electric.Columns["tpre"].Caption = "功率因數";
                                gvw_Electric.Columns["tpre"].Visible = true;
                                gvw_Electric.Columns["ttimen"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm";
                                gvw_Electric.Columns["ttimen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                gvw_Electric.Columns["ttimen"].Caption = "時間";
                                gvw_Electric.Columns["ttimen"].Visible = true;
                                #endregion
                                #region 曲線圖
                                Series Value = new Series($"{device.DeviceName}-功率因數", ViewType.Line);
                                Value.ArgumentDataMember = "ttimen";
                                Value.ValueDataMembers.AddRange(new string[] { "tpre" });
                                Value.CrosshairLabelPattern = "{S} {V:0.##}";
                                ccl_Electric.Series.AddRange(Value);
                                #endregion
                            }
                            break;
                        case ElectricReportType.KW:
                            {
                                #region 報表
                                gvw_Electric.Columns["tkw"].Caption = "瞬間需量";
                                gvw_Electric.Columns["tkw"].Visible = true;
                                gvw_Electric.Columns["ttimen"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm";
                                gvw_Electric.Columns["ttimen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                gvw_Electric.Columns["ttimen"].Caption = "時間";
                                gvw_Electric.Columns["ttimen"].Visible = true;
                                #endregion
                                #region 曲線圖
                                Series Value = new Series($"{device.DeviceName}-瞬間需量", ViewType.Line);
                                Value.ArgumentDataMember = "ttimen";
                                Value.ValueDataMembers.AddRange(new string[] { "tkw" });
                                Value.CrosshairLabelPattern = "{S} {V:0.##} kW";
                                ccl_Electric.Series.AddRange(Value);
                                #endregion
                            }
                            break;
                        case ElectricReportType.KWH:
                            {
                                #region 報表
                                gvw_Electric.Columns["KwhTotal"].Caption = "累積量";
                                gvw_Electric.Columns["KwhTotal"].Visible = true;
                                gvw_Electric.Columns["ttimen"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm";
                                gvw_Electric.Columns["ttimen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                gvw_Electric.Columns["ttimen"].Caption = "時間";
                                gvw_Electric.Columns["ttimen"].Visible = true;
                                #endregion
                                #region 曲線圖
                                Series Value = new Series($"{device.DeviceName}-累積量", ViewType.Bar);
                                Value.ArgumentDataMember = "ttimen";
                                Value.ValueDataMembers.AddRange(new string[] { "KwhTotal" });
                                Value.CrosshairLabelPattern = "{S} {V:0.##} kWH";
                                ccl_Electric.Series.AddRange(Value);
                                #endregion
                            }
                            break;
                    }
                    if (ccl_Electric.DataSource != null)
                    {
                        XYDiagram diagram = (XYDiagram)ccl_Electric.Diagram;
                        if (diagram != null)
                        {
                            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Minute; // 顯示設定
                            diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Minute; // 刻度設定
                            diagram.AxisX.Label.TextPattern = "{A:yyyy-MM-dd HH:mm}";//X軸顯示
                            diagram.AxisX.WholeRange.SideMarginsValue = 0;//不需要邊寬
                        }
                    }
                    CloseProgressPanel(handle);
                }
                else
                {
                    FlyoutAction action = new FlyoutAction();
                    action.Caption = "電表查詢報表錯誤";
                    action.Description = "請確認每一個條件是否選取完畢(不可跨年份)";
                    action.Commands.Add(FlyoutCommand.OK);
                    FlyoutDialog.Show(FindForm(), action);
                }
            };
            btn_Electric_Export.Click += (s, e) =>
            {
                if (gvw_Electric.DataSource != null)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Xlsx|*xlsx";
                    saveFileDialog.Title = "Export Data";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        gvw_Electric.ExportToXlsx($"{saveFileDialog.FileName}.xlsx");
                    }
                }
                else
                {
                    FlyoutAction action = new FlyoutAction();
                    action.Caption = "電表報表資訊-匯出報表錯誤";
                    action.Description = "請查詢報表在進行匯出動作";
                    action.Commands.Add(FlyoutCommand.OK);
                    FlyoutDialog.Show(FindForm(), action);
                }
            };
            #endregion
            #region 流量計按鈕
            btn_Flow_Search.Click += (s, e) =>
            {
                if (Check_Time(det_Flow_Start, det_Flow_End) & Check_Device(cbet_Flow_Device, cbet_Flow_Value))
                {
                    handle = SplashScreenManager.ShowOverlayForm(FindForm());
                    if (gvw_Flow.Columns.Count > 0)
                    {
                        gvw_Flow.Columns.Clear();
                    }
                    if (ccl_Flow.Series.Count > 0)
                    {
                        ccl_Flow.Series.Clear();
                    }
                    DeviceSetting device = cbet_Flow_Device.Properties.Items[cbet_Flow_Device.SelectedIndex] as DeviceSetting;
                    List<FlowLog> logs = new List<FlowLog>();
                    List<Flowtotal> flowTotals = new List<Flowtotal>();
                    if (cbet_Flow_Value.SelectedIndex != 2)
                    {
                        logs = SqlMethod.SearchFlow(device, StartTime.ToString("yyyyMMddHHmmss"), EndTime.ToString("yyyyMMddHHmmss"));
                        gcl_Flow.DataSource = logs;
                        ccl_Flow.DataSource = logs;
                    }
                    else
                    {
                        flowTotals = sqlMethod.SearchFlowTotal(device, StartTime.ToString("yyyyMMddHHmmss"), EndTime.ToString("yyyyMMddHHmmss"), 2);
                        gcl_Flow.DataSource = flowTotals;
                        ccl_Flow.DataSource = flowTotals;
                    }

                    for (int i = 0; i < gvw_Flow.Columns.Count(); i++)
                    {
                        gvw_Flow.Columns[i].Visible = false;
                        gvw_Flow.Columns[i].OptionsColumn.AllowEdit = false;
                    }
                    ccl_Flow.Legend.Direction = LegendDirection.TopToBottom;//曲線圖線條說明的排序
                    ccl_Flow.CrosshairOptions.CrosshairLabelMode = CrosshairLabelMode.ShowCommonForAllSeries; //顯示全部線條內容
                    ccl_Flow.CrosshairOptions.LinesMode = CrosshairLinesMode.Auto;//自動獲取點上面的數值
                    ccl_Flow.CrosshairOptions.GroupHeaderTextOptions.Font = new System.Drawing.Font("微軟正黑體", 12);
                    ccl_Flow.CrosshairOptions.ShowArgumentLabels = true;//是否顯示Y軸垂直線
                    ccl_Flow.SideBySideEqualBarWidth = false;//線條是否需要相等寬度
                    FlowReportType type = (FlowReportType)cbet_Flow_Value.SelectedIndex;
                    switch (type)
                    {
                        case FlowReportType.Flow:
                            {
                                #region 報表
                                gvw_Flow.Columns["Flow"].Caption = "瞬間流量";
                                gvw_Flow.Columns["Flow"].Visible = true;
                                gvw_Flow.Columns["ttimen"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm";
                                gvw_Flow.Columns["ttimen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                gvw_Flow.Columns["ttimen"].Caption = "時間";
                                gvw_Flow.Columns["ttimen"].Visible = true;
                                #endregion

                                #region 曲線圖
                                Series Value = new Series($"{device.DeviceName}-瞬間流量", ViewType.Line);
                                Value.ArgumentDataMember = "ttimen";
                                Value.ValueDataMembers.AddRange(new string[] { "Flow" });
                                Value.CrosshairLabelPattern = "{S} {V:0.##} m" + "\xb3" + "/h";
                                ccl_Flow.Series.AddRange(Value);
                                #endregion
                            }
                            break;
                        case FlowReportType.Temp:
                            {
                                #region 報表
                                gvw_Flow.Columns["TempDifference"].Caption = "冰水溫差";
                                gvw_Flow.Columns["TempDifference"].Visible = true;
                                gvw_Flow.Columns["InputTemp"].Caption = "冰水入水溫度";
                                gvw_Flow.Columns["InputTemp"].Visible = true;
                                gvw_Flow.Columns["OutputTemp"].Caption = "冰水出水溫度";
                                gvw_Flow.Columns["OutputTemp"].Visible = true;
                                gvw_Flow.Columns["ttimen"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm";
                                gvw_Flow.Columns["ttimen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                gvw_Flow.Columns["ttimen"].Caption = "時間";
                                gvw_Flow.Columns["ttimen"].Visible = true;
                                #endregion

                                #region 曲線圖
                                Series Value = new Series($"{device.DeviceName}-冰水出水溫度", ViewType.Line);
                                Value.ArgumentDataMember = "ttimen";
                                Value.ValueDataMembers.AddRange(new string[] { "OutputTemp" });
                                Value.CrosshairLabelPattern = "{S} {V:0.##} \xb0" + "C";
                                Series Value2 = new Series($"{device.DeviceName}-冰水入水溫度", ViewType.Line);
                                Value2.ArgumentDataMember = "ttimen";
                                Value2.ValueDataMembers.AddRange(new string[] { "InputTemp" });
                                Value2.CrosshairLabelPattern = "{S} {V:0.##} \xb0" + "C";
                                ccl_Flow.Series.AddRange(Value);
                                ccl_Flow.Series.AddRange(Value2);
                                #endregion
                            }
                            break;
                        case FlowReportType.FlowTotal:
                            {
                                #region 報表
                                gvw_Flow.Columns["FlowTotal"].Caption = "累積流量";
                                gvw_Flow.Columns["FlowTotal"].Visible = true;
                                gvw_Flow.Columns["ttimen"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm";
                                gvw_Flow.Columns["ttimen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                gvw_Flow.Columns["ttimen"].Caption = "時間";
                                gvw_Flow.Columns["ttimen"].Visible = true;
                                #endregion

                                #region 曲線圖
                                Series Value = new Series($"{device.DeviceName}-累積流量", ViewType.Bar);
                                Value.ArgumentDataMember = "ttimen";
                                Value.ValueDataMembers.AddRange(new string[] { "FlowTotal" });
                                Value.CrosshairLabelPattern = "{S} {V:0.##} m" + "\xb3";
                                ccl_Flow.Series.AddRange(Value);
                                #endregion
                            }
                            break;
                    }
                    if (ccl_Flow.DataSource != null)
                    {
                        XYDiagram diagram = (XYDiagram)ccl_Flow.Diagram;
                        if (diagram != null)
                        {
                            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Minute; // 顯示設定
                            diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Minute; // 刻度設定
                            diagram.AxisX.Label.TextPattern = "{A:yyyy-MM-dd HH:mm}";//X軸顯示
                            diagram.AxisX.WholeRange.SideMarginsValue = 0;//不需要邊寬
                        }
                    }
                    CloseProgressPanel(handle);
                }
                else
                {
                    FlyoutAction action = new FlyoutAction();
                    action.Caption = "流量計查詢報表錯誤";
                    action.Description = "請確認每一個條件是否選取完畢(不可跨年份)";
                    action.Commands.Add(FlyoutCommand.OK);
                    FlyoutDialog.Show(FindForm(), action);
                }
            };
            btn_Flow_Export.Click += (s, e) =>
            {
                if (gvw_Flow.DataSource != null)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Xlsx|*xlsx";
                    saveFileDialog.Title = "Export Data";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        gvw_Flow.ExportToXlsx($"{saveFileDialog.FileName}.xlsx");
                    }
                }
                else
                {
                    FlyoutAction action = new FlyoutAction();
                    action.Caption = "流量計報表資訊-匯出報表錯誤";
                    action.Description = "請查詢報表在進行匯出動作";
                    action.Commands.Add(FlyoutCommand.OK);
                    FlyoutDialog.Show(FindForm(), action);
                }
            };
            #endregion
            #region 變壓器按鈕
            btn_TR_Search.Click += (s, e) =>
            {
                if (Check_Time(det_TR_Start, det_TR_End) & Check_Device(cbet_TR_Device, cbet_TR_Value))
                {
                    handle = SplashScreenManager.ShowOverlayForm(FindForm());
                    if (gvw_TR.Columns.Count > 0)
                    {
                        gvw_TR.Columns.Clear();
                    }
                    if (ccl_TR.Series.Count > 0)
                    {
                        ccl_TR.Series.Clear();
                    }
                    DeviceSetting device = cbet_TR_Device.Properties.Items[cbet_TR_Device.SelectedIndex] as DeviceSetting;
                    List<TRLog> logs = new List<TRLog>();
                    List<TRStateLog> stateLogs = new List<TRStateLog>();
                    if (cbet_TR_Value.SelectedIndex != 1)
                    {
                        layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        logs = SqlMethod.SearchTR(device, StartTime.ToString("yyyyMMddHHmmss"), EndTime.ToString("yyyyMMddHHmmss"));
                        gcl_TR.DataSource = logs;
                        ccl_TR.DataSource = logs;
                    }
                    else
                    {
                        layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        stateLogs = SqlMethod.SearchTRState(device, StartTime.ToString("yyyyMMddHHmmss"), EndTime.ToString("yyyyMMddHHmmss"));
                        gcl_TR.DataSource = stateLogs;
                    }
                    for (int i = 0; i < gvw_TR.Columns.Count(); i++)
                    {
                        gvw_TR.Columns[i].Visible = false;
                        gvw_TR.Columns[i].OptionsColumn.AllowEdit = false;
                    }
                    ccl_TR.Legend.Direction = LegendDirection.TopToBottom;//曲線圖線條說明的排序
                    ccl_TR.CrosshairOptions.CrosshairLabelMode = CrosshairLabelMode.ShowCommonForAllSeries; //顯示全部線條內容
                    ccl_TR.CrosshairOptions.LinesMode = CrosshairLinesMode.Auto;//自動獲取點上面的數值
                    ccl_TR.CrosshairOptions.GroupHeaderTextOptions.Font = new System.Drawing.Font("微軟正黑體", 12);
                    ccl_TR.CrosshairOptions.ShowArgumentLabels = true;//是否顯示Y軸垂直線
                    ccl_TR.SideBySideEqualBarWidth = false;//線條是否需要相等寬度
                    TRType type = (TRType)cbet_TR_Value.SelectedIndex;
                    switch (type)
                    {
                        case TRType.Temp:
                            {
                                #region 報表
                                gvw_TR.Columns["Temp1"].Caption = "變壓器溫度";
                                gvw_TR.Columns["Temp1"].Visible = true;
                                gvw_TR.Columns["Temp"].Caption = "環境溫度";
                                gvw_TR.Columns["Temp"].Visible = true;
                                gvw_TR.Columns["ttimen"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm";
                                gvw_TR.Columns["ttimen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                gvw_TR.Columns["ttimen"].Caption = "時間";
                                gvw_TR.Columns["ttimen"].Visible = true;
                                #endregion

                                #region 曲線圖
                                Series Value = new Series($"{device.DeviceName}-環境溫度", ViewType.Line);
                                Value.ArgumentDataMember = "ttimen";
                                Value.ValueDataMembers.AddRange(new string[] { "Temp" });
                                Value.CrosshairLabelPattern = "{S} {V:0.##} \xb0" + "C";
                                Series Value2 = new Series($"{device.DeviceName}-變壓器溫度", ViewType.Line);
                                Value2.ArgumentDataMember = "ttimen";
                                Value2.ValueDataMembers.AddRange(new string[] { "Temp1" });
                                Value2.CrosshairLabelPattern = "{S} {V:0.##} \xb0" + "C";
                                ccl_TR.Series.AddRange(Value);
                                ccl_TR.Series.AddRange(Value2);
                                #endregion
                            }
                            break;
                        case TRType.State:
                            {
                                #region 報表
                                gvw_TR.Columns["Message"].Caption = "警告訊息";
                                gvw_TR.Columns["Message"].Visible = true;
                                gvw_TR.Columns["ttimen"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm";
                                gvw_TR.Columns["ttimen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                                gvw_TR.Columns["ttimen"].Caption = "時間";
                                gvw_TR.Columns["ttimen"].Visible = true;
                                #endregion
                            }
                            break;
                    }
                    if (ccl_TR.DataSource != null)
                    {
                        XYDiagram diagram = (XYDiagram)ccl_TR.Diagram;
                        if (diagram != null)
                        {
                            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Minute; // 顯示設定
                            diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Minute; // 刻度設定
                            diagram.AxisX.Label.TextPattern = "{A:yyyy-MM-dd HH:mm}";//X軸顯示
                            diagram.AxisX.WholeRange.SideMarginsValue = 0;//不需要邊寬
                        }
                    }
                    CloseProgressPanel(handle);
                }
                else
                {
                    FlyoutAction action = new FlyoutAction();
                    action.Caption = "變壓器查詢報表錯誤";
                    action.Description = "請確認每一個條件是否選取完畢(不可跨年份)";
                    action.Commands.Add(FlyoutCommand.OK);
                    FlyoutDialog.Show(FindForm(), action);
                }
            };
            btn_TR_Export.Click += (s, e) =>
            {
                if (gvw_TR.DataSource != null)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Xlsx|*xlsx";
                    saveFileDialog.Title = "Export Data";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        gvw_TR.ExportToXlsx($"{saveFileDialog.FileName}.xlsx");
                    }
                }
                else
                {
                    FlyoutAction action = new FlyoutAction();
                    action.Caption = "變壓器報表資訊-匯出報表錯誤";
                    action.Description = "請查詢報表在進行匯出動作";
                    action.Commands.Add(FlyoutCommand.OK);
                    FlyoutDialog.Show(FindForm(), action);
                }
            };
            #endregion
            #region 年份報表按鈕
            btn_Year_Search.Click += (s, e) =>
            {
                if (det_Year.Text != "" && cbet_Year_Device.Text != "")
                {
                    handle = SplashScreenManager.ShowOverlayForm(FindForm());
                    List<Energydata> energydatas = new List<Energydata>();
                    if (gvw_Year.Columns.Count > 0)
                    {
                        gvw_Year.Columns.Clear();
                    }
                    StartTime = Convert.ToDateTime(det_Year.DateTime.ToString("yyyy/01/01 00:00:00"));
                    EndTime = Convert.ToDateTime(det_Year.DateTime.ToString("yyyy/12/31 23:59:59"));
                    if (DeviceSettings != null)
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            Energydata energydata = new Energydata()
                            {
                                ttimen = Convert.ToDateTime($"{StartTime.Year}/{i + 1}/01 00:00:00"),
                                ElectricTotal = 0,
                                TotalHour = 0
                            };
                            if (i < 9)
                            {
                                energydata.ttime = $"{StartTime.Year}0{i + 1}";
                            }
                            else
                            {
                                energydata.ttime = $"{StartTime.Year}{i + 1}";
                            }
                            energydatas.Add(energydata);
                        }
                        DeviceSetting Flow = cbet_Year_Device.Properties.Items[cbet_Year_Device.SelectedIndex] as DeviceSetting;

                        List<DeviceSetting> electricSetting = DeviceSettings.Where(g => g.GatewayIndex == Flow.GatewayIndex & Flow.ElectricIndex.Contains($"{ g.DeviceIndex}")).ToList();
                        if (electricSetting != null)
                        {
                            List<Flowtotal> flowTotalSettings = SqlMethod.SearchFlowTotal(Flow, StartTime.ToString("yyyyMMdd00"), EndTime.ToString("yyyyMMdd23"), 2);
                            List<ElectricTotal> electricTotals = new List<ElectricTotal>();
                            foreach (var item in electricSetting)
                            {
                                List<ElectricTotal> electricTotal = SqlMethod.SearchElectricTotal(item, StartTime.ToString("yyyyMMdd00"), EndTime.ToString("yyyyMMdd23"), 1);
                                electricTotals.AddRange(electricTotal);
                            }
                            foreach (var item in energydatas)
                            {
                                if (electricTotals != null)
                                {
                                    var electrictotal = electricTotals.Where(g => g.ttime == item.ttime).ToList();
                                    if (electrictotal != null)
                                    {
                                        foreach (var electrictotalitem in electrictotal)
                                        {
                                            item.ElectricTotal += electrictotalitem.KwhTotal;
                                        }
                                    }
                                }
                                if (flowTotalSettings != null)
                                {
                                    var flowtotal = flowTotalSettings.Where(g => Convert.ToInt32(g.ttime) >= Convert.ToInt32(item.ttime + "0100") && Convert.ToInt32(g.ttime) <= Convert.ToInt32(item.ttime + "3123")).ToList();
                                    //var flowtotal = flowTotalSettings.SingleOrDefault(g => g.ttime == item.ttime);
                                    if (flowtotal != null)
                                    {
                                        foreach (var dataitem in flowtotal)
                                        {
                                            if (dataitem.RTH > 0)
                                            {
                                                item.TotalHour += dataitem.RTH;
                                            }
                                        }
                                        //item.TotalHour = flowtotal.RTH;
                                    }
                                }
                            }
                        }
                        else
                        {
                            CloseProgressPanel(handle);
                            FlyoutAction action = new FlyoutAction();
                            action.Caption = "年份能源資訊-查詢報表錯誤";
                            action.Description = "設備未設定電表編號";
                            action.Commands.Add(FlyoutCommand.OK);
                            FlyoutDialog.Show(FindForm(), action);
                        }
                        gcl_Year.DataSource = energydatas;
                        for (int i = 0; i < gvw_Year.Columns.Count(); i++)
                        {
                            gvw_Year.Columns[i].OptionsColumn.AllowEdit = false;
                        }
                        gvw_Year.Columns["ttime"].Visible = false;
                        gvw_Year.Columns["ttimen"].Caption = "時間";
                        gvw_Year.Columns["ttimen"].DisplayFormat.FormatString = "yyyy/MM";
                        gvw_Year.Columns["ttimen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                        gvw_Year.Columns["ElectricTotal"].Caption = "月耗電量(kwh)";

                        gvw_Year.Columns["TotalHour"].Caption = "冰水機群組系統負荷(RTh)";
                        gvw_Year.Columns["Value"].Caption = "效率值(kw/RT)";

                        CloseProgressPanel(handle);
                    }
                }
                else
                {
                    CloseProgressPanel(handle);
                    FlyoutAction action = new FlyoutAction();
                    action.Caption = "年份能源資訊-查詢報表錯誤";
                    action.Description = "請確認每一個條件是否選取完畢";
                    action.Commands.Add(FlyoutCommand.OK);
                    FlyoutDialog.Show(FindForm(), action);
                }
            };
            btn_Year_Export.Click += (s, e) =>
            {
                if (gvw_Year.DataSource != null)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Xlsx|*xlsx";
                    saveFileDialog.Title = "Export Data";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        gvw_Year.ExportToXlsx($"{saveFileDialog.FileName}.xlsx");
                    }
                }
                else
                {
                    FlyoutAction action = new FlyoutAction();
                    action.Caption = "年度報表資訊-匯出報表錯誤";
                    action.Description = "請查詢報表在進行匯出動作";
                    action.Commands.Add(FlyoutCommand.OK);
                    FlyoutDialog.Show(FindForm(), action);
                }
            };
            #endregion
            #region 月份報表按鈕
            btn_Month_Search.Click += (s, e) =>
            {
                if (det_Month.Text != "" && cbet_Month_Device.Text != "")
                {
                    handle = SplashScreenManager.ShowOverlayForm(FindForm());
                    List<Energydata_v1> energydatas = new List<Energydata_v1>();
                    if (gvw_Month.Columns.Count > 0)
                    {
                        gvw_Month.Columns.Clear();
                    }
                    StartTime = Convert.ToDateTime(det_Month.DateTime.ToString("yyyy/MM/01 00:00:00"));
                    EndTime = Convert.ToDateTime(det_Month.DateTime.ToString("yyyy/MM") + $"/{DateTime.DaysInMonth(det_Month.DateTime.Year, det_Month.DateTime.Month)} 23:59:59");
                    if (DeviceSettings != null)
                    {
                        for (int i = 0; i < DateTime.DaysInMonth(det_Month.DateTime.Year, det_Month.DateTime.Month); i++)
                        {
                            Energydata_v1 energydata = new Energydata_v1()
                            {
                                ttime = $"{StartTime.Year}" + StartTime.Month.ToString().PadLeft(2, '0') + $"{i + 1}".PadLeft(2, '0'),
                                ttimen = Convert.ToDateTime($"{StartTime.Year}/" + StartTime.Month.ToString().PadLeft(2, '0') + $"/{i + 1}".PadLeft(2, '0') + " 00:00:00"),
                                Name = cbet_Month_Device.Text,
                                TotalHour0 = 0,
                                TotalHour1 = 0,
                                TotalHour2 = 0,
                                TotalHour3 = 0,
                                TotalHour4 = 0,
                                TotalHour5 = 0,
                                TotalHour6 = 0,
                                TotalHour7 = 0,
                                TotalHour8 = 0,
                                TotalHour9 = 0,
                                TotalHour10 = 0,
                                TotalHour11 = 0,
                                TotalHour12 = 0,
                                TotalHour13 = 0,
                                TotalHour14 = 0,
                                TotalHour15 = 0,
                                TotalHour16 = 0,
                                TotalHour17 = 0,
                                TotalHour18 = 0,
                                TotalHour19 = 0,
                                TotalHour20 = 0,
                                TotalHour21 = 0,
                                TotalHour22 = 0,
                                TotalHour23 = 0,
                                ElectricTotal = 0,
                                TotalHour = 0
                            };
                            energydatas.Add(energydata);
                        }
                        DeviceSetting Flow = cbet_Month_Device.Properties.Items[cbet_Month_Device.SelectedIndex] as DeviceSetting;
                        List<DeviceSetting> electricSetting = DeviceSettings.Where(g => g.GatewayIndex == Flow.GatewayIndex & Flow.ElectricIndex.Contains($"{g.DeviceIndex}")).ToList();
                        if (electricSetting != null)
                        {
                            List<Flowtotal> flowTotas = SqlMethod.SearchFlowTotal(Flow, StartTime.ToString("yyyyMMdd00"), EndTime.ToString("yyyyMMdd23"), 2);
                            List<ElectricTotal> electricTotals = new List<ElectricTotal>();
                            foreach (var item in electricSetting)
                            {
                                List<ElectricTotal> electricTotal = SqlMethod.SearchElectricTotal(item, StartTime.ToString("yyyyMMdd00"), EndTime.ToString("yyyyMMdd23"), 0);
                                electricTotals.AddRange(electricTotal);
                            }
                            foreach (var item in energydatas)
                            {
                                if (electricTotals != null)
                                {
                                    var electrictotal = electricTotals.Where(g => g.ttime == item.ttime).ToList();
                                    if (electrictotal != null)
                                    {
                                        foreach (var electrictotalitem in electrictotal)
                                        {
                                            item.ElectricTotal += electrictotalitem.KwhTotal;
                                        }
                                    }
                                }
                                if (flowTotas != null)
                                {
                                    var flowtotal = flowTotas.Where(g => Convert.ToInt32(g.ttime) >= Convert.ToInt32(item.ttime + "00") && Convert.ToInt32(g.ttime) <= Convert.ToInt32(item.ttime + "23")).ToList();
                                    //var flowtotal = flowTotalSettings.SingleOrDefault(g => g.ttime == item.ttime);
                                    if (flowtotal != null)
                                    {
                                        foreach (var dataitem in flowtotal)
                                        {
                                            Energydata_refresh(item, dataitem);
                                            if (dataitem.RTH > 0)
                                            {
                                                item.TotalHour += dataitem.RTH;
                                            }
                                        }
                                        //item.TotalHour = flowtotal.RTH;
                                    }
                                }
                            }
                        }
                        else
                        {
                            CloseProgressPanel(handle);
                            FlyoutAction action = new FlyoutAction();
                            action.Caption = "月份能源資訊-查詢報表錯誤";
                            action.Description = "設備未設定電表編號";
                            action.Commands.Add(FlyoutCommand.OK);
                            FlyoutDialog.Show(FindForm(), action);
                        }
                        gcl_Month.DataSource = energydatas;
                        gvw_Month.OptionsSelection.EnableAppearanceFocusedCell = false;
                        gvw_Month.OptionsView.ColumnAutoWidth = false;
                        gvw_Month.Columns["ttime"].Visible = false;
                        gvw_Month.Columns["ttimen"].Caption = "日期/時間";
                        gvw_Month.Columns["ttimen"].DisplayFormat.FormatString = "M月dd日";
                        gvw_Month.Columns["ttimen"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                        gvw_Month.Columns["TotalHour0"].Caption = "00:00~01:00";
                        gvw_Month.Columns["TotalHour1"].Caption = "01:00~02:00";
                        gvw_Month.Columns["TotalHour2"].Caption = "02:00~03:00";
                        gvw_Month.Columns["TotalHour3"].Caption = "03:00~04:00";
                        gvw_Month.Columns["TotalHour4"].Caption = "04:00~05:00";
                        gvw_Month.Columns["TotalHour5"].Caption = "05:00~06:00";
                        gvw_Month.Columns["TotalHour6"].Caption = "06:00~07:00";
                        gvw_Month.Columns["TotalHour7"].Caption = "07:00~08:00";
                        gvw_Month.Columns["TotalHour8"].Caption = "08:00~09:00";
                        gvw_Month.Columns["TotalHour9"].Caption = "09:00~10:00";
                        gvw_Month.Columns["TotalHour10"].Caption = "10:00~11:00";
                        gvw_Month.Columns["TotalHour11"].Caption = "11:00~12:00";
                        gvw_Month.Columns["TotalHour12"].Caption = "12:00~13:00";
                        gvw_Month.Columns["TotalHour13"].Caption = "13:00~14:00";
                        gvw_Month.Columns["TotalHour14"].Caption = "14:00~15:00";
                        gvw_Month.Columns["TotalHour15"].Caption = "15:00~16:00";
                        gvw_Month.Columns["TotalHour16"].Caption = "16:00~17:00";
                        gvw_Month.Columns["TotalHour17"].Caption = "17:00~18:00";
                        gvw_Month.Columns["TotalHour18"].Caption = "18:00~19:00";
                        gvw_Month.Columns["TotalHour19"].Caption = "19:00~20:00";
                        gvw_Month.Columns["TotalHour20"].Caption = "20:00~21:00";
                        gvw_Month.Columns["TotalHour21"].Caption = "21:00~22:00";
                        gvw_Month.Columns["TotalHour22"].Caption = "22:00~23:00";
                        gvw_Month.Columns["TotalHour23"].Caption = "23:00~24:00";

                        gvw_Month.Columns["ElectricTotal"].Caption = "當日累積(kwh)";
                        gvw_Month.Columns["Name"].Caption = "設備名稱 ";
                        gvw_Month.Columns["Name"].Group();
                        if (gvw_Month.GroupSummary.Count > 0)
                        {
                            gvw_Month.GroupSummary.Clear();
                        }

                        gvw_Month.Columns["TotalHour"].Caption = "當日累積(RTh)";
                        gvw_Month.GroupSummary.Add(new GridGroupSummaryItem()
                        {
                            FieldName = "TotalHour",
                            SummaryType = SummaryItemType.Sum,
                            DisplayFormat = "當月累積(RTh) : {0:n2}",
                            ShowInGroupColumnFooter = gvw_Month.Columns["TotalHour"]
                        });
                        gvw_Month.GroupSummary.Add(new GridGroupSummaryItem()
                        {
                            FieldName = "ElectricTotal",
                            SummaryType = SummaryItemType.Sum,
                            DisplayFormat = "當月累積(kwh) : {0:n2}",
                            ShowInGroupColumnFooter = gvw_Month.Columns["ElectricTotal"]
                        });

                        for (int i = 0; i < gvw_Month.Columns.Count(); i++)
                        {
                            gvw_Month.Columns[i].BestFit();
                            gvw_Month.Columns[i].OptionsColumn.AllowEdit = false;
                        }
                        gvw_Month.ExpandAllGroups();
                        CloseProgressPanel(handle);
                    }
                }
                else
                {
                    CloseProgressPanel(handle);
                    FlyoutAction action = new FlyoutAction();
                    action.Caption = "月份能源資訊-查詢報表錯誤";
                    action.Description = "請確認每一個條件是否選取完畢";
                    action.Commands.Add(FlyoutCommand.OK);
                    FlyoutDialog.Show(FindForm(), action);
                }
            };
            btn_Month_Export.Click += (s, e) =>
            {
                if (gvw_Month.DataSource != null)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Xlsx|*xlsx";
                    saveFileDialog.Title = "Export Data";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        gvw_Month.ExportToXlsx($"{saveFileDialog.FileName}.xlsx");
                    }
                }
                else
                {
                    FlyoutAction action = new FlyoutAction();
                    action.Caption = "月份報表資訊-匯出報表錯誤";
                    action.Description = "請查詢報表在進行匯出動作";
                    action.Commands.Add(FlyoutCommand.OK);
                    FlyoutDialog.Show(FindForm(), action);
                }
            };
            #endregion
        }
        private bool Check_Time(DateEdit Start, DateEdit End)
        {
            bool Flag = false;
            if (!string.IsNullOrEmpty(Start.Text) & !string.IsNullOrEmpty(End.Text))
            {
                var startTime = Convert.ToDateTime(Start.DateTime.ToString("yyyy/MM/dd 00:00:00"));
                var endTime = Convert.ToDateTime(End.DateTime.ToString("yyyy/MM/dd 23:59:59"));
                if (startTime.Year == endTime.Year & startTime <= endTime)
                {
                    StartTime = startTime;
                    EndTime = endTime;
                    Flag = true;
                }
            }
            return Flag;
        }
        private bool Check_Device(ComboBoxEdit Device, ComboBoxEdit Value)
        {
            bool Flag = false;
            if (!string.IsNullOrEmpty(Device.Text) & !string.IsNullOrEmpty(Value.Text))
            {
                Flag = true;
            }
            return Flag;
        }
        #region 效率直物件

        private class Energydata
        {
            /// <summary>
            /// 時間
            /// </summary>
            public string ttime { get; set; }
            /// <summary>
            /// 時間
            /// </summary>
            public DateTime ttimen { get; set; }
            /// <summary>
            /// 月耗電量
            /// </summary>
            public decimal ElectricTotal { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/月供氣量
            /// </summary>
            public decimal TotalHour { get; set; }
            /// <summary>
            /// 效率值
            /// </summary>
            public decimal Value
            {
                get
                {
                    decimal data = 0;
                    if (TotalHour != 0)
                    {
                        data = Math.Round(ElectricTotal / TotalHour, 2);
                    }
                    return data;
                }
            }
        }
        private class Energydata_v1
        {
            /// <summary>
            /// 時間
            /// </summary>
            public string ttime { get; set; }
            public DateTime ttimen { get; set; }
            /// <summary>
            /// 名字
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour0 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour1 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour2 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour3 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour4 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour5 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour6 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour7 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour8 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour9 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour10 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour11 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour12 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour13 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour14 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour15 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour16 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour17 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour18 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour19 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour20 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour21 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour22 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/時供氣量
            /// </summary>
            public decimal TotalHour23 { get; set; }
            /// <summary>
            /// 冰機群組系統負荷/日供氣量
            /// </summary>
            public decimal TotalHour { get; set; }
            /// <summary>
            /// 日耗電量
            /// </summary>
            public decimal ElectricTotal { get; set; }
        }
        private void Energydata_refresh(Energydata_v1 item, Flowtotal dataitem)
        {
            switch (Convert.ToInt32(dataitem.ttime.Substring(8, 2)))
            {
                case 0:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour0 = dataitem.RTH;
                        }
                    }
                    break;
                case 1:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour1 = dataitem.RTH;
                        }
                    }
                    break;
                case 2:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour2 = dataitem.RTH;
                        }
                    }
                    break;
                case 3:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour3 = dataitem.RTH;
                        }
                    }
                    break;
                case 4:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour4 = dataitem.RTH;
                        }
                    }
                    break;
                case 5:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour5 = dataitem.RTH;
                        }
                    }
                    break;
                case 6:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour6 = dataitem.RTH;
                        }
                    }
                    break;
                case 7:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour7 = dataitem.RTH;
                        }
                    }
                    break;
                case 8:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour8 = dataitem.RTH;
                        }
                    }
                    break;
                case 9:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour9 = dataitem.RTH;
                        }
                    }
                    break;
                case 10:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour10 = dataitem.RTH;
                        }
                    }
                    break;
                case 11:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour11 = dataitem.RTH;
                        }
                    }
                    break;
                case 12:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour12 = dataitem.RTH;
                        }
                    }
                    break;
                case 13:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour13 = dataitem.RTH;
                        }
                    }
                    break;
                case 14:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour14 = dataitem.RTH;
                        }
                    }
                    break;
                case 15:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour15 = dataitem.RTH;
                        }
                    }
                    break;
                case 16:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour16 = dataitem.RTH;
                        }
                    }
                    break;
                case 17:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour17 = dataitem.RTH;
                        }
                    }
                    break;
                case 18:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour18 = dataitem.RTH;
                        }
                    }
                    break;
                case 19:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour19 = dataitem.RTH;
                        }
                    }
                    break;
                case 20:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour20 = dataitem.RTH;
                        }
                    }
                    break;
                case 21:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour21 = dataitem.RTH;
                        }
                    }
                    break;
                case 22:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour22 = dataitem.RTH;
                        }
                    }
                    break;
                case 23:
                    {
                        if (dataitem.RTH > 0)
                        {
                            item.TotalHour23 = dataitem.RTH;
                        }
                    }
                    break;
            }
        }
        #endregion
    }
}
