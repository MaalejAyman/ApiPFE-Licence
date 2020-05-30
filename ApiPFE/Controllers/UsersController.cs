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
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Userss>>> GetUsers()
        {
            return await _context.Userss.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Boolean>> GetUser(int id)
        {
            var user = await _context.Userss.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        // GET: api/Users/5
        [HttpGet("{login}")]
        public async Task<ActionResult<Boolean>> CheckLogin(string login)
        {
            var user = await _context.Userss.Where(user => user.Login == login).FirstOrDefaultAsync();

            if (user == null)
            {
                return false;
            }

            return true;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, Userss user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
         [HttpPost]
        public async Task<ActionResult<Userss>> PostUser(Userss user)
        {
            _context.Userss.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }
        [HttpPost]
        public async Task<ActionResult<Userss>> UserByData(Userss user)
        {
            var us = await _context.Userss.Where(use => use.Login == user.Login && use.Password == user.Password).FirstOrDefaultAsync();
            if(us == null)
            {
                return null;
            }
            return us;
        }
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Userss>> DeleteUser(long id)
        {
            var user = await _context.Userss.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Userss.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(long id)
        {
            return _context.Userss.Any(e => e.Id == id);
        }
        
    }
}
