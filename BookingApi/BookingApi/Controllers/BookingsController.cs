using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private static List<Resource> resources = new List<Resource>
        {
            new Resource { Id = 1, Name = "Hotel Room 1", Type = "Hotel" },
            new Resource { Id = 2, Name = "Flight A1", Type = "Flight" }
        };
        private static List<Booking> bookings = new List<Booking>();
        private static int nextId = 1;

        [HttpPost("Создание нового бронирования и проверка по врмени.")]
        public IActionResult Book([FromBody] Booking booking)
        {
            if (!resources.Any(r => r.Id == booking.ResourceId)) return BadRequest("Invalid resource");

            if (bookings.Any(b => b.ResourceId == booking.ResourceId &&
                ((booking.StartTime >= b.StartTime && booking.StartTime < b.EndTime) ||
                 (booking.EndTime > b.StartTime && booking.EndTime <= b.EndTime))))
                return Conflict("Resource is already booked for this time");

            booking.Id = nextId++;
            bookings.Add(booking);
            return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
        }

        [HttpGet("Получение списка в заданный период времени.")]
        public IActionResult GetAvailable([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] string? type)
        {
            var availableResources = resources
                .Where(r => string.IsNullOrEmpty(type) || r.Type == type)
                .Where(r => !bookings.Any(b => b.ResourceId == r.Id &&
                    ((start >= b.StartTime && start < b.EndTime) ||
                     (end > b.StartTime && end <= b.EndTime))))
                .ToList();
            return Ok(availableResources);
        }

        [HttpGet("Получение конкретного бронирования по его ID.")]
        public IActionResult GetById(int id)
        {
            var booking = bookings.FirstOrDefault(b => b.Id == id);
            return booking != null ? Ok(booking) : NotFound();
        }
        [HttpGet("Получение списка всех бронирований.")]
        public IActionResult GetAll()
        {
            return Ok(bookings);
        }

        [HttpPut("Обновление времени бронирования")]
        public IActionResult Update(int id, [FromBody] Booking updatedBooking)
        {
            var booking = bookings.FirstOrDefault(b => b.Id == id);
            if (booking == null) return NotFound();

            booking.StartTime = updatedBooking.StartTime;
            booking.EndTime = updatedBooking.EndTime;
            return NoContent();
        }

        [HttpDelete("Отмена.")]
        public IActionResult Cancel(int id)
        {
            var booking = bookings.FirstOrDefault(b => b.Id == id);
            if (booking == null) return NotFound();

            bookings.Remove(booking);
            return NoContent();
        }
    }
}
