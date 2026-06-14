using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlojPodataka.KlasePodataka;

[Table("ZapisnikKoncerta")]
public class ZapisnikKoncerta : OsnovniEntitet
{
    [Key]
    public int ZapisnikID { get; set; }

    [Required]
    [StringLength(100)]
    public string NazivFestivala { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Lokacija { get; set; } = string.Empty;

    [Required]
    public DateTime DatumKoncerta { get; set; }

    [Required]
    [StringLength(50)]
    public string Bina { get; set; } = string.Empty;

    [ForeignKey("Izvodjac")]
    [Required]
    public int IzvodjacID { get; set; }
    public Izvodjac Izvodjac { get; set; } = null!;

    [Required]
    public TimeSpan VremePocetka { get; set; }

    [Required]
    public TimeSpan VremeZavrsetka { get; set; }

    [Required]
    public bool NaVreme { get; set; }

    [StringLength(200)]
    public string? RazlogKasnjenja { get; set; }

    [Required]
    [Range(1, 480)]
    public int TrajanjeNastupaMinuta { get; set; }

    [Required]
    [Range(1, 50)]
    public int BrojPesama { get; set; }

    [Required]
    public bool SpecijalniEfekti { get; set; }

    [StringLength(500)]
    public string? Napomena { get; set; }

    [Required]
    public bool OzvucenjeIspravno { get; set; }

    [Required]
    public bool RasvetaIspravna { get; set; }

    [StringLength(500)]
    public string? TehnickiProblemi { get; set; }

    [Required]
    [StringLength(100)]
    public string OdgovornoLice { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Potpis { get; set; } = string.Empty;

    public decimal OsnovnaKotizacija { get; set; }
    public decimal PopustProcenat { get; set; }
    public decimal KotizacijaSaPopustom { get; set; }

    public ICollection<StavkaIzvedenePesme> Stavke { get; set; } = new List<StavkaIzvedenePesme>();
}
