using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Community
{
    public class Resources : ResourceDictionary
    {
        public ThemeType Theme
        {
            set => InitializeTheme(value);
        }

        protected void InitializeTheme(ThemeType themeType)
        {
            MergedDictionaries.Clear();
            var path = GetResourceUri(GetThemeResourceName(themeType));
            MergedDictionaries.Add(new ResourceDictionary { Source = path });
        }

        protected Uri GetResourceUri(string path)
        {
            return new Uri($"pack://application:,,,/Community;component/Themes/Basic/{path}.xaml");
        }

        protected string GetThemeResourceName(ThemeType themeType)
        {
            return themeType == ThemeType.Light ? "Light.Color" : "Dark.Color";
        }
    }

    public enum ThemeType
    {
        Light,
        Dark,
    }
}
