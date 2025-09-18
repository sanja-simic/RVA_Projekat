using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TravelSystem.Models.Entities;
using TravelSystem.Models.Enums;

namespace TravelSystem.Server.DataAccess.FileHandlers
{
    public class TravelArrangementCsvFileHandler : IFileHandler<TravelArrangement>
    {
        public void Save(List<TravelArrangement> entities, string filePath)
        {
            try
            {
                var lines = new List<string>();

                // Custom header for TravelArrangement
                lines.Add("Id,Transportation,NumberOfDays,State,CreatedAt,UpdatedAt,DestinationId,DestinationTown,DestinationCountry,DestinationPrice,PassengerIds,PassengerNames");

                // Data rows
                foreach (var arrangement in entities)
                {
                    var passengerIds = string.Join(";", arrangement.Passengers.Select(p => p.Id));
                    var passengerNames = string.Join(";", arrangement.Passengers.Select(p => $"{p.FirstName} {p.LastName}"));
                    
                    var line = $"{EscapeCsv(arrangement.Id)}," +
                              $"{(int)arrangement.AssociatedTransportation}," +
                              $"{arrangement.NumberOfDays}," +
                              $"{(int)arrangement.State}," +
                              $"{arrangement.CreatedAt:yyyy-MM-ddTHH:mm:ss.fffZ}," +
                              $"{arrangement.UpdatedAt:yyyy-MM-ddTHH:mm:ss.fffZ}," +
                              $"{EscapeCsv(arrangement.Destination?.Id ?? "")}," +
                              $"{EscapeCsv(arrangement.Destination?.TownName ?? "")}," +
                              $"{EscapeCsv(arrangement.Destination?.CountryName ?? "")}," +
                              $"{arrangement.Destination?.StayPriceByDay ?? 0}," +
                              $"{EscapeCsv(passengerIds)}," +
                              $"{EscapeCsv(passengerNames)}";
                    
                    lines.Add(line);
                }

                File.WriteAllLines(filePath, lines);
                Console.WriteLine($"TravelArrangement CSV: Saved {entities.Count} arrangements to {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving TravelArrangement CSV: {ex.Message}");
                throw new InvalidOperationException($"Failed to save TravelArrangement CSV file: {ex.Message}", ex);
            }
        }

        public List<TravelArrangement> Load(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"TravelArrangement CSV: File {filePath} does not exist");
                    return new List<TravelArrangement>();
                }

                var lines = File.ReadAllLines(filePath);
                if (lines.Length < 2) // Header + at least one data row
                {
                    Console.WriteLine("TravelArrangement CSV: No data rows found");
                    return new List<TravelArrangement>();
                }

                var arrangements = new List<TravelArrangement>();

                // Skip header (first line)
                for (int i = 1; i < lines.Length; i++)
                {
                    var line = lines[i];
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    try
                    {
                        var arrangement = ParseCsvLine(line);
                        if (arrangement != null)
                        {
                            arrangements.Add(arrangement);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing CSV line {i}: {ex.Message}");
                        // Continue with other lines
                    }
                }

                Console.WriteLine($"TravelArrangement CSV: Loaded {arrangements.Count} arrangements from {filePath}");
                return arrangements;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading TravelArrangement CSV: {ex.Message}");
                throw new InvalidOperationException($"Failed to load TravelArrangement CSV file: {ex.Message}", ex);
            }
        }

        private TravelArrangement ParseCsvLine(string line)
        {
            var parts = SplitCsvLine(line);
            if (parts.Length < 12) return null;

            try
            {
                // Create destination
                var destination = new Destination(
                    parts[6], // DestinationId
                    parts[7], // DestinationTown
                    parts[8], // DestinationCountry
                    double.Parse(parts[9]) // DestinationPrice
                );

                // Create arrangement
                var arrangement = new TravelArrangement(
                    parts[0], // Id
                    (ModeOfTransport)int.Parse(parts[1]), // Transportation
                    int.Parse(parts[2]), // NumberOfDays
                    destination
                )
                {
                    State = (EntityState)int.Parse(parts[3]), // State
                    CreatedAt = DateTime.Parse(parts[4]), // CreatedAt
                    UpdatedAt = DateTime.Parse(parts[5]) // UpdatedAt
                };

                // Parse passenger IDs (if any)
                if (!string.IsNullOrEmpty(parts[10]))
                {
                    var passengerIds = parts[10].Split(';');
                    var passengerNames = parts.Length > 11 && !string.IsNullOrEmpty(parts[11]) 
                        ? parts[11].Split(';') 
                        : new string[0];

                    for (int i = 0; i < passengerIds.Length; i++)
                    {
                        var passengerId = passengerIds[i].Trim();
                        if (!string.IsNullOrEmpty(passengerId))
                        {
                            // Create basic passenger info (we'll need to load full details from passenger CSV separately)
                            var passengerName = i < passengerNames.Length ? passengerNames[i] : "Unknown";
                            var nameParts = passengerName.Split(' ');
                            var firstName = nameParts.Length > 0 ? nameParts[0] : "Unknown";
                            var lastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "";

                            var passenger = new Passenger(passengerId, firstName, lastName, "", 0);
                            arrangement.Passengers.Add(passenger);
                        }
                    }
                }

                return arrangement;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing arrangement from CSV line: {ex.Message}");
                return null;
            }
        }

        private string[] SplitCsvLine(string line)
        {
            var result = new List<string>();
            var current = "";
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(current.Trim('"'));
                    current = "";
                }
                else
                {
                    current += c;
                }
            }

            result.Add(current.Trim('"'));
            return result.ToArray();
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

        public string GetFileExtension()
        {
            return ".csv";
        }
    }
}