using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TravelSystem.Models.Entities;

namespace Tim11.Travel
{
    public class CSVRepository : ITravelArrangementRepository
    {
        private readonly string _filePath;
        private List<TravelArrangement> _arrangements;

        public CSVRepository(string filePath = "travel_arrangements.csv")
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
            if (!File.Exists(_filePath))
                return;

            var lines = File.ReadAllLines(_filePath);
            // Implementacija parsiranja CSV-a
            // Za sada samo prazan, možete proširiti kasnije
        }

        private void SaveToFile()
        {
            // Implementacija snimanja u CSV
            // Za sada samo prazan, možete proširiti kasnije
        }
    }
}
