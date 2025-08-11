using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookDbApi.DataAccess;

namespace BookDbApi.Controllers
{
    public class BookController : BaseController
    {
        CRUD crud = new CRUD();

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Book API is running");
        }

        [HttpGet]
        [Route("api/book/{title}")]
        public async Task<IActionResult> BookExists(string title)
        {
            bool result = await crud.BookExists(title);

            if (result)
            {
                return Ok($"Book with title '{title}' exists.");
            }
            else
            {
                return NotFound($"Book with title '{title}' not found.");
            }
        }
    }
}
