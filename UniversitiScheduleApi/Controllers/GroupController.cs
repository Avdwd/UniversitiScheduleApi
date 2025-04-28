using Microsoft.AspNetCore.Mvc;
using UniSchedule.Core.Interfaces.ServiceInterfaces;
using UNISchedule.Core.Models;
using UniversitiScheduleApi.Contracts.Request;
using UniversitiScheduleApi.Contracts.Response;

namespace UniversitiScheduleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }
        [HttpGet]
        public async Task<ActionResult<List<GroupResponse>>> GetAllGroups()
        {
            var groups = await _groupService.GetAllGroups();
            var groupResponses = groups.Select(g => new GroupResponse(g.Id, g.Name, g.Institute));
            return Ok(groupResponses);
        }
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateGroup([FromBody] GroupRequest groupRequest)
        {
            var (group, error) = Group.Create(
                Guid.NewGuid(),
                groupRequest.Name,
                groupRequest.Institute);
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }
            var groupId = await _groupService.CreateGroup(group);
            return Ok(groupId);
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateGroup(Guid id, [FromBody] GroupRequest groupRequest)
        {
            var groupId = await _groupService.UpdateGroup(id, groupRequest.Name, groupRequest.Institute.Id);
            return Ok(groupId);
        }
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteGroup(Guid id)
        {
            var groupId = await _groupService.DeleteGroup(id);
            return Ok(groupId);
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GroupResponse>> GetGroupById(Guid id)
        {
            var group = await _groupService.GetGroupById(id);
            if (group == null)
            {
                return NotFound();
            }
            var groupResponse = new GroupResponse(group.Id, group.Name, group.Institute);
            return Ok(groupResponse);
        }
        [HttpGet("name/{name}")]
        public async Task<ActionResult<GroupResponse>> GetGroupByName(string name)
        {
            var group = await _groupService.GetGroupByName(name);
            if (group == null)
            {
                return NotFound();
            }
            var groupResponse = new GroupResponse(group.Id, group.Name, group.Institute);
            return Ok(groupResponse);
        }
        [HttpGet("institute/{institute:institute}")]
        public async Task<ActionResult<List<GroupResponse>>> GetGroupByInstitute(Institute institute)
        {
            var instituteId = institute;
            var groups = await _groupService.GetGroupByInstitute(instituteId);
            if (groups == null || !groups.Any())
            {
                return NotFound();
            }
            var groupResponses = groups.Select(g => new GroupResponse(g.Id, g.Name, g.Institute));
            return Ok(groupResponses);
        }
        [HttpGet("pagination")]
        public async Task<ActionResult<List<GroupResponse>>> GetGroups(int pageNumber, int pageSize)
        {
            var groups = await _groupService.GetGroups(pageNumber, pageSize);
            if (groups == null || !groups.Any())
            {
                return NotFound();
            }
            var groupResponses = groups.Select(g => new GroupResponse(g.Id, g.Name, g.Institute));
            return Ok(groupResponses);
        }

    }

}   


