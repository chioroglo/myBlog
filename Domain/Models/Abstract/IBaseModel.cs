namespace Domain.Models.Abstract
{
    public interface IBaseModel
    {
        public int Id { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}