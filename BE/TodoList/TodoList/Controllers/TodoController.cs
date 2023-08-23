using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.Models;

namespace TodoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly DataContext _context;

        public TodoController(DataContext context)
        {
            _context = context;
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Todos todos)
        {
            if (todos == null)
            {
                return BadRequest("Mising inputs");
            }
            var existing = await _context.Todo!.FirstOrDefaultAsync(b => b.content == todos.content);
            if (existing != null)
            {
                return BadRequest("Content already exists");
            }
            _context.Todo?.Add(todos);
            await _context.SaveChangesAsync(); ;
            return Ok(new
            {
                success = true,
                create = todos
            });
        }
        /*Updated*/
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Updated(int id, [FromBody] Todos todos)
        {
            if (todos == null)
            {
                return BadRequest("Mising input");
            }
            var existing = await _context.Todo!.FindAsync(id);

            if (existing == null)
            {
                return NotFound("No found");
            }
            var todoTitle = await _context.Todo.FirstOrDefaultAsync(b => b.content == todos.content && b.Id != id);
            if (todoTitle != null)
            {
                return BadRequest("Content with the same title already exists");
            }
            existing!.content = todos.content;
            await _context.SaveChangesAsync();
            return Ok(new
            {
                success = true,
                updated = existing
            });
        }
        //Update status
        [HttpPut("updateStatus/{id}")]
        public IActionResult UpdateTodoStatus(int id)
        {
            var todo = _context.Todo!.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }
            todo.status = !todo.status;
            _context.SaveChanges();
            return Ok(new
            {
                success = true,
                data = todo
            });
        }
        /*Delete */
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var todo = await _context.Todo!.FindAsync(id);
            if (todo == null)
            {
                return NotFound("no found");
            }
            _context.Todo.Remove(todo);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                succuess = true,
                delete = todo
            });
        }
        /*Get all*/
        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10, string? search = "")
        {
            IQueryable<Todos> query = _context.Todo!.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(todo => todo.content!.Contains(search));
            }
            int totalCount = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            int skip = (page - 1) * pageSize;
            List<Todos> todos = query.Skip(skip).Take(pageSize).ToList();
            return Ok(new
            {
                success = true,
                data = todos
            });
        }
        /*Get by id*/
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetTodoById(int id)
        {
            var todos = await _context.Todo!.FindAsync(id);

            if (todos == null)
            {
                return NotFound("No found");
            }

            return Ok(new
            {
                success = true,
                data = todos
            });
        }

       
    }
}
