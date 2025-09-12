using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tim11.Travel
{
    public class XMLRepository : ITravelArrangementRepository
    {
        private readonly string _filePath;
        private List<TravelArrangement> _arrangements;

        public XMLRepository(string filePath = "travel_arrangements.xml")
        {
            _filePath = filePath;
            _arrangements = new List<TravelArrangement>();
            LoadFromFile();
        }

        public void Add(TravelArrangement arrangement)
        {
            _arrangements.Add(arrangement);
            SaveToFile();
        }

        public void Delete(string id)
        {
            var arrangement = _arrangements.FirstOrDefault(a => a.Id == id);
            if (arrangement != null)
            {
                _arrangements.Remove(arrangement);
                SaveToFile();
            }
        }

        public List<TravelArrangement> GetAll()
        {
            return new List<TravelArrangement>(_arrangements);
        }

        public void Update(TravelArrangement arrangement)
        {
            var existingArrangement = _arrangements.FirstOrDefault(a => a.Id == arrangement.Id);
            if (existingArrangement != null)
            {
                var index = _arrangements.IndexOf(existingArrangement);
                _arrangements[index] = arrangement;
                SaveToFile();
            }
        }

        private void LoadFromFile()
        {
            // Jednostavna implementacija - XML serijalizacija može imati problema sa ciklicnim referncama
            // Za sada ostavljamo prazno
            Console.WriteLine("XML Repository: LoadFromFile - simplified implementation");
        }

        private void SaveToFile()
        {
            // Jednostavna implementacija - XML serijalizacija može imati problema sa ciklicnim referncama
            // Za sada ostavljamo prazno
            Console.WriteLine("XML Repository: SaveToFile - simplified implementation");
        }
    }
}
