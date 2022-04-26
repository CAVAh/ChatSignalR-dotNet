using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChatSignalR.Models;
using ChatSignalR.Services;

namespace ChatSignalR.Controllers
{
    [Route("grupo")]
    [Produces("application/json")]
    public class GrupoController : Controller
    {
        private readonly GrupoService _service;

        public GrupoController()
        {
            _service = new GrupoService();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ConsultarListaGrupo([FromQuery] string filter)
        {
            try
            {
                IEnumerable<Grupo> lista;
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
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [Consultar Lista Grupo]", ex));
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}", Name = "ConsultarObjetoGrupo")]
        public IActionResult ConsultarObjetoGrupo(int id)
        {
            try
            {
                var objeto = _service.ConsultarObjeto(id);

                if (objeto == null)
                {
                    return StatusCode(404, new RetornoJsonErro(404, "Registro não localizado [Consultar Objeto Grupo]", null));
                }
                else
                {
                    return Ok(objeto);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [Consultar Objeto Grupo]", ex));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult InserirGrupo([FromBody] Grupo objJson)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new RetornoJsonErro(400, "Objeto inválido [Inserir Grupo]", null));
                }
                _service.Inserir(objJson);

                return CreatedAtRoute("ConsultarObjetoGrupo", new { id = objJson.Id }, objJson);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [Inserir Grupo]", ex));
            }
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public IActionResult AlterarGrupo([FromBody] Grupo objJson, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new RetornoJsonErro(400, "Objeto inválido [Alterar Grupo]", null));
                }

                if (objJson.Id != id)
                {
                    return StatusCode(400, new RetornoJsonErro(400, "Objeto inválido [Alterar Grupo] - ID do objeto difere do ID da URL.", null));
                }

                _service.Alterar(objJson);

                return ConsultarObjetoGrupo(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [Alterar Grupo]", ex));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult ExcluirGrupo(int id)
        {
            try
            {
                var objeto = _service.ConsultarObjeto(id);

                _service.Excluir(objeto);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [Excluir Grupo]", ex));
            }
        }

    }
}
