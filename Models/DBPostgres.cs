using Npgsql;

namespace Cinepax.Models
{
    public class DBPostgres
    {
        string connexionString;
        NpgsqlConnection connexion;

        public NpgsqlConnection Connection()
        {
            connexionString = $"Server=localhost;Port=5432;Database=btp;User Id=postgres;Password=root";
            connexion = new NpgsqlConnection(connexionString);
            return connexion;
        }

        public void close()
        {
            try
            {
                this.Connection().Close();
            }
            catch (System.Exception)
            {
                throw new Exception("Erreur du fermeture du connection");
            }
        }

        public void open()
        {
            try
            {
                this.Connection().Open();
            }
            catch (System.Exception)
            {
                throw new Exception("Erreur d'ouverture du connection");
            }
        }
    }
}
