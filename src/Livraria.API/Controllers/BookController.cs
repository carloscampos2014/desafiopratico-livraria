using Livraria.Domain.Contracts.Interfaces.Services;
using Livraria.Domain.Contracts.Models.Data;
using Livraria.Domain.Contracts.Models.Requests;
using Livraria.Domain.Contracts.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Livraria.API.Controllers
{
    [ApiController]
    public class BookController : BaseController
    {
        private readonly IAddBookService _addBookService;
        private readonly IDeleteBookService _deleteBookService;
        private readonly IGetAllBookService _getAllBookService;
        private readonly IGetByIdBookService _getByIdBookService;
        private readonly IUpdateBookService _updateBookService;

        public BookController(
            IAddBookService addBookService, 
            IDeleteBookService deleteBookService,
            IGetAllBookService getAllBookService,
            IGetByIdBookService getByIdBookService,
            IUpdateBookService updateBookService)
        {
            _addBookService = addBookService;
            _deleteBookService = deleteBookService;
            _getAllBookService = getAllBookService;
            _getByIdBookService = getByIdBookService;
            _updateBookService = updateBookService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status401Unauthorized)]
        public IActionResult GetAll()
        {
            if (GetCustomKey().ToLower() != "pam@102030")
            {
                return Unauthorized(new BookResponse()
                {
                    Success = false,
                    ErrorMessage = "A Chave e invalida."
                });
            }

            var response = _getAllBookService.Execute();

            return !response.Success? BadRequest(response) : Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status401Unauthorized)]
        public IActionResult GetById([FromRoute] Guid id)
        {
            if (GetCustomKey().ToLower() != "pam@102030")
            {
                return Unauthorized(new BookResponse()
                {
                    Success = false,
                    ErrorMessage = "A Chave e invalida."
                });
            }

            var response = _getByIdBookService.Execute(id);

            return !response.Success ? BadRequest(response) : Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status401Unauthorized)]
        public IActionResult Add([FromBody] BookRequest model)
        {
            if (GetCustomKey().ToLower() != "pam@102030")
            {
                return Unauthorized(new BookResponse()
                {
                    Success = false,
                    ErrorMessage = "A Chave e invalida."
                });
            }

            var response = _addBookService.Execute(model);

            return !response.Success ? BadRequest(response) : Created("/add", response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status401Unauthorized)]
        public IActionResult Update([FromRoute] Guid id, [FromBody] BookRequest model)
        {
            if (GetCustomKey().ToLower() != "pam@102030")
            {
                return Unauthorized(new BookResponse()
                {
                    Success = false,
                    ErrorMessage = "A Chave e invalida."
                });
            }

            var response = _updateBookService.Execute(id, model);

            return !response.Success ? BadRequest(response) : Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BookResponse), StatusCodes.Status401Unauthorized)]
        public IActionResult Delete([FromRoute] Guid id)
        {
            if (GetCustomKey().ToLower() != "pam@102030")
            {
                return Unauthorized(new BookResponse()
                {
                    Success = false,
                    ErrorMessage = "A Chave e invalida."
                });
            }

            var response = _deleteBookService.Execute(id);

            return !response.Success ? BadRequest(response) : Ok(response);
        }
    }
}
