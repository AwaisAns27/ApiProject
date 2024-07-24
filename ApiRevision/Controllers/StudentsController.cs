using ApiRevision.Model;
using ApiRevision.Model.Dto;
using Azure.Core.Pipeline;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace ApiRevision.Controllers
{

    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
      private readonly ApplicationDbContext _context;

        #region Constructor
        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAll
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(StudentDto))]
        public IActionResult GetStudents() 
        {
            IEnumerable<StudentDto> studentsDtoList = _context.Students.
                                                       Select( s => new StudentDto { Id= s.Id, GrNo = s.GrNo, Name =s.Name,Stream = s.Stream});
            return Ok(studentsDtoList);
        }
        #endregion

        #region GetById
        [HttpGet("{id:int}",Name ="GetMyId")]
        [ProducesResponseType(StatusCodes.Status404NotFound,Type=typeof(StudentDto))] 
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type =typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDto))]

        public IActionResult GetStudent(int id) 
        {
            if(id == 0)
            {
                return NotFound(); 
            }

            var student = _context.Students.FirstOrDefault(x => x.Id == id) ;
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
        #endregion

        #region POST

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentDto))]
        public IActionResult Create([FromBody]StudentDto studentDto)
        {
            if (studentDto == null) return BadRequest ();
            Student student = new()
            { 
                Name = studentDto.Name, 
                GrNo = studentDto.GrNo,
                Stream = studentDto.Stream,
                DateOfAdmission= DateTime.UtcNow
            
            };
            _context.Students.Add(student);
            _context.SaveChanges();

            studentDto.Id = student.Id;

            return CreatedAtRoute("GetMyId",new {id = student.Id},student);
        }
        #endregion

        #region Update
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Update(int? id,[FromBody] StudentDto studentDto)
        {
            if (id ==null || id == 0 || id != studentDto.Id) return BadRequest ();

            Student? studentInDb = _context.Students.Find(id);

            if (studentInDb == null) return BadRequest ();

            studentInDb.Name= studentDto.Name;
            studentInDb.GrNo= studentDto.GrNo;
            studentInDb.Stream= studentDto.Stream;
            studentInDb.DateOfAdmission= DateTime.UtcNow;

            _context.Students.Update(studentInDb);
            _context.SaveChanges ();

            return NoContent();

        }


        #endregion

        #region Delete
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int? id)
        {
            if(id == null || id ==0) return BadRequest ();

           var studentInDb = _context.Students.Find(id);

            if (studentInDb == null) return NotFound ();

            _context.Students.Remove(studentInDb);
            _context.SaveChanges();
            return NoContent();

        }
        #endregion

        #region Patch
        [HttpPatch]

        public IActionResult Patch(int id, [FromBody] JsonPatchDocument<StudentDto> jsonPatch)
        {
            if (id == null || jsonPatch == null) return BadRequest ();

            Student? studentInDb = _context.Students.AsNoTracking().SingleOrDefault(e => e.Id ==id);
            if (studentInDb == null) return BadRequest ();
            StudentDto studentDto = new() 
            {
                Id = studentInDb.Id,
                Name = studentInDb.Name,
                GrNo = studentInDb.GrNo,
                Stream = studentInDb.Stream
            };


            jsonPatch.ApplyTo(studentDto);
            Student student = new()
            {
                Id = studentDto.Id,
                Name = studentDto.Name,
                GrNo = studentDto.GrNo,
                Stream = studentDto.Stream
            };
            _context.Students.Update(student);
            _context.SaveChanges();
            return NoContent();
        }

        #endregion
    }
}
