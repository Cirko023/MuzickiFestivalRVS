namespace SlojServisa.KlaseDTO;

public class IzvodjacDTO
{
    public int IzvodjacID { get; set; }
    public string NazivBenda { get; set; } = string.Empty;
    public string Grad { get; set; } = string.Empty;
    public string ZanrMuzike { get; set; } = string.Empty;
    public decimal Kotizacija { get; set; }
}
