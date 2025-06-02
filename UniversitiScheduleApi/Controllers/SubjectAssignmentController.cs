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
        public readonly IGroupService _groupService;
        public readonly IScheduleRecordService _scheduleRecordService;
        public readonly ISubjectService _subjectService;
        public readonly ITypeSubjectService _typeSubjectService;
        private readonly ITeacherProfileService _teacherProfileService;
        public SubjectAssignmentController(
            ITypeSubjectService typeSubjectService,
            ISubjectService subjectService,
            IGroupService groupService, 
            IScheduleRecordService scheduleRecordService,
            ISubjectAssignmentService subjectAssignmentService,
            ITeacherProfileService teacherProfileService)
        {
            _groupService = groupService;
            _scheduleRecordService = scheduleRecordService;
            _subjectAssignmentService = subjectAssignmentService;
            _subjectService = subjectService;
            _typeSubjectService = typeSubjectService;
            _teacherProfileService = teacherProfileService;
        }
        // GET: /SubjectAssignment
        [HttpGet]
        public async Task<ActionResult<List<SubjectAssignmentResponse>>> GetAllSubjectAssignments()
        {
            var subjectAssignments = await _subjectAssignmentService.GetAllSubjectAssignments();
            var subjectAssignmentResponses = subjectAssignments.Select(c => new SubjectAssignmentResponse(c.Id, c.ScheduleRecord, c.Group, c.Subject,c.Teacher ,c.TypeSubject));
            return Ok(subjectAssignmentResponses);
        }
        // POST: /SubjectAssignment
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateSubjectAssignment([FromBody] SubjectAssignmentRequest subjectAssignmentRequest)
        {
            if (subjectAssignmentRequest == null)
            {
                return BadRequest("Invalid subject assignment data.");
            }
            var group = await _groupService.GetGroupById(subjectAssignmentRequest.Group.Id);
            if (group == null)
            {
                return NotFound("Group not found.");
            }
            var scheduleRecord = await _scheduleRecordService.GetScheduleRecordById(subjectAssignmentRequest.ScheduleRecord.Id);
            if (scheduleRecord == null)
            {
                return NotFound("ScheduleRecord not found.");
            }
            var subject = await _subjectService.GetSubjectById(subjectAssignmentRequest.Subject.Id);
            if (subject == null)
            {
                return NotFound("Subject not found.");
            }
            var typeSubject = await _typeSubjectService.GetTypeSubjectById(subjectAssignmentRequest.TypeSubject.Id);
            if (typeSubject == null)
            {
                return NotFound("TypeSubject not  found.");
            }
            

            var (subjectAssignment, error) = SubjectAssignment.Create(
                Guid.NewGuid(),
                scheduleRecord,
                group,
                subject,
                typeSubject,
                subjectAssignmentRequest.TeacherProfile
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
            var subjectAssignmentId = await _subjectAssignmentService.UpdateSubjectAssignment(id, subjectAssignmentRequest.ScheduleRecord.Id, subjectAssignmentRequest.Group.Id, subjectAssignmentRequest.Subject.Id, subjectAssignmentRequest.TypeSubject.Id,subjectAssignmentRequest.TeacherProfile.UserDetails.Id);
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
            var subjectAssignmentResponse = new SubjectAssignmentResponse(subjectAssignment.Id, subjectAssignment.ScheduleRecord, subjectAssignment.Group, subjectAssignment.Subject,subjectAssignment.Teacher, subjectAssignment.TypeSubject);
            return Ok(subjectAssignmentResponse);
        }
        // GET: /SubjectAssignment/Group/{group:group}
        [HttpGet("Group/{groupId:guid}")]
        public async Task<ActionResult<SubjectAssignmentResponse>> GetSubjectAssignmentByGroup(Guid groupId)
        {
            var group = await _groupService.GetGroupById(groupId);
            if (group == null)
            {
                return NotFound();
            }

            var subjectAssignment = await _subjectAssignmentService.GetSubjectAssignmentByGroup(group);
            if (subjectAssignment == null)
            {
                return NotFound();
            }

            var subjectAssignmentResponse = new SubjectAssignmentResponse(subjectAssignment.Id, subjectAssignment.ScheduleRecord, subjectAssignment.Group, subjectAssignment.Subject,subjectAssignment.Teacher, subjectAssignment.TypeSubject);
            return Ok(subjectAssignmentResponse);
        }
        // GET: /SubjectAssignment/Group/{group:group}/ScheduleRecord/{scheduleRecord:scheduleRecord}
        [HttpGet("Group/{groupId:guid}/ScheduleRecord/{scheduleRecordId:guid}")]
        public async Task<ActionResult<SubjectAssignmentResponse>> GetSubjectAssignmentByGroupAndScheduleRecord(
        [FromRoute] Guid groupId,
        [FromRoute] Guid scheduleRecordId)
        {

            var group = await _groupService.GetGroupById(groupId);
            var scheduleRecord = await _scheduleRecordService.GetScheduleRecordById(scheduleRecordId);

            if (group == null || scheduleRecord == null)
            {
                return NotFound("Group or ScheduleRecord not found.");
            }

            
            var subjectAssignment = await _subjectAssignmentService.GetSubjectAssignmentByGroupAndScheduleRecord(group, scheduleRecord);

            if (subjectAssignment == null)
            {
                return NotFound();
            }

            var subjectAssignmentResponse = new SubjectAssignmentResponse(subjectAssignment.Id, subjectAssignment.ScheduleRecord, subjectAssignment.Group, subjectAssignment.Subject,subjectAssignment.Teacher, subjectAssignment.TypeSubject);
            return Ok(subjectAssignmentResponse);
        }
        // GET: /SubjectAssignment/Group/{group:group}/Subject/{subject:subject}
        [HttpGet("Group/{groupId:guid}/Subject/{subjectId:guid}")]
        public async Task<ActionResult<SubjectAssignmentResponse>> GetSubjectAssignmentByGroupAndSubject(Guid groupId, Guid subjectId)
        {
            var group = await _groupService.GetGroupById(groupId);
            var subject = await _subjectService.GetSubjectById(subjectId); 

            if (group == null || subject == null)
            {
                return NotFound("Group or Subject not found.");
            }

            var subjectAssignment = await _subjectAssignmentService.GetSubjectAssignmentByGroupAndSubject(group, subject);
            if (subjectAssignment == null)
            {
                return NotFound();
            }

            var response = new SubjectAssignmentResponse(subjectAssignment.Id, subjectAssignment.ScheduleRecord, subjectAssignment.Group, subjectAssignment.Subject,subjectAssignment.Teacher, subjectAssignment.TypeSubject);
            return Ok(response);
        }
        // GET: /SubjectAssignment/Group/{group:group}/TypeSubject/{typeSubject:typeSubject}
        [HttpGet("Group/{groupId:guid}/TypeSubject/{typeSubjectId:guid}")]
        public async Task<ActionResult<SubjectAssignmentResponse>> GetSubjectAssignmentByGroupAndTypeSubject(Guid groupId, Guid typeSubjectId)
        {
            var group = await _groupService.GetGroupById(groupId);
            var typeSubject = await _typeSubjectService.GetTypeSubjectById(typeSubjectId);  

            if (group == null || typeSubject == null)
            {
                return NotFound("Group or TypeSubject not found.");
            }

            var subjectAssignment = await _subjectAssignmentService.GetSubjectAssignmentByGroupAndTypeSubject(group, typeSubject);
            if (subjectAssignment == null)
            {
                return NotFound();
            }

            var response = new SubjectAssignmentResponse(subjectAssignment.Id, subjectAssignment.ScheduleRecord, subjectAssignment.Group, subjectAssignment.Subject,subjectAssignment.Teacher, subjectAssignment.TypeSubject);
            return Ok(response);
        }
        // GET: /SubjectAssignment/ScheduleRecord/{scheduleRecord:scheduleRecord}
        [HttpGet("ScheduleRecord/{scheduleRecordId:guid}")]
        public async Task<ActionResult<SubjectAssignmentResponse>> GetSubjectAssignmentByScheduleRecord(Guid scheduleRecordId)
        {
            var scheduleRecord = await _scheduleRecordService.GetScheduleRecordById(scheduleRecordId);
            if (scheduleRecord == null)
            {
                return NotFound();
            }

            var subjectAssignment = await _subjectAssignmentService.GetSubjectAssignmentByScheduleRecord(scheduleRecord);
            if (subjectAssignment == null)
            {
                return NotFound();
            }

            var response = new SubjectAssignmentResponse(subjectAssignment.Id, subjectAssignment.ScheduleRecord, subjectAssignment.Group, subjectAssignment.Subject,subjectAssignment.Teacher, subjectAssignment.TypeSubject);
            return Ok(response);
        }
        // GET: /SubjectAssignment/Subject/{subject:subject}
        [HttpGet("Subject/{subjectId:guid}")]
        public async Task<ActionResult<SubjectAssignmentResponse>> GetSubjectAssignmentBySubject(Guid subjectId)
        {
            var subject = await _subjectService.GetSubjectById(subjectId); 
            if (subject == null)
            {
                return NotFound();
            }

            var subjectAssignment = await _subjectAssignmentService.GetSubjectAssignmentBySubject(subject);
            if (subjectAssignment == null)
            {
                return NotFound();
            }

            var response = new SubjectAssignmentResponse(subjectAssignment.Id, subjectAssignment.ScheduleRecord, subjectAssignment.Group, subjectAssignment.Subject,subjectAssignment.Teacher, subjectAssignment.TypeSubject);
            return Ok(response);
        }
        // GET: /SubjectAssignment/TypeSubject/{typeSubject:typeSubject}
        [HttpGet("TypeSubject/{typeSubjectId:guid}")]
        public async Task<ActionResult<SubjectAssignmentResponse>> GetSubjectAssignmentByTypeSubject(Guid typeSubjectId)
        {
            var typeSubject = await _typeSubjectService.GetTypeSubjectById(typeSubjectId); 
            if (typeSubject == null)
            {
                return NotFound();
            }

            var subjectAssignment = await _subjectAssignmentService.GetSubjectAssignmentByTypeSubject(typeSubject);
            if (subjectAssignment == null)
            {
                return NotFound();
            }

            var response = new SubjectAssignmentResponse(subjectAssignment.Id, subjectAssignment.ScheduleRecord, subjectAssignment.Group, subjectAssignment.Subject,subjectAssignment.Teacher, subjectAssignment.TypeSubject);
            return Ok(response);
        }
        // GET: /SubjectAssignment/Page/{pageNumber:int}/Size/{pageSize:int}
        [HttpGet("Page/{pageNumber:int}/Size/{pageSize:int}")]
        public async Task<ActionResult<List<SubjectAssignmentResponse>>> GetSubjectAssignments(int pageNumber, int pageSize)
        {
            var subjectAssignments = await _subjectAssignmentService.GetSubjectAssignments(pageNumber, pageSize);
            var subjectAssignmentResponses = subjectAssignments.Select(c => new SubjectAssignmentResponse(c.Id, c.ScheduleRecord, c.Group, c.Subject, c.Teacher, c.TypeSubject));
            return Ok(subjectAssignmentResponses);
        }
    }


}
