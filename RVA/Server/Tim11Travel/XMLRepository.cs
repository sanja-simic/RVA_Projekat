using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TravelSystem.Models.Entities;

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
            try
            {
                if (File.Exists(_filePath))
                {
                    // Simple XML loading - read all lines and parse basic structure
                    var xmlContent = File.ReadAllText(_filePath);
                    if (!string.IsNullOrWhiteSpace(xmlContent) && xmlContent.Contains("<TravelArrangement"))
                    {
                        // For now, just log that we found existing data
                        // In a full implementation, you would parse XML properly
                        Console.WriteLine($"XML Repository: Found existing data in {_filePath}");
                        
                        // Parse simple XML structure (simplified for demonstration)
                        var arrangements = ParseSimpleXml(xmlContent);
                        _arrangements.AddRange(arrangements);
                    }
                }
                else
                {
                    Console.WriteLine($"XML Repository: No existing file {_filePath}, starting with empty list");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"XML Repository: Error loading from file: {ex.Message}");
                _arrangements.Clear(); // Start fresh on error
            }
        }

        private void SaveToFile()
        {
            try
            {
                var xmlContent = GenerateSimpleXml(_arrangements);
                File.WriteAllText(_filePath, xmlContent);
                Console.WriteLine($"XML Repository: Saved {_arrangements.Count} arrangements to {_filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"XML Repository: Error saving to file: {ex.Message}");
            }
        }

        private List<TravelArrangement> ParseSimpleXml(string xmlContent)
        {
            // Simplified XML parser - in production use proper XML parser
            var arrangements = new List<TravelArrangement>();
            
            try
            {
                // For demonstration, return empty list to avoid complexity
                // In full implementation, parse XML structure properly
                Console.WriteLine("XML Repository: Simple XML parser used - returning empty list for safety");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"XML parsing error: {ex.Message}");
            }
            
            return arrangements;
        }

        private string GenerateSimpleXml(List<TravelArrangement> arrangements)
        {
            var xml = new System.Text.StringBuilder();
            xml.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            xml.AppendLine("<TravelArrangements>");
            
            foreach (var arrangement in arrangements)
            {
                xml.AppendLine("  <TravelArrangement>");
                xml.AppendLine($"    <Id>{EscapeXml(arrangement.Id)}</Id>");
                xml.AppendLine($"    <Transportation>{(int)arrangement.AssociatedTransportation}</Transportation>");
                xml.AppendLine($"    <NumberOfDays>{arrangement.NumberOfDays}</NumberOfDays>");
                xml.AppendLine($"    <State>{(int)arrangement.State}</State>");
                xml.AppendLine($"    <CreatedAt>{arrangement.CreatedAt:yyyy-MM-ddTHH:mm:ss.fffZ}</CreatedAt>");
                xml.AppendLine($"    <UpdatedAt>{arrangement.UpdatedAt:yyyy-MM-ddTHH:mm:ss.fffZ}</UpdatedAt>");
                
                if (arrangement.Destination != null)
                {
                    xml.AppendLine("    <Destination>");
                    xml.AppendLine($"      <Id>{EscapeXml(arrangement.Destination.Id)}</Id>");
                    xml.AppendLine($"      <TownName>{EscapeXml(arrangement.Destination.TownName)}</TownName>");
                    xml.AppendLine($"      <CountryName>{EscapeXml(arrangement.Destination.CountryName)}</CountryName>");
                    xml.AppendLine($"      <StayPriceByDay>{arrangement.Destination.StayPriceByDay}</StayPriceByDay>");
                    xml.AppendLine("    </Destination>");
                }
                
                xml.AppendLine("    <Passengers>");
                foreach (var passenger in arrangement.Passengers)
                {
                    xml.AppendLine("      <Passenger>");
                    xml.AppendLine($"        <Id>{EscapeXml(passenger.Id)}</Id>");
                    xml.AppendLine($"        <FirstName>{EscapeXml(passenger.FirstName)}</FirstName>");
                    xml.AppendLine($"        <LastName>{EscapeXml(passenger.LastName)}</LastName>");
                    xml.AppendLine($"        <PassportNumber>{EscapeXml(passenger.PassportNumber)}</PassportNumber>");
                    xml.AppendLine($"        <LuggageWeight>{passenger.LuggageWeight}</LuggageWeight>");
                    xml.AppendLine("      </Passenger>");
                }
                xml.AppendLine("    </Passengers>");
                xml.AppendLine("  </TravelArrangement>");
            }
            
            xml.AppendLine("</TravelArrangements>");
            return xml.ToString();
        }

        private string EscapeXml(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            
            return input.Replace("&", "&amp;")
                       .Replace("<", "&lt;")
                       .Replace(">", "&gt;")
                       .Replace("\"", "&quot;")
                       .Replace("'", "&apos;");
        }
    }
}
