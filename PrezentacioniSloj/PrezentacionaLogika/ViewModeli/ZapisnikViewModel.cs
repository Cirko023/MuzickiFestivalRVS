using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PrezentacioniSloj.PrezentacionaLogika.ViewModeli;

public class ZapisnikViewModel
{
    public int ZapisnikID { get; set; }

    [Required(ErrorMessage = "Naziv festivala je obavezan.")]
    [StringLength(100)]
    public string NazivFestivala { get; set; } = string.Empty;

    [Required(ErrorMessage = "Lokacija je obavezna.")]
    [StringLength(100)]
    public string Lokacija { get; set; } = string.Empty;

    [Required(ErrorMessage = "Datum koncerta je obavezan.")]
    [DataType(DataType.Date)]
    public DateTime DatumKoncerta { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Bina je obavezna.")]
    [StringLength(50)]
    public string Bina { get; set; } = string.Empty;

    [Required(ErrorMessage = "Izvođač je obavezan.")]
    public int IzvodjacID { get; set; }

    [Required(ErrorMessage = "Vreme početka je obavezno.")]
    public string VremePocetka { get; set; } = "20:00";

    [Required(ErrorMessage = "Vreme završetka je obavezno.")]
    public string VremeZavrsetka { get; set; } = "21:00";

    public bool NaVreme { get; set; } = true;

    [StringLength(200)]
    public string? RazlogKasnjenja { get; set; }

    [Required(ErrorMessage = "Trajanje nastupa je obavezno.")]
    [Range(1, 480)]
    public int TrajanjeNastupaMinuta { get; set; } = 60;

    [Required(ErrorMessage = "Broj pesama je obavezan.")]
    [Range(1, 50)]
    public int BrojPesama { get; set; } = 1;

    public bool SpecijalniEfekti { get; set; }

    [StringLength(500)]
    public string? Napomena { get; set; }

    public bool OzvucenjeIspravno { get; set; } = true;
    public bool RasvetaIspravna { get; set; } = true;

    [StringLength(500)]
    public string? TehnickiProblemi { get; set; }

    [Required(ErrorMessage = "Odgovorno lice je obavezno.")]
    [StringLength(100)]
    public string OdgovornoLice { get; set; } = string.Empty;

    [Required(ErrorMessage = "Potpis je obavezan.")]
    [StringLength(100)]
    public string Potpis { get; set; } = string.Empty;

    public DateTime? DatumOd { get; set; }
    public DateTime? DatumDo { get; set; }
    public int? FilterIzvodjacId { get; set; }

    public List<StavkaPesmeViewModel> Stavke { get; set; } = new();
    public List<SelectListItem> Izvodjaci { get; set; } = new();
}
