using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZYM.Development.Views.Daily
{
    /// <summary>
    /// CrystalBallWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CrystalBallWindow : BasicWindow
    {
        public CrystalBallWindow()
        {
            InitializeComponent();
        }


        public override void Refresh()
        {
            CrystalBall.InitShowImage();
        }

        /// <summary>
        /// 重写的Dispose方法
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            //清理托管资源
            if (disposing)
            {
                CrystalBall.Dispose();
            }
            //告诉自己已经被释放
            disposed = true;
        }
    }
}
