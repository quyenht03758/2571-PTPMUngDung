using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Week67
{
    public static class Db
    {
        private static readonly string _cs =
            ConfigurationManager.ConnectionStrings["CompanyDb"].ConnectionString;

        public static SqlConnection Open()
        {
            var cn = new SqlConnection(_cs);
            cn.Open();
            return cn;
        }

        public static DataTable Query(string sql, params SqlParameter[] ps)
        {
            using (var cn = Open())
            using (var cmd = new SqlCommand(sql, cn))
            {
                if (ps != null && ps.Length > 0) cmd.Parameters.AddRange(ps);
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public static int Execute(string sql, params SqlParameter[] ps)
        {
            using (var cn = Open())
            using (var cmd = new SqlCommand(sql, cn))
            {
                if (ps != null && ps.Length > 0) cmd.Parameters.AddRange(ps);
                return cmd.ExecuteNonQuery();
            }
        }
    }
}