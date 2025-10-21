using System;
using System.Windows.Forms;

namespace ExperimentalManagementSystem
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login()); // 启动登录窗体
        }
    }
}