namespace LockerManagementSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string MiddleName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Group { get; set; } = "";
        public string BarCode { get; set; } = "";
        public string Iin { get; set; } = "";
        public ICollection<LockerAssignment> Assignments { get; set; }
    }
}
