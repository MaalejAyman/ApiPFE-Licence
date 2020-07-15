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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserssGroupesController : ControllerBase
    {
        private readonly userscontext _context;

        public UserssGroupesController(userscontext context)
        {
            _context = context;
        }

        // GET: api/UserssGroupes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserssGroupes>>> GetUserssGroupes()
        {
            return await _context.UserssGroupes.ToListAsync();
        }

        // GET: api/UserssGroupes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserssGroupes>> GetUserssGroupes(long id)
        {
            var userssGroupes = await _context.UserssGroupes.FindAsync(id);

            if (userssGroupes == null)
            {
                return NotFound();
            }

            return userssGroupes;
        }

        // PUT: api/UserssGroupes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserssGroupes(long id, UserssGroupes userssGroupes)
        {
            if (id != userssGroupes.IdUsr)
            {
                return BadRequest();
            }

            _context.Entry(userssGroupes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserssGroupesExists(id))
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

        // POST: api/UserssGroupes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UserssGroupes>> PostUserssGroupes(UserssGroupes userssGroupes)
        {
            _context.UserssGroupes.Add(userssGroupes);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserssGroupesExists(userssGroupes.IdUsr))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserssGroupes", new { id = userssGroupes.IdUsr }, userssGroupes);
        }

        // DELETE: api/UserssGroupes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserssGroupes>> DeleteUserssGroupes(long id)
        {
            var userssGroupes = await _context.UserssGroupes.FindAsync(id);
            if (userssGroupes == null)
            {
                return NotFound();
            }

            _context.UserssGroupes.Remove(userssGroupes);
            await _context.SaveChangesAsync();

            return userssGroupes;
        }

        private bool UserssGroupesExists(long id)
        {
            return _context.UserssGroupes.Any(e => e.IdUsr == id);
        }
        [HttpPost]
        public async Task<List<long>> GetUsersByGroupes(Groupes g)
        {
            var U = new List<long>();
            var GU = await _context.UserssGroupes.Where(gr => gr.IdGrp == g.Id).ToListAsync();
            foreach(UserssGroupes GU1 in GU)
            {
                U.Add(GU1.IdUsr);
            }
            return U;
        }
    }
}
