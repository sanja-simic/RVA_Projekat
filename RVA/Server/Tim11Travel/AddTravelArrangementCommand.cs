using System;
using TravelSystem.Server.Patterns.Command;
using TravelSystem.Models.Entities;

namespace Tim11.Travel
{
    public class AddTravelArrangementCommand : ICommand
    {
        private readonly TravelService _travelService;
        private readonly TravelArrangement _arrangement;

        public AddTravelArrangementCommand(TravelService travelService, TravelArrangement arrangement)
        {
            _travelService = travelService;
            _arrangement = arrangement;
        }

        public void Execute()
        {
            _travelService.AddArrangement(_arrangement);
        }

        public void Undo()
        {
            _travelService.DeleteArrangement(_arrangement.Id);
        }

        public bool CanUndo => true;

        public string Description => $"Add Travel Arrangement: {_arrangement.Id}";
    }
}
