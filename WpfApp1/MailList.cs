using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class MailList : INotifyPropertyChanged
    {
        public string senduser;
        public string topic;
        public string file;
        public string time;
        public event PropertyChangedEventHandler PropertyChanged;
        public string Senderuser
        {
            get
            {
                return senduser;
            }
            set
            {
                senduser = value;
                if (this.PropertyChanged != null)//激发事件，参数为Age属性  
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Age"));
                }
            }
        }

        public string Topic
        {
            get
            {
                return topic;
            }
            set
            {
                topic = value;
                if (this.PropertyChanged != null)//激发事件，参数为Age属性  
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Age"));
                }
            }
        }

        public string Ffile
        {
            get
            {
                return file;
            }
            set
            {
                file = value;
                if (this.PropertyChanged != null)//激发事件，参数为Age属性  
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Age"));
                }
            }
        }

        public string Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                if (this.PropertyChanged != null)//激发事件，参数为Age属性  
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Age"));
                }
            }
        }

        public MailList() { }
        public MailList(string senduser, string topic, string file, string time)
        {
            this.senduser = senduser;
            this.topic = topic;
            this.file = file;
            this.time = time;
        }
    }
}
