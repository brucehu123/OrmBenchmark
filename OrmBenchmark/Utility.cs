using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Transactions;
using System.Data.Common;
using System.Data.SqlClient;

namespace OrmBenchmark
{
    public static class Utility
    {
        static Utility()
        {
            ConnectionString = "Server=.;Initial Catalog=OrmBenchmark;User ID= sa;Password=123456;";
        }
        public static long Watch(Action action)
        {
            var watch = Stopwatch.StartNew();
            using (var tran = new TransactionScope(TransactionScopeOption.Required))
            {
                action();
            }
            watch.Stop();
            return watch.ElapsedTicks;
        }

        public static string ConnectionString { get; }

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static SqlDataAdapter CreateAdapter(string sql, DbConnection con)
        {
            return new SqlDataAdapter(sql, (SqlConnection)con);
        }
    }
}
