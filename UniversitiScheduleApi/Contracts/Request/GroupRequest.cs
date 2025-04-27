using UNISchedule.Core.Models;

namespace UniversitiScheduleApi.Contracts.Request
{
    public record GroupRequest(
        string Name,
        Institute Institute
    );
}
