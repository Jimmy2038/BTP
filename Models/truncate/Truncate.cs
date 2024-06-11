using Cinepax.Models;
using Npgsql;
using System.Data;
using System.Data.SqlClient;

namespace BTP.Models.truncate
{
    public class Truncate
    {
        DBPostgres db = new DBPostgres();
        private string connectionString = "Server=localhost;Port=5432;Database=btp;Username=postgres;Password=root";
        

        public Truncate(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void TruncateTablesAndResetSequences()
        {
            // Récupérer la liste des tables existantes

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                List<string> tables = GetAllTables(connection);

                // Désactiver temporairement les contraintes de clé étrangère
                DisableForeignKeys(connection);

                // Tronquer les tables
                TruncateTables(connection, tables);

                // Réinitialiser les séquences
                ResetSequences(connection, tables);

                // Réactiver les contraintes de clé étrangère
                EnableForeignKeys(connection);
            }
        }
        public List<string> GetAllTables(NpgsqlConnection connection)
        {
            List<string> tables = new List<string>();

            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_type = 'BASE TABLE';";

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(reader.GetString(0));
                    }
                }
            }

            return tables;
        }
        private List<string> GetExistingTables()
        {
            List<string> tables = new List<string>();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand("SELECT table_name FROM information_schema.tables WHERE table_schema = 'public'", connection))
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetString(0).GetHashCode() != "maison_traveau_temp".GetHashCode() && reader.GetString(0).GetHashCode() != "devis_temp".GetHashCode() && reader.GetString(0).GetHashCode() != "payement_temp".GetHashCode())
                        {
                            tables.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return tables;
        }

        private void DisableForeignKeys(NpgsqlConnection connection)
        {
            using (NpgsqlCommand command = new NpgsqlCommand("SET session_replication_role = 'replica';", connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void TruncateTables(NpgsqlConnection connection, List<string> tables)
        {

            foreach (string table in tables)
            {
                
                    using (NpgsqlCommand command = new NpgsqlCommand($"TRUNCATE TABLE {table} CASCADE;", connection))
                    {
                        command.ExecuteNonQuery();
                    }

                
            }
        }

        private void ResetSequences(NpgsqlConnection connection, List<string> tables)
        {
            foreach (string table in tables)
            {
                using (NpgsqlCommand command = new NpgsqlCommand($"SELECT setval(pg_get_serial_sequence('{table}', 'id'), 1, false);", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void EnableForeignKeys(NpgsqlConnection connection)
        {
            using (NpgsqlCommand command = new NpgsqlCommand("SET session_replication_role = 'origin';", connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
