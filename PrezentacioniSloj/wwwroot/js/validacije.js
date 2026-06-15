document.addEventListener("DOMContentLoaded", function () {
    var formaZapisnik = document.getElementById("forma-zapisnik");
    if (formaZapisnik) {
        formaZapisnik.addEventListener("submit", function (e) {
            var greske = [];

            var regexVreme = /^([01]\d|2[0-3]):[0-5]\d$/;
            var vremePocetak = document.getElementById("vreme-pocetka");
            var vremeZavrsetak = document.getElementById("vreme-zavrsetka");
            if (vremePocetak && !regexVreme.test(vremePocetak.value))
                greske.push("Vreme početka mora biti u formatu HH:MM.");
            if (vremeZavrsetak && !regexVreme.test(vremeZavrsetak.value))
                greske.push("Vreme završetka mora biti u formatu HH:MM.");

            var regexIme = /^[A-ZČĆŽŠĐ][a-zčćžšđ]+(\s[A-ZČĆŽŠĐ][a-zčćžšđ]+)+$/;
            var odgovornoLice = document.getElementById("odgovorno-lice");
            if (odgovornoLice && odgovornoLice.value && !regexIme.test(odgovornoLice.value.trim()))
                greske.push("Odgovorno lice mora biti ime i prezime (npr. Jovan Ćirić).");

            var regexPotpis = /^[A-ZČĆŽŠĐ][a-zčćžšđ]+\s[A-ZČĆŽŠĐ]\.$/;
            var potpis = document.getElementById("potpis");
            if (potpis && potpis.value && !regexPotpis.test(potpis.value.trim()))
                greske.push("Potpis mora biti u formatu 'Ime P.' (npr. Jovan Ć.).");

            var regexPesma = /^[A-ZČĆŽŠĐ0-9][a-zčćžšđ0-9\s\-']{1,148}$/;
            document.querySelectorAll(".naziv-pesme").forEach(function (input) {
                if (input.value && !regexPesma.test(input.value.trim()))
                    greske.push("Naziv pesme '" + input.value + "' nije ispravan.");
            });

            var poruka = document.getElementById("poruka-validacije");
            if (greske.length > 0) {
                e.preventDefault();
                if (poruka) poruka.innerHTML = greske.join("<br/>");
            } else if (poruka) {
                poruka.innerHTML = "";
            }
        });
    }

    var formaRegistracija = document.getElementById("forma-registracija");
    if (formaRegistracija) {
        formaRegistracija.addEventListener("submit", function (e) {
            var regexEmail = /^[a-zA-Z0-9._%+\-]+@[a-zA-Z0-9.\-]+\.[a-zA-Z]{2,}$/;
            var email = document.getElementById("email");
            if (email && !regexEmail.test(email.value)) {
                e.preventDefault();
                alert("Unesite ispravnu email adresu.");
            }

            var regexKorisnickoIme = /^[a-zA-Z0-9_]{3,50}$/;
            var korisnickoIme = document.getElementById("korisnicko-ime");
            if (korisnickoIme && !regexKorisnickoIme.test(korisnickoIme.value)) {
                e.preventDefault();
                alert("Korisničko ime mora imati 3-50 karaktera (slova, brojevi, _).");
            }
        });
    }
});
