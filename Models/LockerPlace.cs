namespace LockerManagementSystem.Models {
    public class LockerPlace {
        public int Id { get; set; }
        public int PlaceIndex { get; set; }
        public int LockerId { get; set; }
        public Locker Locker { get; set; } = null!;
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
