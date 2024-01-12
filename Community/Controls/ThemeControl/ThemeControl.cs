using Community.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Resources;
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
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Community.Controls.ThemeControl"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Community.Controls.ThemeControl;assembly=Community.Controls.ThemeControl"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:ThemeControl/>
    ///
    /// </summary>
    public class ThemeControl : Control
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<ThemeModel>), typeof(ThemeControl),
                new PropertyMetadata(null));

        static ThemeControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ThemeControl), new FrameworkPropertyMetadata(typeof(ThemeControl)));
        }

        public ObservableCollection<ThemeModel> ItemsSource
        {
            get => (ObservableCollection<ThemeModel>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(double), typeof(ThemeControl),
                new PropertyMetadata(30.0));

        public double Size
        {
            get=> (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ItemsSource = new ObservableCollection<ThemeModel>();
            foreach (FestvialType type in Enum.GetValues(typeof(FestvialType)))
            {
                if (ResourceExists($"Themes/Dynamic/Light.{type}.baml"))
                    ItemsSource.Add(new ThemeModel
                    {
                        Color = "#409EFF",
                        ResourcePath = $"pack://application:,,,/Community;component/Themes/Dynamic/Light.{type}.xaml",
                        Image = new BitmapImage(new Uri($"pack://application:,,,/Community;component/Images/Themes/{type}.png", UriKind.RelativeOrAbsolute)),
                        Type = type
                    });
            }
            if (ThemeCache.ThemesDictCache.Count > 0)
                foreach (var item in ThemeCache.ThemesDictCache)
                    if (ItemsSource.Any(x => x.Type != item.Key))
                        ItemsSource.Add(item.Value);
            SelectChecked();
            ItemsSource.CollectionChanged += ItemsSource_CollectionChanged;
            foreach (var theme in ItemsSource)
                theme.PropertyChanged += Theme_PropertyChanged;
        }

        private void SelectChecked()
        {
            var existsTheme = ItemsSource.FirstOrDefault(y =>
                Application.Current.Resources.MergedDictionaries.ToList().Exists(j =>
                    j.Source != null && y.ResourcePath.Contains(j.Source.AbsoluteUri)));

            if (existsTheme != null)
            {
                existsTheme.IsChecked = true;
                ControlsHelper.FestvialType = existsTheme.Type;
            }
        }

        private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (ThemeModel item in e.NewItems)
                    {
                        if (!ThemeCache.ThemesDictCache.ContainsKey(item.Type))
                            ThemeCache.ThemesDictCache.Add(item.Type, item);
                        item.PropertyChanged += Theme_PropertyChanged;
                        SelectChecked();
                        if (!item.IsChecked) return;
                        ReviseTheme(item);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (ThemeModel item in e.NewItems)
                        if (ThemeCache.ThemesDictCache.ContainsKey(item.Type))
                            ThemeCache.ThemesDictCache.Remove(item.Type);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
            }
        }

        private void Theme_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ThemeModel.IsChecked))
            {
                var theme = sender as ThemeModel;
                if (!theme.IsChecked) return;
                ReviseTheme(theme);
            }
        }

        private void ReviseTheme(ThemeModel theme)
        {
            if (theme == null) return;
            var old = ItemsSource.FirstOrDefault(x => x.IsChecked && x.Type != theme.Type);
            if (old != null)
            {
                ItemsSource.Where(y => !y.Type.Equals(theme.Type) && y.IsChecked).ToList()
                    .ForEach(h => h.IsChecked = false);
                var existingResourceDictionary =
                    Application.Current.Resources.MergedDictionaries.FirstOrDefault(x =>
                        x.Source != null && x.Source.Equals(old.ResourcePath));
                if (existingResourceDictionary != null)
                    Application.Current.Resources.MergedDictionaries.Remove(existingResourceDictionary);
                var newResource =
                    Application.Current.Resources.MergedDictionaries.FirstOrDefault(x =>
                        x.Source != null && x.Source.Equals(theme.ResourcePath));
                if (newResource != null) return;
                var newResourceDictionary = new ResourceDictionary { Source = new Uri(theme.ResourcePath) };
                Application.Current.Resources.MergedDictionaries.Insert(0, newResourceDictionary);
                ControlsHelper.ThemeRefresh();
            }
        }

        /// <summary>
        /// 判断资源是否存在
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        public static bool ResourceExists(string resourcePath)
        {
            var assembly = Assembly.GetExecutingAssembly();

            return ResourceExists(assembly, resourcePath);
        }

        public static bool ResourceExists(Assembly assembly, string resourcePath)
        {
            return GetResourcePaths(assembly)
                .Contains(resourcePath.ToLowerInvariant());
        }

        public static IEnumerable<object> GetResourcePaths(Assembly assembly)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var resourceName = assembly.GetName().Name + ".g";
            var resourceManager = new ResourceManager(resourceName, assembly);

            try
            {
                var resourceSet = resourceManager.GetResourceSet(culture, true, true);

                foreach (System.Collections.DictionaryEntry resource in resourceSet)
                {
                    yield return resource.Key;
                }
            }
            finally
            {
                resourceManager.ReleaseAllResources();
            }
        }
    }

    public class ThemeCache
    {
        public static Dictionary<FestvialType, ThemeModel> ThemesDictCache = new Dictionary<FestvialType, ThemeModel>();
    }
}
