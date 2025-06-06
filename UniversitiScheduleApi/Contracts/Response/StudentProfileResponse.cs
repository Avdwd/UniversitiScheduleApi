﻿namespace UniversitiScheduleApi.Contracts.Response
{
    public record StudentProfileResponse(
        string Id,
        string FirstName,
        string LastName,
        string MiddleName,
        Guid InstituteId,
        Guid GroupId
    );
    
}
