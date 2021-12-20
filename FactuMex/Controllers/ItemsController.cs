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
    public class ItemsController : ControllerBase
    {
        private readonly FactuMexContext _context;
        private readonly IDataRepository<Item> _repo;

        public ItemsController(FactuMexContext context, IDataRepository<Item> repo)
        {
            _repo = repo;
            _context = context;
        }

        // GET: api/Items
        [HttpGet]
        public IEnumerable<Item> GetItem()
        {
            return _context.Item.OrderByDescending(i => i.Iditem);
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _context.Item.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(long id, Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.Iditem)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                _repo.Update(item);
                var save = await _repo.SaveAsync(item);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
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

        // POST: api/Items
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _repo.Add(item);
            var save= await _repo.SaveAsync(item);

            return CreatedAtAction("GetItem", new { id = item.Iditem }, item);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Item>> DeleteItem(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _repo.Delete(item);
            var save = await _repo.SaveAsync(item);

            return item;
        }

        private bool ItemExists(long id)
        {
            return _context.Item.Any(e => e.Iditem == id);
        }
    }
}
