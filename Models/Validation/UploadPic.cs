using ImageMagick;
using System.ComponentModel.DataAnnotations;

namespace BTP.Models.Validation
{
    public class UploadPic
    {
        [Required(ErrorMessage = "Veuillez sélectionner un fichier.")]
        [AllowedExtensions(".jpg", ".jpeg", ".png,", ".csv", ErrorMessage = "Seuls les fichiers avec les extensions .jpg, .jpeg, .png, .csv sont autorisés.")]
        public IFormFile maisonTravaux { get; set; }
        public IFormFile? devis { get; set; }

        public static string toBase64(string path)
        {
            // Chemin vers l'image à lire
            string imagePath = path;

            // Lire l'image en tant que tableau d'octets
            byte[] imageBytes = File.ReadAllBytes(imagePath);

            // Convertir le tableau d'octets en une chaîne base 64
            string base64String = Convert.ToBase64String(imageBytes);

            return base64String;
        }

        public static async Task<string> TransferFile(IFormFile file, string targetDirectory)
        {
            // Vérifie si le fichier et le répertoire cible sont valides
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Le fichier est vide ou n'existe pas.");
            }
            if (string.IsNullOrEmpty(targetDirectory))
            {
                throw new ArgumentException("Le répertoire cible n'est pas spécifié.");
            }

            // Crée le chemin complet de destination
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(targetDirectory, fileName);

            // Copie le contenu du fichier dans le dossier cible
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath; // Retourne le chemin complet du fichier transféré
        }

        public static string resizeImage(string path)
        {
            // Chemin vers l'image d'entrée
            string inputImageName = "input.jpg";

            // Définir les dimensions souhaitées (width, height)
            int desiredWidth = 800;
            int desiredHeight = 600;
            string outputImagePath = null;
            // Charger l'image à redimensionner
            using (MagickImage image = new MagickImage(path))
            {
                // Redimensionner l'image aux dimensions souhaitées
                image.Resize(desiredWidth, desiredHeight);
                inputImageName = image.FileName;

                // Redimensionner l'image en termes de taille en Mo (méga-octets)
                double targetFileSizeInMb = 0.4; // Taille cible en Mo
                image.Quality = 100; // Qualité initiale à 100%

                // Réduire progressivement la qualité jusqu'à atteindre la taille cible
                while (image.ToByteArray().Length > targetFileSizeInMb * 1024 * 1024)
                {
                    image.Quality -= 5; // Réduction de la qualité par pas de 5%
                }

                // Sauvegarder l'image redimensionnée
                outputImagePath = inputImageName;
                image.Write(outputImagePath);
            }
            return outputImagePath;
        }

        public static string getBase64FileName(string path, string nameFile)
        {
            string pathresize = UploadPic.resizeImage(path);
            string base64 = UploadPic.toBase64(pathresize);
            //renomer le fichier
            string directory = Path.GetDirectoryName(pathresize);
            string newFilePath = Path.Combine(directory, nameFile);

            System.IO.File.Move(pathresize, newFilePath);
            //System.IO.File.Delete(pathresize);
            return base64;
        }
    }
}
