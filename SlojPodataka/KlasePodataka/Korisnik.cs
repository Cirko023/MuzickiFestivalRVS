using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlojPodataka.KlasePodataka;

[Table("Korisnik")]
public class Korisnik : OsnovniEntitet
{
    [Key]
    public int KorisnikID { get; set; }

    [Required]
    [StringLength(50)]
    public string KorisnickoIme { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(256)]
    public string LozinkaHes { get; set; } = string.Empty;

    [Required]
    [StringLength(64)]
    public string Salt { get; set; } = string.Empty;
}
