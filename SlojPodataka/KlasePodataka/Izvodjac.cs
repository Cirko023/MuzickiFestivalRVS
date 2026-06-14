using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlojPodataka.KlasePodataka;

[Table("Izvodjac")]
public class Izvodjac : OsnovniEntitet
{
    [Key]
    public int IzvodjacID { get; set; }

    [Required]
    [StringLength(100)]
    public string NazivBenda { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Grad { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string ZanrMuzike { get; set; } = string.Empty;

    [Required]
    [Range(0, 1000000)]
    public decimal Kotizacija { get; set; }

    public ICollection<ZapisnikKoncerta> Koncerti { get; set; } = new List<ZapisnikKoncerta>();
}
