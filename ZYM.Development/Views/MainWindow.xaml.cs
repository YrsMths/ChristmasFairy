using Community.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZYM.Development.Views.Christmas;

namespace ZYM.Development.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Window window;

        Random _random = new Random();

        private IntPtr programHandle;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainLoaded;
            Closed += MainClosed;
        }

        private void MainLoaded(object sender, RoutedEventArgs e)
        {
            var type = Type.GetType(Properties.Settings.Default.LastWindow);
            if (type != null)
            {
                window = System.Activator.CreateInstance(type) as BasicWindow;
                window.Show();
            }
            else
            {
                Twikle_Click(null, null);
            }
            this.Hide();
        }

        private void MainClosed(object sender, EventArgs e)
        {
            Properties.Settings.Default.LastWindow = window.GetType().ToString();
            Properties.Settings.Default.Save();
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        private void Twikle_Click(object sender, RoutedEventArgs e)
        {
            if (null != window)
            {
                window.Close();
                window = null;
                GC.Collect();
            }
            Type[] types = (from t in Assembly.Load("ZYM.Development").GetTypes()
                    where t.IsClass && t.Namespace == "ZYM.Development.Views.Christmas"
                            select t).ToArray();
            window = System.Activator.CreateInstance(types[_random.Next(0, types.Length)]) as BasicWindow;
            if (null == window) return;
            window.Loaded -= Desktop_Loaded;
            window.Loaded += Desktop_Loaded;
            window.Owner = this;
            window.Show();
        }

        private void SetText_Click(object sender, RoutedEventArgs e)
        {
            SantaWindow setWindow = new SantaWindow(window.FontSize, window.RenderSize);
            setWindow.Event_Yes += (s1, e1) =>
            {
                (window as BasicWindow).Refresh();
            };
            setWindow.ShowDialog();
        }

        private void Desktop_Loaded(object sender, RoutedEventArgs e)
        {
            SendMsgToProgman();
            Win32ApiHelper.SetParent(new WindowInteropHelper(window).Handle, programHandle);
            App.CurrentMainWindow.WindowState = WindowState.Minimized;
        }

        void SendMsgToProgman()
        {
            // 桌面窗口句柄，在外部定义，用于后面将我们自己的窗口作为子窗口放入
            programHandle = Win32ApiHelper.FindWindow("Progman", null);

            IntPtr result = IntPtr.Zero;
            // 向 Program Manager 窗口发送消息 0x52c 的一个消息，超时设置为2秒
            Win32ApiHelper.SendMessageTimeout(programHandle, 0x52c, IntPtr.Zero, IntPtr.Zero, 0, 2, result);

            // 遍历顶级窗口
            Win32ApiHelper.EnumWindows((hwnd, lParam) =>
            {
                // 找到第一个 WorkerW 窗口，此窗口中有子窗口 SHELLDLL_DefView，所以先找子窗口
                if (Win32ApiHelper.FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", null) != IntPtr.Zero)
                {
                    // 找到当前第一个 WorkerW 窗口的，后一个窗口，及第二个 WorkerW 窗口。
                    IntPtr tempHwnd = Win32ApiHelper.FindWindowEx(IntPtr.Zero, hwnd, "WorkerW", null);

                    // 隐藏第二个 WorkerW 窗口
                    Win32ApiHelper.ShowWindow(tempHwnd, 0);
                }
                return true;
            }, IntPtr.Zero);
        }
    }
}
