using SlojPodataka.KlasePodataka;
using SlojServisa.KlaseDTO;

namespace SlojServisa.KlaseMapiranja;

public class ZapisnikMapper
{
    public ZapisnikKoncertaDTO UDTO(ZapisnikKoncerta entitet)
    {
        return new ZapisnikKoncertaDTO
        {
            ZapisnikID = entitet.ZapisnikID,
            NazivFestivala = entitet.NazivFestivala,
            Lokacija = entitet.Lokacija,
            DatumKoncerta = entitet.DatumKoncerta,
            Bina = entitet.Bina,
            IzvodjacID = entitet.IzvodjacID,
            NazivBenda = entitet.Izvodjac?.NazivBenda ?? string.Empty,
            VremePocetka = entitet.VremePocetka.ToString(@"hh\:mm"),
            VremeZavrsetka = entitet.VremeZavrsetka.ToString(@"hh\:mm"),
            NaVreme = entitet.NaVreme,
            RazlogKasnjenja = entitet.RazlogKasnjenja,
            TrajanjeNastupaMinuta = entitet.TrajanjeNastupaMinuta,
            BrojPesama = entitet.BrojPesama,
            SpecijalniEfekti = entitet.SpecijalniEfekti,
            Napomena = entitet.Napomena,
            OzvucenjeIspravno = entitet.OzvucenjeIspravno,
            RasvetaIspravna = entitet.RasvetaIspravna,
            TehnickiProblemi = entitet.TehnickiProblemi,
            OdgovornoLice = entitet.OdgovornoLice,
            Potpis = entitet.Potpis,
            OsnovnaKotizacija = entitet.OsnovnaKotizacija,
            PopustProcenat = entitet.PopustProcenat,
            KotizacijaSaPopustom = entitet.KotizacijaSaPopustom,
            Stavke = entitet.Stavke.OrderBy(s => s.RedniBroj).Select(s => new StavkaPesmeDTO
            {
                StavkaID = s.StavkaID,
                RedniBroj = s.RedniBroj,
                NazivPesme = s.NazivPesme,
                TrajanjeMinute = s.TrajanjeMinute
            }).ToList()
        };
    }

    public List<ZapisnikKoncertaDTO> UListuDTO(List<ZapisnikKoncerta> entiteti)
        => entiteti.Select(UDTO).ToList();

    public ZapisnikKoncerta UEntitet(ZapisnikKoncertaDTO dto)
    {
        return new ZapisnikKoncerta
        {
            ZapisnikID = dto.ZapisnikID,
            NazivFestivala = dto.NazivFestivala,
            Lokacija = dto.Lokacija,
            DatumKoncerta = dto.DatumKoncerta,
            Bina = dto.Bina,
            IzvodjacID = dto.IzvodjacID,
            VremePocetka = TimeSpan.Parse(dto.VremePocetka),
            VremeZavrsetka = TimeSpan.Parse(dto.VremeZavrsetka),
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
            OsnovnaKotizacija = dto.OsnovnaKotizacija,
            PopustProcenat = dto.PopustProcenat,
            KotizacijaSaPopustom = dto.KotizacijaSaPopustom,
            Stavke = dto.Stavke.Select(s => new StavkaIzvedenePesme
            {
                StavkaID = s.StavkaID,
                RedniBroj = s.RedniBroj,
                NazivPesme = s.NazivPesme,
                TrajanjeMinute = s.TrajanjeMinute
            }).ToList()
        };
    }
}
