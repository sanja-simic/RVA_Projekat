using System;
using System.Collections.Generic;
using System.IO;
using TravelSystem.Models.Entities;
using TravelSystem.Server.DataAccess.FileHandlers;

namespace TravelSystem.Server.DataAccess
{
    public class DataManager
    {
        private readonly DataFileFormat _fileFormat;
        private readonly string _dataDirectory;
        
        public DataManager(DataFileFormat fileFormat)
        {
            _fileFormat = fileFormat;
            _dataDirectory = Path.Combine(Environment.CurrentDirectory, "Data");
            
            // Ensure data directory exists
            if (!Directory.Exists(_dataDirectory))
                Directory.CreateDirectory(_dataDirectory);
        }

        public void SaveData<T>(List<T> entities, string fileName) where T : BaseEntity, new()
        {
            var handler = FileHandlerFactory.CreateFileHandler<T>(_fileFormat);
            var extension = FileHandlerFactory.GetFileExtension(_fileFormat);
            var filePath = Path.Combine(_dataDirectory, $"{fileName}{extension}");
            
            handler.Save(entities, filePath);
        }

        public List<T> LoadData<T>(string fileName) where T : BaseEntity, new()
        {
            var handler = FileHandlerFactory.CreateFileHandler<T>(_fileFormat);
            var extension = FileHandlerFactory.GetFileExtension(_fileFormat);
            var filePath = Path.Combine(_dataDirectory, $"{fileName}{extension}");
            
            return handler.Load(filePath);
        }

        public string GetDataFilePath(string fileName)
        {
            var extension = FileHandlerFactory.GetFileExtension(_fileFormat);
            return Path.Combine(_dataDirectory, $"{fileName}{extension}");
        }

        public DataFileFormat CurrentFormat => _fileFormat;
    }
}