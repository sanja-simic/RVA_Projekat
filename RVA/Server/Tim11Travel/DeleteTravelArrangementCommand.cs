using System;
using TravelSystem.Server.Patterns.Command;
using TravelSystem.Models.Entities;

namespace Tim11.Travel
{
    public class DeleteTravelArrangementCommand : ICommand
    {
        private readonly TravelService _travelService;
        private readonly string _arrangementId;
        private TravelArrangement _deletedArrangement;
        private bool _wasDeleted = false;

        public DeleteTravelArrangementCommand(TravelService travelService, string arrangementId)
        {
            _travelService = travelService ?? throw new ArgumentNullException(nameof(travelService));
            _arrangementId = arrangementId ?? throw new ArgumentNullException(nameof(arrangementId));
        }

        public void Execute()
        {
            if (_wasDeleted)
                throw new InvalidOperationException("Command has already been executed");

            // Prvo pronađi i sačuvaj aranžman pre brisanja
            var arrangements = _travelService.GetAllArrangements();
            _deletedArrangement = arrangements.Find(a => a.Id == _arrangementId);

            if (_deletedArrangement == null)
                throw new InvalidOperationException($"Travel arrangement with ID {_arrangementId} not found");

            // Zatim obriši
            _travelService.DeleteArrangement(_arrangementId);
            _wasDeleted = true;
        }

        public void Undo()
        {
            if (!_wasDeleted || _deletedArrangement == null)
                throw new InvalidOperationException("Command has not been executed or cannot be undone");

            _travelService.AddArrangement(_deletedArrangement);
            _wasDeleted = false;
        }

        public bool CanUndo => _wasDeleted && _deletedArrangement != null;

        public string Description => $"Delete Travel Arrangement: {_arrangementId}";
    }

}
