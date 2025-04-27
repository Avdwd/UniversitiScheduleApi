
using Microsoft.AspNetCore.Mvc;
using UniSchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.Core.Models;
using UniversitiScheduleApi.Contracts.Request;
using UniversitiScheduleApi.Contracts.Response;

namespace UniversitiScheduleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClassTimeController: ControllerBase
    {
        public readonly IClassTimeService _classTimeService;
        public ClassTimeController(IClassTimeService classTimeService)
        {
            _classTimeService = classTimeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ClassTimeResponse>>> GetAllClassTimes()
        {
            var classTimes = await _classTimeService.GetAllClassTimes();
            var classTimeResponses = classTimes.Select(c => new ClassTimeResponse(c.Id, c.Timeframe));
            return Ok(classTimeResponses);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateClassTime([FromBody] ClassTimeRequest classTimeRequest)
        {
            var (classTime, error) = ClassTime.Create(
                Guid.NewGuid(),
                classTimeRequest.TimeFrame);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }
            var classTimeId = await _classTimeService.CreateClassTime(classTime);
            return Ok(classTimeId);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateClassTime(Guid id, [FromBody] ClassTimeRequest classTimeRequest)
        {
            var classTimeId = await _classTimeService.UpdateClassTime(id, classTimeRequest.TimeFrame);
            return Ok(classTimeId);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteClassTime(Guid id)
        {
            var classTimeId = await _classTimeService.DeleteClassTime(id);
            return Ok(classTimeId);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ClassTimeResponse>> GetClassTimeById(Guid id)
        {
            var classTime = await _classTimeService.GetClassTimeById(id);
            if (classTime == null)
            {
                return NotFound();
            }
            var classTimeResponse = new ClassTimeResponse(classTime.Id, classTime.Timeframe);
            return Ok(classTimeResponse);
        }
        [HttpGet("timeframe/{timeFrame}")]
        public async Task<ActionResult<ClassTimeResponse>> GetClassTimeByTimeFrame(string timeFrame)
        {
            var classTime = await _classTimeService.GetClassTimeByTimeFrame(timeFrame);
            if (classTime == null)
            {
                return NotFound();
            }
            var classTimeResponse = new ClassTimeResponse(classTime.Id, classTime.Timeframe);
            return Ok(classTimeResponse);
        }
        [HttpGet("Pagination")]
        public async Task<ActionResult<ClassTimeResponse>> GetClassTimeByTimeFramePagination([FromQuery] string timeFrame)
        {
            var classTime = await _classTimeService.GetClassTimeByTimeFrame(timeFrame);
            if (classTime == null)
            {
                return NotFound();
            }
            var classTimeResponse = new ClassTimeResponse(classTime.Id, classTime.Timeframe);
            return Ok(classTimeResponse);
        }
    }
}
