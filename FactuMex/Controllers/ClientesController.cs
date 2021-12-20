using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallCenter.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CallCenter.DBModel;

namespace CallCenter.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly FactuMexContext _context;
        private readonly IDataRepository<Ente> _repo;

        public ClientesController(FactuMexContext context, IDataRepository<Ente> repo)
        {
            _context = context;
            _repo = repo;

        }

        // GET: api/Clientes
        [HttpGet]
        public IEnumerable<Ente> GetClientes()
        {
            return _context.Ente.OrderByDescending(p => p.Idente);
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ente>> GetClienes(long id)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ente = await _context.Ente.FindAsync(id);

            if (ente == null)
            {
                return NotFound();
            }

            return Ok(ente);
        }

        // PUT: api/Clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClientes([FromRoute] long id, Ente ente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ente.Idente)
            {
                return BadRequest();
            }

            _context.Entry(ente).State = EntityState.Modified;

            try
            {
                _repo.Update(ente);
                var save = await _repo.SaveAsync(ente);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Clientes
        [HttpPost]
        public async Task<ActionResult<Ente>> PostClientes(Ente ente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _repo.Add(ente);
            var save = await _repo.SaveAsync(ente);

            return CreatedAtAction("GetClientes", new { id = ente.Idente }, ente);
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Ente>> DeleteEnte([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ente = await _context.Ente.FindAsync(id);
            if (ente == null)
            {
                return NotFound();
            }

            _repo.Delete(ente);
            var save = await _repo.SaveAsync(ente);

            return ente;
        }

        private bool EnteExists(long id)
        {
            return _context.Ente.Any(e => e.Idente == id);
        }
    }
}
