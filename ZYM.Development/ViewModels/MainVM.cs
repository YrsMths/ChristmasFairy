using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZYM.Development.Commands;
using ZYM.Development.Models;
using ZYM.Development.Views;
using ZYM.Development.Views.Christmas;
using ZYM.Development.Views.Daily;

namespace ZYM.Development.ViewModels
{
    public class MainVM:ViewModelBase
    {
        //private ObservableCollection<NavigateMenuModel> _navigateMenuModelList;

        //public ObservableCollection<NavigateMenuModel> NavigateMenuModelList
        //{
        //    get { return _navigateMenuModelList; }
        //    set { _navigateMenuModelList = value; }
        //}

        private ObservableCollection<ListBoxItem> _navigateMenuModelList;

        public ObservableCollection<ListBoxItem> NavigateMenuModelList
        {
            get { return _navigateMenuModelList; }
            set { _navigateMenuModelList = value; }
        }

        private NavigateMenuModel _navigateMenuItem;
        /// <summary>
        /// 当前选中
        /// </summary>
        public NavigateMenuModel NavigateMenuItem
        {
            get { return _navigateMenuItem; }
            set
            {
                _navigateMenuItem = value;
                this.NotifyPropertyChange("NavigateMenuItem");
            }
        }
        private object _controlPanel;
        /// <summary>
        /// 更换右侧面板
        /// </summary>
        public object ControlPanel
        {
            get { return _controlPanel; }
            set
            {
                _controlPanel = value;
                this.NotifyPropertyChange("ControlPanel");
            }
        }
        public MainVM()
        {
            //NavigateMenuModelList = new ObservableCollection<NavigateMenuModel>();
            //foreach (MenuEnum menuEnum in Enum.GetValues(typeof(MenuEnum)))
            //{
            //    NavigateMenuModelList.Add(new NavigateMenuModel { Name = menuEnum.ToString() });
            //}
            //NavigateMenuModelList.Add(new NavigateMenuModel { Name = "持续更新中" });

            NavigateMenuModelList = new ObservableCollection<ListBoxItem>();
            foreach (MenuEnum menuEnum in Enum.GetValues(typeof(MenuEnum)))
            {
                NavigateMenuModelList.Add(new ListBoxItem { Content = menuEnum.ToString() });
            }
            NavigateMenuModelList.Add(new ListBoxItem { Content = "持续更新中" });
            //ControlPanel = new AnimationNavigationBar3DExample();
        }

        public ICommand ViewLoaded => new RelayCommand(obj =>
        {
            
        });

        public ICommand MenuSelectionChangedCommand => new RelayCommand(obj =>
        {
            if (obj == null) return;
            //var model = obj as NavigateMenuModel;
            //MenuItemSelection(model.Name);
            var model = obj as ListBoxItem;
            MenuItemSelection(model.Content.ToString());
        });

        public ICommand CloseCommand => new RelayCommand(obj =>
        {
            //Environment.Exit(0);
            Application.Current.MainWindow.Close();
        });

        void MenuItemSelection(string _menuName)
        {
            MenuEnum flag;
            if (!Enum.TryParse<MenuEnum>(_menuName, true, out flag))
                return;
            var menuEnum = (MenuEnum)Enum.Parse(typeof(MenuEnum), _menuName, true);
            switch (menuEnum)
            {
                default:
                    new SantaSkyWindow().Show();
                    break;
            }
        }
    }
}
