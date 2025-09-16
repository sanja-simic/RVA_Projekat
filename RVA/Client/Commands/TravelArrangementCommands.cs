using System;
using System.Collections.ObjectModel;
using TravelSystem.Client.Commands;
using TravelSystem.Models.DTOs;

namespace TravelSystem.Client.Commands
{
    /// <summary>
    /// Command for adding travel arrangement with undo/redo support
    /// </summary>
    public class AddTravelArrangementCommand : UndoableCommand
    {
        private readonly ObservableCollection<TravelArrangementDto> _collection;
        private readonly TravelArrangementDto _arrangement;

        public AddTravelArrangementCommand(
            ObservableCollection<TravelArrangementDto> collection, 
            TravelArrangementDto arrangement)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
            _arrangement = arrangement ?? throw new ArgumentNullException(nameof(arrangement));
        }

        public override string Description => $"Add arrangement to {_arrangement.Destination?.TownName}";

        public override void Execute()
        {
            _collection.Add(_arrangement);
        }

        public override void Undo()
        {
            _collection.Remove(_arrangement);
        }
    }

    /// <summary>
    /// Command for deleting travel arrangement with undo/redo support
    /// </summary>
    public class DeleteTravelArrangementCommand : UndoableCommand
    {
        private readonly ObservableCollection<TravelArrangementDto> _collection;
        private readonly TravelArrangementDto _arrangement;
        private int _originalIndex;

        public DeleteTravelArrangementCommand(
            ObservableCollection<TravelArrangementDto> collection, 
            TravelArrangementDto arrangement)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
            _arrangement = arrangement ?? throw new ArgumentNullException(nameof(arrangement));
            _originalIndex = _collection.IndexOf(_arrangement);
        }

        public override string Description => $"Delete arrangement to {_arrangement.Destination?.TownName}";

        public override void Execute()
        {
            _originalIndex = _collection.IndexOf(_arrangement);
            _collection.Remove(_arrangement);
        }

        public override void Undo()
        {
            if (_originalIndex >= 0 && _originalIndex <= _collection.Count)
            {
                _collection.Insert(_originalIndex, _arrangement);
            }
            else
            {
                _collection.Add(_arrangement);
            }
        }
    }

    /// <summary>
    /// Command for updating travel arrangement with undo/redo support
    /// </summary>
    public class UpdateTravelArrangementCommand : UndoableCommand
    {
        private readonly ObservableCollection<TravelArrangementDto> _collection;
        private readonly TravelArrangementDto _originalArrangement;
        private readonly TravelArrangementDto _updatedArrangement;
        private readonly int _index;

        public UpdateTravelArrangementCommand(
            ObservableCollection<TravelArrangementDto> collection,
            TravelArrangementDto originalArrangement,
            TravelArrangementDto updatedArrangement)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
            _originalArrangement = originalArrangement ?? throw new ArgumentNullException(nameof(originalArrangement));
            _updatedArrangement = updatedArrangement ?? throw new ArgumentNullException(nameof(updatedArrangement));
            _index = _collection.IndexOf(_originalArrangement);
        }

        public override string Description => $"Update arrangement to {_updatedArrangement.Destination?.TownName}";

        public override void Execute()
        {
            if (_index >= 0)
            {
                _collection[_index] = _updatedArrangement;
            }
        }

        public override void Undo()
        {
            if (_index >= 0 && _index < _collection.Count)
            {
                _collection[_index] = _originalArrangement;
            }
        }
    }
}