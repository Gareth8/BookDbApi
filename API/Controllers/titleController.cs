using Microsoft.AspNetCore.Mvc;
using BookDbApi.DataAccess;

namespace BookDbApi.Controllers
{
    public class titleController : BaseController
    {
        private readonly CRUD crud = new CRUD();
        
        [HttpGet]
        [Route("{title}")]
        public async Task<IActionResult> BookExists(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return Problem(
                    title: "No title provided.",
                    detail: "An empty title was provided. Please provide a book's title.",
                    statusCode: StatusCodes.Status422UnprocessableEntity,
                    instance: HttpContext.Request.Path
                );
            }
            
            bool result = await crud.BookExistsTitle(title);

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