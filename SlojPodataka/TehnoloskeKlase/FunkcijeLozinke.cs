using System.Security.Cryptography;
using System.Text;

namespace SlojPodataka.TehnoloskeKlase;

public static class FunkcijeLozinke
{
    public static string GenerisiSalt()
    {
        var bajtovi = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bajtovi);
    }

    public static string HesirajLozinku(string lozinka, string salt)
    {
        var kombinovano = Encoding.UTF8.GetBytes(lozinka + salt);
        var hes = SHA256.HashData(kombinovano);
        return Convert.ToBase64String(hes);
    }

    public static bool ProveriLozinku(string lozinka, string salt, string hes)
    {
        return HesirajLozinku(lozinka, salt) == hes;
    }
}
