using Microsoft.AspNetCore.Mvc;
using UniSchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.Core.Models;
using UniversitiScheduleApi.Contracts.Request;
using UniversitiScheduleApi.Contracts.Response;

namespace UniversitiScheduleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubjectAssignmentController: ControllerBase
    {
        public readonly ISubjectAssignmentService _subjectAssignmentService;
        public SubjectAssignmentController(ISubjectAssignmentService subjectAssignmentService)
        {
            _subjectAssignmentService = subjectAssignmentService;
        }
        // GET: /SubjectAssignment
        [HttpGet]
        public async Task<ActionResult<List<SubjectAssignmentResponse>>> GetAllSubjectAssignments()
        {
            var subjectAssignments = await _subjectAssignmentService.GetAllSubjectAssignments();
            var subjectAssignmentResponses = subjectAssignments.Select(c => new SubjectAssignmentResponse(c.Id, c.ScheduleRecord, c.Group, c.Subject, c.TypeSubject));
            return Ok(subjectAssignmentResponses);
        }
        // POST: /SubjectAssignment
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateSubjectAssignment([FromBody] SubjectAssignmentRequest subjectAssignmentRequest)
        {
            var (subjectAssignment, error) = SubjectAssignment.Create(
                Guid.NewGuid(),
                subjectAssignmentRequest.ScheduleRecord,
                subjectAssignmentRequest.Group,
                subjectAssignmentRequest.Subject,
                subjectAssignmentRequest.TypeSubject
            );
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }
            var subjectAssignmentId = await _subjectAssignmentService.CreateSubjectAssignment(subjectAssignment);
            return Ok(subjectAssignmentId);
        }
        // PUT: /SubjectAssignment/{id}

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateSubjectAssignment(Guid id, [FromBody] SubjectAssignmentRequest subjectAssignmentRequest)
        {
            var subjectAssignmentId = await _subjectAssignmentService.UpdateSubjectAssignment(id, subjectAssignmentRequest.ScheduleRecord.Id, subjectAssignmentRequest.Group.Id, subjectAssignmentRequest.Subject.Id, subjectAssignmentRequest.TypeSubject.Id);
            return Ok(subjectAssignmentId);
        }
        // DELETE: /SubjectAssignment/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteSubjectAssignment(Guid id)
        {
            var subjectAssignmentId = await _subjectAssignmentService.DeleteSubjectAssignment(id);
            return Ok(subjectAssignmentId);
        }
        // GET: /SubjectAssignment/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SubjectAssignmentResponse>> GetSubjectAssignmentById(Guid id)
        {
            var subjectAssignment = await _subjectAssignmentService.GetSubjectAssignmentById(id);
            if (subjectAssignment == null)
            {
                return NotFound();
            }
            var subjectAssignmentResponse = new SubjectAssignmentResponse(subjectAssignment.Id, subjectAssignment.ScheduleRecord, subjectAssignment.Group, subjectAssignment.Subject, subjectAssignment.TypeSubject);
            return Ok(subjectAssignmentResponse);
        }
        // GET: /SubjectAssignment/Group/{group:group}
        [HttpGet("Group/{group:group}")]
        public async Task<ActionResult<SubjectAssignmentResponse>> GetSubjectAssignmentByGroup(Group group)
        {
            var subjectAssignment = await _subjectAssignmentService.GetSubjectAssignmentByGroup(group);
            if (subjectAssignment == null)
            {
                return NotFound();
            }
            var subjectAssignmentResponse = new SubjectAssignmentResponse(subjectAssignment.Id, subjectAssignment.ScheduleRecord, subjectAssignment.Group, subjectAssignment.Subject, subjectAssignment.TypeSubject);
            return Ok(subjectAssignmentResponse);
        }
        // GET: /SubjectAssignment/Group/{group:group}/ScheduleRecord/{scheduleRecord:scheduleRecord}
        [HttpGet("Group/{group:group}/ScheduleRecord/{scheduleRecord:scheduleRecord}")]
        public async Task<ActionResult<SubjectAssignmentResponse>> GetSubjectAssignmentByGroupAndScheduleRecord(Group group, ScheduleRecord scheduleRecord)
        {
            var subjectAssignment = await _subjectAssignmentService.GetSubjectAssignmentByGroupAndScheduleRecord(group, scheduleRecord);
            if (subjectAssignment == null)
            {
                return NotFound();
            }
            var subjectAssignmentResponse = new SubjectAssignmentResponse(subjectAssignment.Id, subjectAssignment.ScheduleRecord, subjectAssignment.Group, subjectAssignment.Subject, subjectAssignment.TypeSubject);
            return Ok(subjectAssignmentResponse);
        }
        // GET: /SubjectAssignment/Group/{group:group}/Subject/{subject:subject}
        [HttpGet("Group/{group:group}/Subject/{subject:subject}")]
        public async Task<ActionResult<SubjectAssignmentResponse>> GetSubjectAssignmentByGroupAndSubject(Group group, Subject subject)
        {
            var subjectAssignment = await _subjectAssignmentService.GetSubjectAssignmentByGroupAndSubject(group, subject);
            if (subjectAssignment == null)
            {
                return NotFound();
            }
            var subjectAssignmentResponse = new SubjectAssignmentResponse(subjectAssignment.Id, subjectAssignment.ScheduleRecord, subjectAssignment.Group, subjectAssignment.Subject, subjectAssignment.TypeSubject);
            return Ok(subjectAssignmentResponse);
        }
        // GET: /SubjectAssignment/Group/{group:group}/TypeSubject/{typeSubject:typeSubject}
        [HttpGet("Group/{group:group}/TypeSubject/{typeSubject:typeSubject}")]
        public async Task<ActionResult<SubjectAssignmentResponse>> GetSubjectAssignmentByGroupAndTypeSubject(Group group, TypeSubject typeSubject)
        {
            var subjectAssignment = await _subjectAssignmentService.GetSubjectAssignmentByGroupAndTypeSubject(group, typeSubject);
            if (subjectAssignment == null)
            {
                return NotFound();
            }
            var subjectAssignmentResponse = new SubjectAssignmentResponse(subjectAssignment.Id, subjectAssignment.ScheduleRecord, subjectAssignment.Group, subjectAssignment.Subject, subjectAssignment.TypeSubject);
            return Ok(subjectAssignmentResponse);
        }
        // GET: /SubjectAssignment/ScheduleRecord/{scheduleRecord:scheduleRecord}
        [HttpGet("ScheduleRecord/{scheduleRecord:scheduleRecord}")]
        public async Task<ActionResult<SubjectAssignmentResponse>> GetSubjectAssignmentByScheduleRecord(ScheduleRecord scheduleRecord)
        {
            var subjectAssignment = await _subjectAssignmentService.GetSubjectAssignmentByScheduleRecord(scheduleRecord);
            if (subjectAssignment == null)
            {
                return NotFound();
            }
            var subjectAssignmentResponse = new SubjectAssignmentResponse(subjectAssignment.Id, subjectAssignment.ScheduleRecord, subjectAssignment.Group, subjectAssignment.Subject, subjectAssignment.TypeSubject);
            return Ok(subjectAssignmentResponse);
        }
        // GET: /SubjectAssignment/Subject/{subject:subject}
        [HttpGet("Subject/{subject:subject}")]
        public async Task<ActionResult<SubjectAssignmentResponse>> GetSubjectAssignmentBySubject(Subject subject)
        {
            var subjectAssignment = await _subjectAssignmentService.GetSubjectAssignmentBySubject(subject);
            if (subjectAssignment == null)
            {
                return NotFound();
            }
            var subjectAssignmentResponse = new SubjectAssignmentResponse(subjectAssignment.Id, subjectAssignment.ScheduleRecord, subjectAssignment.Group, subjectAssignment.Subject, subjectAssignment.TypeSubject);
            return Ok(subjectAssignmentResponse);
        }
        // GET: /SubjectAssignment/TypeSubject/{typeSubject:typeSubject}
        [HttpGet("TypeSubject/{typeSubject:typeSubject}")]
        public async Task<ActionResult<SubjectAssignmentResponse>> GetSubjectAssignmentByTypeSubject(TypeSubject typeSubject)
        {
            var subjectAssignment = await _subjectAssignmentService.GetSubjectAssignmentByTypeSubject(typeSubject);
            if (subjectAssignment == null)
            {
                return NotFound();
            }
            var subjectAssignmentResponse = new SubjectAssignmentResponse(subjectAssignment.Id, subjectAssignment.ScheduleRecord, subjectAssignment.Group, subjectAssignment.Subject, subjectAssignment.TypeSubject);
            return Ok(subjectAssignmentResponse);
        }
        // GET: /SubjectAssignment/Page/{pageNumber:int}/Size/{pageSize:int}
        [HttpGet("Page/{pageNumber:int}/Size/{pageSize:int}")]
        public async Task<ActionResult<List<SubjectAssignmentResponse>>> GetSubjectAssignments(int pageNumber, int pageSize)
        {
            var subjectAssignments = await _subjectAssignmentService.GetSubjectAssignments(pageNumber, pageSize);
            var subjectAssignmentResponses = subjectAssignments.Select(c => new SubjectAssignmentResponse(c.Id, c.ScheduleRecord, c.Group, c.Subject, c.TypeSubject));
            return Ok(subjectAssignmentResponses);
        }
    }


}
