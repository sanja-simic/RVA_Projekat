using System.Collections.Generic;
using TravelSystem.Models.Entities;

namespace TravelSystem.Server.DataAccess.FileHandlers
{
    public interface IFileHandler<T> where T : BaseEntity
    {
        void Save(List<T> entities, string filePath);
        List<T> Load(string filePath);
        string GetFileExtension();
    }
}
