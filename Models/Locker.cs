namespace LockerManagementSystem.Models
{
    public class Locker
    {
        public string Number { get; set; } = "";
        public int PlaceCount { get; set; }
        public string Type { get; set; } = "";

        public ICollection<LockerAssignment> Assignments { get; set; }
    }
}
