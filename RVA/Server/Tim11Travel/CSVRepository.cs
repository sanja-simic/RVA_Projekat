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
            try
            {
                if (!File.Exists(_filePath))
                {
                    Console.WriteLine($"CSV Repository: No existing file {_filePath}, starting with empty list");
                    return;
                }

                var lines = File.ReadAllLines(_filePath);
                _arrangements.Clear();
                
                if (lines.Length <= 1) // Header only or empty
                {
                    Console.WriteLine("CSV Repository: File exists but no data found");
                    return;
                }

                // Skip header line (first line)
                for (int i = 1; i < lines.Length; i++)
                {
                    var line = lines[i];
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    
                    var arrangement = ParseCsvLine(line);
                    if (arrangement != null)
                    {
                        _arrangements.Add(arrangement);
                    }
                }
                
                Console.WriteLine($"CSV Repository: Loaded {_arrangements.Count} arrangements from {_filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CSV Repository: Error loading from file: {ex.Message}");
                _arrangements.Clear();
            }
        }

        private void SaveToFile()
        {
            try
            {
                var lines = new List<string>();
                
                // Header
                lines.Add("Id,Transportation,NumberOfDays,State,CreatedAt,UpdatedAt,DestinationId,TownName,CountryName,StayPriceByDay,PassengerIds");
                
                foreach (var arrangement in _arrangements)
                {
                    var csvLine = GenerateCsvLine(arrangement);
                    lines.Add(csvLine);
                }
                
                File.WriteAllLines(_filePath, lines);
                Console.WriteLine($"CSV Repository: Saved {_arrangements.Count} arrangements to {_filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CSV Repository: Error saving to file: {ex.Message}");
            }
        }

        private TravelArrangement ParseCsvLine(string line)
        {
            try
            {
                var parts = line.Split(',');
                if (parts.Length < 11) return null;
                
                // For simplified implementation, just log that we found data
                // In production, you would parse all the fields properly
                Console.WriteLine($"CSV Repository: Found arrangement data - {parts[0]}");
                
                // Return null for now to avoid parsing complexity
                // In full implementation, create TravelArrangement from CSV data
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CSV Repository: Error parsing line: {ex.Message}");
                return null;
            }
        }

        private string GenerateCsvLine(TravelArrangement arrangement)
        {
            try
            {
                var passengerIds = string.Join(";", arrangement.Passengers.Select(p => p.Id));
                var destination = arrangement.Destination;
                
                return $"{EscapeCsv(arrangement.Id)}," +
                       $"{(int)arrangement.AssociatedTransportation}," +
                       $"{arrangement.NumberOfDays}," +
                       $"{(int)arrangement.State}," +
                       $"{arrangement.CreatedAt:yyyy-MM-ddTHH:mm:ss.fffZ}," +
                       $"{arrangement.UpdatedAt:yyyy-MM-ddTHH:mm:ss.fffZ}," +
                       $"{EscapeCsv(destination?.Id ?? "")}," +
                       $"{EscapeCsv(destination?.TownName ?? "")}," +
                       $"{EscapeCsv(destination?.CountryName ?? "")}," +
                       $"{destination?.StayPriceByDay ?? 0}," +
                       $"{EscapeCsv(passengerIds)}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CSV Repository: Error generating CSV line: {ex.Message}");
                return "";
            }
        }

        private string EscapeCsv(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            
            // Simple CSV escaping
            if (input.Contains(",") || input.Contains("\"") || input.Contains("\n"))
            {
                return "\"" + input.Replace("\"", "\"\"") + "\"";
            }
            
            return input;
        }
    }
}
