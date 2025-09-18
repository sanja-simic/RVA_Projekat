using System;
using TravelSystem.Server.Patterns.Command;
using TravelSystem.Models.Entities;

namespace Tim11.Travel
{
    public class UpdateTravelArrangementCommand : ICommand
    {
        private readonly TravelService _travelService;
        private readonly string _arrangementId;
        private readonly TravelArrangement _newArrangement;
        private TravelArrangement _originalArrangement;
        private bool _wasUpdated = false;

        public UpdateTravelArrangementCommand(TravelService travelService, string arrangementId, TravelArrangement newArrangement)
        {
            _travelService = travelService ?? throw new ArgumentNullException(nameof(travelService));
            _arrangementId = arrangementId ?? throw new ArgumentNullException(nameof(arrangementId));
            _newArrangement = newArrangement ?? throw new ArgumentNullException(nameof(newArrangement));
        }

        public void Execute()
        {
            if (_wasUpdated)
                throw new InvalidOperationException("Command has already been executed");

            // Sačuvaj originalni aranžman
            var arrangements = _travelService.GetAllArrangements();
            _originalArrangement = arrangements.Find(a => a.Id == _arrangementId);

            if (_originalArrangement == null)
                throw new InvalidOperationException($"Travel arrangement with ID {_arrangementId} not found");

            // Ažuriraj aranžman
            _travelService.UpdateArrangement(_newArrangement);
            _wasUpdated = true;
        }

        public void Undo()
        {
            if (!_wasUpdated || _originalArrangement == null)
                throw new InvalidOperationException("Command has not been executed or cannot be undone");

            _travelService.UpdateArrangement(_originalArrangement);
            _wasUpdated = false;
        }

        public bool CanUndo => _wasUpdated && _originalArrangement != null;

        public string Description => $"Update Travel Arrangement: {_arrangementId}";
    }

}
