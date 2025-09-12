using System;

namespace Tim11.Travel
{
    public class DeleteTravelArrangementCommand : ICommand
    {
        private readonly TravelService _travelService;
        private readonly string _arrangementId;
        private TravelArrangement _deletedArrangement;

        public DeleteTravelArrangementCommand(TravelService travelService, string arrangementId)
        {
            _travelService = travelService;
            _arrangementId = arrangementId;
        }

        public void Execute()
        {
            // Čuvamo aranžman pre brisanja
            var arrangements = _travelService.GetAllArrangements();
            _deletedArrangement = arrangements.Find(a => a.Id == _arrangementId);
            
            _travelService.DeleteArrangement(_arrangementId);
        }

        public void Undo()
        {
            if (_deletedArrangement != null)
            {
                _travelService.AddArrangement(_deletedArrangement);
            }
        }
    }
}
