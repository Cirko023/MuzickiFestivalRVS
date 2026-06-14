using SlojPoslovneLogike.Ogranicenja;

namespace SlojPoslovneLogike;

public class ValidatorPoslovnogPravila
{
    private readonly CitacPravila _citacPravila;

    public ValidatorPoslovnogPravila(CitacPravila citacPravila)
    {
        _citacPravila = citacPravila;
    }

    public (bool Uspesno, string Poruka) ValidirajStavkePesama(List<(int RedniBroj, int TrajanjeMinute)> stavke)
    {
        if (stavke.Count == 0)
            return (false, "Mora postojati bar jedna izvedena pesma.");

        var sortirane = stavke.OrderBy(s => s.RedniBroj).ToList();
        for (int i = 0; i < sortirane.Count; i++)
        {
            if (sortirane[i].RedniBroj != i + 1)
                return (false, "Redni brojevi pesama moraju biti uzastopni počevši od 1.");
        }

        return (true, "Validacija uspešna.");
    }

    public async Task<(decimal PopustProcenat, decimal KotizacijaSaPopustom)> IzracunajKotizaciju(
        bool ozvucenjeIspravno,
        bool rasvetaIspravna,
        decimal osnovnaKotizacija)
    {
        var procenatPopusta = await _citacPravila.DohvatiProcenatPopusta();

        if (ozvucenjeIspravno && rasvetaIspravna)
        {
            var popust = osnovnaKotizacija * procenatPopusta / 100m;
            return (procenatPopusta, osnovnaKotizacija - popust);
        }

        return (0, osnovnaKotizacija);
    }
}
