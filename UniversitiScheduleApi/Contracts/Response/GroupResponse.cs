using UNISchedule.Core.Models;

namespace UniversitiScheduleApi.Contracts.Response
{
    public record GroupResponse(
        Guid Id,
        string Name,
        Institute Institute
    );
}
