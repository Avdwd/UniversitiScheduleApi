using UniversitiScheduleApi.Contracts.Request.RefRequests;

namespace UniversitiScheduleApi.Contracts.Request
{
    public record GroupRequest(
        string Name,
        InstituteRefRequest Institute
    );
}
