using Microsoft.AspNetCore.Mvc;
using BookDbApi.DataAccess;

namespace BookDbApi.Controllers
{
    public class BookController : BaseController
    {
        private readonly CRUD crud = new CRUD();

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Book API is running");
        }

        [HttpGet]
        [Route("{title}")]
        public async Task<IActionResult> BookExists(string title)
        {
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

        [HttpPost]
        [ActionName("isbn/{ISBN}")]
        public async Task<IActionResult> AddBook(string ISBN)
        {
            Book book = await OpenLibraryAccess.GetBook(ISBN);

            bool addSuccess = await crud.Create(book);

            if (addSuccess)
            {
                return Ok($"Book with ISBN '{ISBN}' created.");
            }

            return NotFound($"Book with ISBN '{ISBN}' not found.");
        }

        [HttpGet]
        [ActionName("isbn")]
        public async Task<IActionResult> GetBookByIsbn([FromQuery] string? ISBN)
        {
            if (string.IsNullOrEmpty(ISBN))
            {
                return Problem(
                    title: "No ISBN provided.",
                    detail: "An empty ISBN was provided. Please provide a valid ISBN 10 or 13 number.",
                    statusCode: StatusCodes.Status422UnprocessableEntity,
                    instance: HttpContext.Request.Path
                );
            }

            try
            {
                Book book = await crud.GetBookIsbn(ISBN);

                return Ok(book.ToString());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
