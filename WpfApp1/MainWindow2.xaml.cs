using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow2 : Window
    {
        List<Book> lstBook = new List<Book>();

        List<MailList> lstMailList = new List<MailList>();

        public MainWindow2()
        {
            InitializeComponent();
            InitListView();
            btnQuery.Click += BtnQuery_Click;
            pg.PageChanged += Pg_PageChanged;
        }

        private void Pg_PageChanged(object sender, EventArgs e)
        {
            var result = lstBook.Skip(pg.CurrentPageNumber - 1).Take(pg.PageDataCount);

            UpdateUI(() => { listview.ItemsSource = null;listview.ItemsSource = result; });

        }

        private void UpdateUI(Action CallBack)
        {
            new Thread(() => {
                this.Dispatcher.Invoke(new Action(() => {
                    App.Current.Dispatcher.Invoke((Action)delegate ()
                    {
                        CallBack();
                    });
                }));
            }).Start();
        }
        private void UpdateListView<T>(ListView listview,List<T> lstData)
        {
            listview.ItemsSource = null;
            listview.ItemsSource = lstData;
        }


        private void BtnQuery_Click(object sender, RoutedEventArgs e)
        {
            var name = txtName.Text.Trim().ToUpper();
            var type = txtType.Text.Trim().ToUpper();
            var result = lstBook.Where(s => s.Name.ToUpper().Contains(name) && s.Type.ToUpper().Contains(type)).ToList();
            UpdateListView(listview, result);
        }

        private void content_key(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnQuery_Click(null, null);
            }
        }

        /// <summary>
        /// 初始化ListView
        /// </summary>
        public  void InitListView()
        {


            //可以考虑通过MVVM 模式进行改造升级 把页面改变绑定到一个变量上上面！ 查询直接通过Command 的形式进行实现！

            lstBook.Add(new Book { Name = "C++程序设计", Author = "张三", Type = "程序设计", Press = "中文出版社" });
            lstBook.Add(new Book { Name = "C#程序设计", Author = "张三", Type = "程序设计", Press = "中文出版社" });
            lstBook.Add(new Book { Name = "Java程序设计", Author = "张三", Type = "程序设计", Press = "中文出版社" });
            lstBook.Add(new Book { Name = "PhP程序设计", Author = "张三", Type = "程序设计", Press = "中文出版社" });

            for (int i = 0; i <=10; i++)
            {
                lstBook.AddRange(lstBook);
            }
            pg.TotalDataCount = lstBook.Count;
            
            
            //listview.ItemsSource = lstBook;
            //lstMailList.Add(new MailList("ces", "ces", "ce", "ce"));
            //lstMailList.Add(new MailList("ces", "ces", "ce", "ce"));
            
        }
    }
}
