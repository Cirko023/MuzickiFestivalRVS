using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlojPodataka.KlasePodataka;

[Table("StavkaIzvedenePesme")]
public class StavkaIzvedenePesme : OsnovniEntitet
{
    [Key]
    public int StavkaID { get; set; }

    [ForeignKey("Zapisnik")]
    [Required]
    public int ZapisnikID { get; set; }
    public ZapisnikKoncerta Zapisnik { get; set; } = null!;

    [Required]
    [Range(1, 50)]
    public int RedniBroj { get; set; }

    [Required]
    [StringLength(150)]
    public string NazivPesme { get; set; } = string.Empty;

    [Required]
    [Range(1, 30)]
    public int TrajanjeMinute { get; set; }
}
