using ApiRevision.Model;
using Microsoft.AspNetCore.Mvc;

namespace ApiRevision.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : Controller
    {
      private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetStudents() 
        {
            IEnumerable<Student> students = _context.Students;
            return Ok(students);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound,Type=typeof(Student))] 
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type =typeof(Student))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]

        public IActionResult GetStudent(int id) 
        {
            if(id == 0)
            {
                return NotFound(); 
            }

            var student = _context.Students.FirstOrDefault(x => x.Id == id);
            if (student == null)
            {
                return BadRequest ();
            }
            return Ok(student);
        }
    }
}
