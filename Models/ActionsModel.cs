namespace EmployeeCRUD.Models
{
    public class ActionsModel
    {
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateUpdated { get; set; }

        public string? UpdateBy { get; set; }
        public string? CreatedBy { get; set; }
    }
}
