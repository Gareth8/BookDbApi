using Microsoft.AspNetCore.Mvc;
using BookDbApi.DataAccess;

namespace BookDbApi.Controllers
{
    public class isbnController : BaseController
    {
        private readonly CRUD crud = new CRUD();
        
        [HttpGet]
        public async Task<IActionResult> GetBook([FromQuery] string? ISBN)
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
        
        [HttpPost]
        public async Task<IActionResult> AddBook([FromRoute] string ISBN)
        {
            Book book = await OpenLibraryAccess.GetBook(ISBN);

            bool addSuccess = await crud.Create(book);

            if (addSuccess)
            {
                return Ok($"Book with ISBN '{ISBN}' created.");
            }

            return NotFound($"Book with ISBN '{ISBN}' not found.");
        }
    }
}