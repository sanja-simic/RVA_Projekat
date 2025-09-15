# Travel Management System

A complete WPF client-server application for managing travel arrangements, built with .NET Framework 4.7.2 and WCF services.

## Features

### Core Functionality

- **Travel Arrangements Management**: Full CRUD operations for travel arrangements
- **Passenger Management**: Add and manage passengers for travel arrangements
- **Destination Management**: Manage travel destinations with pricing
- **State Management**: Travel arrangements can progress through different states (Reserved → Paid → InProgress → Completed)
- **Real-time Updates**: Changes are immediately reflected in the UI

### Technical Features

- **WPF Client**: Rich user interface with data binding and validation
- **WCF Service**: RESTful web service for data operations
- **Background Server**: Server starts automatically when client launches (hidden console)
- **Design Patterns**: Implementation of various design patterns including Repository, Command, Observer, State, and Factory patterns

## Project Structure

```
RVA/
├── Client/                          # WPF Client Application
│   ├── Views/                       # Dialog windows for add/edit operations
│   ├── Services/                    # WCF service client proxy
│   └── MainWindow.xaml/.cs          # Main application window
├── Server/                          # WCF Service Host
│   ├── Services/                    # Service implementations
│   ├── DataAccess/                  # Repository pattern implementation
│   └── Program.cs                   # Service host startup
└── Shared/                          # Shared libraries
    ├── TravelSystem.Models/         # Entity models and DTOs
    └── TravelSystem.Contracts/      # WCF service contracts
```

## How to Run

### Quick Start

1. **Set Client as Startup Project**: Right-click on `TravelSystem.Client` in Solution Explorer and select "Set as Startup Project"
2. **Build Solution**: Build the entire solution (Ctrl+Shift+B)
3. **Run**: Press F5 or click Start - the client will automatically start the server in the background

### Manual Start

If you prefer to run them separately:

1. **Start Server**: Run the Server project first
2. **Start Client**: Run the TravelSystem.Client project

## Usage Guide

### Main Window

- **Data Grid**: Displays all travel arrangements with their details
- **Add Button**: Create new travel arrangements
- **Edit Button**: Modify selected arrangements
- **Delete Button**: Remove arrangements with confirmation
- **Refresh Button**: Reload data from server
- **Connect Button**: Reconnect to server if connection is lost

### Managing Travel Arrangements

1. **Adding**: Click "Add New" → Fill destination details → Select transportation → Add passengers → Save
2. **Editing**: Select arrangement → Click "Edit" → Modify details → Save
3. **State Changes**: Double-click any arrangement to advance its state
4. **Deleting**: Select arrangement → Click "Delete" → Confirm

### Data Validation

- All forms include comprehensive validation
- Required fields are clearly marked
- Numeric fields validate input ranges
- Duplicate passport numbers are prevented

## Architecture

### Design Patterns Used

- **Repository Pattern**: Data access abstraction
- **Command Pattern**: Encapsulation of operations
- **Observer Pattern**: Change notifications
- **State Pattern**: Travel arrangement state management
- **Factory Pattern**: Object creation management
- **Adapter Pattern**: Legacy system integration

### Service Architecture

- **WCF BasicHttpBinding**: Standard HTTP protocol for cross-platform compatibility
- **Data Transfer Objects (DTOs)**: Serializable data contracts
- **Service Response Pattern**: Standardized response format with error handling

### Client Architecture

- **MVVM-like Pattern**: Clear separation of concerns
- **Data Binding**: Automatic UI updates
- **Async Operations**: Non-blocking UI operations
- **Exception Handling**: Comprehensive error management

## Configuration

### Server Configuration

- **Service Endpoint**: http://localhost:8080/TravelService
- **Binding**: BasicHttpBinding for maximum compatibility
- **Instance Mode**: Single instance for shared state

### Client Configuration

- **Startup Delay**: 3-second delay to allow server startup
- **Connection Retry**: Automatic reconnection capabilities
- **Background Server**: Server runs hidden in background

## Development

### Adding New Features

1. **Models**: Add entity classes in `TravelSystem.Models/Entities`
2. **DTOs**: Create data transfer objects in `TravelSystem.Models/DTOs`
3. **Contracts**: Define service contracts in `TravelSystem.Contracts`
4. **Services**: Implement service logic in `Server/Services`
5. **UI**: Create WPF views in `Client/Views`

### Design Pattern Extensions

The codebase is designed to be easily extensible:

- **New Commands**: Implement `ICommand` interface
- **New States**: Extend state machine in entity classes
- **New Repositories**: Implement repository interfaces
- **New Adapters**: Create adapter classes for integration

## Troubleshooting

### Common Issues

1. **Server Connection Failed**: Ensure server is running and port 8080 is available
2. **Server Not Found**: Build the Server project and ensure executable exists
3. **Permission Errors**: Run as administrator if file access issues occur
4. **Port Already in Use**: Close other applications using port 8080

### Debug Mode

- Enable debug mode by building in Debug configuration
- Server logs are displayed in console when run separately
- Client shows detailed error messages in status bar

## System Requirements

- Windows 10 or later
- .NET Framework 4.7.2 or later
- Visual Studio 2019 or later (for development)
- Minimum 2GB RAM
- 100MB disk space

## Notes

- The server runs in the background and automatically closes when the client exits
- All data is stored in memory and will be lost when the server stops
- For production use, implement persistent storage (database)
- The system supports multiple concurrent client connections
- State changes are immediately reflected across all connected clients
