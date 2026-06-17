using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SlojPodataka.KlasePodataka;
using SlojPodataka.TehnoloskeKlase;

namespace SlojPodataka.Repozitorijum;

public class ZapisnikKoncertaRepozitorijum
{
    private readonly FestivalDbContext _kontekst;

    public ZapisnikKoncertaRepozitorijum(FestivalDbContext kontekst)
    {
        _kontekst = kontekst;
    }

    public List<ZapisnikKoncerta> DohvatiSve()
    {
        return _kontekst.Zapisnici.AsNoTracking()
            .Include(z => z.Izvodjac)
            .Include(z => z.Stavke)
            .OrderByDescending(z => z.DatumKoncerta)
            .ToList();
    }

    public ZapisnikKoncerta? DohvatiPoId(int id)
    {
        return _kontekst.Zapisnici
            .Include(z => z.Izvodjac)
            .Include(z => z.Stavke)
            .FirstOrDefault(z => z.ZapisnikID == id);
    }

    public void Dodaj(ZapisnikKoncerta zapisnik)
    {
        using var transakcija = _kontekst.Database.BeginTransaction();
        try
        {
            _kontekst.Zapisnici.Add(zapisnik);
            _kontekst.SaveChanges();
            transakcija.Commit();
        }
        catch
        {
            transakcija.Rollback();
            throw;
        }
    }

    public void Izmeni(ZapisnikKoncerta zapisnik)
    {
        using var transakcija = _kontekst.Database.BeginTransaction();
        try
        {
            var stareStavke = _kontekst.StavkePesama
                .Where(s => s.ZapisnikID == zapisnik.ZapisnikID)
                .ToList();
            _kontekst.StavkePesama.RemoveRange(stareStavke);
            _kontekst.SaveChanges();

            var postojeci = _kontekst.Zapisnici
                .FirstOrDefault(z => z.ZapisnikID == zapisnik.ZapisnikID)
                ?? throw new Exception("Zapisnik nije pronađen.");

            postojeci.NazivFestivala = zapisnik.NazivFestivala;
            postojeci.Lokacija = zapisnik.Lokacija;
            postojeci.DatumKoncerta = zapisnik.DatumKoncerta;
            postojeci.Bina = zapisnik.Bina;
            postojeci.IzvodjacID = zapisnik.IzvodjacID;
            postojeci.VremePocetka = zapisnik.VremePocetka;
            postojeci.VremeZavrsetka = zapisnik.VremeZavrsetka;
            postojeci.NaVreme = zapisnik.NaVreme;
            postojeci.RazlogKasnjenja = zapisnik.RazlogKasnjenja;
            postojeci.TrajanjeNastupaMinuta = zapisnik.TrajanjeNastupaMinuta;
            postojeci.BrojPesama = zapisnik.BrojPesama;
            postojeci.SpecijalniEfekti = zapisnik.SpecijalniEfekti;
            postojeci.Napomena = zapisnik.Napomena;
            postojeci.OzvucenjeIspravno = zapisnik.OzvucenjeIspravno;
            postojeci.RasvetaIspravna = zapisnik.RasvetaIspravna;
            postojeci.TehnickiProblemi = zapisnik.TehnickiProblemi;
            postojeci.OdgovornoLice = zapisnik.OdgovornoLice;
            postojeci.Potpis = zapisnik.Potpis;
            postojeci.OsnovnaKotizacija = zapisnik.OsnovnaKotizacija;
            postojeci.PopustProcenat = zapisnik.PopustProcenat;
            postojeci.KotizacijaSaPopustom = zapisnik.KotizacijaSaPopustom;

            foreach (var stavka in zapisnik.Stavke)
            {
                stavka.ZapisnikID = postojeci.ZapisnikID;
                stavka.StavkaID = 0;
                _kontekst.StavkePesama.Add(stavka);
            }

            _kontekst.SaveChanges();
            transakcija.Commit();
        }
        catch
        {
            transakcija.Rollback();
            throw;
        }
    }

    public void Obrisi(int id)
    {
        using var transakcija = _kontekst.Database.BeginTransaction();
        try
        {
            var zapisnik = _kontekst.Zapisnici
                .Include(z => z.Stavke)
                .FirstOrDefault(z => z.ZapisnikID == id)
                ?? throw new Exception("Zapisnik nije pronađen.");

            _kontekst.StavkePesama.RemoveRange(zapisnik.Stavke);
            _kontekst.Zapisnici.Remove(zapisnik);
            _kontekst.SaveChanges();
            transakcija.Commit();
        }
        catch
        {
            transakcija.Rollback();
            throw;
        }
    }

    public List<ZapisnikKoncerta> Filtriraj(DateTime? datumOd, DateTime? datumDo, int? izvodjacId)
    {
        var upit = _kontekst.Zapisnici
            .Include(z => z.Izvodjac)
            .Include(z => z.Stavke)
            .AsQueryable();

        if (datumOd.HasValue)
            upit = upit.Where(z => z.DatumKoncerta >= datumOd.Value);

        if (datumDo.HasValue)
            upit = upit.Where(z => z.DatumKoncerta <= datumDo.Value);

        if (izvodjacId.HasValue)
            upit = upit.Where(z => z.IzvodjacID == izvodjacId.Value);

        return upit.OrderByDescending(z => z.DatumKoncerta).ToList();
    }

    public int DohvatiUkupanBrojZapisnikaPrekoSP()
    {
        using var conn = new SqlConnection(_kontekst.Database.GetConnectionString());
        using var cmd = new SqlCommand("sp_DajUkupanBrojZapisnika", conn)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };
        conn.Open();
        return (int)cmd.ExecuteScalar()!;
    }
}
