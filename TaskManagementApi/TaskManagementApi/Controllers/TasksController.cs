using Microsoft.AspNetCore.Mvc;
using TaskManagementApi;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private static List<TaskItem> tasks = new List<TaskItem>();
    private static int nextId = 1;

    [HttpPost("Создание новой задачи и добавление её в список.")]
    public IActionResult Create([FromBody] TaskItem task)
    {
        task.Id = nextId++;
        tasks.Add(task);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpGet("Получение списка всех задач.")]
    public IActionResult GetAll()
    {
        return Ok(tasks);
    }

    [HttpGet("Получение конкретной задачи по её ID")]
    public IActionResult GetById(int id)
    {
        var task = tasks.FirstOrDefault(t => t.Id == id);
        return task != null ? Ok(task) : NotFound();
    }

    [HttpPut("Обновление статуса задачи по её ID")]
    public IActionResult UpdateStatus(int id, [FromBody] string status)
    {
        var task = tasks.FirstOrDefault(t => t.Id == id);
        if (task == null) return NotFound();

        task.Status = status;
        return NoContent();
    }

    [HttpDelete("Удаление задачи по её ID")]
    public IActionResult Delete(int id)
    {
        var task = tasks.FirstOrDefault(t => t.Id == id);
        if (task == null) return NotFound();

        tasks.Remove(task);
        return NoContent();
    }
}
