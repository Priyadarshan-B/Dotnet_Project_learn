using Microsoft.AspNetCore.Mvc;

namespace sample_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public String GetStudentName()
        {
            return "Priyan..";
        }
    }
}
