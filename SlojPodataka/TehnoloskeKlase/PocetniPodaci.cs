using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using SlojPodataka.KlasePodataka;

namespace SlojPodataka.TehnoloskeKlase;

public static class PocetniPodaci
{
    public static void PopuniSve(FestivalDbContext kontekst, string putanjaXml)
    {

        if (!kontekst.Izvodjaci.Any())
        {
            var xml = XDocument.Load(putanjaXml);
            var izvodjaci = xml.Root!.Elements("Izvodjac").Select(el => new Izvodjac
            {
                NazivBenda = el.Element("NazivBenda")!.Value,
                Grad = el.Element("Grad")!.Value,
                ZanrMuzike = el.Element("ZanrMuzike")!.Value,
                Kotizacija = decimal.Parse(el.Element("Kotizacija")!.Value)
            });
            kontekst.Izvodjaci.AddRange(izvodjaci);
            kontekst.SaveChanges();
        }

        if (!kontekst.Korisnici.Any())
        {
            var salt = FunkcijeLozinke.GenerisiSalt();
            kontekst.Korisnici.Add(new Korisnik
            {
                KorisnickoIme = "admin",
                Email = "admin@festival.rs",
                Salt = salt,
                LozinkaHes = FunkcijeLozinke.HesirajLozinku("admin", salt)
            });
            kontekst.SaveChanges();
        }
    }
}
