using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TravelSystem.Models.Entities;

namespace TravelSystem.Server.DataAccess.FileHandlers
{
    public class CsvFileHandler<T> : IFileHandler<T> where T : BaseEntity, new()
    {
        public void Save(List<T> entities, string filePath)
        {
            try
            {
                var properties = typeof(T).GetProperties();
                var lines = new List<string>();

                // Header
                var header = string.Join(",", properties.Select(p => p.Name));
                lines.Add(header);

                // Data
                foreach (var entity in entities)
                {
                    var values = properties.Select(p => 
                    {
                        var value = p.GetValue(entity);
                        return value?.ToString() ?? "";
                    });
                    lines.Add(string.Join(",", values));
                }

                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save CSV file: {ex.Message}", ex);
            }
        }

        public List<T> Load(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return new List<T>();

                var lines = File.ReadAllLines(filePath);
                if (lines.Length < 2) // Header + at least one data row
                    return new List<T>();

                var properties = typeof(T).GetProperties();
                var headerNames = lines[0].Split(',');
                var entities = new List<T>();

                for (int i = 1; i < lines.Length; i++)
                {
                    var values = lines[i].Split(',');
                    var entity = new T();

                    for (int j = 0; j < Math.Min(headerNames.Length, values.Length); j++)
                    {
                        var property = properties.FirstOrDefault(p => p.Name == headerNames[j]);
                        if (property != null && property.CanWrite)
                        {
                            var convertedValue = Convert.ChangeType(values[j], property.PropertyType);
                            property.SetValue(entity, convertedValue);
                        }
                    }

                    entities.Add(entity);
                }

                return entities;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load CSV file: {ex.Message}", ex);
            }
        }

        public string GetFileExtension()
        {
            return ".csv";
        }
    }
}
