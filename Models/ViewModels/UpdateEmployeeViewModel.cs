namespace ProjectManagement.Models.ViewModels
{
    public class UpdateEmployeeViewModel
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public long PhoneNo { get; set; }
        public string Position { get; set; }
        public DateTime JoiningDate { get; set; }
    }
}
