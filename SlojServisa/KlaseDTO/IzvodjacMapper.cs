using SlojPodataka.KlasePodataka;
using SlojServisa.KlaseDTO;

namespace SlojServisa.KlaseMapiranja;

public class IzvodjacMapper
{
    public IzvodjacDTO UDTO(Izvodjac entitet) => new()
    {
        IzvodjacID = entitet.IzvodjacID,
        NazivBenda = entitet.NazivBenda,
        Grad = entitet.Grad,
        ZanrMuzike = entitet.ZanrMuzike,
        Kotizacija = entitet.Kotizacija
    };

    public List<IzvodjacDTO> UListuDTO(List<Izvodjac> entiteti) => entiteti.Select(UDTO).ToList();
}
