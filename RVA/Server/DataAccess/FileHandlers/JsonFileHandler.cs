using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TravelSystem.Models.Entities;
using TravelSystem.Models.Enums;

namespace TravelSystem.Server.DataAccess.FileHandlers
{
    public class JsonFileHandler<T> : IFileHandler<T> where T : BaseEntity, new()
    {
        public void Save(List<T> entities, string filePath)
        {
            try
            {
                var json = SimpleJsonSerializer.Serialize(entities);
                File.WriteAllText(filePath, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save JSON file: {ex.Message}", ex);
            }
        }

        public List<T> Load(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return new List<T>();

                var json = File.ReadAllText(filePath, Encoding.UTF8);
                if (string.IsNullOrWhiteSpace(json))
                    return new List<T>();

                return SimpleJsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load JSON file: {ex.Message}", ex);
            }
        }

        public string GetFileExtension()
        {
            return ".json";
        }
    }

    // Simple JSON serializer for basic functionality
    public static class SimpleJsonSerializer
    {
        public static string Serialize<T>(T obj)
        {
            if (obj == null)
                return "null";

            // Check if it's a list of BaseEntity-derived objects
            if (obj is System.Collections.IList list && list.Count > 0 && list[0] is BaseEntity)
            {
                var entities = list.Cast<BaseEntity>().ToList();
                return SerializeEntities(entities);
            }
            else if (obj is System.Collections.IList emptyList)
            {
                return "[]";
            }

            // For single objects, wrap in basic JSON
            return $"{{\"data\": \"{obj}\"}}";
        }

        public static T Deserialize<T>(string json) where T : new()
        {
            if (string.IsNullOrWhiteSpace(json) || json.Trim() == "null")
                return new T();

            // For entities list, try to parse
            if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
            {
                return DeserializeEntities<T>(json);
            }

            return new T();
        }

        private static string SerializeEntities(IList<BaseEntity> entities)
        {
            if (entities == null || entities.Count == 0)
                return "[]";

            var json = new StringBuilder();
            json.AppendLine("[");

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                json.AppendLine("  {");
                
                // Serialize all properties using reflection
                var properties = entity.GetType().GetProperties().Where(p => p.CanRead).ToArray();
                
                for (int propIndex = 0; propIndex < properties.Length; propIndex++)
                {
                    var prop = properties[propIndex];
                    var value = prop.GetValue(entity);
                    var jsonValue = SerializePropertyValue(value);
                    
                    json.Append($"    \"{prop.Name}\": {jsonValue}");
                    
                    if (propIndex < properties.Length - 1)
                        json.AppendLine(",");
                    else
                        json.AppendLine();
                }
                
                json.Append("  }");
                
                if (i < entities.Count - 1)
                    json.AppendLine(",");
                else
                    json.AppendLine();
            }

            json.AppendLine("]");
            return json.ToString();
        }
        
        private static string SerializePropertyValue(object value)
        {
            if (value == null)
                return "null";
                
            if (value is string str)
                return $"\"{EscapeJson(str)}\"";
                
            if (value is DateTime dt)
                return $"\"{dt:yyyy-MM-ddTHH:mm:ss.fffZ}\"";
                
            if (value is bool || value is int || value is double || value is decimal || value is float)
                return value.ToString().ToLower();
                
            if (value is Enum)
                return $"\"{value}\"";
                
            // For complex objects, just use ToString for now
            return $"\"{EscapeJson(value.ToString())}\"";
        }

        private static T DeserializeEntities<T>(string json) where T : new()
        {
            try 
            {
                if (string.IsNullOrWhiteSpace(json) || json.Trim() == "[]")
                {
                    return new T();
                }

                // Basic JSON parsing for simple entities (Destination, Passenger)
                var lines = json.Split('\n').Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l)).ToArray();
                
                var result = new T();
                if (result is System.Collections.IList list)
                {
                    // Parse entities from JSON array
                    for (int i = 0; i < lines.Length; i++)
                    {
                        var line = lines[i];
                        if (line == "{" && (i == 1 || lines[i-1] == "," || lines[i-1].Contains("[")))
                        {
                            var entity = ParseSimpleEntity(lines, ref i, typeof(T).GetGenericArguments()[0]);
                            if (entity != null)
                            {
                                list.Add(entity);
                            }
                        }
                    }
                }
                
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"JSON deserialization error: {ex.Message}");
                return new T();
            }
        }

        private static object ParseSimpleEntity(string[] lines, ref int index, Type entityType)
        {
            try
            {
                object entity = null;
                
                // Create entity based on type
                if (entityType == typeof(Destination))
                {
                    entity = ParseDestination(lines, ref index);
                }
                else if (entityType == typeof(Passenger))
                {
                    entity = ParsePassenger(lines, ref index);
                }
                
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing entity: {ex.Message}");
                return null;
            }
        }

        private static Destination ParseDestination(string[] lines, ref int index)
        {
            string id = null, townName = null, countryName = null;
            double stayPrice = 0;
            DateTime createdAt = DateTime.Now, updatedAt = DateTime.Now;

            index++; // Skip opening {
            
            while (index < lines.Length && !lines[index].StartsWith("}"))
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

        private static Passenger ParsePassenger(string[] lines, ref int index)
        {
            string id = null, firstName = null, lastName = null, passportNumber = null;
            int luggageWeight = 0;
            DateTime createdAt = DateTime.Now, updatedAt = DateTime.Now;

            index++; // Skip opening {
            
            while (index < lines.Length && !lines[index].StartsWith("}"))
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

        private static string ExtractStringValue(string line)
        {
            var startIndex = line.IndexOf('"', line.IndexOf(':')) + 1;
            var endIndex = line.LastIndexOf('"');
            if (startIndex > 0 && endIndex > startIndex)
            {
                return line.Substring(startIndex, endIndex - startIndex);
            }
            return null;
        }

        private static int ExtractIntValue(string line)
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

        private static double ExtractDoubleValue(string line)
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

        private static DateTime ExtractDateValue(string line)
        {
            var dateStr = ExtractStringValue(line);
            if (DateTime.TryParse(dateStr, out var result))
            {
                return result;
            }
            return DateTime.Now;
        }

        private static string EscapeJson(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return input.Replace("\\", "\\\\")
                       .Replace("\"", "\\\"")
                       .Replace("\r", "\\r")
                       .Replace("\n", "\\n")
                       .Replace("\t", "\\t");
        }
    }
}
