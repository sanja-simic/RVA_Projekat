using System;
using TravelSystem.Models.Entities;

namespace TravelSystem.Server.DataAccess.FileHandlers
{
    public enum DataFileFormat
    {
        XML,
        JSON,
        CSV
    }

    public class FileHandlerFactory
    {
        public static IFileHandler<T> CreateFileHandler<T>(DataFileFormat format) where T : BaseEntity, new()
        {
            switch (format)
            {
                case DataFileFormat.XML:
                    return new XmlFileHandler<T>();
                case DataFileFormat.JSON:
                    // Use specialized handler for TravelArrangement
                    if (typeof(T) == typeof(TravelArrangement))
                    {
                        return (IFileHandler<T>)(object)new TravelArrangementJsonFileHandler();
                    }
                    return new JsonFileHandler<T>();
                case DataFileFormat.CSV:
                    // Use specialized handler for TravelArrangement
                    if (typeof(T) == typeof(TravelArrangement))
                    {
                        return (IFileHandler<T>)(object)new TravelArrangementCsvFileHandler();
                    }
                    return new CsvFileHandler<T>();
                default:
                    throw new ArgumentException($"Unsupported file format: {format}");
            }
        }

        public static DataFileFormat ParseFileFormat(string formatString)
        {
            switch (formatString.ToUpper())
            {
                case "XML":
                    return DataFileFormat.XML;
                case "JSON":
                    return DataFileFormat.JSON;
                case "CSV":
                    return DataFileFormat.CSV;
                default:
                    throw new ArgumentException($"Invalid format: {formatString}. Supported formats: XML, JSON, CSV");
            }
        }

        public static string GetFileExtension(DataFileFormat format)
        {
            switch (format)
            {
                case DataFileFormat.XML:
                    return ".xml";
                case DataFileFormat.JSON:
                    return ".json";
                case DataFileFormat.CSV:
                    return ".csv";
                default:
                    throw new ArgumentException($"Unsupported file format: {format}");
            }
        }
    }
}