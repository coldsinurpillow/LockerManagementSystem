namespace LockerManagementSystem.Models {
    public class Locker {
        public int Id { get; set; }
        public string Number { get; set; } = "";
        public int PlaceCount { get; set; }
        public string Type { get; set; } = "";

        public ICollection<LockerPlace> Places { get; set; } = new List<LockerPlace>();
    }
}
