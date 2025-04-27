using Microsoft.AspNetCore.Mvc;
using UniSchedule.Applications.Services;
using UniSchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.Core.Models;
using UniversitiScheduleApi.Contracts.Request;
using UniversitiScheduleApi.Contracts.Response;

namespace UniversitiScheduleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClassroomController : ControllerBase
    {
        private readonly IClassroomService _classroomService;
        public ClassroomController(IClassroomService classroomService)
        {
            _classroomService = classroomService;
        }
        [HttpGet]
        public async Task<ActionResult<List<ClassroomResponse>>> GetAllClassrooms()
        {
            var classrooms = await _classroomService.GetAllClassrooms();
            var classroomResponses = classrooms.Select(c => new ClassroomResponse(c.Id, c.Number, c.Building));
            return Ok(classroomResponses);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateClassroom([FromBody] ClassroomRequest classroomRequest)
        {
            var (classroom, error) = Classroom.Create(
                Guid.NewGuid(),
                classroomRequest.Number,
                classroomRequest.Building);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var classroomId = await _classroomService.CreateClassroom(classroom);

            return Ok(classroomId);

        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateClassroom(Guid id, [FromBody] ClassroomRequest classroomRequest)
        {
            var classroomId = await _classroomService.UpdateClassroom(id, classroomRequest.Number, classroomRequest.Building);
            return Ok(classroomId);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteClassroom(Guid id)
        {
            var classroomId = await _classroomService.DeleteClassroom(id);
            return Ok(classroomId);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ClassroomResponse>> GetClassroomById(Guid id)
        {
            var classroom = await _classroomService.GetClassroomById(id);
            if (classroom == null)
            {
                return NotFound();
            }
            var classroomResponse = new ClassroomResponse(classroom.Id, classroom.Number, classroom.Building);
            return Ok(classroomResponse);
        }

        [HttpGet("number/{number:int}")]
        public async Task<ActionResult<ClassroomResponse>> GetClassroomByNumber(int number)
        {
            var classroom = await _classroomService.GetClassroomByNumber(number);
            if (classroom == null)
            {
                return NotFound();
            }
            var classroomResponse = new ClassroomResponse(classroom.Id, classroom.Number, classroom.Building);
            return Ok(classroomResponse);
        }

        [HttpGet("building/{building:int}")]
        public async Task<ActionResult<List<ClassroomResponse>>> GetClassroomByBuilding(int building)
        {
            var classrooms = await _classroomService.GetClassroomByBuilding(building);
            if (classrooms == null || !classrooms.Any())
            {
                return NotFound();
            }
            var classroomResponses = classrooms.Select(c => new ClassroomResponse(c.Id, c.Number, c.Building));
            return Ok(classroomResponses);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<List<ClassroomResponse>>> GetClassrooms(int pageNumber, int pageSize)
        {
            var classrooms = await _classroomService.GetClassrooms(pageNumber, pageSize);
            if (classrooms == null || !classrooms.Any())
            {
                return NotFound();
            }
            var classroomResponses = classrooms.Select(c => new ClassroomResponse(c.Id, c.Number, c.Building));
            return Ok(classroomResponses);
        }


    }
}
