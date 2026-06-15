using System.ComponentModel.DataAnnotations;

namespace PrezentacioniSloj.PrezentacionaLogika.ViewModeli;

public class RegistracijaViewModel
{
    [Required(ErrorMessage = "Korisničko ime je obavezno.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Korisničko ime mora imati 3-50 karaktera.")]
    public string KorisnickoIme { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email je obavezan.")]
    [EmailAddress(ErrorMessage = "Unesite ispravnu email adresu.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Lozinka je obavezna.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Lozinka mora imati najmanje 6 karaktera.")]
    [DataType(DataType.Password)]
    public string Lozinka { get; set; } = string.Empty;
}
