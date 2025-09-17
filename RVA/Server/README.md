# TravelSystem Server - Dokumentacija

## Pregled

TravelSystem Server je WCF (Windows Communication Foundation) servisna aplikacija koja omogućava upravljanje turističkim aranžmanima, destinacijama i putnicima. Aplikacija podržava tri formata za čuvanje podataka: XML, JSON i CSV.

## Funkcionalnosti

### 1. Podrška za Multiple Data Formata

- **XML** - Strukturirano čuvanje podataka u XML format
- **JSON** - Moderni JSON format za čuvanje podataka
- **CSV** - Jednostavan CSV format za čuvanje tabularnih podataka

### 2. WCF Servis

- Implementiran je kompletni WCF servis preko Named Pipes komunikacije
- Endpoint: `net.pipe://localhost/TravelService`
- Podržane su sve CRUD operacije za:
  - Travel Arrangements (Turistički aranžmani)
  - Destinations (Destinacije)
  - Passengers (Putnici)

## Pokretanje Serverske Aplikacije

1. **Kompajliranje:**

   ```
   dotnet build Server/Server.csproj
   ```

2. **Pokretanje:**

   ```
   cd Server/bin/Debug
   Server.exe
   ```

3. **Izbor Formata:**
   Aplikacija će zatražiti od korisnika da izabere format čuvanja podataka:

   ```
   === TravelSystem Server ===

   Choose data storage format:
   1. XML
   2. JSON
   3. CSV
   Enter your choice (1-3):
   ```

4. **Pokretanje Servisa:**
   Nakon izbora formata, servis će se pokrenuti i prikazaće informacije:

   ```
   ✓ TravelSystem Service is running successfully!
     Endpoint: net.pipe://localhost/TravelService
     Data format: XML
     Data directory: C:\...\Server\bin\Debug\Data\

   Press any key to stop the service...
   ```

## Struktura Podataka

### Travel Arrangements (Turistički aranžmani)

- ID, CreatedAt, UpdatedAt
- AssociatedTransportation (način transporta)
- NumberOfDays (broj dana)
- Destination (destinacija)
- Passengers (lista putnika)
- State (stanje aranžmana: Reserved, Paid, InProgress, Completed)

### Destinations (Destinacije)

- ID, CreatedAt, UpdatedAt
- TownName (ime grada)
- CountryName (ime zemlje)
- StayPriceByDay (cena boravka po danu)

### Passengers (Putnici)

- ID, CreatedAt, UpdatedAt
- FirstName, LastName (ime i prezime)
- IdentificationNumber (broj lične karte)
- Age (uzrast)

## Lokacija Podataka

Podaci se čuvaju u `Data` direktorijumu u istom folderu gde se nalazi `Server.exe`:

- `arrangements.xml/json/csv` - turistički aranžmani
- `destinations.xml/json/csv` - destinacije
- `passengers.xml/json/csv` - putnici

## Design Patterns Implementirani

1. **Factory Pattern** - `FileHandlerFactory` za kreiranje odgovarajućeg data access objekta
2. **Data Access Layer** - `DataManager` za upravljanje podataka
3. **Repository Pattern** - Enkapsulacija data access logike
4. **Service Layer** - WCF servis kao interfejs za klijentske aplikacije

## WCF Servis Operacije

### Travel Arrangements

- `GetAllArrangements()` - vraća sve aranžmane
- `GetArrangementById(string id)` - vraća aranžman po ID-u
- `AddArrangement(TravelArrangementDto)` - dodaje novi aranžman
- `UpdateArrangement(TravelArrangementDto)` - ažurira postojeći aranžman
- `DeleteArrangement(string id)` - briše aranžman
- `SearchArrangements(SearchCriteriaDto)` - pretražuje aranžmane
- `ChangeArrangementState(string id)` - menja stanje aranžmana

### Destinations

- `GetAllDestinations()` - vraća sve destinacije
- `GetDestinationById(string id)` - vraća destinaciju po ID-u
- `AddDestination(DestinationDto)` - dodaje novu destinaciju
- `UpdateDestination(DestinationDto)` - ažurira postojeću destinaciju
- `DeleteDestination(string id)` - briše destinaciju

### Passengers

- `GetAllPassengers()` - vraća sve putnike
- `GetPassengerById(string id)` - vraća putnika po ID-u
- `AddPassenger(PassengerDto)` - dodaje novog putnika
- `UpdatePassenger(PassengerDto)` - ažurira postojećeg putnika
- `DeletePassenger(string id)` - briše putnika

## Konfiguracija

Servis je konfigurisan kroz `App.config` fajl sa:

- Named Pipe binding za lokalne konekcije
- Odgovarajuće endpoint konfiguracije
- Service behaviors za debugging

## Sample Data

Pri prvom pokretanju, aplikacija automatski kreira sample podatke:

- 3 destinacije (Paris, London, Rome)
- 2 putnika (John Doe, Jane Smith)
- 2 aranžmana (povezana sa destinacijama i putnicima)

## Klijentske Aplikacije

Za pristup WCF servisu, klijentske aplikacije treba da koriste:

- Endpoint: `net.pipe://localhost/TravelService`
- Contract: `ITravelManagementService`
- Binding: `NetNamedPipeBinding`

## Primer Korišćenja iz Klijenta

```csharp
var binding = new NetNamedPipeBinding();
var endpoint = new EndpointAddress("net.pipe://localhost/TravelService");
var factory = new ChannelFactory<ITravelManagementService>(binding, endpoint);
var client = factory.CreateChannel();

// Poziv servisa
var response = client.GetAllArrangements();
if (response.IsSuccess)
{
    // Koristi response.Data
}
```

## Napomene

- Servis mora biti pokrenut pre povezivanja klijentskih aplikacija
- Podaci se automatski čuvaju nakon svake izmene
- Format podataka se bira samo pri pokretanju servera
- Sistem validira da se ne brišu destinacije/putnici koji se koriste u aranžmanima
