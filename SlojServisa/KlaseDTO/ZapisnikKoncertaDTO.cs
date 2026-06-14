namespace SlojServisa.KlaseDTO;

public class ZapisnikKoncertaDTO
{
    public int ZapisnikID { get; set; }
    public string NazivFestivala { get; set; } = string.Empty;
    public string Lokacija { get; set; } = string.Empty;
    public DateTime DatumKoncerta { get; set; }
    public string Bina { get; set; } = string.Empty;
    public int IzvodjacID { get; set; }
    public string NazivBenda { get; set; } = string.Empty;
    public string VremePocetka { get; set; } = string.Empty;
    public string VremeZavrsetka { get; set; } = string.Empty;
    public bool NaVreme { get; set; }
    public string? RazlogKasnjenja { get; set; }
    public int TrajanjeNastupaMinuta { get; set; }
    public int BrojPesama { get; set; }
    public bool SpecijalniEfekti { get; set; }
    public string? Napomena { get; set; }
    public bool OzvucenjeIspravno { get; set; }
    public bool RasvetaIspravna { get; set; }
    public string? TehnickiProblemi { get; set; }
    public string OdgovornoLice { get; set; } = string.Empty;
    public string Potpis { get; set; } = string.Empty;
    public decimal OsnovnaKotizacija { get; set; }
    public decimal PopustProcenat { get; set; }
    public decimal KotizacijaSaPopustom { get; set; }
    public List<StavkaPesmeDTO> Stavke { get; set; } = new();
}
