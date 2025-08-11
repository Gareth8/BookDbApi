using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookDbApi.Shared;

namespace BookDbApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected SharedError Error { get; set; }

        public BaseController()
        {
            Error = new SharedError();
        }
    }
}
