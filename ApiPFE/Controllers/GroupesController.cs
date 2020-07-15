﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiPFE.Models;
using ApiPFE.Models.Write;

namespace ApiPFE.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GroupesController : ControllerBase
    {
        private readonly userscontext _context;

        public GroupesController(userscontext context)
        {
            _context = context;
        }

        // GET: api/Groupes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Groupes>>> GetGroupes()
        {
            return await _context.Groupes.ToListAsync();
        }

        // GET: api/Groupes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Groupes>> GetGroupes(long id)
        {
            var groupes = await _context.Groupes.FindAsync(id);

            if (groupes == null)
            {
                return NotFound();
            }

            return groupes;
        }

        // PUT: api/Groupes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupes(long id, Groupes groupes)
        {
            if (id != groupes.Id)
            {
                return BadRequest();
            }

            _context.Entry(groupes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupesExists(id))
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

        // POST: api/Groupes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Groupes>> PostGroupes(GroupesWrite grp)
        {
            var groupes = Sync(grp).Result;
            _context.Groupes.Add(groupes);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GroupesExists(groupes.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            foreach(Userss u in grp.Users)
            {
                UserssGroupes ug = new UserssGroupes();
                ug.IdGrp = groupes.Id;
                ug.IdUsr = u.Id;
                _context.UserssGroupes.Add(ug);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                    throw;
            }
            return CreatedAtAction("GetGroupes", new { id = groupes.Id ,Name = groupes.Name});
        }

        // DELETE: api/Groupes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Groupes>> DeleteGroupes(long id)
        {
            var groupes = await _context.Groupes.FindAsync(id);
            if (groupes == null)
            {
                return NotFound();
            }

            _context.Groupes.Remove(groupes);
            await _context.SaveChangesAsync();

            return groupes;
        }

        private bool GroupesExists(long id)
        {
            return _context.Groupes.Any(e => e.Id == id);
        }
        private async Task<Groupes> Sync(GroupesWrite G)
        {
            var grp = new Groupes();
            grp.Name = G.Name;
            grp.IdUser = G.IdUser;
            grp.IdUserNavigation = await _context.Userss.Where(g => g.Id == G.IdUser).FirstOrDefaultAsync();
            return grp;
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Groupes>>> GroupesByUserId(UsersWrite u)
        {
            var t = await _context.Groupes.Where(g => g.IdUser == u.Id).ToListAsync();
            return t;
        }
        [HttpPost]
        public async Task<ActionResult<Groupes>> UpdateGroupes(GroupesWrite grp)
        {
            var groupes = Sync(grp).Result;
            var p = await _context.Groupes.FindAsync(grp.Id);
            p.Name = groupes.Name;
            p.Id = grp.Id;
            var t = await _context.UserssGroupes.Where(GP => GP.IdGrp == p.Id).ToListAsync();
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
            foreach (UserssGroupes UG1 in t)
            {
                _context.UserssGroupes.Remove(UG1);
            }
            foreach (Userss u in grp.Users)
            {
                UserssGroupes ug = new UserssGroupes();
                ug.IdGrp = grp.Id;
                ug.IdUsr = u.Id;
                _context.UserssGroupes.Add(ug);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
            return CreatedAtAction("GetGroupes", new { id = groupes.Id, Name = groupes.Name });
        }
    }
}
