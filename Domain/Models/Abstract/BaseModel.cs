using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Abstract
{
    public abstract class BaseModel : IBaseModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }
    }
}