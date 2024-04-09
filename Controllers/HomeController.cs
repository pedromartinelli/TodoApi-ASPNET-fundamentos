using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("/")]
        public IActionResult Get([FromServices] AppDbContext context)
            => Ok(context.Todos.ToList());

        [HttpGet("/{id:int}")]
        public IActionResult GetOne([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var todo = context.Todos.FirstOrDefault(x => x.Id == id);

            if (todo == null)
                return NotFound();

            return Ok(todo);
        }

        [HttpPost("/")]
        public IActionResult Post([FromBody] TodoModel todo, [FromServices] AppDbContext context)
        {
            context.Todos.Add(todo);
            context.SaveChanges();

            return Created($"/{todo.Id}", todo);
        }


        [HttpPut("/{id:int}")]
        public IActionResult Update([FromRoute] int id, [FromBody] TodoModel todo, [FromServices] AppDbContext context)
        {
            var todoToUpdate = context.Todos.FirstOrDefault(x => x.Id == id);
            if (todoToUpdate == null) return NotFound();

            todoToUpdate.Title = todo.Title;
            todoToUpdate.Done = todo.Done;

            context.Todos.Update(todoToUpdate);
            context.SaveChanges();

            return Ok(todoToUpdate);
        }

        [HttpDelete("/{id:int}")]
        public IActionResult Delete([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var todo = context.Todos.FirstOrDefault(x => x.Id == id);

            if (todo == null) return NotFound();
            context.Todos.Remove(todo);
            context.SaveChanges();

            return Ok(todo);
        }
    }
}
