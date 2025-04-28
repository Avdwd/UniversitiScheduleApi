using Microsoft.AspNetCore.Mvc;
using UniSchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.Core.Models;
using UniversitiScheduleApi.Contracts.Request;
using UniversitiScheduleApi.Contracts.Response;

namespace UniversitiScheduleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubjectController: ControllerBase
    {
        public readonly ISubjectService _subjectService;
        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SubjectResponse>>> GetAllSubjects()
        {
            var subjects = await _subjectService.GetAllSubjects();
            var subjectResponses = subjects.Select(s => new SubjectResponse(s.Id, s.Name));
            return Ok(subjectResponses);
        }
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateSubject([FromBody] SubjectRequest subjectRequest)
        {
            var (subject, error) = Subject.Create(
                Guid.NewGuid(),
                subjectRequest.Name);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }
            var subjectId = await _subjectService.CreateSubject(subject);
            return Ok(subjectId);
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateSubject(Guid id, [FromBody] SubjectRequest subjectRequest)
        {
            var subjectId = await _subjectService.UpdateSubject(id, subjectRequest.Name);
            return Ok(subjectId);
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteSubject(Guid id)
        {
            var subjectId = await _subjectService.DeleteSubject(id);
            return Ok(subjectId);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SubjectResponse>> GetSubjectById(Guid id)
        {
            var subject = await _subjectService.GetSubjectById(id);
            if (subject == null)
            {
                return NotFound();
            }
            var subjectResponse = new SubjectResponse(subject.Id, subject.Name);
            return Ok(subjectResponse);
        }
        [HttpGet("name/{name}")]
        public async Task<ActionResult<SubjectResponse>> GetSubjectByName(string name)
        {
            var subject = await _subjectService.GetSubjectByName(name);
            if (subject == null)
            {
                return NotFound();
            }
            var subjectResponse = new SubjectResponse(subject.Id, subject.Name);
            return Ok(subjectResponse);
        }
        [HttpGet("page/{pageNumber:int}/{pageSize:int}")]
        public async Task<ActionResult<List<SubjectResponse>>> GetSubjects(int pageNumber, int pageSize)
        {
            var subjects = await _subjectService.GetSubjects(pageNumber, pageSize);
            var subjectResponses = subjects.Select(s => new SubjectResponse(s.Id, s.Name));
            return Ok(subjectResponses);
        }
       

    }
}
