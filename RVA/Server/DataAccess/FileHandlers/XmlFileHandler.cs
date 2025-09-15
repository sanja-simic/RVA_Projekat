using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using TravelSystem.Models.Entities;

namespace TravelSystem.Server.DataAccess.FileHandlers
{
    public class XmlFileHandler<T> : IFileHandler<T> where T : BaseEntity, new()
    {
        public void Save(List<T> entities, string filePath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<T>));
                using (var writer = new FileStream(filePath, FileMode.Create))
                {
                    serializer.Serialize(writer, entities);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save XML file: {ex.Message}", ex);
            }
        }

        public List<T> Load(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return new List<T>();

                var serializer = new XmlSerializer(typeof(List<T>));
                using (var reader = new FileStream(filePath, FileMode.Open))
                {
                    return (List<T>)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load XML file: {ex.Message}", ex);
            }
        }

        public string GetFileExtension()
        {
            return ".xml";
        }
    }
}
