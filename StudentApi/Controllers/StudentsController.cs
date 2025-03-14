using Microsoft.AspNetCore.Mvc;

namespace StudentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private static List<Student> students = new List<Student>();
        private static int nextId = 1;

        [HttpGet("Получение списка всех студентов.")]
        public IActionResult GetAll()
        {
            return Ok(students);
        }

        [HttpPost("Добавление нового студента в список")]
        public IActionResult Add([FromBody] Student student)
        {
            student.Id = nextId++;
            students.Add(student);
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }

        [HttpGet("Получение данных по id")]
        public IActionResult GetById(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);
            return student != null ? Ok(student) : NotFound();
        }

        [HttpPut("Обновление данных студента по его ID")]
        public IActionResult Update(int id, [FromBody] Student updatedStudent)
        {
            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null) return NotFound();

            student.Name = updatedStudent.Name;
            student.Age = updatedStudent.Age;
            return NoContent();
        }

        [HttpDelete("Удаление студента по его ID")]
        public IActionResult Delete(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null) return NotFound();

            students.Remove(student);
            return NoContent();
        }
    }
}