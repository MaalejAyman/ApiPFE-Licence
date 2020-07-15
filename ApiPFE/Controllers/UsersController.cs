using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiPFE.Models;
using ApiPFE.Models.Write;
using ApiPFE.Models.Read;

namespace ApiPFE.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly userscontext _context;

        public UsersController(userscontext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Userss>>> GetUsers()
        {
            var u= await _context.Userss.ToListAsync();
            foreach(Userss user in u)
            {
                user.Password = null;
            }
            return u;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Boolean>> GetUser(long id)
        {
            var user = await _context.Userss.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            user.Passwords = await _context.Passwords.Where(psd => psd.IdUser == user.Id).ToListAsync();
            foreach( Passwords psd in user.Passwords)
            {
                psd.IdUserNavigation = null;
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
            Folders fw = new Folders();
            fw.Name = "Shared";
            fw.IdUser = user.Id;
            fw.Parent = null;
            fw.IdParentFolder = null;
            _context.Folders.Add(fw);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }
        [HttpPost]
        public async Task<ActionResult<UsersRead>> UserByData(UsersWrite user)
        {
            var us = await _context.Userss.Where(use => use.Login == user.Login && use.Password == user.Password).FirstOrDefaultAsync();
            if(us == null)
            {
                return null;
            }
            var usR = new UsersRead();
            usR.Id = us.Id;
            usR.Login = us.Login;
            usR.Password = us.Password;
            return usR;
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
