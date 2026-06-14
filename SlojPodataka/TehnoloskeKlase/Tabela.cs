using System.Data;
using Microsoft.Data.SqlClient;

namespace SlojPodataka.TehnoloskeKlase;

public abstract class Tabela
{
    protected string StringKonekcije => Konekcija.NizKonekcije;

    public DataTable IzvrsiUpit(string sql)
    {
        using var conn = new SqlConnection(StringKonekcije);
        using var da = new SqlDataAdapter(sql, conn);
        var dt = new DataTable();
        da.Fill(dt);
        return dt;
    }
}
