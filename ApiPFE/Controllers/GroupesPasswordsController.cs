using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiPFE.Models;

namespace ApiPFE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupesPasswordsController : ControllerBase
    {
        private readonly userscontext _context;

        public GroupesPasswordsController(userscontext context)
        {
            _context = context;
        }

        // GET: api/GroupesPasswords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupesPasswords>>> GetGroupesPasswords()
        {
            return await _context.GroupesPasswords.ToListAsync();
        }

        // GET: api/GroupesPasswords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupesPasswords>> GetGroupesPasswords(long id)
        {
            var groupesPasswords = await _context.GroupesPasswords.FindAsync(id);

            if (groupesPasswords == null)
            {
                return NotFound();
            }

            return groupesPasswords;
        }

        // PUT: api/GroupesPasswords/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupesPasswords(long id, GroupesPasswords groupesPasswords)
        {
            if (id != groupesPasswords.IdPass)
            {
                return BadRequest();
            }

            _context.Entry(groupesPasswords).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupesPasswordsExists(id))
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

        // POST: api/GroupesPasswords
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<GroupesPasswords>> PostGroupesPasswords(GroupesPasswords groupesPasswords)
        {
            _context.GroupesPasswords.Add(groupesPasswords);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GroupesPasswordsExists(groupesPasswords.IdPass))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetGroupesPasswords", new { id = groupesPasswords.IdPass }, groupesPasswords);
        }

        // DELETE: api/GroupesPasswords/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GroupesPasswords>> DeleteGroupesPasswords(long id)
        {
            var groupesPasswords = await _context.GroupesPasswords.FindAsync(id);
            if (groupesPasswords == null)
            {
                return NotFound();
            }

            _context.GroupesPasswords.Remove(groupesPasswords);
            await _context.SaveChangesAsync();

            return groupesPasswords;
        }

        private bool GroupesPasswordsExists(long id)
        {
            return _context.GroupesPasswords.Any(e => e.IdPass == id);
        }
    }
}
