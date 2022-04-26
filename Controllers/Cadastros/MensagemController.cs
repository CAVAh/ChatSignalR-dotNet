using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChatSignalR.Models;
using ChatSignalR.Services;

namespace ChatSignalR.Controllers
{
    [Route("mensagem")]
    [Produces("application/json")]
    public class MensagemController : Controller
    {
        private readonly MensagemService _service;

        public MensagemController()
        {
            _service = new MensagemService();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ConsultarListaMensagem([FromQuery] string filter)
        {
            try
            {
                IEnumerable<Mensagens> lista;
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
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [Consultar Lista Mensagem]", ex));
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}", Name = "ConsultarObjetoMensagem")]
        public IActionResult ConsultarObjetoMensagem(int id)
        {
            try
            {
                var objeto = _service.ConsultarObjeto(id);

                if (objeto == null)
                {
                    return StatusCode(404, new RetornoJsonErro(404, "Registro não localizado [Consultar Objeto Mensagem]", null));
                }
                else
                {
                    return Ok(objeto);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [Consultar Objeto Mensagem]", ex));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult InserirMensagem([FromBody] Mensagens objJson)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new RetornoJsonErro(400, "Objeto inválido [Inserir Mensagem]", null));
                }
                _service.Inserir(objJson);

                return CreatedAtRoute("ConsultarObjetoMensagem", new { id = objJson.Id }, objJson);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [Inserir Mensagem]", ex));
            }
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public IActionResult AlterarMensagem([FromBody] Mensagens objJson, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new RetornoJsonErro(400, "Objeto inválido [Alterar Mensagem]", null));
                }

                if (objJson.Id != id)
                {
                    return StatusCode(400, new RetornoJsonErro(400, "Objeto inválido [Alterar Mensagem] - ID do objeto difere do ID da URL.", null));
                }

                _service.Alterar(objJson);

                return ConsultarObjetoMensagem(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [Alterar Mensagem]", ex));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult ExcluirMensagem(int id)
        {
            try
            {
                var objeto = _service.ConsultarObjeto(id);

                _service.Excluir(objeto);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [Excluir Mensagem]", ex));
            }
        }

    }
}
