using NetFoodia.Shared.CommonResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Services_Abstraction
{
    public interface IAdminCharityService
    {
        Task<Result> VerifyCharityAsync(int charityId);
        Task<Result> ActivateCharityAsync(int charityId);
    }
}
