using Microsoft.AspNetCore.Authorization;

namespace GOtica.API.Attributes;

public class OpticalStoreMemberAttribute : AuthorizeAttribute
{
    public OpticalStoreMemberAttribute()
    {
        Policy = "OpticalStoreMember";
    }
}
