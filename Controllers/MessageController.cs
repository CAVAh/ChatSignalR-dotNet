using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChatSignalR.Models;
using ChatSignalR.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatSignalR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MessageController : Controller
    {
        private readonly ChatContext _context;

        public MessageController(ChatContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> ListMessages([FromQuery] string? filter = null)
        {
            try
            {
                IEnumerable<Message> lista;
                if (filter == null)
                {
                    lista = await _context.Messages.ToListAsync();
                }
                else
                {
                    // define o filtro
                    lista = await _context.Messages.ToListAsync();
                    //Filtro filtro = new Filtro(filter);
                    //lista = _context.ConsultarListaFiltro(filtro);
                }
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [ListMessages]", ex));
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(int id)
        {
            try
            {
                var obj = await _context.Messages.FindAsync(id);

                if (obj == null)
                {
                    return StatusCode(404, new RetornoJsonErro(404, "Registro não localizado [GetMessage]", null));
                }
                else
                {
                    return Ok(obj);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [GetMessage]", ex));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Message>> CreateMessage([FromBody] Message objJson)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new RetornoJsonErro(400, "Objeto inválido [Inserir Message]", null));
                }

                _context.Messages.Add(objJson);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetMessage),
                    new { id = objJson.Id },
                    objJson);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [Inserir Message]", ex));
            }
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage([FromBody] Message objJson, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new RetornoJsonErro(400, "Objeto inválido [UpdateMessage]", null));
                }

                if (objJson.Id != id)
                {
                    return StatusCode(400, new RetornoJsonErro(400, "Objeto inválido [UpdateMessage] - ID do objeto difere do ID da URL.", null));
                }

                var message = await _context.Messages.FindAsync(id);
                if (message == null)
                {
                    return StatusCode(404, new RetornoJsonErro(404, "Objeto não encontrado [UpdateMessage] - Objeto não encontrado.", null));
                }

                message.Text = objJson.Text;
                /*message.Created_At = objJson.Created_At;
                message.User = objJson.User;
                message.Group = objJson.Group;*/

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [UpdateMessage]", ex));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            try
            {
                var message = await _context.Messages.FindAsync(id);

                if (message == null)
                {
                    return StatusCode(404, new RetornoJsonErro(404, "Objeto não encontrado [DeleteMessage] - Objeto não encontrado.", null));
                }

                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [DeleteMessage]", ex));
            }
        }

        private static Message MessageToDTO(Message message)
        {
            return new Message
            {
                Id = message.Id,
                Text = message.Text,
                Created_At = message.Created_At,
                UserId = message.UserId,
                GroupId = message.GroupId,
            };
        }

    }

}
