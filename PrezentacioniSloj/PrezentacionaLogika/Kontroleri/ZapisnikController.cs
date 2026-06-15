using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrezentacioniSloj.PrezentacionaLogika.ViewModeli;
using SlojServisa.KlaseDTO;
using System.Text;
using System.Text.Json;

namespace PrezentacioniSloj.PrezentacionaLogika.Kontroleri;

public class ZapisnikController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ZapisnikController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient KreirajKlijenta() =>
        _httpClientFactory.CreateClient("FestivalApi");

    private async Task<List<IzvodjacDTO>> DohvatiIzvodjace()
    {
        var klijent = KreirajKlijenta();
        var odgovor = await klijent.GetAsync("api/IzvodjacRest");
        if (!odgovor.IsSuccessStatusCode) return new List<IzvodjacDTO>();
        var json = await odgovor.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<IzvodjacDTO>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<IzvodjacDTO>();
    }

    private List<SelectListItem> KreirajDropdown(List<IzvodjacDTO> izvodjaci, int? izabraniId = null)
    {
        return izvodjaci.Select(i => new SelectListItem
        {
            Value = i.IzvodjacID.ToString(),
            Text = i.NazivBenda,
            Selected = i.IzvodjacID == izabraniId
        }).ToList();
    }

    private ZapisnikKoncertaDTO MapirajUDto(ZapisnikViewModel model) => new()
    {
        ZapisnikID = model.ZapisnikID,
        NazivFestivala = model.NazivFestivala,
        Lokacija = model.Lokacija,
        DatumKoncerta = model.DatumKoncerta,
        Bina = model.Bina,
        IzvodjacID = model.IzvodjacID,
        VremePocetka = model.VremePocetka,
        VremeZavrsetka = model.VremeZavrsetka,
        NaVreme = model.NaVreme,
        RazlogKasnjenja = model.RazlogKasnjenja,
        TrajanjeNastupaMinuta = model.TrajanjeNastupaMinuta,
        BrojPesama = model.BrojPesama,
        SpecijalniEfekti = model.SpecijalniEfekti,
        Napomena = model.Napomena,
        OzvucenjeIspravno = model.OzvucenjeIspravno,
        RasvetaIspravna = model.RasvetaIspravna,
        TehnickiProblemi = model.TehnickiProblemi,
        OdgovornoLice = model.OdgovornoLice,
        Potpis = model.Potpis,
        Stavke = model.Stavke.Select(s => new StavkaPesmeDTO
        {
            StavkaID = s.StavkaID,
            RedniBroj = s.RedniBroj,
            NazivPesme = s.NazivPesme,
            TrajanjeMinute = s.TrajanjeMinute
        }).ToList()
    };

    public async Task<IActionResult> Spisak(ZapisnikViewModel filter)
    {
        if (HttpContext.Session.GetString("KorisnickoIme") == null)
            return RedirectToAction("Prijava", "Nalog");

        var klijent = KreirajKlijenta();

        var statOdgovor = await klijent.GetAsync("api/ZapisnikRest/statistika");
        if (statOdgovor.IsSuccessStatusCode)
        {
            var statJson = await statOdgovor.Content.ReadAsStringAsync();
            ViewBag.UkupnoZapisnika = int.Parse(statJson);
        }

        var url = "api/ZapisnikRest/filter?";
        if (filter.DatumOd.HasValue)
            url += $"datumOd={filter.DatumOd:yyyy-MM-dd}&";
        if (filter.DatumDo.HasValue)
            url += $"datumDo={filter.DatumDo:yyyy-MM-dd}&";
        if (filter.FilterIzvodjacId.HasValue)
            url += $"izvodjacId={filter.FilterIzvodjacId}";

        var odgovor = await klijent.GetAsync(url);
        var json = await odgovor.Content.ReadAsStringAsync();
        var zapisnici = JsonSerializer.Deserialize<List<ZapisnikKoncertaDTO>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ZapisnikKoncertaDTO>();

        filter.Izvodjaci = KreirajDropdown(await DohvatiIzvodjace());
        ViewBag.Zapisnici = zapisnici;
        return View(filter);
    }

    public async Task<IActionResult> Unos()
    {
        if (HttpContext.Session.GetString("KorisnickoIme") == null)
            return RedirectToAction("Prijava", "Nalog");

        var klijent = KreirajKlijenta();
        var brojOdgovor = await klijent.GetAsync("api/IzvodjacRest/broj");
        if (brojOdgovor.IsSuccessStatusCode)
        {
            var brojJson = await brojOdgovor.Content.ReadAsStringAsync();
            ViewBag.BrojIzvodjaca = int.Parse(brojJson);
        }

        var izvodjaci = await DohvatiIzvodjace();
        var model = new ZapisnikViewModel
        {
            DatumKoncerta = DateTime.Today,
            Izvodjaci = KreirajDropdown(izvodjaci),
            Stavke = new List<StavkaPesmeViewModel>()
        };

        var naziviOdgovor = await klijent.GetAsync("api/IzvodjacRest/nazivi");
        if (naziviOdgovor.IsSuccessStatusCode)
        {
            var naziviJson = await naziviOdgovor.Content.ReadAsStringAsync();
            ViewBag.NaziviBendova = JsonSerializer.Deserialize<List<string>>(naziviJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Unos(ZapisnikViewModel model)
    {
        if (HttpContext.Session.GetString("KorisnickoIme") == null)
            return RedirectToAction("Prijava", "Nalog");

        if (!ModelState.IsValid)
        {
            model.Izvodjaci = KreirajDropdown(await DohvatiIzvodjace());
            return View(model);
        }

        var dto = MapirajUDto(model);
        var klijent = KreirajKlijenta();
        var json = JsonSerializer.Serialize(dto);
        var sadrzaj = new StringContent(json, Encoding.UTF8, "application/json");
        var odgovor = await klijent.PostAsync("api/ZapisnikRest", sadrzaj);

        if (!odgovor.IsSuccessStatusCode)
        {
            var greska = await odgovor.Content.ReadAsStringAsync();
            ModelState.AddModelError("", greska);
            model.Izvodjaci = KreirajDropdown(await DohvatiIzvodjace());
            return View(model);
        }

        return RedirectToAction("Spisak");
    }

    public async Task<IActionResult> Detalji(int id)
    {
        if (HttpContext.Session.GetString("KorisnickoIme") == null)
            return RedirectToAction("Prijava", "Nalog");

        var klijent = KreirajKlijenta();
        var odgovor = await klijent.GetAsync($"api/ZapisnikRest/{id}");
        if (!odgovor.IsSuccessStatusCode)
            return NotFound();

        var json = await odgovor.Content.ReadAsStringAsync();
        var zapisnik = JsonSerializer.Deserialize<ZapisnikKoncertaDTO>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(zapisnik);
    }

    public async Task<IActionResult> Izmena(int id)
    {
        if (HttpContext.Session.GetString("KorisnickoIme") == null)
            return RedirectToAction("Prijava", "Nalog");

        var klijent = KreirajKlijenta();
        var odgovor = await klijent.GetAsync($"api/ZapisnikRest/{id}");
        if (!odgovor.IsSuccessStatusCode)
            return NotFound();

        var json = await odgovor.Content.ReadAsStringAsync();
        var dto = JsonSerializer.Deserialize<ZapisnikKoncertaDTO>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        var model = new ZapisnikViewModel
        {
            ZapisnikID = dto.ZapisnikID,
            NazivFestivala = dto.NazivFestivala,
            Lokacija = dto.Lokacija,
            DatumKoncerta = dto.DatumKoncerta,
            Bina = dto.Bina,
            IzvodjacID = dto.IzvodjacID,
            VremePocetka = dto.VremePocetka,
            VremeZavrsetka = dto.VremeZavrsetka,
            NaVreme = dto.NaVreme,
            RazlogKasnjenja = dto.RazlogKasnjenja,
            TrajanjeNastupaMinuta = dto.TrajanjeNastupaMinuta,
            BrojPesama = dto.BrojPesama,
            SpecijalniEfekti = dto.SpecijalniEfekti,
            Napomena = dto.Napomena,
            OzvucenjeIspravno = dto.OzvucenjeIspravno,
            RasvetaIspravna = dto.RasvetaIspravna,
            TehnickiProblemi = dto.TehnickiProblemi,
            OdgovornoLice = dto.OdgovornoLice,
            Potpis = dto.Potpis,
            Izvodjaci = KreirajDropdown(await DohvatiIzvodjace(), dto.IzvodjacID),
            Stavke = dto.Stavke.Select(s => new StavkaPesmeViewModel
            {
                StavkaID = s.StavkaID,
                RedniBroj = s.RedniBroj,
                NazivPesme = s.NazivPesme,
                TrajanjeMinute = s.TrajanjeMinute
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Izmena(int id, ZapisnikViewModel model)
    {
        if (HttpContext.Session.GetString("KorisnickoIme") == null)
            return RedirectToAction("Prijava", "Nalog");

        if (!ModelState.IsValid)
        {
            model.Izvodjaci = KreirajDropdown(await DohvatiIzvodjace());
            return View(model);
        }

        var dto = MapirajUDto(model);
        dto.ZapisnikID = id;

        var klijent = KreirajKlijenta();
        var json = JsonSerializer.Serialize(dto);
        var sadrzaj = new StringContent(json, Encoding.UTF8, "application/json");
        var odgovor = await klijent.PutAsync($"api/ZapisnikRest/{id}", sadrzaj);

        if (!odgovor.IsSuccessStatusCode)
        {
            var greska = await odgovor.Content.ReadAsStringAsync();
            ModelState.AddModelError("", greska);
            model.Izvodjaci = KreirajDropdown(await DohvatiIzvodjace());
            return View(model);
        }

        return RedirectToAction("Spisak");
    }

    [HttpPost]
    public async Task<IActionResult> Obrisi(int id)
    {
        if (HttpContext.Session.GetString("KorisnickoIme") == null)
            return RedirectToAction("Prijava", "Nalog");

        var klijent = KreirajKlijenta();
        await klijent.DeleteAsync($"api/ZapisnikRest/{id}");
        return RedirectToAction("Spisak");
    }

    public async Task<IActionResult> Stampa(int? id, ZapisnikViewModel filter)
    {
        if (HttpContext.Session.GetString("KorisnickoIme") == null)
            return RedirectToAction("Prijava", "Nalog");

        var klijent = KreirajKlijenta();

        if (id.HasValue)
        {
            var odgovor = await klijent.GetAsync($"api/ZapisnikRest/{id}");
            var json = await odgovor.Content.ReadAsStringAsync();
            var zapisnik = JsonSerializer.Deserialize<ZapisnikKoncertaDTO>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            ViewBag.JedanZapisnik = true;
            return View(new List<ZapisnikKoncertaDTO> { zapisnik! });
        }

        var url = "api/ZapisnikRest/filter?";
        if (filter.DatumOd.HasValue)
            url += $"datumOd={filter.DatumOd:yyyy-MM-dd}&";
        if (filter.DatumDo.HasValue)
            url += $"datumDo={filter.DatumDo:yyyy-MM-dd}&";
        if (filter.FilterIzvodjacId.HasValue)
            url += $"izvodjacId={filter.FilterIzvodjacId}";

        var filterOdgovor = await klijent.GetAsync(url);
        var filterJson = await filterOdgovor.Content.ReadAsStringAsync();
        var zapisnici = JsonSerializer.Deserialize<List<ZapisnikKoncertaDTO>>(filterJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ZapisnikKoncertaDTO>();

        ViewBag.JedanZapisnik = false;
        return View(zapisnici);
    }
}
