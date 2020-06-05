using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
   public class Recruit
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public int Number { get; set; }

        public string AreaName { get; set; }

        public string CreateTime { get; set; }
    }
}
