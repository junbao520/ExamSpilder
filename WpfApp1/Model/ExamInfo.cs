using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class ExamInfo:Object
    {
        public long Id { get; set; }
        public string Date { get; set; }

        public string Area { get; set; }

        public string Company { get; set; }


        public string Code { get; set; }

        public string Position { get; set; }

        public int HasPay { get; set; }


        public int NotPay { get; set; }

        public int NeedNumber { get; set; }

        public string CreateTime { get; set; }

        [NotMapped]
        public double xw { get; set; }
        [NotMapped]
        public double zw { get; set; }
        [NotMapped]
        public double dw { get; set; }


    }
}
