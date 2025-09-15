using System;
using TravelSystem.Models.Enums;
using TravelSystem.Server.DataAccess.FileHandlers;
using TravelSystem.Models.Entities;

namespace TravelSystem.Server.Patterns.Factory
{
    public class FileHandlerFactory
    {
        public static IFileHandler<T> CreateFileHandler<T>(FileFormat format) where T : BaseEntity, new()
        {
            switch (format)
            {
                case FileFormat.XML:
                    return new XmlFileHandler<T>();
                case FileFormat.JSON:
                    return new JsonFileHandler<T>();
                case FileFormat.CSV:
                    return new CsvFileHandler<T>();
                default:
                    throw new ArgumentException($"Unsupported file format: {format}");
            }
        }
    }
}
