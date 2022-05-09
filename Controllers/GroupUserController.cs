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
    public class GroupUserController : Controller
    {
        private readonly ChatContext _context;

        public GroupUserController(ChatContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupUser>>> ListGroupUsers([FromQuery] int? userId = null)
        {
            try
            {
                IEnumerable<GroupUser> lista;
                if (userId == null)
                {
                    lista = await _context.GroupUsers.ToListAsync();
                }
                else
                {
                    // define o filtro
                    lista = await _context.GroupUsers.Where(p => p.UserId == userId).ToListAsync();
                }
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [ListGroupUsers]", ex));
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupUser>> GetGroupUser(int id)
        {
            try
            {
                var obj = await _context.GroupUsers.FindAsync(id);

                if (obj == null)
                {
                    return StatusCode(404, new RetornoJsonErro(404, "Registro não localizado [GetGroupUser]", null));
                }
                else
                {
                    return Ok(obj);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [GetGroupUser]", ex));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<GroupUser>> CreateGroupUser([FromBody] GroupUser objJson)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new RetornoJsonErro(400, "Objeto inválido [Inserir GrupoUsuario]", null));
                }

                _context.GroupUsers.Add(objJson);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetGroupUser),
                    new { id = objJson.Id },
                    objJson);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [Inserir GrupoUsuario]", ex));
            }
        }

        [AllowAnonymous]
        [HttpPost("{groupId}")]
        public async Task<ActionResult<GroupUser>> CreateGroupUsersByGroup(int groupId, [FromBody] GroupUser objJson)
        {
            try
            {
                if (!ModelState.IsValid || objJson.UserIds?.Length == 0)
                {
                    return StatusCode(400, new RetornoJsonErro(400, "Objeto inválido [Inserir GrupoUsuario]", null));
                }


                foreach (int userId in objJson.UserIds)
                {
                    GroupUser groupUser = new GroupUser { UserId = userId, GroupId = groupId };

                    _context.GroupUsers.Add(groupUser);
                }

                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetGroupUser),
                    new { id = objJson.Id },
                    objJson);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [Inserir GrupoUsuario]", ex));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupUser(int id)
        {
            try
            {
                var groupUser = await _context.GroupUsers.FindAsync(id);

                if (groupUser == null)
                {
                    return StatusCode(404, new RetornoJsonErro(404, "Objeto não encontrado [DeleteGroupUser] - Objeto não encontrado.", null));
                }

                _context.GroupUsers.Remove(groupUser);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [DeleteGroupUser]", ex));
            }
        }

        private static GroupUser GroupToDTO(GroupUser groupUser)
        {
            return new GroupUser
            {
                Id = groupUser.Id,
                GroupId = groupUser.GroupId,
                UserId = groupUser.UserId,
            };
        }

    }

}
