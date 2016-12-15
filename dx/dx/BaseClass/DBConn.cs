using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace dx.BaseClass
{
    class DBConn
    {
        public static MySqlConnection DxCon()
        {
            return new MySqlConnection("server=localhost;User Id=root;password=dx050609;Database=chuanhaozi;Charset=utf8");
        }
    }
}
