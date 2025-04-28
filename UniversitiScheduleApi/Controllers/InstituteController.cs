using Microsoft.AspNetCore.Mvc;
using UniSchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.Core.Models;
using UniversitiScheduleApi.Contracts.Request;
using UniversitiScheduleApi.Contracts.Response;

namespace UniversitiScheduleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InstituteController: ControllerBase
    {
        public readonly IInstituteService _instituteService;
        public InstituteController(IInstituteService instituteService)
        {
            _instituteService = instituteService;
        }

        [HttpGet]
        public async Task<ActionResult<List<InstituteResponse>>> GetAllInstitutes()
        {
            var institutes = await _instituteService.GetAllInstitutes();
            var instituteResponses = institutes.Select(i => new InstituteResponse(i.Id, i.Name));
            return Ok(instituteResponses);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateInstitute([FromBody] InstituteRequest instituteRequest)
        {
            var (institute, error) = Institute.Create(
                Guid.NewGuid(),
                instituteRequest.Name);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }
            var instituteId = await _instituteService.CreateInstitute(institute);
            return Ok(instituteId);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateInstitute(Guid id, [FromBody] InstituteRequest instituteRequest)
        {
            var instituteId = await _instituteService.UpdateInstitute(id, instituteRequest.Name);
            return Ok(instituteId);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteInstitute(Guid id)
        {
            var instituteId = await _instituteService.DeleteInstitute(id);
            return Ok(instituteId);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<InstituteResponse>> GetInstituteById(Guid id)
        {
            var institute = await _instituteService.GetInstituteById(id);
            if (institute == null)
            {
                return NotFound();
            }
            var instituteResponse = new InstituteResponse(institute.Id, institute.Name);
            return Ok(instituteResponse);
        }
        [HttpGet("name/{name}")]
        public async Task<ActionResult<InstituteResponse>> GetInstituteByName(string name)
        {
            var institute = await _instituteService.GetInstituteByName(name);
            if (institute == null)
            {
                return NotFound();
            }
            var instituteResponse = new InstituteResponse(institute.Id, institute.Name);
            return Ok(instituteResponse);
        }


    }
}
