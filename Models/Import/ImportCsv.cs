using Cinepax.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Npgsql;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace BTP.Models.Import
{
    public class ImportCsv
    {
        private readonly BtpDBContext _context;

        public ImportCsv(BtpDBContext context)
        {
            _context = context;
        }

        public ImportCsv()
        {
        }

        public static List<string[]> ReadCsvFile(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("Le fichier CSV spécifié n'existe pas.");
                return null;
            }

            List<string[]> allRecords = new List<string[]>();

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false, // Change to true if your CSV has headers
            }))
            {
                while (csv.Read())
                {
                    var record = new List<string>();
                    for (var i = 0; csv.TryGetField<string>(i, out var field); i++)
                    {
                        record.Add(field.Replace("%"," "));
                    }
                    allRecords.Add(record.ToArray());
                }
            }

            return allRecords;
        }
        public static List<string[]> csv(string path)
        {
            string csvFilePath = path;

            if (!File.Exists(csvFilePath))
            {
                Console.WriteLine("Le fichier CSV spécifié n'existe pas.");
                return null;
            }
            List<string[]> all = new List<string[]>();

            // Utilisez StreamReader pour lire le fichier CSV en UTF-8
            using (StreamReader reader = new StreamReader(csvFilePath, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Remplacer tous les % par des espaces
                    line = ReplacePercentWithSpace(line);

                    // Supposons que chaque ligne est séparée par un point-virgule (;)
                    string[] fields = line.Split(';');
                    foreach (string field in fields)
                    {
                        Console.Write(field + "\t"); // Affichez les champs sur la console (exemple)
                    }
                    Console.WriteLine(); // Saut de ligne pour la prochaine ligne
                    all.Add(fields);
                }
            }

            return all;
        }

        public static string ReplacePercentWithSpace(string input)
        {
            return input.Replace('%', ' ');
        }

        /*public static List<string[]> csv(string path)
        {
            string csvFilePath = path;

            if (!File.Exists(csvFilePath))
            {
                Console.WriteLine("Le fichier CSV spécifié n'existe pas.");
                return null;
            }
            List<string[]> all = new List<string[]>();

            // Utilisez StreamReader pour lire le fichier CSV en UTF-8
            using (StreamReader reader = new StreamReader(csvFilePath, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Supposons que chaque ligne est séparée par un point-virgule (;)
                    string[] fields = line.Split(';');
                    foreach (string field in fields)
                    {
                        Console.Write(field + "\t"); // Affichez les champs sur la console (exemple)
                    }
                    Console.WriteLine(); // Saut de ligne pour la prochaine ligne
                    all.Add(fields);
                }
            }

            return all;
        }*/

        public async Task insertMaisonTravauTemp(List<string[]> donnee)
        {
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                await connection.OpenAsync();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {

                    try
                    {
                        try
                        {
                            using (NpgsqlCommand cmd = new NpgsqlCommand("TRUNCATE TABLE maison_traveau_temp", connection))
                            {
                                cmd.ExecuteNonQuery();
                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Erreura: " + e.Message);
                        }
                        //_context.Database.ExecuteSqlRaw("TRUNCATE TABLE seanceTemporaire");
                        for (int i = 1; i < donnee.Count; i++)
                        {
                            using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO maison_traveau_temp  VALUES (@type_maison,@description,@surface,@code_travaux,@type_travaux,@unite,@prix_unitaire,@quantite,@duree_travaux)", connection))
                            {

                                command.Parameters.AddWithValue("@type_maison", donnee[i][0]);
                                command.Parameters.AddWithValue("@description", donnee[i][1]); 
                                command.Parameters.AddWithValue("@surface", donnee[i][2].Replace(',', '.')); 
                                command.Parameters.AddWithValue("@code_travaux", donnee[i][3]);
                                command.Parameters.AddWithValue("@type_travaux", donnee[i][4]);
                                command.Parameters.AddWithValue("@unite", donnee[i][5]);
                                command.Parameters.AddWithValue("@prix_unitaire", donnee[i][6].Replace(',', '.'));
                                command.Parameters.AddWithValue("@quantite", donnee[i][7].Replace(',', '.'));
                                command.Parameters.AddWithValue("@duree_travaux", donnee[i][8]);

                                await command.ExecuteNonQueryAsync();
                            }
                        }
                        await transaction.CommitAsync(); // Commit la transaction si l'insertion réussit
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); // Rollback la transaction en cas d'erreur
                        Console.WriteLine("Message d'erreru: " + ex.Message);
                        throw new Exception("Erreur lors de l'insertion : " + ex.Message);
                    }

                }
                connection.Close();
            }
        }

        public async Task insertPayement_temp(List<string[]> donnee)
        {
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                await connection.OpenAsync();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {

                    try
                    {
                        try
                        {
                            using (NpgsqlCommand cmd = new NpgsqlCommand("TRUNCATE TABLE payement_temp", connection))
                            {
                                cmd.ExecuteNonQuery();
                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Erreura: " + e.Message);
                        }
                        //_context.Database.ExecuteSqlRaw("TRUNCATE TABLE seanceTemporaire");
                        for (int i = 1; i < donnee.Count; i++)
                        {
                            try
                            {
                                using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO payement_temp  VALUES (@ref_devis,@ref_paiement,@date_paiement,@montant)", connection))
                                {
                                    command.Parameters.AddWithValue("@ref_devis", donnee[i][0]);
                                    command.Parameters.AddWithValue("@ref_paiement", donnee[i][1]);
                                    command.Parameters.AddWithValue("@date_paiement", donnee[i][2]);
                                    command.Parameters.AddWithValue("@montant", donnee[i][3].Replace(',', '.'));

                                    await command.ExecuteNonQueryAsync();
                                }
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine("tafiditra catch: ");
                                continue;
                            }
                        }
                        await transaction.CommitAsync(); // Commit la transaction si l'insertion réussit
                    }
                    catch (Exception ex)
                    {
                        /*transaction.Rollback(); */// Rollback la transaction en cas d'erreur
                        Console.WriteLine("Message d'erreru: " + ex.Message);
                        /*throw new Exception("Erreur lors de l'insertion : " + ex.Message);*/
                    }

                }
                connection.Close();
            }
        }
        public async Task insertDevisTempFinition(List<string[]> donnee)
        {
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                await connection.OpenAsync();
                using (NpgsqlTransaction transaction = connection.BeginTransaction())
                {

                    try
                    {
                        try
                        {
                            using (NpgsqlCommand cmd = new NpgsqlCommand("TRUNCATE TABLE devis_temp", connection))
                            {
                                cmd.ExecuteNonQuery();
                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Erreura: " + e.Message);
                        }
                        //_context.Database.ExecuteSqlRaw("TRUNCATE TABLE seanceTemporaire");
                        for (int i = 1; i < donnee.Count; i++)
                        {
                            using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO devis_temp  VALUES (@client,@ref_devis,@type_maison,@finition,@taux_finition,@date_devis,@date_debut,@lieu)", connection))
                            {
 
                                command.Parameters.AddWithValue("@client", donnee[i][0]);
                                command.Parameters.AddWithValue("@ref_devis", donnee[i][1]);
                                command.Parameters.AddWithValue("@type_maison", donnee[i][2]);
                                command.Parameters.AddWithValue("@finition", donnee[i][3]);
                                command.Parameters.AddWithValue("@taux_finition", donnee[i][4].Replace(',', '.'));
                                command.Parameters.AddWithValue("@date_devis", donnee[i][5]);
                                command.Parameters.AddWithValue("@date_debut", donnee[i][6]);
                                command.Parameters.AddWithValue("@lieu", donnee[i][7]);

                                await command.ExecuteNonQueryAsync();
                            }
                        }
                        await transaction.CommitAsync(); // Commit la transaction si l'insertion réussit
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); // Rollback la transaction en cas d'erreur
                        Console.WriteLine("Message d'erreru: " + ex.Message);
                        throw new Exception("Erreur lors de l'insertion : " + ex.Message);
                    }

                }
                connection.Close();
            }
        }

        public List<string> verifErreurMaisonTravauxTemp(List<string[]> all)
        {
            List<string> erreur = new List<string>();
            for (int i = 1; i < all.Count; i++)
            {

                MaisonTravauxTemp maisonTravauxTemp = new MaisonTravauxTemp(all[i][0], all[i][1], all[i][2], all[i][3], all[i][4], all[i][5], all[i][6], all[i][7], all[i][8]);
                var context = new ValidationContext(maisonTravauxTemp, serviceProvider: null, items: null);
                var results = new List<ValidationResult>();

                bool isValid = Validator.TryValidateObject(maisonTravauxTemp, context, results, true);

                if (!isValid)
                {
                    foreach (var validationResult in results)
                    {
                        erreur.Add("il y une erreur à la ligne " + i + ": " + validationResult.ErrorMessage);
                        Console.WriteLine("il y une erreur à la ligne: " + i + " " + validationResult.ErrorMessage);
                    }
                }
                /*else
                {
                    Console.WriteLine("Validation réussie à la ligne:" + i);
                }*/

                    }

                    return erreur;
        }

        public List<string> verifErreurClientDevis(List<string[]> all)
        {
            List<string> erreur = new List<string>();
            for (int i = 1; i < all.Count; i++)
            {

                ClientDevisFinition maisonTravauxTemp = new ClientDevisFinition(all[i][0], all[i][1], all[i][2], all[i][3], all[i][4], all[i][5], all[i][6], all[i][7]);
                var context = new ValidationContext(maisonTravauxTemp, serviceProvider: null, items: null);
                var results = new List<ValidationResult>();

                bool isValid = Validator.TryValidateObject(maisonTravauxTemp, context, results, true);

                if (!isValid)
                {
                    foreach (var validationResult in results)
                    {
                        erreur.Add("il y une erreur à la ligne " + i + ": " + validationResult.ErrorMessage);
                        Console.WriteLine("il y une erreur à la ligne: " + i + " " + validationResult.ErrorMessage);
                    }
                }
                /*else
                {
                    Console.WriteLine("Validation réussie à la ligne:" + i);
                }*/

            }

            return erreur;
        }

        public List<string> verifErreurPaiement(List<string[]> all)
        {
            List<string> erreur = new List<string>();
            for (int i = 1; i < all.Count; i++)
            {

                PaiementTemp payementTemp = new PaiementTemp(all[i][0], all[i][1], all[i][2], all[i][3]);
                var context = new ValidationContext(payementTemp, serviceProvider: null, items: null);
                var results = new List<ValidationResult>();

                bool isValid = Validator.TryValidateObject(payementTemp, context, results, true);

                if (!isValid)
                {
                    foreach (var validationResult in results)
                    {
                        erreur.Add("il y une erreur à la ligne " + i + ": " + validationResult.ErrorMessage);
                        Console.WriteLine("il y une erreur à la ligne: " + i + " " + validationResult.ErrorMessage);
                    }
                }
                /*else
                {
                    Console.WriteLine("Validation réussie à la ligne:" + i);
                }*/

            }

            return erreur;
        }
    }
}
