using System;

namespace Tim11.Travel
{
    public class TravelNotificationObserver : IObserver
    {
        private string observerName;

        public TravelNotificationObserver(string name)
        {
            observerName = name;
        }

        public void Update(TravelArrangement arrangement, string eventType)
        {
            Console.WriteLine($"[{observerName}] Notifikacija: Aranžman {arrangement.Id} je {eventType}");
            Console.WriteLine($"  - Destinacija: {arrangement.Destination.TownName}, {arrangement.Destination.CountryName}");
            Console.WriteLine($"  - Prevoz: {arrangement.AssociatedTransportation}");
            Console.WriteLine($"  - Broj dana: {arrangement.NumberOfDays}");
            Console.WriteLine($"  - Trenutno stanje: {arrangement.GetCurrentStateName()}");
            Console.WriteLine($"  - Broj putnika: {arrangement.Passengers.Count}");
            Console.WriteLine();
        }
    }
}
