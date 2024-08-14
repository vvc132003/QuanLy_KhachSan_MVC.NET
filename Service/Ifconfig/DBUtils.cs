using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace ketnoicsdllan1
{
    public class DBUtils
    {
        public static SqlConnection GetDBConnection()
        {
            string datasource = @"LAPCN-CHINHVV\SQLEXPRESS";
            string database = "QuanLyKhachSan";
            string username = "sa";
            string password = "chinh@2003";
            return DBSQLServerUtils.GetDBConnection(datasource, database, username, password);
        }
        /*  public static SqlConnection GetDBConnection()
          {
              string datasource = @"SQL8010.site4now.net";
              string database = "db_aab98b_quanlykhachsan";
              string username = "db_aab98b_quanlykhachsan_admin";
              string password = "gamehay1275";
              return DBSQLServerUtils.GetDBConnection(datasource, database, username, password);
          }*/
    }
}