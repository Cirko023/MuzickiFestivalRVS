using Microsoft.AspNetCore.Mvc;
using PrezentacioniSloj.PrezentacionaLogika.ViewModeli;
using SlojServisa.KlaseDTO;
using System.Text;
using System.Text.Json;

namespace PrezentacioniSloj.PrezentacionaLogika.Kontroleri;

public class NalogController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public NalogController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient KreirajKlijenta() =>
        _httpClientFactory.CreateClient("FestivalApi");

    [HttpGet]
    public IActionResult Prijava() => View();

    [HttpPost]
    public async Task<IActionResult> Prijava(PrijavaViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var dto = new PrijavaDTO { KorisnickoIme = model.KorisnickoIme, Lozinka = model.Lozinka };
        var klijent = KreirajKlijenta();
        var json = JsonSerializer.Serialize(dto);
        var sadrzaj = new StringContent(json, Encoding.UTF8, "application/json");
        var odgovor = await klijent.PostAsync("api/KorisnikRest/prijava", sadrzaj);

        if (!odgovor.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Pogrešno korisničko ime ili lozinka.");
            return View(model);
        }

        HttpContext.Session.SetString("KorisnickoIme", model.KorisnickoIme);
        return RedirectToAction("Spisak", "Zapisnik");
    }

    [HttpGet]
    public IActionResult Registracija() => View();

    [HttpPost]
    public async Task<IActionResult> Registracija(RegistracijaViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var dto = new RegistracijaDTO
        {
            KorisnickoIme = model.KorisnickoIme,
            Email = model.Email,
            Lozinka = model.Lozinka
        };

        var klijent = KreirajKlijenta();
        var json = JsonSerializer.Serialize(dto);
        var sadrzaj = new StringContent(json, Encoding.UTF8, "application/json");
        var odgovor = await klijent.PostAsync("api/KorisnikRest/registracija", sadrzaj);

        if (!odgovor.IsSuccessStatusCode)
        {
            var greska = await odgovor.Content.ReadAsStringAsync();
            ModelState.AddModelError("", greska);
            return View(model);
        }

        HttpContext.Session.SetString("KorisnickoIme", model.KorisnickoIme);
        return RedirectToAction("Spisak", "Zapisnik");
    }

    public IActionResult Odjava()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Prijava");
    }
}
