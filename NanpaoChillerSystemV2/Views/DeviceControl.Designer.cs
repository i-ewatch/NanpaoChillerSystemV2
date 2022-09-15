
namespace NanpaoChillerSystemV2.Views
{
    partial class DeviceControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraGauges.Core.Model.IndicatorState indicatorState1 = new DevExpress.XtraGauges.Core.Model.IndicatorState();
            DevExpress.XtraGauges.Core.Model.IndicatorState indicatorState2 = new DevExpress.XtraGauges.Core.Model.IndicatorState();
            DevExpress.XtraGauges.Core.Model.IndicatorState indicatorState3 = new DevExpress.XtraGauges.Core.Model.IndicatorState();
            DevExpress.XtraGauges.Core.Model.IndicatorState indicatorState4 = new DevExpress.XtraGauges.Core.Model.IndicatorState();
            this.gcl_Device = new DevExpress.XtraEditors.GroupControl();
            this.gaugeControl1 = new DevExpress.XtraGauges.Win.GaugeControl();
            this.stateIndicatorGauge1 = new DevExpress.XtraGauges.Win.Gauges.State.StateIndicatorGauge();
            this.stateIndicatorComponent1 = new DevExpress.XtraGauges.Win.Gauges.State.StateIndicatorComponent();
            this.lbl_Time = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gcl_Device)).BeginInit();
            this.gcl_Device.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stateIndicatorGauge1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stateIndicatorComponent1)).BeginInit();
            this.SuspendLayout();
            // 
            // gcl_Device
            // 
            this.gcl_Device.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 12F);
            this.gcl_Device.AppearanceCaption.Options.UseFont = true;
            this.gcl_Device.Controls.Add(this.gaugeControl1);
            this.gcl_Device.Controls.Add(this.lbl_Time);
            this.gcl_Device.Controls.Add(this.labelControl1);
            this.gcl_Device.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcl_Device.Location = new System.Drawing.Point(0, 0);
            this.gcl_Device.Name = "gcl_Device";
            this.gcl_Device.Size = new System.Drawing.Size(273, 78);
            this.gcl_Device.TabIndex = 0;
            this.gcl_Device.Text = "N/A";
            // 
            // gaugeControl1
            // 
            this.gaugeControl1.BackColor = System.Drawing.Color.Transparent;
            this.gaugeControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gaugeControl1.Gauges.AddRange(new DevExpress.XtraGauges.Base.IGauge[] {
            this.stateIndicatorGauge1});
            this.gaugeControl1.Location = new System.Drawing.Point(227, 14);
            this.gaugeControl1.Name = "gaugeControl1";
            this.gaugeControl1.Size = new System.Drawing.Size(43, 44);
            this.gaugeControl1.TabIndex = 2;
            // 
            // stateIndicatorGauge1
            // 
            this.stateIndicatorGauge1.Bounds = new System.Drawing.Rectangle(6, 6, 31, 32);
            this.stateIndicatorGauge1.Indicators.AddRange(new DevExpress.XtraGauges.Win.Gauges.State.StateIndicatorComponent[] {
            this.stateIndicatorComponent1});
            this.stateIndicatorGauge1.Name = "stateIndicatorGauge1";
            // 
            // stateIndicatorComponent1
            // 
            this.stateIndicatorComponent1.Center = new DevExpress.XtraGauges.Core.Base.PointF2D(124F, 124F);
            this.stateIndicatorComponent1.Name = "stateIndicatorComponent1";
            this.stateIndicatorComponent1.Size = new System.Drawing.SizeF(200F, 200F);
            this.stateIndicatorComponent1.StateIndex = 0;
            indicatorState1.Name = "State1";
            indicatorState1.ShapeType = DevExpress.XtraGauges.Core.Model.StateIndicatorShapeType.ElectricLight1;
            indicatorState2.Name = "State2";
            indicatorState2.ShapeType = DevExpress.XtraGauges.Core.Model.StateIndicatorShapeType.ElectricLight2;
            indicatorState3.Name = "State3";
            indicatorState3.ShapeType = DevExpress.XtraGauges.Core.Model.StateIndicatorShapeType.ElectricLight3;
            indicatorState4.Name = "State4";
            indicatorState4.ShapeType = DevExpress.XtraGauges.Core.Model.StateIndicatorShapeType.ElectricLight4;
            this.stateIndicatorComponent1.States.AddRange(new DevExpress.XtraGauges.Core.Model.IIndicatorState[] {
            indicatorState1,
            indicatorState2,
            indicatorState3,
            indicatorState4});
            // 
            // lbl_Time
            // 
            this.lbl_Time.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lbl_Time.Appearance.Options.UseFont = true;
            this.lbl_Time.Appearance.Options.UseTextOptions = true;
            this.lbl_Time.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lbl_Time.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lbl_Time.Location = new System.Drawing.Point(11, 55);
            this.lbl_Time.Name = "lbl_Time";
            this.lbl_Time.Size = new System.Drawing.Size(254, 19);
            this.lbl_Time.TabIndex = 1;
            this.lbl_Time.Text = "-";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(14, 26);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(96, 19);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "最後連線時間";
            // 
            // DeviceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gcl_Device);
            this.Name = "DeviceControl";
            this.Size = new System.Drawing.Size(273, 78);
            ((System.ComponentModel.ISupportInitialize)(this.gcl_Device)).EndInit();
            this.gcl_Device.ResumeLayout(false);
            this.gcl_Device.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stateIndicatorGauge1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stateIndicatorComponent1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl gcl_Device;
        private DevExpress.XtraEditors.LabelControl lbl_Time;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGauges.Win.GaugeControl gaugeControl1;
        private DevExpress.XtraGauges.Win.Gauges.State.StateIndicatorGauge stateIndicatorGauge1;
        private DevExpress.XtraGauges.Win.Gauges.State.StateIndicatorComponent stateIndicatorComponent1;
    }
}
