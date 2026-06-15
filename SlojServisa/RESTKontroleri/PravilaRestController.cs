using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace SlojServisa.RESTKontroleri;

[ApiController]
[Route("api/[controller]")]
public class PravilaRestController : ControllerBase
{
    [HttpGet]
    public IActionResult DohvatiPravila()
    {
        try
        {
            var putanja = Path.Combine(AppContext.BaseDirectory, "Ogranicenja", "PravilaPopusta.xml");
            var xml = XDocument.Load(putanja);

            return Ok(new
            {
                ProcenatPopusta = decimal.Parse(xml.Root!.Element("ProcenatPopusta")!.Value),
                Opis = xml.Root.Element("Opis")!.Value
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Greška pri čitanju XML-a: {ex.Message}");
        }
    }
}
