using System.Reflection.Emit;

namespace SlojPodataka.TehnoloskeKlase;

public class IzvodjacRepoDBUtils : Tabela
{
    public int IzbrojIzvodjace()
    {
        var dt = IzvrsiUpit("SELECT COUNT(*) FROM Izvodjac");
        return Convert.ToInt32(dt.Rows[0][0]);
    }

    public List<string> DohvatiNaziveBendova()
    {
        var dt = IzvrsiUpit("SELECT NazivBenda FROM Izvodjac ORDER BY NazivBenda");
        var rezultat = new List<string>();
        foreach (System.Data.DataRow red in dt.Rows)
            rezultat.Add(red[0].ToString()!);
        return rezultat;
    }
}
