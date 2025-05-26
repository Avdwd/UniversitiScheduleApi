using Microsoft.AspNetCore.Mvc;
using UniSchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.Core.Models;
using UniversitiScheduleApi.Contracts.Request;
using UniversitiScheduleApi.Contracts.Response;

namespace UniversitiScheduleApi.Controllers
{
    [ApiController]
    [Route("[controller]")] 
    public class ScheduleRecordController: ControllerBase
    {
        public readonly IScheduleRecordService _scheduleRecordService;
        public ScheduleRecordController(IScheduleRecordService scheduleRecordService)
        {
            _scheduleRecordService = scheduleRecordService;
        }
        // GET: /ScheduleRecord
        [HttpGet]
        public async Task<ActionResult<List<ScheduleRecordResponse>>> GetAllScheduleRecords()
        {
            var scheduleRecords = await _scheduleRecordService.GetAllScheduleRecords();
            var scheduleRecordResponses = scheduleRecords.Select(c => new ScheduleRecordResponse(c.Id,c.Date,c.AdditionalData, c.ClassTime, c.Classroom));
            return Ok(scheduleRecordResponses);
        }
        // POST: /ScheduleRecord
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateScheduleRecord([FromBody] ScheduleRecordRequest scheduleRecordRequest)
        {
            var (scheduleRecord, error) = ScheduleRecord.Create(
                Guid.NewGuid(),
                scheduleRecordRequest.Date,
                scheduleRecordRequest.AdditionalData,
                scheduleRecordRequest.ClassTime,
                scheduleRecordRequest.Classroom
                );
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }
            var scheduleRecordId = await _scheduleRecordService.CreateScheduleRecord(scheduleRecord);
            return Ok(scheduleRecordId);
        }
        // PUT: /ScheduleRecord/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateScheduleRecord(Guid id, [FromBody] ScheduleRecordRequest scheduleRecordRequest)
        {
            var scheduleRecordId = await _scheduleRecordService.UpdateScheduleRecord(id, scheduleRecordRequest.Date, scheduleRecordRequest.AdditionalData , scheduleRecordRequest.ClassTime.Id,  scheduleRecordRequest.Classroom.Id);
            return Ok(scheduleRecordId);
        }
        // DELETE: /ScheduleRecord/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteScheduleRecord(Guid id)
        {
            var scheduleRecordId = await _scheduleRecordService.DeleteScheduleRecord(id);
            return Ok(scheduleRecordId);
        }
        // GET: /ScheduleRecord/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ScheduleRecordResponse>> GetScheduleRecordById(Guid id)
        {
            var scheduleRecords = await _scheduleRecordService.GetScheduleRecordById(id);
            if (scheduleRecords == null)
            {
                return NotFound();
            }
            var response = new ScheduleRecordResponse(scheduleRecords.Id, scheduleRecords.Date, scheduleRecords.AdditionalData, scheduleRecords.ClassTime, scheduleRecords.Classroom);
            return Ok(response);
        }
        // GET: /ScheduleRecord/classroom/{classroomId:guid}
        [HttpGet("classroom/{classroomId:guid}")]
        public async Task<ActionResult<ScheduleRecordResponse>> GetScheduleRecordByClassroom(Classroom classroom)
        {
            var scheduleRecords = await _scheduleRecordService.GetScheduleRecordByClassroom(classroom);
            if (scheduleRecords == null)
            {
                return NotFound();
            }
            var response = new ScheduleRecordResponse(scheduleRecords.Id, scheduleRecords.Date, scheduleRecords.AdditionalData, scheduleRecords.ClassTime, scheduleRecords.Classroom);
            return Ok(response);
        }
        // GET: /ScheduleRecord/classTime/{classTimeId:guid}
        [HttpGet("classTime/{classTimeId:guid}")]
        public async Task<ActionResult<ScheduleRecordResponse>> GetScheduleRecordByClassTime(ClassTime classTime)
        {
            var scheduleRecords = await _scheduleRecordService.GetScheduleRecordByClassTime(classTime);
            if (scheduleRecords == null)
            {
                return NotFound();
            }
            var response = new ScheduleRecordResponse(scheduleRecords.Id, scheduleRecords.Date, scheduleRecords.AdditionalData, scheduleRecords.ClassTime, scheduleRecords.Classroom);
            return Ok(response);
        }
        // GET: /ScheduleRecord/date/{date:DateOnly}
        [HttpGet("date/{date}")]
        public async Task<ActionResult<ScheduleRecordResponse>> GetScheduleRecordByDate(string date)
        {
            if (!DateOnly.TryParse(date, out var parsedDate))
            {
                return BadRequest("Invalid date format. Expected format: yyyy-MM-dd");
            }

            var scheduleRecords = await _scheduleRecordService.GetScheduleRecordByDate(parsedDate);
            if (scheduleRecords == null)
            {
                return NotFound();
            }

            var response = new ScheduleRecordResponse(
                scheduleRecords.Id,
                scheduleRecords.Date,
                scheduleRecords.AdditionalData,
                scheduleRecords.ClassTime,
                scheduleRecords.Classroom
            );

            return Ok(response);
        }
        // GET: /ScheduleRecord/page/{pageNumber:int}/{pageSize:int}
        [HttpGet("page/{pageNumber:int}/{pageSize:int}")]// pagination 
        public async Task<ActionResult<List<ScheduleRecordResponse>>> GetScheduleRecords(int pageNumber, int pageSize)
        {
            var scheduleRecords = await _scheduleRecordService.GetScheduleRecords(pageNumber, pageSize);
            var scheduleRecordResponses = scheduleRecords.Select(c => new ScheduleRecordResponse(c.Id, c.Date, c.AdditionalData, c.ClassTime, c.Classroom));
            return Ok(scheduleRecordResponses);
        }
    }
}
