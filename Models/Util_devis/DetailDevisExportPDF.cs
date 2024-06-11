namespace BTP.Models.Util_devis
{
    public class DetailDevisExportPDF
    {
        public List<DetailDevis> detailDevis { get; set; }
        public List<Paiement>? allPaiement { get; set; }
        public double totalpaiement { get; set; }
        public double totaldevis { get; set; }
    }
}
