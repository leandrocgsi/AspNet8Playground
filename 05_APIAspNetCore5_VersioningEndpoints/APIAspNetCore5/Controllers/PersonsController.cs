using APIAspNetCore5.Model;
using APIAspNetCore5.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIAspNetCore5.Controllers
{

    /* Mapeia as requisições de http://localhost:{porta}/api/person/
    Por padrão o ASP.NET Core mapeia todas as classes que extendem Controller
    pegando a primeira parte do nome da classe em lower case [Person]Controller
    e expõe como endpoint REST
    */
    [ApiVersion("1")]
    [ApiController]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class PersonsController : ControllerBase
    {
        //Declaração do serviço usado
        private IPersonService _personService;

        /* Injeção de uma instancia de IPersonService ao criar
        uma instancia de PersonController */
        public PersonsController(IPersonService personService)
        {
            _personService = personService;
        }

        //Mapeia as requisições GET para http://localhost:{porta}/api/person/v{version_number}
        //Get sem parâmetros para o FindAll --> Busca Todos
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_personService.FindAll());
        }

        //Mapeia as requisições GET para http://localhost:{porta}/api/person/v{version_number}/{id}
        //recebendo um ID como no Path da requisição
        //Get com parâmetros para o FindById --> Busca Por ID
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var person = _personService.FindById(id);
            if (person == null) return NotFound();
            return Ok(person);
        }

        //Mapeia as requisições POST para http://localhost:{porta}/api/person/v{version_number}
        //O [FromBody] consome o Objeto JSON enviado no corpo da requisição
        [HttpPost]
        public IActionResult Post([FromBody] Person person)
        {
            if (person == null) return BadRequest();
            return new ObjectResult(_personService.Create(person));
        }

        //Mapeia as requisições PUT para http://localhost:{porta}/api/person/v{version_number}
        //O [FromBody] consome o Objeto JSON enviado no corpo da requisição
        [HttpPut]
        public IActionResult Put([FromBody] Person person)
        {
            if (person == null) return BadRequest();
            return new ObjectResult(_personService.Update(person));
        }


        //Mapeia as requisições DELETE para http://localhost:{porta}/api/person/v{version_number}/{id}
        //recebendo um ID como no Path da requisição
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _personService.Delete(id);
            return NoContent();
        }
    }
}
