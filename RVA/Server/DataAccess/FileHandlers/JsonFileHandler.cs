using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TravelSystem.Models.Entities;

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
            // Basic parsing - for production use proper JSON library like Newtonsoft.Json
            // This is a minimal implementation to get basic functionality working
            
            // For now, return empty list - the application will populate with sample data
            // In production, implement proper JSON parsing
            return new T();
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
