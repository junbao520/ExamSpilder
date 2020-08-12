using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModel.Common;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using WpfApp1.Model;
using WpfApp1.Helper;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.Windows;
using System.Runtime.CompilerServices;
using System.IO;
using System.Threading;

namespace WpfApp1
{
    public class ExamViewModel : NotificationObject
    {


        //public List<Book> lstBook = new  ObservableCollection<Book>();


        public List<ExamInfo> lstExam = new List<ExamInfo>();
        private ObservableCollection<ExamInfo> _Exams { get; set; }

        public string AreaNames = "锦江、青羊、金牛、武侯、成华、龙泉驿、青白江、新都、温江、双流、郫都、简阳、都江堰、彭州、邛崃、崇州、金堂、大邑、蒲江、新津";
        public List<string> lstArea = new List<string>();

        public ExamViewModel()
        {

            this.Date = DateTime.Now.ToString("yyyy-MM-dd");
            this.Address = "http://cdpta.cdhrss.chengdu.gov.cn/frt/frtuploadfile/uploadfile/bulletin/2020/2006040939118502006040939118507w6n.html";
            this.Position = string.Empty;
            this.CurrentPageNumber = 1;
            this.GetDataEnable = true;
            this.IsTeacher = false;
            this.PageSize = 50;
            this.GetDataCommandText = "采集数据";
            lstArea = AreaNames.Split('、').ToList();
            this.TotalDataCount = GetTotalPage();
            LoadPageData();


        }

        //我这次点的是2 但是他返回的是上一次跌
        public ObservableCollection<ExamInfo> Exams
        {
            get
            {
                return _Exams;
            }
            set
            {
                _Exams = value;
                this.RaisePropertyChanged();
            }
        }

        private bool toClose = false;
        /// <summary>
        /// 是否要关闭窗口
        /// </summary>
        public bool ToClose
        {
            get
            {
                return toClose;
            }
            set
            {
                toClose = value;
                if (toClose)
                {
                    this.RaisePropertyChanged("ToClose");
                }
            }
        }

        /// <summary>
        /// 获取区域名称
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetAreaName(string str)
        {
            var result = lstArea.Where(s => str.Contains(s));
            return result.Count() > 0 ? result.FirstOrDefault() : string.Empty;
        }

        private long _TotalDataCount { get; set; }

        public long TotalDataCount
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

        private string _Position { get; set; }


        private string _Address { get; set; }

        private string _Date { get; set; }


        private bool _IsTeacher { get; set; }


        public bool IsTeacher
        {
            get
            {
                return _IsTeacher;
            }
            set
            {
                _IsTeacher = value;
                this.RaisePropertyChanged();
            }
        }

        public string Position
        {
            get
            {
                return _Position;
            }
            set
            {
                _Position = value;
                this.RaisePropertyChanged();
            }
        }


        public string Date
        {
            get
            {
                return _Date;
            }
            set
            {
                _Date = value;
                this.RaisePropertyChanged();
            }
        }

        public string Address
        {
            get
            {
                return _Address;
            }
            set
            {
                _Address = value;
                this.RaisePropertyChanged();
            }
        }


        public string _getDataCommandText = string.Empty;

        public string GetDataCommandText
        {
            get
            {
                return _getDataCommandText;
            }
            set
            {
                _getDataCommandText = value;
                this.RaisePropertyChanged();
            }
        }


        public bool _getDataEnable { get; set; }
        public bool GetDataEnable
        {
            get
            {
                return _getDataEnable;
            }
            set
            {
                _getDataEnable = value;
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
                         this.CurrentPageNumber = (int)o;
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

        private BaseCommand initDataCommand;

        private BaseCommand queryCommand;

        private BaseCommand getDataCommad;

        private BaseCommand enterScoreCommnd;

        public BaseCommand InitDataCommand
        {
            get
            {
                if (initDataCommand == null)
                {
                    initDataCommand = new BaseCommand(new Action<object>(o =>
                    {


                        //执行登录逻辑
                        // LoadPageData();
                        ReadExcelData();


                    }));
                }
                return initDataCommand;
            }
        }

        public BaseCommand EnterScoreCommnd
        {
            get
            {
                if (enterScoreCommnd== null)
                {
                    enterScoreCommnd= new BaseCommand(new Action<object>(o =>
                    {
                        WindowManager.Show("EnterScoreWindow", null);
                    }));
                }
                return enterScoreCommnd;
            }
        }
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
                        //ReadExcelData();


                    }));
                }
                return queryCommand;
            }
        }

        public void ReadExcelData1()
        {
            var data = File.ReadAllLines("DataBase\\datanew.txt");
            List<Recruit> recruits = new List<Recruit>();
            List<Task> taskList = new List<Task>();
            TaskFactory taskFactory = new TaskFactory();
            var beginTime = DateTime.Now;
            IdWorker worker = new IdWorker(1);
            var AreaName = string.Empty;
            foreach (var line in data)
            {
                //限制线程数量             //  ThreadPool.SetMaxThreads(20, 20);
                if (taskList.Count(t => t.Status != TaskStatus.RanToCompletion) >= 1)
                {
                    Task.WaitAny(taskList.ToArray());taskList = taskList.Where(t => t.Status != TaskStatus.RanToCompletion).ToList();
                }
                var Tasks = Task.Run(() =>
                 {
                     Recruit recruit = new Recruit(); recruit.Id = worker.nextId();recruit.CreateTime = DateTime.Now.ToString();
                    foreach (var item in line.Split('\t'))
                     {
                         var str = item.Replace("&nbsp;", "").Replace("\r", "").Replace("\n", "").Replace("\t", ""); ;
                         var tempAreaName = GetAreaName(str);
                         if (!string.IsNullOrEmpty(tempAreaName))
                         {AreaName = tempAreaName;}recruit.AreaName = AreaName;
                         if (str.StartsWith("01") || str.StartsWith("02") || str.StartsWith("03") || str.StartsWith("04") || str.StartsWith("05"))
                         {
                             recruit.Code = str;
                         }
                         else
                         {
                             try
                             {
                                 recruit.Number = Convert.ToInt32(str);
                             }
                             catch (Exception)
                             {
                             }
                         }
                     }
                     recruits.Add(recruit);
                 });
                taskList.Add(Tasks);
            }
            taskFactory.ContinueWhenAll(taskList.ToArray(), t =>
            {
                var ts = (DateTime.Now - beginTime).TotalMilliseconds; //6秒
                recruits = recruits.Where(r => r.Number > 0).Distinct().ToList();
                FreeSqlHelper.freeSql.Delete<Recruit>().Where(s => 1 == 1).ExecuteAffrows();
                FreeSqlHelper.freeSql.Insert<Recruit>(recruits).ExecuteAffrows();
            });
        }

        public void ReadExcelData()
        {
            var data = File.ReadAllLines("DataBase\\datanew.txt");

            List<Recruit> recruits = new List<Recruit>();

            List<Task> taskList = new List<Task>();
            TaskFactory taskFactory = new TaskFactory();
            var beginTime = DateTime.Now;
            var AreaName = string.Empty;
            foreach (var line in data)
            {
                Recruit recruit = new Recruit();
                recruit.CreateTime = DateTime.Now.ToString();
                //使用多线程
                foreach (var item in line.Split('\t'))
                {

                    var str = item.Replace("&nbsp;", "").Replace("\r", "").Replace("\n", "").Replace("\t", ""); ;
                    var tempAreaName = GetAreaName(str);
                    if (!string.IsNullOrEmpty(tempAreaName))
                    {
                        AreaName = tempAreaName;
                    }
                    //保持到下一个值有之间
                    recruit.AreaName = AreaName;
                    if (str.StartsWith("01") || str.StartsWith("02") || str.StartsWith("03") || str.StartsWith("04") || str.StartsWith("05"))
                    {
                        recruit.Code = str;
                    }
                    else
                    {
                        try
                        {
                            recruit.Number = Convert.ToInt32(str);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                recruits.Add(recruit);


            }
            var ts = (DateTime.Now - beginTime).TotalMilliseconds;
            //14秒

        }


        public void ReadExcelData2()
        {
            var data = File.ReadAllLines("DataBase\\datanew.txt");

            List<Recruit> recruits = new List<Recruit>();

            List<Task> taskList = new List<Task>();
            TaskFactory taskFactory = new TaskFactory();
            var beginTime = DateTime.Now;
            //  ThreadPool.SetMaxThreads(20, 20);
            IdWorker worker = new IdWorker(1);
            var AreaName = string.Empty;
            foreach (var line in data)
            {

                //限制线程数量
                if (taskList.Count(t => t.Status != TaskStatus.RanToCompletion) >= 1)
                {
                    Task.WaitAny(taskList.ToArray());
                    taskList = taskList.Where(t => t.Status != TaskStatus.RanToCompletion).ToList();
                }
                var Tasks = Task.Run(() =>
                {
                    Recruit recruit = new Recruit();
                    recruit.Id = worker.nextId();
                    recruit.CreateTime = DateTime.Now.ToString();
                    //使用多线程
                    foreach (var item in line.Split('\t'))
                    {

                        var str = item.Replace("&nbsp;", "").Replace("\r", "").Replace("\n", "").Replace("\t", ""); ;
                        var tempAreaName = GetAreaName(str);
                        if (!string.IsNullOrEmpty(tempAreaName))
                        {
                            AreaName = tempAreaName;
                        }
                        //保持到下一个值有之间
                        recruit.AreaName = AreaName;
                        if (str.StartsWith("01") || str.StartsWith("02") || str.StartsWith("03") || str.StartsWith("04") || str.StartsWith("05"))
                        {
                            recruit.Code = str;
                        }
                        else
                        {
                            try
                            {
                                recruit.Number = Convert.ToInt32(str);
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                    recruits.Add(recruit);
                });
                taskList.Add(Tasks);
            }
            taskFactory.ContinueWhenAll(taskList.ToArray(), t =>
            {

                var ts = (DateTime.Now - beginTime).TotalMilliseconds;
                recruits = recruits.Where(r => r.Number > 0).Distinct().ToList();
                var res = recruits.Where(s => s.Code == "02015");
                FreeSqlHelper.freeSql.Delete<Recruit>().Where(s => 1 == 1).ExecuteAffrows();
                FreeSqlHelper.freeSql.Insert<Recruit>(recruits).ExecuteAffrows();
                //28997.9987
                //35436.0009
                //37141 50线程
                //33554.998 10个线程
                //8140. 1 个线程
                //8134.9996 2个线程
                //12427.998800000001 5个线程
                //8509
                //20969.0165 Threed Pool 8个线程
                //38214.9979 Threed Pool 2个线程
            });
        }
        public BaseCommand GetDataCommand
        {
            get
            {

                if (getDataCommad == null)
                {
                    getDataCommad = new BaseCommand(new Action<object>(o =>
                    {


                        this.GetDataEnable = false;

                        var result = httpHelper.httpGet(this.Address);
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(result);
                        var data = doc.DocumentNode.SelectNodes("//table/tbody/tr");
                        var AreaName = string.Empty;
                        ExamInfo examInfo = new ExamInfo();
                        List<ExamInfo> examInfos = new List<ExamInfo>();
                        IdWorker worker = new IdWorker(1);
                        foreach (var item in data)
                        {
                            var td = item.SelectNodes("td");

                            if (td == null || td.Count() <= 0)
                            {
                                continue;
                            }
                            else if (td.Count() == 1)
                            {
                                var tdText = td.FirstOrDefault().InnerText;
                                if (tdText.Contains("考试名称"))
                                {
                                    AreaName = GetAreaName(tdText);
                                }
                            }
                            else if (td.Count() == 4)
                            {
                                var tdText = td.FirstOrDefault().InnerText.Replace("&nbsp;", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                                if (tdText.Contains("报考单位") || string.IsNullOrEmpty(tdText))
                                {
                                    continue;
                                }
                                examInfo = new ExamInfo();
                                examInfo.Id = worker.nextId();
                                examInfo.Company = td[0].InnerText.Trim();
                                examInfo.Position = td[1].InnerText.Trim();
                                examInfo.Code = Regex.Replace(examInfo.Position, @"[^0-9]+", "");
                                examInfo.Position = examInfo.Position.Replace(examInfo.Code, "");
                                examInfo.HasPay = Convert.ToInt32(td[2].InnerText);
                                examInfo.NotPay = Convert.ToInt32(td[3].InnerText);
                                examInfo.CreateTime = DateTime.Now.ToString();
                                examInfo.Area = AreaName;
                                examInfo.Date = DateTime.Now.ToString("yyyy-MM-dd");
                                examInfos.Add(examInfo);
                            }
                        }

                        //删除数据 在插入数据
                        var rows = FreeSqlHelper.freeSql.Delete<ExamInfo>().Where(s => s.Date == DateTime.Now.ToString("yyyy-MM-dd")).ExecuteAffrows();

                        rows = FreeSqlHelper.freeSql.Insert<ExamInfo>(examInfos).ExecuteAffrows();


                        this.GetDataEnable = true;
                        this.GetDataCommandText = "采集数据";



                        System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            MessageBox.Show("采集成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        }));


                    }));
                }


                return getDataCommad;
            }
        }

        public void LoadPageData()
        {
            var data = FreeSqlHelper.freeSql.Queryable<ExamInfo>().
                Where(s => s.Date == this.Date && s.Position.Contains(this.Position));


            var recruits = FreeSqlHelper.freeSql.Queryable<Recruit>().ToList();

            if (IsTeacher)
            {
                data = data.Where(s => s.Position.Contains("优秀教师"));
            }
            else
            {
                data = data.Where(s => s.Position.Contains("优秀教师") == false);
            }
            this.TotalDataCount = data.Count();
            data = data.Skip((CurrentPageNumber - 1) * PageSize).Take(PageSize);


            var result = data.ToList();

            for (int i = 0; i < result.Count; i++)
            {
                if (recruits.Any(s => s.Code == result[i].Code && s.AreaName == result[i].Area))
                {
                    result[i].NeedNumber = recruits.Where(s => s.Code == result[i].Code && s.AreaName == result[i].Area).FirstOrDefault().Number;
                }

            }
            this.Exams = new ObservableCollection<ExamInfo>(result);
        }


        public long GetTotalPage()
        {
            return FreeSqlHelper.freeSql.Queryable<ExamInfo>().Count();
        }



    }
}
