using Microsoft.EntityFrameworkCore;
using SlojPodataka.KlasePodataka;
using System.Reflection.Emit;

namespace SlojPodataka.TehnoloskeKlase;

public class FestivalDbContext : DbContext
{
    public FestivalDbContext(DbContextOptions<FestivalDbContext> options) : base(options) { }

    public DbSet<Korisnik> Korisnici { get; set; }
    public DbSet<Izvodjac> Izvodjaci { get; set; }
    public DbSet<ZapisnikKoncerta> Zapisnici { get; set; }
    public DbSet<StavkaIzvedenePesme> StavkePesama { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ZapisnikKoncerta>()
            .HasOne(z => z.Izvodjac)
            .WithMany(i => i.Koncerti)
            .HasForeignKey(z => z.IzvodjacID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StavkaIzvedenePesme>()
            .HasOne(s => s.Zapisnik)
            .WithMany(z => z.Stavke)
            .HasForeignKey(s => s.ZapisnikID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
