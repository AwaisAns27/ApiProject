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
        public ActionResult<IEnumerable<Student>> GetStudents() //In ActionResult It will show Schema
        {
            IEnumerable<Student> students = _context.Students;
            return Ok(students);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound,Type=typeof(Student))] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public IActionResult GetStudent(int id) //While using IActionResult we have to use typeof to display schema
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
