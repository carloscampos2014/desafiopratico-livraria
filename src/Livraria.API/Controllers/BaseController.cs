using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Livraria.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected string GetCustomKey() => Request.Headers["MyKey"].ToString();
    }
}
