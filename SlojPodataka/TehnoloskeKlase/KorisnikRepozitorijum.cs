using SlojPodataka.KlasePodataka;
using Microsoft.EntityFrameworkCore;

namespace SlojPodataka.TehnoloskeKlase;

public class KorisnikRepozitorijum
{
    private readonly FestivalDbContext _kontekst;

    public KorisnikRepozitorijum(FestivalDbContext kontekst)
    {
        _kontekst = kontekst;
    }

    public Korisnik? DohvatiPoKorisnickomImenu(string korisnickoIme)
    {
        return _kontekst.Korisnici.FirstOrDefault(k => k.KorisnickoIme == korisnickoIme);
    }

    public bool PostojiKorisnickoIme(string korisnickoIme)
    {
        return _kontekst.Korisnici.Any(k => k.KorisnickoIme == korisnickoIme);
    }

    public bool PostojiEmail(string email)
    {
        return _kontekst.Korisnici.Any(k => k.Email == email);
    }

    public void Dodaj(Korisnik korisnik)
    {
        _kontekst.Korisnici.Add(korisnik);
        _kontekst.SaveChanges();
    }
}
