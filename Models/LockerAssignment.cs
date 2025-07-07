namespace LockerManagementSystem.Models
{
    public class LockerAssignment
    {
        public int Id { get; set; }

        public string LockerNumber { get; set; }
        public Locker Locker { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int Place { get; set; }
    }
}
