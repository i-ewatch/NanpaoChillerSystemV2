using NanpaoChillerSystemV2.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace NanpaoChillerSystemV2.Methods
{
    public class InitailMethod
    {
        /// <summary>
        /// 工作路徑
        /// </summary>
        private static readonly string WorkPath = AppDomain.CurrentDomain.BaseDirectory;
        public static SystemSetting System_Load()
        {
            if (!Directory.Exists($"{WorkPath}\\stf"))
                Directory.CreateDirectory($"{WorkPath}\\stf");
            string setFile = $"{WorkPath}\\stf\\system.json";
            SystemSetting setting = null;
            if (File.Exists(setFile))
            {
                string json = File.ReadAllText(setFile, Encoding.UTF8);
                setting = JsonConvert.DeserializeObject<SystemSetting>(json);
            }
            else
            {
                setting = new SystemSetting
                {
                    ServerIp = "127.0.0.1",
                    Database = "NanpaoChillerV2",
                    UserId = "sa",
                    UserPwd = "1234",
                };
                string output = JsonConvert.SerializeObject(setting, Formatting.Indented, new JsonSerializerSettings());
                File.WriteAllText(setFile, output, Encoding.UTF8);
            }
            return setting;
        }
        public static SlaveSetting Slave_Load()
        {
            if (!Directory.Exists($"{WorkPath}\\stf"))
                Directory.CreateDirectory($"{WorkPath}\\stf");
            string setFile = $"{WorkPath}\\stf\\Slave.json";
            SlaveSetting setting = null;
            if (File.Exists(setFile))
            {
                string json = File.ReadAllText(setFile, Encoding.UTF8);
                setting = JsonConvert.DeserializeObject<SlaveSetting>(json);
            }
            else
            {
                setting = new SlaveSetting
                {
                    SlaveIP = "127.0.0.1",
                    SlavePort = 502
                };
                string output = JsonConvert.SerializeObject(setting, Formatting.Indented, new JsonSerializerSettings());
                File.WriteAllText(setFile, output, Encoding.UTF8);
            }
            return setting;
        }
        public static UpToEwatchSetting UpToEwatch_Load()
        {
            if (!Directory.Exists($"{WorkPath}\\stf"))
                Directory.CreateDirectory($"{WorkPath}\\stf");
            string setFile = $"{WorkPath}\\stf\\UpToEwatch.json";
            UpToEwatchSetting setting = null;
            if (File.Exists(setFile))
            {
                string json = File.ReadAllText(setFile, Encoding.UTF8);
                setting = JsonConvert.DeserializeObject<UpToEwatchSetting>(json);
            }
            else
            {
                setting = new UpToEwatchSetting
                {
                    SendFlag = true,
                    IP = "127.0.0.1",
                    Port = 502
                };
                string output = JsonConvert.SerializeObject(setting, Formatting.Indented, new JsonSerializerSettings());
                File.WriteAllText(setFile, output, Encoding.UTF8);
            }
            return setting;
        }
        #region 按鈕Json 建檔與讀取
        /// <summary>
        /// 按鈕Json 建檔與讀取
        /// </summary>
        /// <returns></returns>
        public static ButtonSetting Button_Load()
        {
            ButtonSetting setting = null;
            if (!Directory.Exists($"{WorkPath}\\stf"))
                Directory.CreateDirectory($"{WorkPath}\\stf");
            string SettingPath = $"{WorkPath}\\stf\\button.json";
            if (File.Exists(SettingPath))
            {
                string json = File.ReadAllText(SettingPath, Encoding.UTF8);
                setting = JsonConvert.DeserializeObject<ButtonSetting>(json);
            }
            else
            {
                ButtonSetting Setting = new ButtonSetting()
                {
                    //群組與列表按鈕設定
                    ButtonGroupSettings =
                        {
                            new ButtonGroupSetting()
                            {
                                // 0 = 群組，1 = 列表
                                ButtonStyle = 1,
                                //群組名稱
                                GroupName = "群組名稱",
                                // 群組標註
                                GroupTag = 0,
                                //列表按鈕設定
                                ButtonItemSettings=
                                {
                                    new ButtonItemSetting()
                                    {
                                        //列表名稱
                                        ItemName = "列表名稱",
                                        //列表標註
                                        ItemTag = 0,
                                        //控制畫面顯示
                                        ControlVisible = true
                                    }
                                }
                            }
                        }
                };
                setting = Setting;
                string output = JsonConvert.SerializeObject(setting, Formatting.Indented, new JsonSerializerSettings());
                File.WriteAllText(SettingPath, output);
            }

            return setting;
        }
        #endregion
    }
}
