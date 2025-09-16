# Travel Management System - Design Patterns Implementation

## Implementirani design pattern-i u WPF klijentskoj aplikaciji

### 1. MVVM (Model-View-ViewModel) Pattern

**Lokacija**: `Client/ViewModels/`

- **BaseViewModel.cs** - Bazna klasa za sve ViewModels sa INotifyPropertyChanged implementacijom
- **MainViewModel.cs** - Glavni ViewModel koji upravlja svim podacima i komandama
- **AddEditArrangementViewModel.cs** - ViewModel za dodavanje/uređivanje putnih aranžmana

**Funkcionalnost**:

- Potpuna separacija UI logike od prezentacione logike
- Two-way data binding između View i ViewModel
- Property change notifications za real-time UI updates
- IDataErrorInfo implementacija za validaciju

### 2. Command Pattern

**Lokacija**: `Client/Commands/`

- **RelayCommand.cs** - Implementacija ICommand interface-a
- **UndoRedoCommand.cs** - Command za undo/redo operacije
- **AddTravelArrangementCommand.cs** - Specifičan command za dodavanje putnih aranžmana
- **DeleteTravelArrangementCommand.cs** - Specifičan command za brisanje putnih aranžmana

**Funkcionalnost**:

- Enkapsulacija operacija u komande
- Undo/Redo funkcionalnost sa UndoRedoManager
- Execute i CanExecute logika
- Command binding u XAML-u

### 3. Observer Pattern

**Lokacija**: Implementiran kroz .NET's INotifyPropertyChanged

- **BaseViewModel** implementira INotifyPropertyChanged
- **ObservableCollection<T>** za automatske UI updates
- Event-driven komunikacija između komponenti

**Funkcionalnost**:

- Automatsko ažuriranje UI kada se podaci promene
- Loose coupling između View i ViewModel
- Real-time chart updates kada se stanje putnih aranžmana promeni

### 4. Adapter Pattern ✨

**Lokacija**: `Client/Adapters/` i `Client/Legacy/`

- **LegacyTravelData.cs** - Simulirana legacy klasa sa nekompatibilnom strukturom
- **LegacyTravelAdapter.cs** - Adapter koji konvertuje legacy format u moderni format

**Funkcionalnost**:

- Integracija legacy sistema sa nekompatibilnom strukturom podataka
- Konverzija između različitih formata datuma, cena i statusâ
- Mapping između legacy property names (destination_name, price_amount) i modernih DTO-a
- Demonstracija kroz "Import Legacy Data" dugme u UI

**Legacy format**:

```csharp
// Legacy svojstva sa drugačijim naming konvencijama
string destination_name;
decimal price_amount;
string departure_timestamp;
string current_status;
```

**Moderni format (TravelArrangementDto)**:

```csharp
// Moderni DTO format
string Id;
double TotalPrice;
DateTime CreatedAt;
EntityState State;
```

### 5. Repository Pattern

**Lokacija**: `Client/Services/`

- **TravelServiceClient.cs** - Service client koji enkapsulira komunikaciju sa serverom
- Interface-based pristup za lako testiranje i mocking

### 6. Helper Pattern

**Lokacija**: `Client/Helpers/`

- **SearchHelper.cs** - Helper za pretragu i filtriranje putnih aranžmana
- **StateTransitionHelper.cs** - Helper za simulaciju promene stanja
- **UndoRedoManager.cs** - Manager za upravljanje undo/redo operacijama

## Dodatne funkcionalnosti

### Real-time Charts (LiveCharts2)

- Pie chart koji prikazuje distribuciju putnih aranžmana po stanju
- Automatsko ažuriranje chart-a kada se podaci promene
- SkiaSharp rendering za performanse

### Validation

- IDataErrorInfo implementacija za property-level validaciju
- Required field validation
- Custom validation rules za cene i datume
- Error display u UI kroz binding

### Search & Filtering

- Real-time pretraga kroz destinacije i putnike
- Filter po stanju putnog aranžmana
- Kombinovanje multiple filtere
- Instant rezultati kroz binding

### Undo/Redo Functionality

- Stack-based implementacija
- Support za multiple operacije
- Visual indicators u UI (dugmad enabled/disabled)
- Command pattern integracija

## Demonstracija Adapter Pattern-a

Za demonstraciju Adapter pattern-a, koristite "Import Legacy Data" dugme u glavnom prozoru:

1. Kliknite na "Import Legacy Data" dugme
2. Adapter će kreirati sample legacy podatke sa starım formatom
3. Podaci će biti automatski konvertovani u moderni format
4. Novi aranžman će se dodati u listu
5. Success dialog će pokazati detalje konverzije

Ovo demonstrira kako Adapter pattern omogućava integraciju starijih sistema sa nekompatibilnim formatima podataka u modernu aplikaciju bez potrebe za promenom postojeće arhitekture.
