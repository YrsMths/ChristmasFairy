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

namespace ZYM.Development.Views.Christmas
{
    /// <summary>
    /// NeonWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NeonWindow : BasicWindow
    {
        public NeonWindow()
        {
            InitializeComponent();
        }


        public override void Refresh()
        {
            NeonText.Init();
        }
    }
}
