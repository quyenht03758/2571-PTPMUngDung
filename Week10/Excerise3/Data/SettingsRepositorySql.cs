using System.Data.SqlClient;
using Exercise3.Models;

namespace Exercise3.Data
{
    public class SettingsRepositorySql : ISettingsRepository
    {
        public GameSettings Load()
        {
            using (var cn = SqlHelper.Open())
            using (var cmd = new SqlCommand("SELECT TOP 1 Rows, Cols, Pairs, FlipMs, MismatchDelayMs FROM dbo.Settings", cn))
            using (var rd = cmd.ExecuteReader())
            {
                if (rd.Read())
                {
                    return new GameSettings
                    {
                        Rows = rd.GetInt32(0),
                        Cols = rd.GetInt32(1),
                        Pairs = rd.GetInt32(2),
                        FlipMs = rd.GetInt32(3),
                        MismatchDelayMs = rd.GetInt32(4)
                    };
                }
                else
                {
                    // default fallback
                    return new GameSettings { Rows = 4, Cols = 4, Pairs = 8, FlipMs = 300, MismatchDelayMs = 700 };
                }
            }
        }

        public void Save(GameSettings s)
        {
            using (var cn = SqlHelper.Open())
            using (var cmd = new SqlCommand(@"
MERGE dbo.Settings AS t
USING (SELECT 1 AS Id) AS s
ON (t.Id = s.Id)
WHEN MATCHED THEN
    UPDATE SET Rows=@r, Cols=@c, Pairs=@p, FlipMs=@f, MismatchDelayMs=@m
WHEN NOT MATCHED THEN
    INSERT (Id, Rows, Cols, Pairs, FlipMs, MismatchDelayMs)
    VALUES (1,@r,@c,@p,@f,@m);", cn))
            {
                cmd.Parameters.AddWithValue("@r", s.Rows);
                cmd.Parameters.AddWithValue("@c", s.Cols);
                cmd.Parameters.AddWithValue("@p", s.Pairs);
                cmd.Parameters.AddWithValue("@f", s.FlipMs);
                cmd.Parameters.AddWithValue("@m", s.MismatchDelayMs);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
