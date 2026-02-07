using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Presentation.Controllers
{
    public class VolunteersController : ApiBaseController
    {
        [HttpGet]
        public ActionResult GetAllVolunteers()
        {
            return Ok("Hello Metaaa");
        }
    }
}
