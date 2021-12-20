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
    [ApiController]
    public class ComprobantesController : ControllerBase
    {
        private readonly FactuMexContext _context;
        private readonly IDataRepository<Comprobante> _repo;

        public ComprobantesController(FactuMexContext context, IDataRepository<Comprobante> repo)
        {
            _context = context;
            _repo = repo;
        }

        // GET: api/Comprobantes
        [HttpGet]
        public IEnumerable<Comprobante> GetComprobante()
        {
            return _context.Comprobante.OrderByDescending(c => c.Idcomp);
        }

        // GET: api/Comprobantes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comprobante>> GetComprobante(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comprobante = await _context.Comprobante.FindAsync(id);

            if (comprobante == null)
            {
                return NotFound();
            }

            return Ok(comprobante);
        }

        // PUT: api/Comprobantes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComprobante(long id, Comprobante comprobante)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != comprobante.Idcomp)
            {
                return BadRequest();
            }

            _context.Entry(comprobante).State = EntityState.Modified;

            try
            {
                _repo.Update(comprobante);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComprobanteExists(id))
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

        // POST: api/Comprobantes
        [HttpPost]
        public async Task<ActionResult<Comprobante>> PostComprobante(Comprobante comprobante)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repo.Add(comprobante);
            
            var save = await _repo.SaveAsync(comprobante);

            return CreatedAtAction("GetComprobante", new { id = comprobante.Idcomp }, comprobante);
        }

        // DELETE: api/Comprobantes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Comprobante>> DeleteComprobante(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comprobante = await _context.Comprobante.FindAsync(id);
            if (comprobante == null)
            {
                return NotFound();
            }

            _repo.Delete(comprobante);
            var save = await _repo.SaveAsync(comprobante);

            return comprobante;
        }

        private bool ComprobanteExists(long id)
        {
            return _context.Comprobante.Any(e => e.Idcomp == id);
        }
    }
}
