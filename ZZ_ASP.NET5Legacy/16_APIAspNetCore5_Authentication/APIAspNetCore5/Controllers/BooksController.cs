﻿using APIAspNetCore5.Business;
using APIAspNetCore5.Data.VO;
using APIAspNetCore5.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace APIAspNetCore5.Controllers
{

    [ApiVersion("1")]
    [Authorize("Bearer")]
    [ApiController]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class BooksController : Controller
    {
        //Declaração do serviço usado
        private IBookBusiness _bookBusiness;

        /* Injeção de uma instancia de IBookBusiness ao criar
        uma instancia de BookController */
        public BooksController(IBookBusiness bookBusiness)
        {
            _bookBusiness = bookBusiness;
        }

        // Configura o Swagger para a operação
        // http://localhost:{porta}/api/books/v1/
        // [ProducesResponseType((202), Type = typeof(List<Book>))]
        // determina o objeto de retorno em caso de sucesso List<Book>
        // O [ProducesResponseType(XYZ)] define os códigos de retorno 204, 400 e 401
        [HttpGet]
        [ProducesResponseType((200), Type = typeof(List<BookVO>))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get()
        {
            return new OkObjectResult(_bookBusiness.FindAll());
        }

        // Configura o Swagger para a operação
        // http://localhost:{porta}/api/books/v1/{id}
        // [ProducesResponseType((202), Type = typeof(Book))]
        // determina o objeto de retorno em caso de sucesso Book
        // O [ProducesResponseType(XYZ)] define os códigos de retorno 204, 400 e 401
        [HttpGet("{id}")]
        [ProducesResponseType((200), Type = typeof(BookVO))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(long id)
        {
            var book = _bookBusiness.FindById(id);
            if (book == null) return NotFound();
            return new OkObjectResult(book);
        }

        // Configura o Swagger para a operação
        // http://localhost:{porta}/api/
        // [ProducesResponseType((202), Type = typeof(Book))]
        // determina o objeto de retorno em caso de sucesso Book
        // O [ProducesResponseType(XYZ)] define os códigos de retorno 400 e 401
        [HttpPost]
        [ProducesResponseType((201), Type = typeof(BookVO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] BookVO book)
        {
            if (book == null) return BadRequest();
            return new OkObjectResult(_bookBusiness.Create(book));
        }

        // Configura o Swagger para a operação
        // http://localhost:{porta}/api/books/v1/
        // determina o objeto de retorno em caso de sucesso Book
        // O [ProducesResponseType(XYZ)] define os códigos de retorno 400 e 401
        [HttpPut]
        [ProducesResponseType((202), Type = typeof(BookVO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Put([FromBody] BookVO book)
        {
            if (book == null) return BadRequest();
            var updatedBook = _bookBusiness.Update(book);
            if (updatedBook == null) return BadRequest();
            return new OkObjectResult(updatedBook);
        }

        // Configura o Swagger para a operação
        // http://localhost:{porta}/api/books/v1/{id}
        // O [ProducesResponseType(XYZ)] define os códigos de retorno 400 e 401
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Delete(int id)
        {
            _bookBusiness.Delete(id);
            return NoContent();
        }
    }
}