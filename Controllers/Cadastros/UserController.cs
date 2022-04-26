using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChatSignalR.Models;
using ChatSignalR.Services;


namespace ChatSignalR.Controllers
{
    [Route("user")]
    [Produces("application/json")]
    public class UserController : Controller
    {
        private readonly UserService _service;

        public UserController()
        {
            _service = new UserService();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ConsultarListaUser([FromQuery] string filter)
        {
            try
            {
                IEnumerable<User> lista;
                if (filter == null)
                {
                    lista = _service.ConsultarLista();
                }
                else
                {
                    // define o filtro
                    Filtro filtro = new Filtro(filter);
                    lista = _service.ConsultarListaFiltro(filtro);
                }
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [Consultar Lista User]", ex));
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}", Name = "ConsultarObjetoUser")]
        public IActionResult ConsultarObjetoUser(int id)
        {
            try
            {
                var objeto = _service.ConsultarObjeto(id);

                if (objeto == null)
                {
                    return StatusCode(404, new RetornoJsonErro(404, "Registro não localizado [Consultar Objeto User]", null));
                }
                else
                {
                    return Ok(objeto);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [Consultar Objeto User]", ex));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult InserirUser([FromBody] User objJson)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new RetornoJsonErro(400, "Objeto inválido [Inserir User]", null));
                }
                _service.Inserir(objJson);

                return CreatedAtRoute("ConsultarObjetoUser", new { id = objJson.Id }, objJson);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [Inserir User]", ex));
            }
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public IActionResult AlterarUser([FromBody] User objJson, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new RetornoJsonErro(400, "Objeto inválido [Alterar User]", null));
                }

                if (objJson.Id != id)
                {
                    return StatusCode(400, new RetornoJsonErro(400, "Objeto inválido [Alterar User] - ID do objeto difere do ID da URL.", null));
                }

                _service.Alterar(objJson);

                return ConsultarObjetoUser(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [Alterar User]", ex));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult ExcluirUser(int id)
        {
            try
            {
                var objeto = _service.ConsultarObjeto(id);

                _service.Excluir(objeto);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [Excluir User]", ex));
            }
        }

    }
}
