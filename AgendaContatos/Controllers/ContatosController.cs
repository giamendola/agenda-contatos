using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgendaContatos.DbRepository;
using AgendaContatos.Models;
using Microsoft.AspNetCore.JsonPatch;
using Swashbuckle.AspNetCore.Annotations;

namespace AgendaContatos.Controllers
{
  
    [ApiController]
    [Route("api/")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class ContatosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContatosController(AppDbContext context)
        {
            _context = context;
        }

        [SwaggerOperation(Summary = "Recupera a lista de todos os contatos")]
        [HttpGet("[controller]")]        
        public async Task<ActionResult<IEnumerable<Contato>>> GetContatos()
        {
            return await _context.Contatos.ToListAsync();
        }


        [SwaggerOperation(Summary = "Recupera apenas um contato")]
        [HttpGet("Contato/{id}")]      
        public async Task<ActionResult<Contato>> GetContato(int id)
        {
            var contato = await _context.Contatos.FindAsync(id);

            if (contato == null)
            {
                return NotFound();
            }

            return contato;
        }

        [SwaggerOperation(Summary = "Realiza a atualização de todos os campos do Contato", Description = "Aceita apenas um objeto de contato, sendo necessário todos os campos para atualização")]
        [HttpPut("Contato/{id}")]
        public async Task<IActionResult> PutContato(int id, Contato contato)
        {
            if (contato != null)
            {
                var contatoToFind = await _context.Contatos.FindAsync(id);

                if (contatoToFind == null)
                {
                    return NotFound();
                }

                contatoToFind.nome = contato.nome;
                contatoToFind.telefone = contato.telefone;
                contatoToFind.email = contato.email;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) when (!ContatoExists(id))
                {
                    return NotFound();
                }                

            }

            return NoContent();
        }

        
        [SwaggerOperation(Summary = "Atualiza o dados do Contato de acordo com o corpo", Description = "Aceita mais de um objeto de atualização de contato. Leia o Schema para mais informações.")]
        [HttpPatch("Contato/{id}")]
        [Consumes("application/json-patch+json")]
        [Produces("application/json-patch+json")]
        public async Task<IActionResult> PatchContato([FromRoute] int id, [FromBody] JsonPatchDocument<Contato> contato)
        {
            if (contato == null)
            {
                return BadRequest();
            }

            var contatosDB = await _context.Contatos.FirstOrDefaultAsync(cont => cont.id == id);
            if (contatosDB == null)
            {
                return NotFound();
            }

            contato.ApplyTo(contatosDB, ModelState);
            var isValid = TryValidateModel(contatosDB);
            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            await _context.SaveChangesAsync();
            return Ok(contato);
        }

        [SwaggerOperation(Summary = "Cria apenas um contato", Description = "Aceita apenas um objeto de contato")]
        [HttpPost("Contato")]
        public async Task<ActionResult<Contato>> PostContato(Contato contato)
        {            
            _context.Contatos.Add(contato);
            await _context.SaveChangesAsync(); 

            return CreatedAtAction("GetContato", new { id = contato.id}, contato);
        }

        [SwaggerOperation(Summary = "Cria uma lista de contatos", Description = "Aceita mais de um objeto de contato")]
        [HttpPost]
        [Route("[controller]")]
        public async Task<ActionResult<List<Contato>>> PostContatos(List<Contato> contatos)
        {            
            foreach(Contato contato in contatos)
            {
                _context.Contatos.Add(contato);
                await _context.SaveChangesAsync();
            }
                       
            return CreatedAtAction("GetContatos", contatos);
        }

        [SwaggerOperation(Summary = "Deleta um contato", Description = "Deleta um contato de acordo com um ID")]
        [HttpDelete("Contato/{id}")]
        public async Task<IActionResult> DeleteContato(int id)
        {
            var contato = await _context.Contatos.FindAsync(id);
            if (contato == null)
            {
                return NotFound();
            }

            _context.Contatos.Remove(contato);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContatoExists(int id)
        {
            return _context.Contatos.Any(e => e.id == id);
        }
    }
}
