using System;
using TravelSystem.Models.Interfaces;

namespace TravelSystem.Models.Entities
{
    public abstract class BaseEntity : IValidatable
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        protected BaseEntity(string id)
        {
            Id = id;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public virtual bool IsValid()
        {
            return !string.IsNullOrEmpty(Id);
        }

        public virtual string GetValidationErrors()
        {
            if (string.IsNullOrEmpty(Id))
                return "ID cannot be null or empty";
            
            return string.Empty;
        }
    }
}
