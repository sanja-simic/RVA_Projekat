using System;
using TravelSystem.Server.Patterns.Command;
using TravelSystem.Models.Entities;

namespace Tim11.Travel
{
    public class UpdateTravelArrangementCommand : ICommand
    {
        private readonly TravelService _travelService;
        private readonly TravelArrangement _newArrangement;
        private TravelArrangement _oldArrangement;

        public UpdateTravelArrangementCommand(TravelService travelService, TravelArrangement newArrangement)
        {
            _travelService = travelService;
            _newArrangement = newArrangement;
        }

        public void Execute()
        {
            // Čuvamo staro stanje pre ažuriranja
            var arrangements = _travelService.GetAllArrangements();
            _oldArrangement = arrangements.Find(a => a.Id == _newArrangement.Id);
            
            _travelService.UpdateArrangement(_newArrangement);
        }

        public void Undo()
        {
            if (_oldArrangement != null)
            {
                _travelService.UpdateArrangement(_oldArrangement);
            }
        }

        public bool CanUndo => _oldArrangement != null;

        public string Description => $"Update Travel Arrangement: {_newArrangement.Id}";
    }
}
