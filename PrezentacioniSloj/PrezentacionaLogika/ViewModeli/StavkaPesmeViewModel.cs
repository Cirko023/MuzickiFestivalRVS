using System.ComponentModel.DataAnnotations;

namespace PrezentacioniSloj.PrezentacionaLogika.ViewModeli;

public class StavkaPesmeViewModel
{
    public int StavkaID { get; set; }

    [Required(ErrorMessage = "Redni broj je obavezan.")]
    [Range(1, 50, ErrorMessage = "Redni broj mora biti između 1 i 50.")]
    public int RedniBroj { get; set; }

    [Required(ErrorMessage = "Naziv pesme je obavezan.")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Naziv pesme mora imati 2-150 karaktera.")]
    public string NazivPesme { get; set; } = string.Empty;

    [Required(ErrorMessage = "Trajanje je obavezno.")]
    [Range(1, 30, ErrorMessage = "Trajanje mora biti između 1 i 30 minuta.")]
    public int TrajanjeMinute { get; set; }
}
