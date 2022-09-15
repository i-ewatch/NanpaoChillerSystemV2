using System.Collections.Generic;

namespace NanpaoChillerSystemV2.Configuration
{
    public class ButtonSetting
    {
        public List<ButtonGroupSetting> ButtonGroupSettings = new List<ButtonGroupSetting>();
    }
    /// <summary>
    /// 按鈕群組Data
    /// </summary>
    public class ButtonGroupSetting
    {
        /// <summary>
        /// 按鈕類型
        /// <para>1 = 不建立群組</para>
        /// <para>0 = 建立群組</para>
        /// </summary>
        public int ButtonStyle { get; set; }
        /// <summary>
        /// 群組名稱
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 群組類型
        /// </summary>
        public int GroupTag { get; set; }
        /// <summary>
        /// 列表名稱
        /// </summary>
        public List<ButtonItemSetting> ButtonItemSettings { get; set; } = new List<ButtonItemSetting>();
    }
    /// <summary>
    /// 按鈕列表Data
    /// </summary>
    public class ButtonItemSetting
    {
        /// <summary>
        /// 列表名稱
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 按鈕分頁編號
        /// </summary>
        public int ItemTag { get; set; }
        /// <summary>
        /// 控制畫面顯示
        /// </summary>
        public bool ControlVisible { get; set; }
    }
}
