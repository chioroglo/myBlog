using System.ComponentModel.DataAnnotations;

namespace Entities.Abstract
{
    public abstract class BaseEntity : IBaseEntity
    {
        [Required]
        [Key]
        public int Id { get ; set ; }
        [Required]
        public DateTime RegistrationDate { get ; set; }
    }
}
