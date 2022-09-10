using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Abstract
{
    public abstract class BaseModel : IBaseModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }
    }
}