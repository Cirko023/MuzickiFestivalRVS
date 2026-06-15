using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PrezentacioniSloj.PrezentacionaLogika.ViewModeli;

public class PrijavaViewModel
{
    [Required(ErrorMessage = "Korisničko ime je obavezno.")]
    public string KorisnickoIme { get; set; } = string.Empty;

    [Required(ErrorMessage = "Lozinka je obavezna.")]
    [DataType(DataType.Password)]
    public string Lozinka { get; set; } = string.Empty;
}
