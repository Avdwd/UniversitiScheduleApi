using Microsoft.AspNetCore.Mvc;
using UniSchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.Core.Models;
using UniversitiScheduleApi.Contracts.Request;
using UniversitiScheduleApi.Contracts.Response;

namespace UniversitiScheduleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TypeSubjectController: ControllerBase
    {
        public readonly ITypeSubjectService _typeSubjectService;
        public TypeSubjectController(ITypeSubjectService typeSubjectService)
        {
            _typeSubjectService = typeSubjectService;
        }
        [HttpGet]
        public async Task<ActionResult<List<TypeSubjectResponse>>> GetAllTypeSubjects()
        {
            var typeSubjects = await _typeSubjectService.GetAllTypeSubjects();
            var typeSubjectResponses = typeSubjects.Select(s => new TypeSubjectResponse(s.Id, s.Type));
            return Ok(typeSubjectResponses);
        }
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateTypeSubject([FromBody] TypeSubjectRequest typeSubjectRequest)
        {
            var (typeSubject, error) = TypeSubject.Create(
                Guid.NewGuid(),
                typeSubjectRequest.Type);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }
            var typeSubjectId = await _typeSubjectService.CreateTypeSubject(typeSubject);
            return Ok(typeSubjectId);
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateTypeSubject(Guid id, [FromBody] TypeSubjectRequest typeSubjectRequest)
        {
            var typeSubjectId = await _typeSubjectService.UpdateTypeSubject(id, typeSubjectRequest.Type);
            return Ok(typeSubjectId);
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteTypeSubject(Guid id)
        {
            var typeSubjectId = await _typeSubjectService.DeleteTypeSubject(id);
            return Ok(typeSubjectId);
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TypeSubjectResponse>> GetTypeSubjectById(Guid id)
        {
            var typeSubject = await _typeSubjectService.GetTypeSubjectById(id);
            if (typeSubject == null)
            {
                return NotFound();
            }
            var typeSubjectResponse = new TypeSubjectResponse(typeSubject.Id, typeSubject.Type);
            return Ok(typeSubjectResponse);
        }
    }
    
    
}
