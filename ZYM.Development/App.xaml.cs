using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ZYM.Development.Views;

namespace ZYM.Development
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static double Wdith
        {
            get { return SystemParameters.WorkArea.Width; }
        }
        public static double Height
        {
            get { return SystemParameters.WorkArea.Height; }
        }

        public static double WindowsWidth
        {
            get { return SystemParameters.WorkArea.Width / 1.5; }
        }

        public static double WindowHeight
        {
            get { return SystemParameters.WorkArea.Height / 1.5; }
        }

        public static Window CurrentMainWindow
        {
            get { return Current.MainWindow; }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Process[] ps = Process.GetProcessesByName("ZYM.Development");
            if (null != ps && ps.Count() > 1)
            {
                foreach (var p in ps)
                {
                    if (p == Process.GetCurrentProcess()) continue;
                    p.Kill();
                }
            }
            new MainWindow().Show();
            base.OnStartup(e);
        }
    }
}
