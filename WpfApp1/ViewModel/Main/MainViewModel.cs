using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModel.Common;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace WpfApp1
{
    public class MainViewModel : NotificationObject
    {


        //public List<Book> lstBook = new  ObservableCollection<Book>();


        private List<Book> lstBook = new List<Book>();
        private ObservableCollection<Book> _Books { get; set; }



        //我这次点的是2 但是他返回的是上一次跌
        public ObservableCollection<Book> Books
        {
            get
            {
                return _Books;
            }
            set
            {
                _Books = value;
                this.RaisePropertyChanged();
            }
        }


        public MainViewModel()
        {
            InitBookList();
            this.TotalDataCount = lstBook.Count;
            this.BookName = "";
            this.BookType = "";
            this.CurrentPageNumber = 1;
            this.PageSize= 10;
            LoadPageData();

        }

        private int _TotalDataCount { get; set; }

        public int TotalDataCount
        {
            get
            {
                return _TotalDataCount;
            }
            set
            {
                _TotalDataCount = value;
                this.RaisePropertyChanged();
            }
        }
        private int PageSize { get; set; }

        private string _BookName { get; set; }

        public string BookName
        {
            get
            {
                return _BookName;
            }
            set
            {
                _BookName = value;
                this.RaisePropertyChanged();
            }
        }

        public string _BookType { get; set; }

        public string BookType
        {
            get
            {
                return _BookType;
            }
            set
            {
                _BookType = value;
                this.RaisePropertyChanged();
            }
        }

        private int _CurrentPageNumber { get; set; }

        public int CurrentPageNumber
        {
            get
            {
                return _CurrentPageNumber;
            }
            set
            {
                _CurrentPageNumber = value;
                this.RaisePropertyChanged();
            }
        }


        private BaseCommand pageNumberChangedCommand;

        public BaseCommand PageNumberChangedCommand
        {
            get
            {
                if (pageNumberChangedCommand == null)
                {
                    pageNumberChangedCommand = new BaseCommand(new Action<object>(o =>
                     {
                      this.CurrentPageNumber=(int)o;
                         LoadPageData();
                     }));
                }
                return pageNumberChangedCommand;
            }
        }

        private BaseCommand pageDataCountChangedCommand;

        public BaseCommand PageDataCountChangedCommand
        {
            get
            {
                if (pageDataCountChangedCommand == null)
                {
                    pageDataCountChangedCommand = new BaseCommand(new Action<object>(o =>
                    {
                        this.PageSize = (int)o;
                        LoadPageData();

                    }));
                }
                return pageDataCountChangedCommand;
            }
        }

        private BaseCommand queryCommand;
        /// <summary>
        /// 查询命令
        /// </summary>
        public BaseCommand QueryCommand
        {
            get
            {
                if (queryCommand == null)
                {
                    queryCommand = new BaseCommand(new Action<object>(o =>
                    {
                        //执行登录逻辑
                        LoadPageData();


                    }));
                }
                return queryCommand;
            }
        }

        public void LoadPageData()
        {
            var totalCount = lstBook.Where(s => s.Name.Contains(BookName) && s.Type.Contains(BookType)).Count();
            this.TotalDataCount = totalCount;
            var result = lstBook.Where(s => s.Name.Contains(BookName) && s.Type.Contains(BookType)).Skip((CurrentPageNumber - 1)*PageSize).Take(PageSize).ToList();
            this.Books = new ObservableCollection<Book>(result);
        }


        public void InitBookList()
        {
            lstBook.Add(new Book { Name = "C++程序设计", Author = "张三", Type = "程序设计", Press = "中文出版社" });
            lstBook.Add(new Book { Name = "C#程序设计", Author = "张三", Type = "程序设计", Press = "中文出版社" });
            lstBook.Add(new Book { Name = "Java程序设计", Author = "张三", Type = "程序设计", Press = "中文出版社" });
            lstBook.Add(new Book { Name = "PhP程序设计", Author = "张三", Type = "程序设计", Press = "中文出版社" });

            for (int i = 0; i <=2; i++)
            {
                lstBook.AddRange(lstBook);
            }
           // this.Books = new ObservableCollection<Book>(lstBook);
        }

    }
}
