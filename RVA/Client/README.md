# Travel Management System - WPF Client Application

## Pregled

Kompletan WPF klijentska aplikacija za upravljanje turističkim aranžmanima implementirana koristeći MVVM pattern i dodatne design pattern-e.

## Funkcionalnosti

### ✅ Implementirane funkcionalnosti

- **MVVM Pattern** - Kompletna implementacija sa BaseViewModel, MainViewModel, AddEditArrangementViewModel
- **Pretraga i filtriranje** - Real-time pretraga putnih aranžmana po destinaciji, putnicima i stanju
- **CRUD operacije** - Dodavanje, uređivanje, brisanje putnih aranžmana sa validacijom
- **Undo/Redo** - Command pattern implementacija sa stack-based undo/redo funkcionalnost
- **State Transition Simulation** - Simulacija promene stanja putnih aranžmana
- **Real-time UI Updates** - Observer pattern kroz INotifyPropertyChanged i ObservableCollection
- **LiveCharts2 Integration** - Real-time pie chart za prikaz distribucije stanja aranžmana
- **Adapter Pattern** - Integracija legacy sistema sa nekompatibilnom strukturom podataka

### 🎯 Ključne komponente

#### ViewModels

- `BaseViewModel` - Bazna klasa sa INotifyPropertyChanged
- `MainViewModel` - Glavni ViewModel sa svim funkcionalnostima
- `AddEditArrangementViewModel` - ViewModel za form sa validacijom

#### Commands

- `RelayCommand` - Generička command implementacija
- `UndoRedoCommand` - Command za undo/redo operacije
- Specifični command-i za travel arrangements

#### Helpers

- `SearchHelper` - Pretraga i filtriranje
- `StateTransitionHelper` - Simulacija promene stanja
- `UndoRedoManager` - Upravljanje undo/redo stack-om

#### Adapter Pattern

- `LegacyTravelData` - Legacy klasa sa nekompatibilnom strukturom
- `LegacyTravelAdapter` - Adapter za konverziju u moderni format

## Instalacija i pokretanje

### Preduslov

- .NET Framework 4.7.2 ili noviji
- Visual Studio 2019+ ili Visual Studio Code
- Windows 10/11

### Pokretanje

1. **Clone repository**

   ```bash
   git clone [repository-url]
   cd RVA_Projekat/RVA
   ```

2. **Restore packages**

   ```bash
   dotnet restore
   ```

3. **Build aplikacije**

   ```bash
   cd Client
   dotnet build TravelSystem.Client.csproj
   ```

4. **Pokretanje WPF aplikacije**

   ```bash
   dotnet run --project TravelSystem.Client.csproj
   ```

   **Ili direktno:**

   ```bash
   ./bin/Debug/net472/TravelSystem.Client.exe
   ```

## Korišćenje aplikacije

### Glavne funkcionalnosti

1. **Pretraga aranžmana**

   - Koristite search box za pretragu po destinaciji ili putniku
   - Koristite dropdown za filtriranje po stanju

2. **CRUD operacije**

   - "Add New" - dodavanje novog aranžmana
   - "Edit" - uređivanje selektovanog aranžmana
   - "Delete" - brisanje selektovanog aranžmana
   - "Refresh" - osvežavanje podataka

3. **Undo/Redo**

   - "Undo" dugme - poništava poslednju operaciju
   - "Redo" dugme - ponavlja poništenu operaciju

4. **State Simulation**

   - "Simulate State" - simulira promenu stanja selektovanog aranžmana

5. **Adapter Pattern Demo**
   - "Import Legacy" - demonstrira Adapter pattern integraciju legacy podataka

### Real-time Chart

- Pie chart u donjem desnom uglu prikazuje distribuciju aranžmana po stanju
- Automatski se ažurira kada se podaci promene

## Arhitektura

### MVVM Pattern

```
View (MainWindow.xaml)
  ↓ Data Binding
ViewModel (MainViewModel.cs)
  ↓ Service Calls
Model (TravelServiceClient.cs)
```

### Design Patterns korišćeni

1. **MVVM** - Separation of concerns
2. **Command Pattern** - Action encapsulation i undo/redo
3. **Observer Pattern** - Real-time UI updates
4. **Adapter Pattern** - Legacy system integration
5. **Repository Pattern** - Data access abstraction

## Validacija

AddEditArrangementViewModel implementira `IDataErrorInfo` za validaciju:

- Required fields
- Price validation (mora biti pozitivna)
- Date validation (return date mora biti nakon departure date)
- Passenger count validation

## Dependencies

- **LiveCharts2** - Real-time charts
- **SkiaSharp** - Chart rendering
- **.NET Framework 4.7.2** - Target framework
- **WPF** - UI framework

## Struktura projekta

```
Client/
├── Views/
│   ├── MainWindow.xaml              # Glavni prozor
│   └── AddEditArrangementWindow.xaml # Form za dodavanje/uređivanje
├── ViewModels/
│   ├── BaseViewModel.cs             # Bazna klasa
│   ├── MainViewModel.cs             # Glavni ViewModel
│   └── AddEditArrangementViewModel.cs # Form ViewModel
├── Commands/
│   ├── RelayCommand.cs              # Generička komanda
│   └── UndoRedoCommand.cs           # Undo/Redo komanda
├── Helpers/
│   ├── SearchHelper.cs              # Pretraga
│   ├── StateTransitionHelper.cs     # State simulacija
│   └── UndoRedoManager.cs           # Undo/Redo manager
├── Adapters/
│   └── LegacyTravelAdapter.cs       # Adapter pattern
├── Legacy/
│   └── LegacyTravelData.cs          # Legacy klasa
└── Services/
    └── TravelServiceClient.cs       # Service client
```

## Demo Scenario

1. **Pokretanje aplikacije** - Aplicația se učitava sa sample podacima
2. **Pretraga** - Unesite "Paris" u search box
3. **Dodavanje** - Kliknite "Add New" i unesite novi aranžman
4. **Undo** - Kliknite "Undo" da poništite operaciju
5. **Adapter Demo** - Kliknite "Import Legacy" da vidite Adapter pattern u akciji
6. **State Simulation** - Selektuje aranžman i kliknite "Simulate State"
7. **Chart Updates** - Posmatrajte kako se chart ažurira u real-time

## Troubleshooting

### Česti problemi

1. **Build errors** - Proverite da imate .NET Framework 4.7.2
2. **Missing references** - Pokrenite `dotnet restore`
3. **WPF not found** - Proverite Windows verziju i .NET Framework

### Log files

Aplikacija beleži greške u Output konzoli Visual Studio-a ili dotnet CLI.

---

**Napomena**: Ova aplikacija demonstrira implementaciju MVVM pattern-a sa dodatnim design pattern-ima u WPF okruženju koristeći .NET Framework 4.7.2.
