using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TravelSystem.Models.Entities;
using TravelSystem.Models.Enums;

namespace TravelSystem.Server.DataAccess.FileHandlers
{
    public class TravelArrangementJsonFileHandler : IFileHandler<TravelArrangement>
    {
        public void Save(List<TravelArrangement> entities, string filePath)
        {
            try
            {
                var json = SerializeTravelArrangements(entities);
                File.WriteAllText(filePath, json, Encoding.UTF8);
                Console.WriteLine($"TravelArrangement JSON: Saved {entities.Count} arrangements to {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving TravelArrangement JSON: {ex.Message}");
                throw new InvalidOperationException($"Failed to save TravelArrangement JSON file: {ex.Message}", ex);
            }
        }

        public List<TravelArrangement> Load(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"TravelArrangement JSON: File {filePath} does not exist");
                    return new List<TravelArrangement>();
                }

                var json = File.ReadAllText(filePath, Encoding.UTF8);
                var arrangements = DeserializeTravelArrangements(json);
                Console.WriteLine($"TravelArrangement JSON: Loaded {arrangements.Count} arrangements from {filePath}");
                return arrangements;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading TravelArrangement JSON: {ex.Message}");
                return new List<TravelArrangement>();
            }
        }

        private string SerializeTravelArrangements(List<TravelArrangement> arrangements)
        {
            if (arrangements == null || arrangements.Count == 0)
                return "[]";

            var json = new StringBuilder();
            json.AppendLine("[");

            for (int i = 0; i < arrangements.Count; i++)
            {
                var arrangement = arrangements[i];
                json.AppendLine("  {");
                
                // Basic properties
                json.AppendLine($"    \"Id\": \"{EscapeJson(arrangement.Id)}\",");
                json.AppendLine($"    \"AssociatedTransportation\": \"{arrangement.AssociatedTransportation}\",");
                json.AppendLine($"    \"NumberOfDays\": {arrangement.NumberOfDays},");
                json.AppendLine($"    \"State\": \"{arrangement.State}\",");
                json.AppendLine($"    \"CreatedAt\": \"{arrangement.CreatedAt:yyyy-MM-ddTHH:mm:ss.fffZ}\",");
                json.AppendLine($"    \"UpdatedAt\": \"{arrangement.UpdatedAt:yyyy-MM-ddTHH:mm:ss.fffZ}\",");
                
                // Destination object
                if (arrangement.Destination != null)
                {
                    json.AppendLine("    \"Destination\": {");
                    json.AppendLine($"      \"Id\": \"{EscapeJson(arrangement.Destination.Id)}\",");
                    json.AppendLine($"      \"TownName\": \"{EscapeJson(arrangement.Destination.TownName)}\",");
                    json.AppendLine($"      \"CountryName\": \"{EscapeJson(arrangement.Destination.CountryName)}\",");
                    json.AppendLine($"      \"StayPriceByDay\": {arrangement.Destination.StayPriceByDay},");
                    json.AppendLine($"      \"CreatedAt\": \"{arrangement.Destination.CreatedAt:yyyy-MM-ddTHH:mm:ss.fffZ}\",");
                    json.AppendLine($"      \"UpdatedAt\": \"{arrangement.Destination.UpdatedAt:yyyy-MM-ddTHH:mm:ss.fffZ}\"");
                    json.AppendLine("    },");
                }
                else
                {
                    json.AppendLine("    \"Destination\": null,");
                }
                
                // Passengers array
                json.AppendLine("    \"Passengers\": [");
                if (arrangement.Passengers != null && arrangement.Passengers.Count > 0)
                {
                    for (int j = 0; j < arrangement.Passengers.Count; j++)
                    {
                        var passenger = arrangement.Passengers[j];
                        json.AppendLine("      {");
                        json.AppendLine($"        \"Id\": \"{EscapeJson(passenger.Id)}\",");
                        json.AppendLine($"        \"FirstName\": \"{EscapeJson(passenger.FirstName)}\",");
                        json.AppendLine($"        \"LastName\": \"{EscapeJson(passenger.LastName)}\",");
                        json.AppendLine($"        \"PassportNumber\": \"{EscapeJson(passenger.PassportNumber)}\",");
                        json.AppendLine($"        \"LuggageWeight\": {passenger.LuggageWeight},");
                        json.AppendLine($"        \"CreatedAt\": \"{passenger.CreatedAt:yyyy-MM-ddTHH:mm:ss.fffZ}\",");
                        json.AppendLine($"        \"UpdatedAt\": \"{passenger.UpdatedAt:yyyy-MM-ddTHH:mm:ss.fffZ}\"");
                        json.Append("      }");
                        
                        if (j < arrangement.Passengers.Count - 1)
                            json.AppendLine(",");
                        else
                            json.AppendLine();
                    }
                }
                json.AppendLine("    ]");
                
                json.Append("  }");
                
                if (i < arrangements.Count - 1)
                    json.AppendLine(",");
                else
                    json.AppendLine();
            }

            json.AppendLine("]");
            return json.ToString();
        }

        private List<TravelArrangement> DeserializeTravelArrangements(string json)
        {
            var arrangements = new List<TravelArrangement>();
            
            try
            {
                if (string.IsNullOrWhiteSpace(json) || json.Trim() == "[]")
                {
                    return arrangements;
                }

                // Simple JSON parsing specifically for our format
                var lines = json.Split('\n').Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l)).ToArray();
                
                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    
                    // Look for start of arrangement object
                    if (line == "{" && (i == 1 || lines[i-1] == "," || lines[i-1].Contains("[")))
                    {
                        var arrangement = ParseArrangementObject(lines, ref i);
                        if (arrangement != null)
                        {
                            arrangements.Add(arrangement);
                        }
                    }
                }
                
                Console.WriteLine($"TravelArrangement JSON: Parsed {arrangements.Count} arrangements");
                return arrangements;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TravelArrangement JSON parsing error: {ex.Message}");
                return new List<TravelArrangement>();
            }
        }

        private TravelArrangement ParseArrangementObject(string[] lines, ref int index)
        {
            try
            {
                string id = null, transportation = null, state = null;
                int numberOfDays = 0;
                DateTime createdAt = DateTime.Now, updatedAt = DateTime.Now;
                Destination destination = null;
                var passengers = new List<Passenger>();

                index++; // Skip opening {
                
                // Parse arrangement properties
                while (index < lines.Length && !lines[index].StartsWith("}"))
                {
                    var line = lines[index].Trim();
                    
                    if (line.StartsWith("\"Id\":"))
                        id = ExtractStringValue(line);
                    else if (line.StartsWith("\"AssociatedTransportation\":"))
                        transportation = ExtractStringValue(line);
                    else if (line.StartsWith("\"NumberOfDays\":"))
                        numberOfDays = ExtractIntValue(line);
                    else if (line.StartsWith("\"State\":"))
                        state = ExtractStringValue(line);
                    else if (line.StartsWith("\"CreatedAt\":"))
                        createdAt = ExtractDateValue(line);
                    else if (line.StartsWith("\"UpdatedAt\":"))
                        updatedAt = ExtractDateValue(line);
                    else if (line.StartsWith("\"Destination\":") && !line.Contains("null"))
                    {
                        destination = ParseDestinationObject(lines, ref index);
                        continue; // ParseDestinationObject already increments index
                    }
                    else if (line.StartsWith("\"Passengers\":"))
                    {
                        passengers = ParsePassengersArray(lines, ref index);
                        continue; // ParsePassengersArray already increments index
                    }
                    
                    index++;
                }

                if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(transportation))
                {
                    if (Enum.TryParse<ModeOfTransport>(transportation, out var transportMode) &&
                        Enum.TryParse<EntityState>(state, out var entityState))
                    {
                        var arrangement = new TravelArrangement(id, transportMode, numberOfDays, destination)
                        {
                            State = entityState,
                            CreatedAt = createdAt,
                            UpdatedAt = updatedAt
                        };
                        
                        foreach (var passenger in passengers)
                        {
                            arrangement.Passengers.Add(passenger);
                        }
                        
                        return arrangement;
                    }
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing arrangement object: {ex.Message}");
                return null;
            }
        }

        private Destination ParseDestinationObject(string[] lines, ref int index)
        {
            try
            {
                string id = null, townName = null, countryName = null;
                double stayPrice = 0;
                DateTime createdAt = DateTime.Now, updatedAt = DateTime.Now;

                index++; // Skip the "Destination": { line
                
                while (index < lines.Length && !lines[index].Trim().StartsWith("}"))
                {
                    var line = lines[index].Trim();
                    
                    if (line.StartsWith("\"Id\":"))
                        id = ExtractStringValue(line);
                    else if (line.StartsWith("\"TownName\":"))
                        townName = ExtractStringValue(line);
                    else if (line.StartsWith("\"CountryName\":"))
                        countryName = ExtractStringValue(line);
                    else if (line.StartsWith("\"StayPriceByDay\":"))
                        stayPrice = ExtractDoubleValue(line);
                    else if (line.StartsWith("\"CreatedAt\":"))
                        createdAt = ExtractDateValue(line);
                    else if (line.StartsWith("\"UpdatedAt\":"))
                        updatedAt = ExtractDateValue(line);
                    
                    index++;
                }

                if (!string.IsNullOrEmpty(id))
                {
                    return new Destination(id, townName, countryName, stayPrice)
                    {
                        CreatedAt = createdAt,
                        UpdatedAt = updatedAt
                    };
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing destination: {ex.Message}");
                return null;
            }
        }

        private List<Passenger> ParsePassengersArray(string[] lines, ref int index)
        {
            var passengers = new List<Passenger>();
            
            try
            {
                index++; // Skip "Passengers": [
                
                while (index < lines.Length && !lines[index].Trim().StartsWith("]"))
                {
                    var line = lines[index].Trim();
                    
                    if (line == "{")
                    {
                        var passenger = ParsePassengerObject(lines, ref index);
                        if (passenger != null)
                        {
                            passengers.Add(passenger);
                        }
                        continue; // ParsePassengerObject already increments index
                    }
                    
                    index++;
                }
                
                return passengers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing passengers array: {ex.Message}");
                return passengers;
            }
        }

        private Passenger ParsePassengerObject(string[] lines, ref int index)
        {
            try
            {
                string id = null, firstName = null, lastName = null, passportNumber = null;
                int luggageWeight = 0;
                DateTime createdAt = DateTime.Now, updatedAt = DateTime.Now;

                index++; // Skip the { line
                
                while (index < lines.Length && !lines[index].Trim().StartsWith("}"))
                {
                    var line = lines[index].Trim();
                    
                    if (line.StartsWith("\"Id\":"))
                        id = ExtractStringValue(line);
                    else if (line.StartsWith("\"FirstName\":"))
                        firstName = ExtractStringValue(line);
                    else if (line.StartsWith("\"LastName\":"))
                        lastName = ExtractStringValue(line);
                    else if (line.StartsWith("\"PassportNumber\":"))
                        passportNumber = ExtractStringValue(line);
                    else if (line.StartsWith("\"LuggageWeight\":"))
                        luggageWeight = ExtractIntValue(line);
                    else if (line.StartsWith("\"CreatedAt\":"))
                        createdAt = ExtractDateValue(line);
                    else if (line.StartsWith("\"UpdatedAt\":"))
                        updatedAt = ExtractDateValue(line);
                    
                    index++;
                }

                if (!string.IsNullOrEmpty(id))
                {
                    return new Passenger(id, firstName, lastName, passportNumber, luggageWeight)
                    {
                        CreatedAt = createdAt,
                        UpdatedAt = updatedAt
                    };
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing passenger: {ex.Message}");
                return null;
            }
        }

        private string ExtractStringValue(string line)
        {
            var startIndex = line.IndexOf('"', line.IndexOf(':')) + 1;
            var endIndex = line.LastIndexOf('"');
            if (startIndex > 0 && endIndex > startIndex)
            {
                return line.Substring(startIndex, endIndex - startIndex);
            }
            return null;
        }

        private int ExtractIntValue(string line)
        {
            var colonIndex = line.IndexOf(':');
            var commaIndex = line.IndexOf(',');
            if (commaIndex == -1) commaIndex = line.Length;
            
            var valueStr = line.Substring(colonIndex + 1, commaIndex - colonIndex - 1).Trim();
            if (int.TryParse(valueStr, out var result))
            {
                return result;
            }
            return 0;
        }

        private double ExtractDoubleValue(string line)
        {
            var colonIndex = line.IndexOf(':');
            var commaIndex = line.IndexOf(',');
            if (commaIndex == -1) commaIndex = line.Length;
            
            var valueStr = line.Substring(colonIndex + 1, commaIndex - colonIndex - 1).Trim();
            if (double.TryParse(valueStr, out var result))
            {
                return result;
            }
            return 0;
        }

        private DateTime ExtractDateValue(string line)
        {
            var dateStr = ExtractStringValue(line);
            if (DateTime.TryParse(dateStr, out var result))
            {
                return result;
            }
            return DateTime.Now;
        }

        private string EscapeJson(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return input.Replace("\\", "\\\\")
                       .Replace("\"", "\\\"")
                       .Replace("\r", "\\r")
                       .Replace("\n", "\\n")
                       .Replace("\t", "\\t");
        }

        public string GetFileExtension()
        {
            return ".json";
        }
    }
}