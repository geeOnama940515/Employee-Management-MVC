namespace EmployeeCRUD.Models
{
    public class DepartmentModel : ActionsModel
    {
        public int Id { get; set; }
        public required string DepartmentName { get; set; }
        public string? DepartmentHead {get;set;}

        public virtual ICollection<PositionsModel>? Positions { get; set; } = new List<PositionsModel>();
    }
}
