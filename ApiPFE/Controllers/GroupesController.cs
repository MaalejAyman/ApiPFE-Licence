using System;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Groupes>>> GetGroupes()
        {
            return await _context.Groupes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Groupes>> GetGroupes2(long id)
        {
            var groupes = await _context.Groupes.FindAsync(id);

            if (groupes == null)
            {
                return NotFound();
            }

            return groupes;
        }

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

        [HttpPost]
        public async Task<ActionResult<long>> DeleteGroupes(GroupesWrite g)
        {
            var groupes = await _context.Groupes.FindAsync(g.Id);
            var gp = await _context.GroupesPasswords.Where((GP) => GP.IdGrp == g.Id).ToListAsync();
            var ug = await _context.UserssGroupes.Where((UG) => UG.IdGrp == g.Id).ToListAsync();
            if (groupes == null)
            {
                return NotFound();
            }
            if (gp != null)
            {
                foreach(GroupesPasswords p in gp)
                {
                    _context.GroupesPasswords.Remove(p);
                }
            }
            if (ug != null)
            {
                foreach (UserssGroupes u in ug)
                {
                    _context.UserssGroupes.Remove(u);
                }
            }
            _context.Groupes.Remove(groupes);
            await _context.SaveChangesAsync();

            return 1;
        }

        private bool GroupesExists(long id)
        {
            return _context.Groupes.Any(e => e.Id == id);
        }
        private async Task<Groupes> Sync(GroupesWrite G)
        {
            var grp = new Groupes();
            grp.Name = G.Name;
            return grp;
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Groupes>>> GroupesByUserId(UsersWrite u)
        {
            var us = await _context.Userss.Where(usr => usr.Id == u.Id).FirstOrDefaultAsync();
            var t1 = new List<UserssGroupes>();
            if (us.IsAdmin == 0)
            {
                t1 = await _context.UserssGroupes.Where(ug => ug.IdUsr == u.Id).ToListAsync();
            }
            else
            {
                return await _context.Groupes.ToListAsync();
            }
            
            var t = new List<Groupes>();
            foreach(UserssGroupes x in t1)
            {
                var grp = await _context.Groupes.Where(g => g.Id == x.IdGrp).FirstAsync();
                if (grp != null)
                {
                    var g = new Groupes();
                    g.Id = grp.Id;
                    g.Name = grp.Name;
                    t.Add(g);
                }
            }
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
