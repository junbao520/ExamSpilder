using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Pagination
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Pagination"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Pagination;assembly=Paginations"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:Pagination/>
    ///
    /// </summary>
    [TemplatePart(Name = "PART_ComboBox", Type = typeof(ComboBox))]
    [TemplatePart(Name = "PART_ButtonFirstPage", Type = typeof(Button))]
    [TemplatePart(Name = "PART_ButtonPrePage", Type = typeof(Button))]
    [TemplatePart(Name = "PART_ListBoxPages", Type = typeof(ListBox))]
    [TemplatePart(Name = "PART_ButtonNextPage", Type = typeof(Button))]
    [TemplatePart(Name = "PART_ButtonLastPage", Type = typeof(Button))]
    [TemplatePart(Name = "PART_PageInfo", Type = typeof(Panel))]
    public class Pagination : Control
    {
        #region Consts
        #endregion

        #region Fields

        private ComboBox _cbbPageDataCount = null;
        private ListBox _lstShowingPage = null;
        private Button _btnFirstPage = null;
        private Button _btnPrePage = null;
        private Button _btnNextPage = null;
        private Button _btnLastPage = null;

        private bool _isIgnoreListBoxSelectionChanged = false;
        private static object _lock = new object();

        #endregion

        #region Properties

        #endregion

        #region Dependency Properties

        /// <summary>
        /// 是否显示每页数据量选择控件
        /// </summary>
        public static readonly DependencyProperty IsShowPageDataCountSelectorProperty = DependencyProperty.Register("IsShowPageDataCountSelector", typeof(bool), typeof(Pagination),
            new PropertyMetadata(true, null));
        /// <summary>
        /// 可选择的每页显示的数据条数集合
        /// </summary>
        public static readonly DependencyProperty PageDataCountCollectionProperty = DependencyProperty.Register("PageDataCountCollection", typeof(ObservableCollection<int>), typeof(Pagination),
            new PropertyMetadata(new ObservableCollection<int> {10, 20, 30,40, 50 }, null));
        /// <summary>
        /// 每页最多显示的数据条数
        /// </summary>
        public static readonly DependencyProperty PageDataCountProperty = DependencyProperty.Register("PageDataCount", typeof(int), typeof(Pagination),
            new PropertyMetadata(10, OnPageDataCountPropertyChanged));
        /// <summary>
        /// 当前显示的可供选择的分页号集合
        /// </summary>
        public static readonly DependencyProperty ShowingPageNumberCollectionProperty = DependencyProperty.Register("ShowingPageNumberCollection", typeof(ObservableCollection<int>), typeof(Pagination),
            new PropertyMetadata(null, null));
        /// <summary>
        /// 当前选择的页数
        /// </summary>
        public static readonly DependencyProperty CurrentPageNumberProperty = DependencyProperty.Register("CurrentPageNumber", typeof(int), typeof(Pagination),
            new PropertyMetadata(1, OnCurrentPageNumberChanged));
        /// <summary>
        /// 是否显示分页信息
        /// </summary>
        public static readonly DependencyProperty IsShowPageInfoProperty = DependencyProperty.Register("IsShowPageInfo", typeof(bool), typeof(Pagination),
            new PropertyMetadata(true, null));
        /// <summary>
        /// 总的数据量
        /// </summary>
        public static readonly DependencyProperty TotalDataCountProperty = DependencyProperty.Register("TotalDataCount", typeof(int), typeof(Pagination),
            new PropertyMetadata(0, null));
        /// <summary>
        /// 当前页显示的数据条数
        /// </summary>
        public static readonly DependencyProperty CurrentPageDataCountProperty = DependencyProperty.Register("CurrentPageDataCount", typeof(int), typeof(Pagination),
            new PropertyMetadata(0, null));
        /// <summary>
        /// 总页数
        /// </summary>
        public static readonly DependencyProperty TotalPageCountProperty = DependencyProperty.Register("TotalPageCount", typeof(int), typeof(Pagination),
            new PropertyMetadata(1, null));
        /// <summary>
        /// 当前显示页的数据起始编号
        /// </summary>
        public static readonly DependencyProperty ShowingPageDataStartNumberProperty = DependencyProperty.Register("ShowingPageDataStartNumber", typeof(int), typeof(Pagination),
            new PropertyMetadata(0, null));
        /// <summary>
        /// 当前显示页的数据结束编号
        /// </summary>
        public static readonly DependencyProperty ShowingPageDataEndNumberProperty = DependencyProperty.Register("ShowingPageDataEndNumber", typeof(int), typeof(Pagination),
            new PropertyMetadata(0, null));
        /// <summary>
        /// 显示的可选择页的最大数量
        /// </summary>
        public static readonly DependencyProperty MaxShownPageCountProperty = DependencyProperty.Register("MaxShownPageCount", typeof(int), typeof(Pagination),
            new PropertyMetadata(10, null));
        /// <summary>
        /// 选中页的背景色
        /// </summary>
        public static readonly DependencyProperty SelectedPageBackgroundProperty = DependencyProperty.Register("SelectedPageBackground", typeof(Brush), typeof(Pagination),
            new PropertyMetadata(new SolidColorBrush(Colors.Red), null));
        /// <summary>
        /// 未选择的页码的背景色
        /// </summary>
        public static readonly DependencyProperty PageSelectorBackgroundProperty = DependencyProperty.Register("PageSelectorBackground", typeof(Brush), typeof(Pagination),
            new PropertyMetadata(null, null));

        #endregion


        public delegate void PageEventHandler(object sender, EventArgs e);

        public event PageEventHandler PageChanged;

        /// <summary>
        /// 页码改变事件
        /// </summary>
        public static RoutedEvent PageChangedEvent;

        /// <summary>
        /// 每页大小改变事件
        /// </summary>
        public static RoutedEvent PageDataCountChangedEvent;


        #region Property Wrappers


        public bool IsShowPageDataCountSelector
        {
            get { return (bool)GetValue(IsShowPageDataCountSelectorProperty); }
            set { SetValue(IsShowPageDataCountSelectorProperty, value); }
        }
        public ObservableCollection<int> PageDataCountCollection
        {
            get { return (ObservableCollection<int>)GetValue(PageDataCountCollectionProperty); }
            set { SetValue(PageDataCountCollectionProperty, value); }
        }
        public int PageDataCount
        {
            get { return (int)GetValue(PageDataCountProperty); }
            private set { SetValue(PageDataCountProperty, value); }
        }
        public ObservableCollection<int> ShowingPageNumberCollection
        {
            get { return (ObservableCollection<int>)GetValue(ShowingPageNumberCollectionProperty); }
            set { SetValue(ShowingPageNumberCollectionProperty, value); }
        }
        public int CurrentPageNumber
        {
            get { return (int)GetValue(CurrentPageNumberProperty); }
            set { SetValue(CurrentPageNumberProperty, value); }
        }
        public bool IsShowPageInfo
        {
            get { return (bool)GetValue(IsShowPageInfoProperty); }
            set { SetValue(IsShowPageInfoProperty, value); }
        }
        public int TotalDataCount
        {
            get { 
                
                return (int)GetValue(TotalDataCountProperty); 
            
            }
            set {
                
                SetValue(TotalDataCountProperty, value);

                InitData();
            }
        }
        public int TotalPageCount
        {
            get { return (int)GetValue(TotalPageCountProperty); }
            set { SetValue(TotalPageCountProperty, value); }
        }
        public int ShowingPageDataStartNumber
        {
            get { return (int)GetValue(ShowingPageDataStartNumberProperty); }
            set { SetValue(ShowingPageDataStartNumberProperty, value); }
        }
        public int ShowingPageDataEndNumber
        {
            get { return (int)GetValue(ShowingPageDataEndNumberProperty); }
            set { SetValue(ShowingPageDataEndNumberProperty, value); }
        }
        public int MaxShownPageCount
        {
            get { return (int)GetValue(MaxShownPageCountProperty); }
            set { SetValue(MaxShownPageCountProperty, value); }
        }
        public Brush SelectedPageBackground
        {
            get { return (Brush)GetValue(SelectedPageBackgroundProperty); }
            set { SetValue(SelectedPageBackgroundProperty, value); }
        }
        public Brush PageSelectorBackground
        {
            get { return (Brush)GetValue(PageSelectorBackgroundProperty); }
            set { SetValue(PageSelectorBackgroundProperty, value); }
        }

        #endregion

        #region Constructors

        static Pagination()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Pagination), new FrameworkPropertyMetadata(typeof(Pagination)));

            ///注册页码改变事件
            PageChangedEvent = EventManager.RegisterRoutedEvent("PageChanged", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pagination));

            PageDataCountChangedEvent= EventManager.RegisterRoutedEvent("PageDataCountChanged", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pagination));
        }

        public event RoutedEventHandler PageChange
        {
            add { AddHandler(PageChangedEvent, value); }
            remove { RemoveHandler(PageChangedEvent, value); }
        }

        public event RoutedEventHandler PageDataCountChanged {

            add { AddHandler(PageDataCountChangedEvent, value); }
            remove { RemoveHandler(PageDataCountChangedEvent, value); }
        }



        #endregion

        #region Override Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //初始化控件
            InitControls();

            //初始化数据
            ShowingPageNumberCollection = new ObservableCollection<int>();
            InitData();
        }

        #endregion

        #region Property Changed Event Methods

        /// <summary>
        /// 当前选择的页数发生改变时的回调方法
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public static void OnCurrentPageNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Pagination pagination = d as Pagination;
            if (pagination == null)
            {
                return;
            }

            if (pagination._lstShowingPage != null)
            {
                pagination._lstShowingPage.SelectedItem = e.NewValue;
            }
         
            pagination.SetBtnEnable();
        }

        /// <summary>
        /// 每页显示的最大数据量发生改变时的回调方法
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnPageDataCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Pagination pagination = d as Pagination;
            if (pagination == null)
            {
                return;
            }

            pagination.InitData();
        }

        #endregion

        #region Event Methods

        private void _cbbPageDataCount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbb = sender as ComboBox;
            if (cbb == null || cbb.SelectedItem == null)
            {
                return;
            }

            string selectedCountString = cbb.SelectedItem.ToString();
            if (!int.TryParse(selectedCountString, out int selectedDataCount))
            {
                return;
            }
            //PageDataCount 属性
            PageDataCount = selectedDataCount;
            RaiseEvent(new RoutedEventArgs(PageDataCountChangedEvent, this));
            InitData();
        }

        //这应该会触发一个事件 后面才能够
        private void _lstShowingPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isIgnoreListBoxSelectionChanged)
            {
                return;
            }

            try
            {
                _isIgnoreListBoxSelectionChanged = true;

                ListBox lst = sender as ListBox;
                if (lst == null || lst.SelectedItem == null)
                {
                    return;
                }

                string selectedPageString = lst.SelectedItem.ToString();
                if (!int.TryParse(selectedPageString, out int selectedPageNumber))
                {
                    return;
                }

                //总页数小于最大可显示页数表明无论选中的页数为何，均不需要改动页码集合中的数据，此时直接返回
                if (TotalPageCount <= MaxShownPageCount)
                {
                    CurrentPageNumber = selectedPageNumber;
                    UpdateShowingPageInfo();
                    return;
                }

                //计算为保持选中项居中而需要向右移动的次数 比较中间位置与选中项的下标
                int moveCount = MaxShownPageCount / 2 - _lstShowingPage.SelectedIndex;
                int startPageNumber = ShowingPageNumberCollection.First();
                if (moveCount > 0) //向右移动
                {
                    int realMoveCount = moveCount;
                    if (ShowingPageNumberCollection.First() - 1 < moveCount)
                    {
                        realMoveCount = ShowingPageNumberCollection.First() - 1;
                    }

                    startPageNumber = ShowingPageNumberCollection.First() - realMoveCount;
                }
                else if (moveCount < 0) //向左移动
                {
                    int realMoveCount = -moveCount;
                    if (TotalPageCount - ShowingPageNumberCollection.Last() < realMoveCount)
                    {
                        realMoveCount = TotalPageCount - ShowingPageNumberCollection.Last();
                    }

                    startPageNumber = ShowingPageNumberCollection.First() + realMoveCount;
                }

                lock (_lock)
                {
                    ShowingPageNumberCollection.Clear();
                    for (int i = 0; i < MaxShownPageCount; i++)
                    {
                        ShowingPageNumberCollection.Add(startPageNumber + i);
                    }
                }

                int selectedItemIndex = ShowingPageNumberCollection.IndexOf(selectedPageNumber);
                _lstShowingPage.SelectedIndex = selectedItemIndex;

                CurrentPageNumber = selectedPageNumber;
                UpdateShowingPageInfo();
            }
            finally
            {
                _isIgnoreListBoxSelectionChanged = false;
            }
        }
        /// <summary>
        /// 跳转到首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _btnFirstPage_Click(object sender, RoutedEventArgs e)
        {
            if (_lstShowingPage == null
                || ShowingPageNumberCollection == null
                || ShowingPageNumberCollection.Count == 0)
            {
                return;
            }

            if (ShowingPageNumberCollection[0] != 1)
            {
                try
                {
                    _isIgnoreListBoxSelectionChanged = true;
                    lock (_lock)
                    {
                        ShowingPageNumberCollection.Clear();
                        for (int i = 1; i <= MaxShownPageCount; i++)
                        {
                            ShowingPageNumberCollection.Add(i);
                        }
                    }
                }
                finally
                {
                    _isIgnoreListBoxSelectionChanged = false;
                }
            }

            _lstShowingPage.SelectedIndex = 0;
        }
        /// <summary>
        /// 跳转到尾页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _btnLastPage_Click(object sender, RoutedEventArgs e)
        {
            if (_lstShowingPage == null
              || ShowingPageNumberCollection == null
              || ShowingPageNumberCollection.Count == 0)
            {
                return;
            }

            if (ShowingPageNumberCollection.Last() != TotalPageCount)
            {
                try
                {
                    _isIgnoreListBoxSelectionChanged = true;
                    lock (_lock)
                    {
                        ShowingPageNumberCollection.Clear();
                        for (int i = 0; i < MaxShownPageCount; i++)
                        {
                            ShowingPageNumberCollection.Add(TotalPageCount - MaxShownPageCount + i + 1);
                        }
                    }
                }
                finally
                {
                    _isIgnoreListBoxSelectionChanged = false;
                }
            }

            _lstShowingPage.SelectedIndex = _lstShowingPage.Items.Count - 1;
        }
        /// <summary>
        /// 跳转到前一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _btnPrePage_Click(object sender, RoutedEventArgs e)
        {
            if (_lstShowingPage == null
                || ShowingPageNumberCollection == null
                || ShowingPageNumberCollection.Count == 0)
            {
                return;
            }

            if (_lstShowingPage.SelectedIndex > 0)
            {
                _lstShowingPage.SelectedIndex--;
            }
        }
        /// <summary>
        /// 跳转到后一条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (_lstShowingPage == null
                || ShowingPageNumberCollection == null
                || ShowingPageNumberCollection.Count == 0)
            {
                return;
            }

            if (_lstShowingPage.SelectedIndex < MaxShownPageCount - 1)
            {
                _lstShowingPage.SelectedIndex++;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControls()
        {
            _cbbPageDataCount = GetTemplateChild("PART_ComboBox") as ComboBox;
            if (_cbbPageDataCount != null)
            {
                _cbbPageDataCount.SelectionChanged += _cbbPageDataCount_SelectionChanged;
            }

            _lstShowingPage = GetTemplateChild("PART_ListBoxPages") as ListBox;
            if (_lstShowingPage != null)
            {
                _lstShowingPage.SelectionChanged += _lstShowingPage_SelectionChanged;
            }
             _btnFirstPage = GetTemplateChild("PART_ButtonFirstPage") as Button;
            if (_btnFirstPage != null)
            {
                _btnFirstPage.Click += _btnFirstPage_Click;
            }

            _btnPrePage = GetTemplateChild("PART_ButtonPrePage") as Button;
            if (_btnPrePage != null)
            {
                _btnPrePage.Click += _btnPrePage_Click;
            }

            _btnNextPage = GetTemplateChild("PART_ButtonNextPage") as Button;
            if (_btnNextPage != null)
            {
                _btnNextPage.Click += _btnNextPage_Click;
            }

            _btnLastPage = GetTemplateChild("PART_ButtonLastPage") as Button;
            if (_btnLastPage != null)
            {
                _btnLastPage.Click += _btnLastPage_Click;
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            try
            {
                _isIgnoreListBoxSelectionChanged = true;
                if (PageDataCount > 0)
                {
                    //根据总的数据量和每页最大显示的数据量计算总的页数
                    if (TotalDataCount % PageDataCount > 0)
                    {
                        TotalPageCount = TotalDataCount / PageDataCount + 1;
                    }
                    else
                    {
                        TotalPageCount = TotalDataCount / PageDataCount;
                    }

                    //将可选择页码加入到数据绑定集合中
                    if (ShowingPageNumberCollection != null)
                    {
                        lock (_lock)
                        {
                            ShowingPageNumberCollection.Clear();
                            int addPageCount = MaxShownPageCount;
                            if (TotalPageCount < MaxShownPageCount)
                            {
                                addPageCount = TotalPageCount;
                            }

                            for (int i = 1; i <= addPageCount; i++)
                            {
                                ShowingPageNumberCollection.Add(i);
                            }
                        }
                    }

                    //初始化选中页
                    if (_lstShowingPage != null)
                    {
                        _lstShowingPage.SelectedIndex = 0;
                        CurrentPageNumber = 1;
                    }

                    //更新分页数据信息
                    UpdateShowingPageInfo();
                }

                SetBtnEnable();
            }
            finally
            {
                _isIgnoreListBoxSelectionChanged = false;
            }
        }

        private void UpdateShowingPageInfo()
        {
            //事件通知机制
            if(TotalPageCount == 0)
            {
                ShowingPageDataStartNumber = 0;
                ShowingPageDataEndNumber = 0;
            }
            else if(CurrentPageNumber < TotalPageCount)
            {
                ShowingPageDataStartNumber = (CurrentPageNumber - 1) * PageDataCount + 1;
                ShowingPageDataEndNumber = CurrentPageNumber * PageDataCount;
            }
            else if(CurrentPageNumber == TotalPageCount)
            {
                ShowingPageDataStartNumber = (CurrentPageNumber - 1) * PageDataCount + 1;
                ShowingPageDataEndNumber = TotalDataCount;
            }

            //执行自定义事件
  
            RaiseEvent(new RoutedEventArgs(PageChangedEvent, this));

            if (this.PageChanged!=null)
            {
                this.PageChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// 设置按钮的可用性
        /// </summary>
        private void SetBtnEnable()
        {
            if (_btnFirstPage == null || _btnPrePage == null
                || _btnNextPage == null || _btnLastPage == null)
            {
                return;
            }

            _btnPrePage.IsEnabled = true;
            _btnNextPage.IsEnabled = true;
            _btnFirstPage.IsEnabled = true;
            _btnLastPage.IsEnabled = true;

            if (ShowingPageNumberCollection == null || ShowingPageNumberCollection.Count == 0)//集合为空或者无数据，则所有按钮不可用
            {
                _btnPrePage.IsEnabled = false;
                _btnNextPage.IsEnabled = false;
                _btnFirstPage.IsEnabled = false;
                _btnLastPage.IsEnabled = false;
            }
            else
            {
                if (CurrentPageNumber == 1)
                {
                    _btnFirstPage.IsEnabled = false;
                    _btnPrePage.IsEnabled = false;
                }

                if (CurrentPageNumber == TotalPageCount)
                {
                    _btnNextPage.IsEnabled = false;
                    _btnLastPage.IsEnabled = false;
                }
            }
        }

        #endregion

    }
}
