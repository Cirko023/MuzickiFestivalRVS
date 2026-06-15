var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddRazorOptions(opcije =>
    {
        opcije.ViewLocationFormats.Clear();
        opcije.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
        opcije.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
    });

builder.Services.AddHttpClient("FestivalApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5072/");
});

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Nalog}/{action=Prijava}/{id?}");

app.Run();
