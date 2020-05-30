using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiPFE.Models;
using ApiPFE.Models.Read;

namespace ApiPFE.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PasswordsController : ControllerBase
    {
        private readonly UserContext _context;

        public PasswordsController(UserContext context)
        {
            _context = context;
        }

        // GET: api/Passwords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Passwords>>> GetPasswords()
        {
            return await _context.Passwords.ToListAsync();
        }

        // GET: api/Passwords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Passwords>> GetPasswords(long id)
        {
            var passwords = await _context.Passwords.FindAsync(id);

            if (passwords == null)
            {
                return NotFound();
            }

            return passwords;
        }

        // PUT: api/Passwords/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPasswords(long id, Passwords passwords)
        {
            if (id != passwords.Id)
            {
                return BadRequest();
            }

            _context.Entry(passwords).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PasswordsExists(id))
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

        // POST: api/Passwords
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Passwords>> PostPasswords(PasswordsRead pass)
        {
            var passwords = Sync(pass).Result;
            _context.Passwords.Add(passwords);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PasswordsExists(passwords.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPasswords", new { id = passwords.Id });
        }


        // DELETE: api/Passwords/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Passwords>> DeletePasswords(long id)
        {
            var passwords = await _context.Passwords.FindAsync(id);
            if (passwords == null)
            {
                return NotFound();
            }

            _context.Passwords.Remove(passwords);
            await _context.SaveChangesAsync();

            return passwords;
        }

        private bool PasswordsExists(long id)
        {
            return _context.Passwords.Any(e => e.Id == id);
        }
        [HttpPost]
        private async Task<Passwords> Sync(PasswordsRead p)
        {
            
            var pass = new Passwords();
            pass.Login = p.Login;
            pass.Value = p.Value;
            pass.IdFldr = p.IdFldr;
            pass.IdFldrNavigation = await _context.Folders.Where(fldr => fldr.Id == p.IdGrp).FirstOrDefaultAsync();
            pass.IdGrp = p.IdGrp;
            pass.IdGrpNavigation = await _context.Groupes.Where(grp => grp.Id == p.IdGrp).FirstOrDefaultAsync();
            pass.IdUser = p.IdUser;
            pass.IdUserNavigation = await _context.Userss.Where(usr => usr.Id == p.IdUser).FirstOrDefaultAsync();
            pass.IdWs = p.IdWs;
            pass.IdWsNavigation = await _context.WebSites.Where(ws => ws.Id == p.IdWs).FirstOrDefaultAsync();
            return pass;      
        }
    }
}
