using System.Data.SqlClient;

namespace Exercise3.Data
{
    public class ScoreRepositorySql : IScoreRepository
    {
        public void Save(string player, int score, int level, int durationSec)
        {
            using (var cn = SqlHelper.Open())
            using (var cmd = new SqlCommand(
                "INSERT INTO dbo.Scores(Player, Score, Level, DurationSec) VALUES(@p,@s,@l,@d)", cn))
            {
                cmd.Parameters.AddWithValue("@p", player);
                cmd.Parameters.AddWithValue("@s", score);
                cmd.Parameters.AddWithValue("@l", level);
                cmd.Parameters.AddWithValue("@d", durationSec);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
