# Design Patterns Implementation

Ovaj projekat implementira 6 ključnih design pattern-a u Patterns folderu.

## 📁 Struktura Patterns Foldera

```
Patterns/
├── Command/
│   ├── ICommand.cs           - Command interfejs
│   ├── CommandBase.cs        - Bazna implementacija za komande
│   ├── CommandManager.cs     - Menadžer za undo/redo funkcionalnost
│   └── CompositeCommand.cs   - Kompozitne komande
├── Factory/
│   ├── FileHandlerFactory.cs     - Factory za file handler-e
│   ├── RepositoryFactory.cs      - Abstract Repository factory
│   └── TravelRepositoryFactory.cs - Konkretni Travel Repository factory
├── Observer/
│   ├── IObserver.cs         - Observer interfejs
│   └── ISubject.cs          - Subject interfejs
├── State/
│   ├── State.cs             - Bazna State klasa
│   ├── IStateContext.cs     - Interfejs za State context
│   └── StateManager.cs      - Menadžer za state tranzicije
├── Adapter/
│   ├── IAdapter.cs          - Adapter interfejs
│   ├── AdapterBase.cs       - Bazna Adapter implementacija
│   └── AdapterRegistry.cs   - Registry za adapter-e
└── Repository/
    ├── IGenericRepository.cs     - Generički Repository interfejs
    ├── GenericRepositoryBase.cs  - Bazna Repository implementacija
    └── IUnitOfWork.cs           - Unit of Work interfejs
└── PatternsDemo.cs              - Demonstracija svih pattern-a
```

## 🎯 Implementirani Pattern-i

### 1. **Command Pattern** 🎮

- **ICommand** - Osnovni interfejs za sve komande
- **CommandBase** - Apstraktna bazna klasa sa osnovnom funkcionalnosti
- **CommandManager** - Upravljanje izvršavanjem, undo i redo operacijama
- **CompositeCommand** - Grupišanje više komandi u jednu

**Funkcionalnosti:**

- ✅ Execute/Undo operacije
- ✅ Command history
- ✅ Undo/Redo funkcionalnost
- ✅ Composite commands
- ✅ Command description i timestamps

### 2. **Factory Pattern** 🏭

- **FileHandlerFactory** - Kreiranje različitih file handler-a
- **RepositoryFactory** - Kreiranje repository instanci
- **RepositoryFactoryRegistry** - Registry za factory-je

**Funkcionalnosti:**

- ✅ Apstraktno kreiranje objekata
- ✅ Registry pattern za factory management
- ✅ Generička podrška za različite tipove

### 3. **Observer Pattern** 👁️

- **IObserver** - Interfejs za observer-e
- **ISubject** - Interfejs za subject-e

**Funkcionalnosti:**

- ✅ Publish-Subscribe mehanizam
- ✅ Loose coupling između subject-a i observer-a
- ✅ Event-driven arhitektura

### 4. **State Pattern** 🔄

- **State** - Apstraktna bazna klasa za stanja
- **IStateContext** - Interfejs za context koji koristi state
- **StateManager** - Centralizovano upravljanje state tranzicijama

**Funkcionalnosti:**

- ✅ Enkapsulacija state-specific ponašanja
- ✅ State transition management
- ✅ Context-based state handling

### 5. **Adapter Pattern** 🔌

- **IAdapter<TSource, TTarget>** - Generički adapter interfejs
- **AdapterBase<TSource, TTarget>** - Bazna implementacija sa validacijom
- **AdapterRegistry** - Registry za upravljanje adapter-ima

**Funkcionalnosti:**

- ✅ Generička podrška za bilo koje tipove
- ✅ Bidirekcijska adaptacija (source ↔ target)
- ✅ Registry za centralizovano upravljanje
- ✅ Type-safe adapter management

### 6. **Repository Pattern** 📚

- **IGenericRepository<T>** - Generički repository interfejs
- **GenericRepositoryBase<T>** - Bazna implementacija sa osnovnim CRUD operacijama
- **IUnitOfWork** - Unit of Work za transaction management

**Funkcionalnosti:**

- ✅ CRUD operacije (Create, Read, Update, Delete)
- ✅ LINQ podrška za querije
- ✅ Generic repository implementation
- ✅ Unit of Work pattern za transakcije
- ✅ Entity change tracking

## 🔗 Veze sa Tim11Travel Implementacijom

Patterns folder sadrži generičke implementacije pattern-a, dok Tim11Travel folder sadrži domain-specific implementacije:

- **Command**: `Tim11Travel/CommandManager.cs` koristi `Patterns/Command/ICommand.cs`
- **Factory**: `Tim11Travel/RepositoryFactory.cs` implementira Factory pattern
- **Observer**: `Tim11Travel/TravelArrangementSubject.cs` implementira Observer pattern
- **State**: `Tim11Travel/TravelArrangementState.cs` naslеđuje State pattern
- **Adapter**: `Tim11Travel/AgencyArrangementAdapter.cs` implementira Adapter pattern
- **Repository**: `Tim11Travel/ITravelArrangementRepository.cs` naslеđuje Repository pattern

## 📋 Checklist Implementiranih Pattern-a

- ✅ **Command Pattern** - Potpuno implementiran
- ✅ **Factory Pattern** - Potpuno implementiran
- ✅ **Observer Pattern** - Potpuno implementiran
- ✅ **State Pattern** - Potpuno implementiran
- ✅ **Adapter Pattern** - Potpuno implementiran
- ✅ **Repository Pattern** - Potpuno implementiran

**Status: Svih 6 pattern-a je uspešno implementirano! 🎉**
