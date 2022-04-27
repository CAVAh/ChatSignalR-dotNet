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
    public class GroupController : Controller
    {
        private readonly ChatContext _context;

        public GroupController(ChatContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> ListGroups([FromQuery] string? filter = null)
        {
            try
            {
                IEnumerable<Group> lista;
                if (filter == null)
                {
                    lista = await _context.Groups.ToListAsync();
                }
                else
                {
                    // define o filtro
                    lista = await _context.Groups.ToListAsync();
                    //Filtro filtro = new Filtro(filter);
                    //lista = _context.ConsultarListaFiltro(filtro);
                }
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [ListGroups]", ex));
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetGroup(int id)
        {
            try
            {
                var obj = await _context.Groups.FindAsync(id);

                if (obj == null)
                {
                    return StatusCode(404, new RetornoJsonErro(404, "Registro não localizado [GetGroup]", null));
                }
                else
                {
                    return Ok(obj);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [GetGroup]", ex));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroup([FromBody] Group objJson)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new RetornoJsonErro(400, "Objeto inválido [Inserir Grupo]", null));
                }

                _context.Groups.Add(objJson);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetGroup),
                    new { id = objJson.Id },
                    objJson);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [Inserir Grupo]", ex));
            }
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup([FromBody] Group objJson, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, new RetornoJsonErro(400, "Objeto inválido [UpdateGroup]", null));
                }

                if (objJson.Id != id)
                {
                    return StatusCode(400, new RetornoJsonErro(400, "Objeto inválido [UpdateGroup] - ID do objeto difere do ID da URL.", null));
                }

                var group = await _context.Groups.FindAsync(id);
                if (group == null)
                {
                    return StatusCode(404, new RetornoJsonErro(404, "Objeto não encontrado [UpdateGroup] - Objeto não encontrado.", null));
                }

                group.Name = objJson.Name;

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [UpdateGroup]", ex));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            try
            {
                var group = await _context.Groups.FindAsync(id);

                if (group == null)
                {
                    return StatusCode(404, new RetornoJsonErro(404, "Objeto não encontrado [DeleteGroup] - Objeto não encontrado.", null));
                }

                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new RetornoJsonErro(500, "Erro no Servidor [DeleteGroup]", ex));
            }
        }

        private static Group GroupToDTO(Group group)
        {
            return new Group
            {
                Id = group.Id,
                Name = group.Name,
            };
        }

    }

}
