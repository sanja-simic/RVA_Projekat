using System.Collections.Generic;
using TravelSystem.Models.Entities;
using TravelSystem.Models.Enums;
using TravelSystem.Models.Interfaces;

namespace TravelSystem.Models.Entities
{
    public class TravelArrangement : BaseEntity, IStateful
    {
        public ModeOfTransport AssociatedTransportation { get; set; }
        public int NumberOfDays { get; set; }
        public List<Passenger> Passengers { get; set; }
        public Destination Destination { get; set; }
        public EntityState State { get; set; }

        public TravelArrangement() : base()
        {
            Passengers = new List<Passenger>();
            State = EntityState.Reserved;
        }

        public TravelArrangement(string id, ModeOfTransport transportation, int numberOfDays, Destination destination) 
            : base(id)
        {
            AssociatedTransportation = transportation;
            NumberOfDays = numberOfDays;
            Destination = destination;
            Passengers = new List<Passenger>();
            State = EntityState.Reserved;
        }

        public void ChangeState()
        {
            // State pattern implementation will be handled in the server
            switch (State)
            {
                case EntityState.Reserved:
                    State = EntityState.Paid;
                    break;
                case EntityState.Paid:
                    State = EntityState.InProgress;
                    break;
                case EntityState.InProgress:
                    State = EntityState.Completed;
                    break;
            }
            UpdatedAt = System.DateTime.Now;
        }

        public string GetCurrentStateName()
        {
            return State.ToString();
        }

        public override bool IsValid()
        {
            return base.IsValid() && 
                   NumberOfDays > 0 && 
                   Destination != null && 
                   Destination.IsValid();
        }

        public override string GetValidationErrors()
        {
            var errors = base.GetValidationErrors();
            
            if (NumberOfDays <= 0)
                errors += "Number of days must be greater than 0. ";
                
            if (Destination == null)
                errors += "Destination cannot be null. ";
            else if (!Destination.IsValid())
                errors += $"Destination validation failed: {Destination.GetValidationErrors()} ";
                
            return errors.Trim();
        }
    }
}
