using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Shared.CommonResult;

namespace NetFoodia.Services_Abstraction
{
    public interface IAssignmentAttemptService
    {
        Task<Result> ExpirePendingOffersAsync();
    }
}
