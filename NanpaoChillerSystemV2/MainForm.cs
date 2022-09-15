using DevExpress.XtraBars.Docking2010.Customization;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraBars.Navigation;
using NanpaoChillerSystemV2.Components;
using NanpaoChillerSystemV2.Configuration;
using NanpaoChillerSystemV2.EF_Modules;
using NanpaoChillerSystemV2.Methods;
using NanpaoChillerSystemV2.Protocols;
using NanpaoChillerSystemV2.Views;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NanpaoChillerSystemV2
{
    public partial class MainForm : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        private SystemSetting SystemSetting { get; set; }
        private SlaveSetting SlaveSetting { get; set; }
        private UpToEwatchSetting UpToEwatchSetting { get; set; }
        private ButtonSetting ButtonSetting { get; set; }
        private SqlMethod SqlMethod { get; set; }
        private ExcelMethod ExcelMethod { get; set; }
        private List<GatewaySetting> GatewaySettings { get; set; }
        private List<DeviceSetting> DeviceSettings { get; set; }
        private List<LineNotifySetting> LineNotifySettings { get; set; }
        private List<AbsProtocol> AbsProtocols { get; set; } = new List<AbsProtocol>();
        private List<Field4Component> Field4Components { get; set; } = new List<Field4Component>();
        private SlaveTCPComponent SlaveTCPComponent { get; set; }
        private UpDataComponent UpDataComponent { get; set; }
        /// <summary>
        /// 畫面切換功能
        /// </summary>
        private NavigationFrame NavigationFrame { get; set; } = null;
        private ButtonMethod ButtonMethod { get; set; }
        public List<Field4Control> Field4Controls { get; set; } = new List<Field4Control>();
        public MainForm()
        {
            InitializeComponent();
            #region Serilog
            Log.Logger = new LoggerConfiguration()
                       .WriteTo.Console()
                       .WriteTo.File(path: $"{AppDomain.CurrentDomain.BaseDirectory}\\log\\log.txt",
                                     rollingInterval: RollingInterval.Day,
                                     outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                       .CreateLogger();        //宣告Serilog初始化
            #endregion
            CloseBox = false;
            MaximizeBox = false;
            SystemSetting = InitailMethod.System_Load();
            SlaveSetting = InitailMethod.Slave_Load();
            UpToEwatchSetting = InitailMethod.UpToEwatch_Load();
            ButtonSetting = InitailMethod.Button_Load();
            if (SystemSetting != null)
            {
                SqlMethod = new SqlMethod(SystemSetting);
                ExcelMethod = new ExcelMethod(SqlMethod);
                GatewaySettings = SqlMethod.Search_GatewaySetting();
                DeviceSettings = SqlMethod.Search_DeviceSetting();
                LineNotifySettings = SqlMethod.Search_LineNotifySetting();
                foreach (var item in GatewaySettings)
                {
                    List<DeviceSetting> deviceSettings = DeviceSettings.Where(g => g.GatewayIndex == item.GatewayIndex).ToList();
                    TCPComponent component = new TCPComponent(item, deviceSettings, SqlMethod, LineNotifySettings);
                    component.MyWorkState = true;
                    AbsProtocols.AddRange(component.AbsProtocols);
                    Field4Components.Add(component);
                }
                #region View
                NavigationFrame = new NavigationFrame() { Dock = System.Windows.Forms.DockStyle.Fill, Parent = panelControl1 };
                ButtonMethod = new ButtonMethod() { navigationFrame = NavigationFrame};
                ButtonMethod.AccordionLoad(accordionControl1, ButtonSetting);

                ProtocolConnectControl protocol = new ProtocolConnectControl(AbsProtocols, DeviceSettings) { Dock = System.Windows.Forms.DockStyle.Fill };
                Field4Controls.Add(protocol);
                NavigationFrame.AddPage(protocol);
                ReportControl report = new ReportControl(DeviceSettings, SqlMethod) { Dock = System.Windows.Forms.DockStyle.Fill };
                Field4Controls.Add(report);
                NavigationFrame.AddPage(report);
                #endregion

                SlaveTCPComponent = new SlaveTCPComponent(SlaveSetting, AbsProtocols);
                SlaveTCPComponent.MyWorkState = true;

                UpDataComponent = new UpDataComponent(AbsProtocols, SqlMethod, UpToEwatchSetting);
                UpDataComponent.MyWorkState = true;

                timer1.Interval = 1000;
                timer1.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ButtonMethod.ViewIndex < Field4Controls.Count)
            {
                Field4Controls[ButtonMethod.ViewIndex].TextChange();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var item in Field4Components)
            {
                item.MyWorkState = false;
            }
            SlaveTCPComponent.MyWorkState = false;
            UpDataComponent.MyWorkState = false;
            timer1.Enabled = false;
            Application.ExitThread();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Size = new Size(1280, 768);
        }

        private void bbtn_Import_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (var item in Field4Components)
            {
                item.MyWorkState = false;
            }
            SlaveTCPComponent.MyWorkState = false;
            UpDataComponent.MyWorkState = false;
            timer1.Enabled = false;
            if (ExcelMethod.Excel_Load())
            {
                FlyoutAction action = new FlyoutAction();
                action.Caption = "設備資料匯入";
                action.Description = "匯入完成，請重新啟動!!";
                action.Commands.Add(FlyoutCommand.OK);
                FlyoutDialog.Show(FindForm(), action);
                Application.ExitThread();
            }
            else
            {
                FlyoutAction action = new FlyoutAction();
                action.Caption = "設備資料匯入";
                action.Description = "匯入失敗!!";
                action.Commands.Add(FlyoutCommand.OK);
                FlyoutDialog.Show(FindForm(), action);
            }
        }
    }
}
