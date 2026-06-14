using Microsoft.AspNetCore.Mvc;
using SlojPodataka.KlasePodataka;
using SlojPodataka.Repozitorijum;
using SlojPodataka.TehnoloskeKlase;
using SlojServisa.KlaseDTO;

namespace SlojServisa.RESTKontroleri;

[ApiController]
[Route("api/[controller]")]
public class KorisnikRestController : ControllerBase
{
    private readonly KorisnikRepozitorijum _repozitorijum;

    public KorisnikRestController(KorisnikRepozitorijum repozitorijum)
    {
        _repozitorijum = repozitorijum;
    }

    [HttpPost("prijava")]
    public ActionResult Prijava([FromBody] PrijavaDTO dto)
    {
        var korisnik = _repozitorijum.DohvatiPoKorisnickomImenu(dto.KorisnickoIme);
        if (korisnik == null || !FunkcijeLozinke.ProveriLozinku(dto.Lozinka, korisnik.Salt, korisnik.LozinkaHes))
            return Unauthorized("Pogrešno korisničko ime ili lozinka.");

        return Ok("Uspešna prijava.");
    }

    [HttpPost("registracija")]
    public ActionResult Registracija([FromBody] RegistracijaDTO dto)
    {
        if (_repozitorijum.PostojiKorisnickoIme(dto.KorisnickoIme))
            return BadRequest("Korisničko ime već postoji.");

        if (_repozitorijum.PostojiEmail(dto.Email))
            return BadRequest("Email adresa već postoji.");

        var salt = FunkcijeLozinke.GenerisiSalt();
        var korisnik = new Korisnik
        {
            KorisnickoIme = dto.KorisnickoIme,
            Email = dto.Email,
            Salt = salt,
            LozinkaHes = FunkcijeLozinke.HesirajLozinku(dto.Lozinka, salt)
        };

        _repozitorijum.Dodaj(korisnik);
        return Ok("Uspešna registracija.");
    }
}
