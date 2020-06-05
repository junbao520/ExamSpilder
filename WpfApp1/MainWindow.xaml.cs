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

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Book> lstBook = new List<Book>();
        public MainWindow()
        {
            InitializeComponent();
            InitListView();
            btnRefresh.Click += BtnRefresh_Click;
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            lstBook.AddRange(lstBook);
            lstViewBook.ItemsSource = null;
            lstViewBook.ItemsSource = lstBook;
            
        }

        /// <summary>
        /// 初始化ListView
        /// </summary>
        public  void InitListView()
        {


            lstBook.Add(new Book {Name="C++程序设计",Author="张三",Type="程序设计",Press="中文出版社" });
            lstBook.Add(new Book { Name = "C#程序设计", Author = "张三", Type = "程序设计", Press = "中文出版社" });
            lstBook.Add(new Book { Name = "Java程序设计", Author = "张三", Type = "程序设计", Press = "中文出版社" });
            lstBook.Add(new Book { Name = "PhP程序设计", Author = "张三", Type = "程序设计", Press = "中文出版社" });
            lstViewBook.ItemsSource = lstBook;
        }
    }
}
