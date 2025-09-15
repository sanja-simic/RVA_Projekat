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
                // Simple JSON serialization - for production use proper JSON library
                var json = SimpleJsonSerializer.Serialize(entities);
                File.WriteAllText(filePath, json);
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

                var json = File.ReadAllText(filePath);
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

    // Simple JSON serializer placeholder - in production use Newtonsoft.Json
    public static class SimpleJsonSerializer
    {
        public static string Serialize<T>(T obj)
        {
            return $"{{\"placeholder\": \"Use proper JSON library like Newtonsoft.Json\"}}";
        }

        public static T Deserialize<T>(string json) where T : new()
        {
            return new T();
        }
    }
}
