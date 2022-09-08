namespace Domain.Models.Abstract
{
    public abstract class BaseModel : IBaseModel
    {
        public int Id { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}