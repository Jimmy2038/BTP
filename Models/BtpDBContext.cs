using Microsoft.EntityFrameworkCore;
using BTP.Models.Auth;
using BTP.Models.Client;
using BTP.Models.Util_devis;

namespace BTP.Models
{
    public class BtpDBContext : DbContext
    {
        public BtpDBContext(DbContextOptions<BtpDBContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Désactive l'application automatique des migrations
            optionsBuilder.EnableServiceProviderCaching(false);
        }

        public DbSet<Authentif> auth { get; set; }
        public DbSet<Utilisateur> user { get; set; }
        public DbSet<Devis> devis { get; set; }
        public DbSet<DetailMaison> detailMaisons { get; set; }
        public DbSet<Maison> maisons { get; set; }
        public DbSet<Finition> finitions { get; set; }
        public DbSet<Travaux> travaux { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Authentif>().ToTable("auth", schema: "public").HasKey(auth => auth.id);
            modelBuilder.Entity<Utilisateur>().ToTable("client", schema: "public").HasKey(user => user.id);
            modelBuilder.Entity<Devis>().ToTable("devis", schema: "public").HasKey(devis => devis.id);
            modelBuilder.Entity<DetailMaison>().ToTable("detail_travau_maison", schema: "public").HasKey(detailM => detailM.id);
            modelBuilder.Entity<Maison>().ToTable("maison", schema: "public").HasKey(maison => maison.id);
            modelBuilder.Entity<Finition>().ToTable("finition", schema: "public").HasKey(finition => finition.id);
            modelBuilder.Entity<Travaux>().ToTable("travaux", schema: "public").HasKey(trav => trav.id);


        }
    }
}
