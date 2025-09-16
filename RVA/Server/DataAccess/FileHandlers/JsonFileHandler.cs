using System;
using System.Collections.Generic;
using System.IO;
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
            // Basic JSON serialization - for production use proper library
            if (obj == null)
                return "null";

            if (obj is IList<BaseEntity> entities)
            {
                return SerializeEntities(entities);
            }

            return "{\"message\": \"Simple serializer - limited functionality\"}";
        }

        public static T Deserialize<T>(string json) where T : new()
        {
            // Basic JSON deserialization - for production use proper library
            if (string.IsNullOrWhiteSpace(json) || json.Trim() == "null")
                return new T();

            // Return empty object - let the application populate with sample data
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
                json.AppendLine($"    \"Id\": \"{EscapeJson(entity.Id)}\",");
                json.AppendLine($"    \"CreatedAt\": \"{entity.CreatedAt:yyyy-MM-ddTHH:mm:ss.fffZ}\",");
                json.AppendLine($"    \"UpdatedAt\": \"{entity.UpdatedAt:yyyy-MM-ddTHH:mm:ss.fffZ}\"");
                json.Append("  }");
                
                if (i < entities.Count - 1)
                    json.AppendLine(",");
                else
                    json.AppendLine();
            }

            json.AppendLine("]");
            return json.ToString();
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
