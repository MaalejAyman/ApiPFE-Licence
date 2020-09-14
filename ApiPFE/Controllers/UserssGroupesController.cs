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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserssGroupes>>> GetUserssGroupes()
        {
            return await _context.UserssGroupes.ToListAsync();
        }

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

        [HttpPost]
        public async Task<ActionResult<UserssGroupes>> PostUserssGroupes(UserssGroupes ug)
        {
            _context.UserssGroupes.Add(ug);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserssGroupesExists(ug.IdUsr))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserssGroupes", new { id = ug.IdUsr }, ug);
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
