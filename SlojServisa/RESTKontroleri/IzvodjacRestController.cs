using Microsoft.AspNetCore.Mvc;
using SlojPodataka.KlasePodataka;
using SlojPodataka.Repozitorijum;
using SlojPodataka.TehnoloskeKlase;
using SlojServisa.KlaseDTO;
using SlojServisa.KlaseMapiranja;

namespace SlojServisa.RESTKontroleri;

[ApiController]
[Route("api/[controller]")]
public class IzvodjacRestController : ControllerBase
{
    private readonly IzvodjacRepozitorijum _repozitorijum;
    private readonly IzvodjacRepoDBUtils _dbUtils;
    private readonly IzvodjacMapper _mapper = new();

    public IzvodjacRestController(IzvodjacRepozitorijum repozitorijum, IzvodjacRepoDBUtils dbUtils)
    {
        _repozitorijum = repozitorijum;
        _dbUtils = dbUtils;
    }

    [HttpGet]
    public ActionResult<List<IzvodjacDTO>> DohvatiSve()
    {
        return Ok(_mapper.UListuObjekataZaPrenos(_repozitorijum.DohvatiSve()));
    }

    [HttpGet("{id}")]
    public ActionResult<IzvodjacDTO> DohvatiPoId(int id)
    {
        var izvodjac = _repozitorijum.DohvatiPoId(id);
        if (izvodjac == null)
            return NotFound($"Izvođač sa ID-em {id} nije pronađen.");
        return Ok(_mapper.UObjekatZaPrenos(izvodjac));
    }

    [HttpGet("broj")]
    public ActionResult<int> DohvatiBroj()
    {
        return Ok(_dbUtils.IzbrojIzvodjace());
    }

    [HttpGet("nazivi")]
    public ActionResult<List<string>> DohvatiNazive()
    {
        return Ok(_dbUtils.DohvatiNaziveBendova());
    }
}
