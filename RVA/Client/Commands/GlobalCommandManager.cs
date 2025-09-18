using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TravelSystem.Client.Commands;
using TravelSystem.Models.DTOs;

namespace TravelSystem.Client.Commands
{
    public class GlobalCommandManager
    {
        private static GlobalCommandManager _instance;
        private readonly UndoRedoManager _undoRedoManager;

        private GlobalCommandManager()
        {
            _undoRedoManager = new UndoRedoManager();
        }

        public static GlobalCommandManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GlobalCommandManager();
                return _instance;
            }
        }

        public bool CanUndo => _undoRedoManager.CanUndo;
        public bool CanRedo => _undoRedoManager.CanRedo;

        public event EventHandler CanUndoRedoChanged
        {
            add { _undoRedoManager.CanUndoRedoChanged += value; }
            remove { _undoRedoManager.CanUndoRedoChanged -= value; }
        }

        public void ExecuteCommand(IUndoableCommand command)
        {
            _undoRedoManager.ExecuteCommand(command);
            Console.WriteLine($"Command executed: {command.Description}");
        }

        public void Undo()
        {
            if (CanUndo)
            {
                _undoRedoManager.Undo();
                Console.WriteLine("Undo executed");
            }
        }

        public void Redo()
        {
            if (CanRedo)
            {
                _undoRedoManager.Redo();
                Console.WriteLine("Redo executed");
            }
        }

        public void Clear()
        {
            _undoRedoManager.Clear();
        }
    }

    /// <summary>
    /// Passenger Commands - koristi IUndoableCommand
    /// </summary>
    public class AddPassengerCommand : UndoableCommand
    {
        private readonly ObservableCollection<PassengerDto> _collection;
        private readonly PassengerDto _passenger;

        public AddPassengerCommand(
            ObservableCollection<PassengerDto> collection,
            PassengerDto passenger)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
            _passenger = passenger ?? throw new ArgumentNullException(nameof(passenger));
        }

        public override string Description => $"Add passenger: {_passenger.FullName}";

        public override void Execute()
        {
            _collection.Add(_passenger);
        }

        public override void Undo()
        {
            _collection.Remove(_passenger);
        }
    }

    public class RemovePassengerCommand : UndoableCommand
    {
        private readonly ObservableCollection<PassengerDto> _collection;
        private readonly PassengerDto _passenger;
        private int _originalIndex;

        public RemovePassengerCommand(
            ObservableCollection<PassengerDto> collection,
            PassengerDto passenger)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
            _passenger = passenger ?? throw new ArgumentNullException(nameof(passenger));
            _originalIndex = _collection.IndexOf(_passenger);
        }

        public override string Description => $"Remove passenger: {_passenger.FullName}";

        public override void Execute()
        {
            _originalIndex = _collection.IndexOf(_passenger);
            _collection.Remove(_passenger);
        }

        public override void Undo()
        {
            if (_originalIndex >= 0 && _originalIndex <= _collection.Count)
            {
                _collection.Insert(_originalIndex, _passenger);
            }
            else
            {
                _collection.Add(_passenger);
            }
        }
    }

    /// <summary>
    /// Destination Commands - koristi IUndoableCommand
    /// </summary>
    public class AddDestinationCommand : UndoableCommand
    {
        private readonly ObservableCollection<DestinationDto> _collection;
        private readonly DestinationDto _destination;

        public AddDestinationCommand(
            ObservableCollection<DestinationDto> collection,
            DestinationDto destination)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
            _destination = destination ?? throw new ArgumentNullException(nameof(destination));
        }

        public override string Description => $"Add destination: {_destination.TownName}";

        public override void Execute()
        {
            _collection.Add(_destination);
        }

        public override void Undo()
        {
            _collection.Remove(_destination);
        }
    }

    public class RemoveDestinationCommand : UndoableCommand
    {
        private readonly ObservableCollection<DestinationDto> _collection;
        private readonly DestinationDto _destination;
        private int _originalIndex;

        public RemoveDestinationCommand(
            ObservableCollection<DestinationDto> collection,
            DestinationDto destination)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
            _destination = destination ?? throw new ArgumentNullException(nameof(destination));
            _originalIndex = _collection.IndexOf(_destination);
        }

        public override string Description => $"Remove destination: {_destination.TownName}";

        public override void Execute()
        {
            _originalIndex = _collection.IndexOf(_destination);
            _collection.Remove(_destination);
        }

        public override void Undo()
        {
            if (_originalIndex >= 0 && _originalIndex <= _collection.Count)
            {
                _collection.Insert(_originalIndex, _destination);
            }
            else
            {
                _collection.Add(_destination);
            }
        }
    }
}