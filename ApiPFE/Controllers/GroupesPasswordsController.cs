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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupesPasswords>>> GetGroupesPasswords()
        {
            return await _context.GroupesPasswords.ToListAsync();
        }

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

        [HttpPost]
        public async Task<ActionResult<GroupesPasswords>> PostGroupesPasswords(GroupesPasswords gp)
        {
            _context.GroupesPasswords.Add(gp);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GroupesPasswordsExists(gp.IdPass))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetGroupesPasswords", new { id = gp.IdPass }, gp);
        }

        private bool GroupesPasswordsExists(long id)
        {
            return _context.GroupesPasswords.Any(e => e.IdPass == id);
        }
    }
}
