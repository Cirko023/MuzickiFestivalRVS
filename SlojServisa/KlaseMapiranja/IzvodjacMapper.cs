using SlojPodataka.KlasePodataka;
using SlojServisa.KlaseDTO;

namespace SlojServisa.KlaseMapiranja;

public class IzvodjacMapper
{
    public IzvodjacDTO UObjekatZaPrenos(Izvodjac entitet) => new()
    {
        IzvodjacID = entitet.IzvodjacID,
        NazivBenda = entitet.NazivBenda,
        Grad = entitet.Grad,
        ZanrMuzike = entitet.ZanrMuzike,
        Kotizacija = entitet.Kotizacija
    };

    public List<IzvodjacDTO> UListuObjekataZaPrenos(List<Izvodjac> entiteti) => entiteti.Select(UObjekatZaPrenos).ToList();
}
