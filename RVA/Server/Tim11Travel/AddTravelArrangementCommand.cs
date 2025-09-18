using System;
using TravelSystem.Server.Patterns.Command;
using TravelSystem.Models.Entities;

namespace Tim11.Travel
{
    public class AddTravelArrangementCommand : ICommand
    {
        private readonly TravelService _travelService;
        private readonly TravelArrangement _arrangement;
        private bool _wasAdded = false;

        public AddTravelArrangementCommand(TravelService travelService, TravelArrangement arrangement)
        {
            _travelService = travelService ?? throw new ArgumentNullException(nameof(travelService));
            _arrangement = arrangement ?? throw new ArgumentNullException(nameof(arrangement));
        }

        public void Execute()
        {
            if (_wasAdded)
                throw new InvalidOperationException("Command has already been executed");

            _travelService.AddArrangement(_arrangement);
            _wasAdded = true;
        }

        public void Undo()
        {
            if (!_wasAdded)
                throw new InvalidOperationException("Command has not been executed or has already been undone");

            _travelService.DeleteArrangement(_arrangement.Id);
            _wasAdded = false;
        }

        public bool CanUndo => _wasAdded;

        public string Description => $"Add Travel Arrangement: {_arrangement.Id}";
    }

}
