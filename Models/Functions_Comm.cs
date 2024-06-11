using Cinepax.Models;
using Npgsql;

namespace BTP.Models
{
    public class Functions_Comm
    {
        public static string generateId(string sequence, string suffix)
        {
            string idgenerated = "";
            DBPostgres bd = new DBPostgres();
            using (NpgsqlConnection con = bd.Connection())
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("select generate_id(@seq,@suff)", con))
                {
                    cmd.Parameters.AddWithValue("@seq", sequence);
                    cmd.Parameters.AddWithValue("@suff", suffix);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            idgenerated = reader.GetString(0);
                        }
                    }
                }
                con.Close();
            }
            return idgenerated;
        }
    }
}
