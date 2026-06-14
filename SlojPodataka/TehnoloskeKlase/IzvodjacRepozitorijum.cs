using SlojPodataka.KlasePodataka;
using Microsoft.EntityFrameworkCore;

namespace SlojPodataka.TehnoloskeKlase;

public class IzvodjacRepozitorijum
{
    private readonly FestivalDbContext _kontekst;

    public IzvodjacRepozitorijum(FestivalDbContext kontekst)
    {
        _kontekst = kontekst;
    }

    public List<Izvodjac> DohvatiSve()
    {
        return _kontekst.Izvodjaci.AsNoTracking().OrderBy(i => i.NazivBenda).ToList();
    }

    public Izvodjac? DohvatiPoId(int id)
    {
        return _kontekst.Izvodjaci.AsNoTracking().FirstOrDefault(i => i.IzvodjacID == id);
    }
}
