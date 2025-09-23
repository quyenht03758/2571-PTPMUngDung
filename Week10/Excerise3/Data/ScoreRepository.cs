using System;
using System.Data.SqlClient;

namespace Exercise3.Data
{
    public class ScoreRepository
    {
        public void Save(string player, int score, int level, int durationSec)
        {
            using (var cn = SqlHelper.Open())
            using (var cmd = new SqlCommand(@"
INSERT INTO dbo.Scores(Player, Score, Level, DurationSec, PlayedAt)
VALUES(@p, @s, @l, @d, SYSUTCDATETIME());", cn))
            {
                cmd.Parameters.AddWithValue("@p", (object)player ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@s", score);
                cmd.Parameters.AddWithValue("@l", level);
                cmd.Parameters.AddWithValue("@d", durationSec);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
