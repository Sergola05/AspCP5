namespace BookingApi
{
    public class Resource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } 
    }

    public class Booking
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
