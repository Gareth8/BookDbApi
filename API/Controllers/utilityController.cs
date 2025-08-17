using Microsoft.AspNetCore.Mvc;
using BookDbApi.DataAccess;

namespace BookDbApi.Controllers
{
    public class utilityController : BaseController
    {
        private readonly CRUD crud = new CRUD();

        [HttpGet]
        [ActionName("alive")]
        public IActionResult IsRunning()
        {
            return Ok("Book API is running");
        }
    }
}
