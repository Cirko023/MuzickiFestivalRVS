using Microsoft.AspNetCore.Mvc;
using SlojPodataka.Repozitorijum;
using SlojPodataka.TehnoloskeKlase;
using SlojPoslovneLogike;
using SlojPoslovneLogike.Ogranicenja;
using SlojServisa.KlaseDTO;
using SlojServisa.KlaseMapiranja;

namespace SlojServisa.RESTKontroleri;

[ApiController]
[Route("api/[controller]")]
public class ZapisnikRestController : ControllerBase
{
    private readonly ZapisnikKoncertaRepozitorijum _repozitorijum;
    private readonly IzvodjacRepozitorijum _izvodjacRepo;
    private readonly ZapisnikMapper _mapper = new();
    private readonly ValidatorPoslovnogPravila _validator;

    public ZapisnikRestController(
        ZapisnikKoncertaRepozitorijum repozitorijum,
        IzvodjacRepozitorijum izvodjacRepo,
        CitacPravila citacPravila)
    {
        _repozitorijum = repozitorijum;
        _izvodjacRepo = izvodjacRepo;
        _validator = new ValidatorPoslovnogPravila(citacPravila);
    }

    [HttpGet]
    public ActionResult<List<ZapisnikKoncertaDTO>> DohvatiSve()
    {
        return Ok(_mapper.UListuObjekataZaPrenos(_repozitorijum.DohvatiSve()));
    }

    [HttpGet("statistika")]
    public ActionResult<int> DohvatiStatistiku()
    {
        try
        {
            return Ok(_repozitorijum.DohvatiUkupanBrojZapisnikaPrekoSP());
        }
        catch
        {
            return Ok(_repozitorijum.DohvatiSve().Count);
        }
    }

    [HttpGet("{id}")]
    public ActionResult<ZapisnikKoncertaDTO> DohvatiPoId(int id)
    {
        var zapisnik = _repozitorijum.DohvatiPoId(id);
        if (zapisnik == null)
            return NotFound($"Zapisnik sa ID-em {id} nije pronađen.");
        return Ok(_mapper.UObjekatZaPrenos(zapisnik));
    }

    [HttpPost]
    public async Task<ActionResult> Dodaj([FromBody] ZapisnikKoncertaDTO dto)
    {
        var postoji = _repozitorijum.DohvatiSve()
            .Any(z => z.IzvodjacID == dto.IzvodjacID && z.DatumKoncerta.Date == dto.DatumKoncerta.Date);
        if (postoji)
            return BadRequest("Već postoji zapisnik za ovog izvođača na isti datum.");

        var stavkeInfo = dto.Stavke.Select(s => (s.RedniBroj, s.TrajanjeMinute)).ToList();
        var (uspesno, poruka) = _validator.ValidirajStavkePesama(stavkeInfo);
        if (!uspesno)
            return BadRequest(poruka);

        var izvodjac = _izvodjacRepo.DohvatiPoId(dto.IzvodjacID);
        if (izvodjac == null)
            return BadRequest("Izvođač nije pronađen.");

        dto.OsnovnaKotizacija = izvodjac.Kotizacija;
        var (popust, kotizacija) = await _validator.IzracunajKotizaciju(
            dto.OzvucenjeIspravno, dto.RasvetaIspravna, dto.OsnovnaKotizacija);
        dto.PopustProcenat = popust;
        dto.KotizacijaSaPopustom = kotizacija;

        var zapisnik = _mapper.UEntitet(dto);
        _repozitorijum.Dodaj(zapisnik);
        return Ok("Zapisnik je uspešno dodat.");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Izmeni(int id, [FromBody] ZapisnikKoncertaDTO dto)
    {
        dto.ZapisnikID = id;

        var stavkeInfo = dto.Stavke.Select(s => (s.RedniBroj, s.TrajanjeMinute)).ToList();
        var (uspesno, poruka) = _validator.ValidirajStavkePesama(stavkeInfo);
        if (!uspesno)
            return BadRequest(poruka);

        var izvodjac = _izvodjacRepo.DohvatiPoId(dto.IzvodjacID);
        if (izvodjac == null)
            return BadRequest("Izvođač nije pronađen.");

        dto.OsnovnaKotizacija = izvodjac.Kotizacija;
        var (popust, kotizacija) = await _validator.IzracunajKotizaciju(
            dto.OzvucenjeIspravno, dto.RasvetaIspravna, dto.OsnovnaKotizacija);
        dto.PopustProcenat = popust;
        dto.KotizacijaSaPopustom = kotizacija;

        var zapisnik = _mapper.UEntitet(dto);
        _repozitorijum.Izmeni(zapisnik);
        return Ok("Zapisnik je uspešno izmenjen.");
    }

    [HttpDelete("{id}")]
    public ActionResult Obrisi(int id)
    {
        _repozitorijum.Obrisi(id);
        return Ok("Zapisnik je uspešno obrisan.");
    }

    [HttpGet("filter")]
    public ActionResult<List<ZapisnikKoncertaDTO>> Filtriraj(
        [FromQuery] DateTime? datumOd,
        [FromQuery] DateTime? datumDo,
        [FromQuery] int? izvodjacId)
    {
        var zapisnici = _repozitorijum.Filtriraj(datumOd, datumDo, izvodjacId);
        return Ok(_mapper.UListuObjekataZaPrenos(zapisnici));
    }
}
