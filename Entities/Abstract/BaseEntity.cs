using System.ComponentModel.DataAnnotations;

namespace Entities.Abstract
{
    public abstract class BaseEntity : IBaseEntity
    {
        public int Id { get ; set ; }

        public DateTime RegistrationDate { get ; set; }
    }
}
