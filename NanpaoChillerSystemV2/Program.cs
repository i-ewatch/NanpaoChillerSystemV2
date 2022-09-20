using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NanpaoChillerSystemV2
{
    static class Program
    {
        #region 使用的Win32函式的宣告

        /// <summary>
        /// 找到某個視窗與給出的類別名和視窗名相同視窗
        /// 非託管定義為：http://msdn.microsoft.com/en-us/library/windows/desktop/ms633499(v=vs.85).aspx
        /// </summary>
        /// <param name="lpClassName">類別名</param>
        /// <param name="lpWindowName">視窗名</param>
        /// <returns>成功找到返回視窗控制代碼,否則返回null</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 切換到視窗並把視窗設入前臺,類似 SetForegroundWindow方法的功能
        /// </summary>
        /// <param name="hWnd">視窗控制代碼</param>
        /// <param name="fAltTab">True代表視窗正在通過Alt/Ctrl +Tab被切換</param>
        [DllImport("user32.dll ", SetLastError = true)]
        static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        ///// <summary>
        ///// 設定視窗的顯示狀態
        ///// Win32 函式定義為：http://msdn.microsoft.com/en-us/library/windows/desktop/ms633548(v=vs.85).aspx
        ///// </summary>
        ///// <param name="hWnd">視窗控制代碼</param>
        ///// <param name="cmdShow">指示視窗如何被顯示</param>
        ///// <returns>如果窗體之前是可見，返回值為非零；如果窗體之前被隱藏，返回值為零</returns>
        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
        public const int SW_RESTORE = 9;
        public static IntPtr formhwnd;
        #endregion
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Process process = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(process.ProcessName);
            // 該程式已經執行 
            if (processes.Length > 1)
            {
                foreach (Process processitem in processes)
                {
                    if (processitem.Id != process.Id)
                    {
                        // 如果程序的控制代碼為0，即代表沒有找到該窗體，即該窗體隱藏的情況時
                        if (processitem.MainWindowHandle.ToInt32() == 0)
                        {
                            // 重新顯示該窗體並切換到帶入到前臺
                            formhwnd = FindWindow(null, Assembly.GetEntryAssembly().GetName().Name);
                            ShowWindow(formhwnd, SW_RESTORE);
                            SwitchToThisWindow(formhwnd, true);
                        }
                        else
                        {
                            // 如果窗體沒有隱藏，就直接切換到該窗體並帶入到前臺
                            // 因為窗體除了隱藏到托盤，還可以最小化
                            SwitchToThisWindow(processitem.MainWindowHandle, true);
                        }
                    }
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
    }
}
