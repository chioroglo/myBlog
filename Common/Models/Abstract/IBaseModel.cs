namespace Common.Models.Abstract
{
    public interface IBaseModel
    {
        public int Id { get; set; }

        public string RegistrationDate { get; set; }
    }
}