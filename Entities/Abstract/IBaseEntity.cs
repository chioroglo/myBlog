using System.ComponentModel.DataAnnotations;

namespace Entities.Abstract
{
    public interface IBaseEntity
    {
        public int Id { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
