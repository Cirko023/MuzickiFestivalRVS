using Microsoft.EntityFrameworkCore;
using SlojPodataka.Repozitorijum;
using SlojPodataka.TehnoloskeKlase;
using SlojPoslovneLogike.Ogranicenja;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<FestivalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PodrazumevanaKonekcija")));

Konekcija.NizKonekcije = builder.Configuration.GetConnectionString("PodrazumevanaKonekcija")!;

builder.Services.AddScoped<ZapisnikKoncertaRepozitorijum>();
builder.Services.AddScoped<KorisnikRepozitorijum>();
builder.Services.AddScoped<IzvodjacRepozitorijum>();
builder.Services.AddScoped<IzvodjacRepoDBUtils>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<CitacPravila>();

var app = builder.Build();


app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
