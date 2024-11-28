namespace EmployeeCRUD.Models
{
    public class PositionsModel : ActionsModel
    {
        public int Id { get; set; }
        public required string PositionName { get; set; }
        public int DepartmentId { get; set; }
        public virtual DepartmentModel? Department { get; set; }
    }
}
