using ApiRevision.Model;
using ApiRevision.Model.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

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
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(StudentDto))]
        public IActionResult GetStudents() 
        {
            IEnumerable<StudentDto> studentsDtoList = _context.Students.
                                                       Select( s => new StudentDto { Id= s.Id, GrNo = s.GrNo, Name =s.Name,Stream = s.Stream});
            return Ok(studentsDtoList);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound,Type=typeof(StudentDto))] 
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type =typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDto))]

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
            StudentDto studentDto = new() 
                                    { 
                                      Id = student.Id,
                                      GrNo = student.GrNo,
                                      Name =student.Name,
                                      Stream =student.Stream
                                     };
            return Ok(studentDto);

           
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDto))]
        public IActionResult Create(StudentDto studentDto)
        {
            Student student = new() { Name = studentDto.Name, GrNo = studentDto.GrNo, Stream = studentDto.Stream ,DateOfAdmission= DateTime.UtcNow};
            _context.Students.Add(student);
            _context.SaveChanges();

            studentDto.Id = student.Id;
            return Ok(studentDto);
        }
    }
}
