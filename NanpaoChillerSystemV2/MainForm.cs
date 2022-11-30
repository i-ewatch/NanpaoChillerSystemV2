using DevExpress.XtraBars.Docking2010.Customization;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
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
                ButtonMethod = new ButtonMethod() { navigationFrame = NavigationFrame };
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
            UserControl control = new UserControl() { Padding = new Padding(0, 30, 0, 20), Size = new Size(400, 200) };
            DevExpress.XtraEditors.TextEdit textEdit = new DevExpress.XtraEditors.TextEdit() { Dock = DockStyle.Top, Size = new Size(400, 40) };
            textEdit.Properties.Appearance.FontSizeDelta = 12;
            textEdit.Properties.Appearance.Options.UseFont = true;
            textEdit.Properties.Appearance.Options.UseTextOptions = true;
            textEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            textEdit.Parent = control;
            textEdit.Properties.UseSystemPasswordChar = true;
            LabelControl labelControl = new LabelControl() { Dock = DockStyle.Top, Size = new Size(400, 50) };
            labelControl.Appearance.FontSizeDelta = 18;
            labelControl.AutoSizeMode = LabelAutoSizeMode.None;
            labelControl.Text = "請輸入關閉軟體密碼";
            labelControl.Appearance.Options.UseFont = true;
            labelControl.Appearance.Options.UseTextOptions = true;
            labelControl.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            labelControl.Parent = control;
            SimpleButton okButton = new SimpleButton() { Dock = DockStyle.Bottom, Text = "確定", Size = new Size(400, 40) };
            okButton.Appearance.BackColor = Color.FromArgb(80, 80, 80);
            okButton.Appearance.FontSizeDelta = 12;
            okButton.DialogResult = DialogResult.OK;
            okButton.Parent = control;
            if (FlyoutDialog.Show(FindForm(), control) == DialogResult.OK && string.Compare(textEdit.Text, "qu!t", true) == 0)
            {
                foreach (var item in Field4Components)
                {
                    item.MyWorkState = false;
                }
                SlaveTCPComponent.MyWorkState = false;
                UpDataComponent.MyWorkState = false;
                timer1.Enabled = false;
                Application.ExitThread();
                this.Dispose();
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
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

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon1.Visible = true;
            }
            else
            {
                this.notifyIcon1.Visible = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
    }
}
