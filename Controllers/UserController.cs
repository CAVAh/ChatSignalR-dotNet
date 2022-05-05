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
    public class UserController : Controller
    {
        private readonly ChatContext _context;

        public UserController(ChatContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> ListUsers([FromQuery] string? filter = null)
        {
            try
            {
                IEnumerable<User> lista;
                if (filter == null)
                {
                    lista = await _context.Users.ToListAsync();
                }
                else
                {
                    // define o filtro
                    lista = await _context.Users.ToListAsync();
                    //Filtro filtro = new Filtro(filter);
                    //lista = _context.ConsultarListaFiltro(filtro);
                }
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [ListUsers]", ex));
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                var obj = await _context.Users.FindAsync(id);

                if (obj == null)
                {
                    return StatusCode(404, new RetornoJsonErro(404, "Registro não localizado [GetUser]", null));
                }
                else
                {
                    return Ok(obj);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [GetUser]", ex));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User objJson)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new RetornoJsonErro(400, "Objeto inválido [Inserir User]", null));
                }

                _context.Users.Add(objJson);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetUser),
                    new { id = objJson.Id },
                    objJson);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [Inserir User]", ex));
            }
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] User objJson, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new RetornoJsonErro(400, "Objeto inválido [UpdateUser]", null));
                }

                if (objJson.Id != id)
                {
                    return StatusCode(400, new RetornoJsonErro(400, "Objeto inválido [UpdateUser] - ID do objeto difere do ID da URL.", null));
                }

                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return StatusCode(404, new RetornoJsonErro(404, "Objeto não encontrado [UpdateUser] - Objeto não encontrado.", null));
                }

                user.Name = objJson.Name;
                /*user.Created_At = objJson.Created_At;
                user.User = objJson.User;
                user.Group = objJson.Group;*/

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [UpdateUser]", ex));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return StatusCode(404, new RetornoJsonErro(404, "Objeto não encontrado [DeleteUser] - Objeto não encontrado.", null));
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [DeleteUser]", ex));
            }
        }

        private static User UserToDTO(User user)
        {
            return new User
            {
                Id = user.Id,
                Name = user.Name,
            };
        }

    }

}