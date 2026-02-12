using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetFoodia.Services_Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/Admin/Charities")]
    public class AdminCharitiesController : ApiBaseController
    {
        private readonly IAdminCharityService _adminCharityService;

        public AdminCharitiesController(IAdminCharityService adminCharityService)
        {
            _adminCharityService = adminCharityService;
        }

        [HttpPut("{id:int}/Verify")]
        public async Task<IActionResult> Verify(int id)
        {
            var result = await _adminCharityService.VerifyCharityAsync(id);
            return HandleResult(result);
        }

        [HttpPut("{id:int}/Activate")]
        public async Task<IActionResult> Activate(int id)
        {
            var result = await _adminCharityService.ActivateCharityAsync(id);
            return HandleResult(result);
        }
    }
}
