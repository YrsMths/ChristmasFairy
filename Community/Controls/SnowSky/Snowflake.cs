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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Community.Controls
{
    //[TemplatePart(Name = EllipseTemplateName, Type = typeof(Ellipse))]
    public class Snowflake : Control
    {
        //private const string EllipseTemplateName = "PART_Ellipse";

        private Ellipse _ellipse;
        static Snowflake()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Snowflake), new FrameworkPropertyMetadata(typeof(Snowflake)));
        }

        public Snowflake()
        {
            CacheMode = new BitmapCache();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //_ellipse = GetTemplateChild(EllipseTemplateName) as Ellipse;
            //if (_ellipse == null) return;
        }
    }
}
