// Data/SqlHelper.cs
using System.Configuration;
using System.Data.SqlClient;

namespace Exercise3.Data
{
    public static class SqlHelper
    {
        public static SqlConnection Open()
        {
            var cs = ConfigurationManager.ConnectionStrings["MemoryGameDb"].ConnectionString;
            var cn = new SqlConnection(cs);
            cn.Open();
            return cn;
        }
    }
}
