namespace Domain.Models.Abstract
{
    public interface IBaseModel
    {
        public int UserId { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}