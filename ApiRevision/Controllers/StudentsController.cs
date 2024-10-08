﻿using ApiRevision.Model;
using ApiRevision.Model.Dto;
using Azure.Core.Pipeline;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Net;

namespace ApiRevision.Controllers
{

    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StudentsController> _logger;
        private readonly ApiResponse _apiResponse;


        #region Constructor
        public StudentsController(ApplicationDbContext context, ILogger<StudentsController> logger)
        {
            _context = context;
            _logger = logger;
            _apiResponse = new ApiResponse();
        }
        #endregion

        #region GetAll
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(StudentDto))]
        public async Task <ApiResponse> GetStudents() 
        {

            try
            {
                IEnumerable<StudentDto> studentsDtoList = await _context.Students.
                                                  Select(s => new StudentDto { Id = s.Id, GrNo = s.GrNo, Name = s.Name, Stream = s.Stream }).ToListAsync();
                _logger.LogDebug("GetAll is been executed");
                _apiResponse.Result = studentsDtoList;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Message = "No Error - Code is Executed Successfully"; 

            }
            catch (Exception)
            {
                _apiResponse.StatusCode=HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess= false;
                _apiResponse.Message = "Error Occured";
            }
            return _apiResponse;

            
        }
        #endregion

        #region GetById
        [HttpGet("{id:int}",Name ="GetMyId")]
        [ProducesResponseType(StatusCodes.Status404NotFound,Type=typeof(StudentDto))] 
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type =typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDto))]

        public async Task<ApiResponse> GetStudent(int id) 
        {
                if (id == 0) 
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.IsSuccess = false;
                    _apiResponse.Message = "Id is Zero Pls enter valid No.";
                    return _apiResponse;
                }
            var studentInDb =await _context.Students.FindAsync(id);

            if (studentInDb == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.IsSuccess = false;
                _apiResponse.Message = "There is no data corresponding to the Id you Entered";
                return _apiResponse;
            }
            else 
            {
                var student = await _context.Students.FirstOrDefaultAsync(x => x.Id == id);
                StudentDto studentDto = new()
                {
                    Id = student.Id,
                    GrNo = student.GrNo,
                    Name = student.Name,
                    Stream = student.Stream
                };
                _apiResponse.Result = studentDto;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Message = "No Error - Code is Executed Successfully";

                return _apiResponse;
            }
        }
        #endregion

        #region POST

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDto))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentDto))]
        public async Task<IActionResult> Create([FromBody]StudentDto studentDto)
        {
            if (studentDto == null) return BadRequest ();
            Student student = new()
            { 
                Name = studentDto.Name, 
                GrNo = studentDto.GrNo,
                Stream = studentDto.Stream,
                DateOfAdmission= DateTime.UtcNow
            
            };
            _context.Students.AddAsync(student);
            _context.SaveChangesAsync();

            studentDto.Id = student.Id;

            return CreatedAtRoute("GetMyId",new {id = student.Id},student);
        }
        #endregion

        #region Update
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int? id,[FromBody] StudentDto studentDto)
        {
            if (id ==null || id == 0 || id != studentDto.Id) return BadRequest ();

            Student? studentInDb =await _context.Students.FindAsync(id);

            if (studentInDb == null) return BadRequest ();

            studentInDb.Name= studentDto.Name;
            studentInDb.GrNo= studentDto.GrNo;
            studentInDb.Stream= studentDto.Stream;
            studentInDb.DateOfAdmission= DateTime.UtcNow;

            _context.Students.Update(studentInDb);
            _context.SaveChangesAsync();

            return NoContent();

        }


        #endregion

        #region Delete
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ApiResponse> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.IsSuccess = false;
                    _apiResponse.Message = "Id is Zero Pls enter valid No.";
                    return _apiResponse;
                }
            };

           var studentInDb = await _context.Students.FindAsync(id);

            if (studentInDb == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.IsSuccess = false;
                _apiResponse.Message = "There is no data corresponding to the Id you Entered";
                return _apiResponse;
            }

            else 
            {
                _context.Students.Remove(studentInDb);
                _context.SaveChangesAsync();
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Message = "Data is Deleted Successfully";
                return _apiResponse;
            }
        }
        #endregion

        #region Patch
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<StudentDto> jsonPatch)
        {
            if (id == null || jsonPatch == null) return BadRequest ();

            Student? studentInDb = await _context.Students.AsNoTracking().SingleOrDefaultAsync(e => e.Id ==id);
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
            _context.SaveChangesAsync();
            return NoContent();
        }

        #endregion
    }
}
