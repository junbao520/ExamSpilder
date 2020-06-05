using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;

namespace WpfApp1.Helper
{
    public class FreeSqlHelper
    {
       public static IFreeSql freeSql = null;
        static FreeSqlHelper()
        {
          freeSql=   new FreeSql.FreeSqlBuilder().UseConnectionString(DataType.Sqlite, "data source=DataBase\\data.db").Build();
        }
    }
}
