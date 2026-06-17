Muzički Festival – Evidencija koncerta
Aplikacija za digitalno vođenje zapisnika o koncertima na muzičkim festivalima. Omogućava unos, izmenu, brisanje, pregled i štampu zapisnika, uz automatski obračun kotizacije prema poslovnom pravilu.

Tehnologije
.NET 9 + C#
ASP.NET Core MVC – prezentacioni sloj
ASP.NET Core Web API – REST servis
Entity Framework Core 9 – ORM
SQL Server – baza podataka
Bootstrap 5 – responzivni dizajn
JavaScript + jQuery Validation – validacije na klijentskoj strani

Arhitektura
Projekat je organizovan kroz četiri sloja:

Sloj	Projekat	Opis
Prezentacioni	PrezentacioniSloj	MVC aplikacija, View modeli, korisnički interfejs
Servis	SlojServisa	REST API (CRUD, filtriranje, statistika)
Poslovna logika	SlojPoslovneLogike	Poslovno pravilo o popustu, validacije
Podaci	SlojPodataka	Repository pattern, EF Core, DBUtils, stored procedure
Pokretanje
1. Kreiraj bazu podataka
Pokreni SQL skriptu iz fajla baza podataka.txt na svom SQL Server-u.

2. Podesi konekcioni string
U appsettings.json (u PrezentacioniSloj i SlojServisa) postavi:

json
"ConnectionStrings": {
  "PodrazumevanaKonekcija": "Server=localhost\\SQLEXPRESS;Database=MuzickiFestivalRVS;Trusted_Connection=True;TrustServerCertificate=True;"
}
3. Pokreni oba projekta
Podesiti Multiple startup projects u Visual Studio:

SlojServisa → Start

PrezentacioniSloj → Start


4. Prijava
Korisničko ime: admin

Lozinka: admin

Ključne funkcionalnosti
Autentifikacija – prijava/registracija sa heširanom lozinkom (SHA256 + salt)

CRUD operacije – unos, izmena, brisanje zapisnika (master-detail sa transakcijama)

Filtriranje – po datumu i izvođaču

Štampa – pojedinačnog zapisnika ili filtrirane liste (printer-friendly)

Poslovno pravilo – automatski popust na kotizaciju ako su ozvučenje i rasveta ispravni (parametar se učitava iz XML-a preko REST servisa)

Validacije – serverske (Data Annotations) i klijentske (JavaScript + regex)

Projekti u rešenju
text
MuzickiFestivalRVS/
├── PrezentacioniSloj/          # MVC aplikacija (UI)
├── SlojServisa/                # REST API
├── SlojPoslovneLogike/         # Poslovna logika
├── SlojPodataka/               # Rad sa podacima
└── start-dev.bat               # Pokretanje oba projekta

Autor
Jovan Ćirić – SI 18/22, Tehnički fakultet "Mihajlo Pupin" Zrenjanin
