using Exercise3.Models;
using System.Data.SqlClient;

namespace Exercise3.Data
{
    public class SettingsRepository
    {
        public GameSettings Load()
        {
            using (var cn = SqlHelper.Open())
            using (var cmd = new SqlCommand("SELECT Rows, Cols, Pairs, FlipMs, MismatchDelayMs FROM dbo.Settings WHERE Id=1", cn))
            using (var rd = cmd.ExecuteReader())
            {
                rd.Read();
                return new GameSettings
                {
                    Rows = rd.GetInt32(0),
                    Cols = rd.GetInt32(1),
                    Pairs = rd.GetInt32(2),
                    FlipMs = rd.GetInt32(3),
                    MismatchDelayMs = rd.GetInt32(4)
                };
            }
        }

        public void Save(GameSettings s)
        {
            using (var cn = SqlHelper.Open())
            using (var cmd = new SqlCommand(@"
UPDATE dbo.Settings
SET Rows=@r, Cols=@c, Pairs=@p, FlipMs=@f, MismatchDelayMs=@m
WHERE Id=1;", cn))
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
