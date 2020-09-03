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
    public class PasswordsController : ControllerBase
    {
        private readonly userscontext _context;
        private PasswordsRead[] pass;

        public PasswordsController(userscontext context)
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
            passwords.Id = id;
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
        public async Task<ActionResult<Passwords>> PostPasswords(PasswordsWrite pass)
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
            if (pass.Groupes != null)
            {
                int x = 0;
                foreach (long grp in pass.Groupes)
                {
                    var GP = new GroupesPasswords();
                    GP.IdGrp = grp;
                    GP.IdPass = passwords.Id;
                    GP.PasswordCrypPub = pass.PasswordCrypPubs.GetValue(x).ToString();
                    x++;
                    this._context.GroupesPasswords.Add(GP);
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException)
                    {

                    }
                }
            }
            return CreatedAtAction("GetPasswords", new { Id = passwords.Id });
        }
        [HttpPost]
        public async Task<ActionResult<Passwords>> UpdatePasswords(PasswordsWrite pass)
        {
            var passwords = Sync(pass).Result;
            var p = await _context.Passwords.FindAsync(pass.Id);
            p.Login = passwords.Login;
            p.Value = passwords.Value;
            p.IdWs = passwords.IdWs;
            p.IdUser = passwords.IdUser;
            p.Score = passwords.Score;
            p.IdFldr = passwords.IdFldr;
            var t = await _context.GroupesPasswords.Where(GP => GP.IdPass == pass.Id).ToListAsync();
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

            }
            foreach (GroupesPasswords GP1 in t)
            {
                _context.GroupesPasswords.Remove(GP1);
            }

            if (pass.Groupes != null)
            {
                int x = 0;
                foreach (long grp in pass.Groupes)
                {
                    var GP = new GroupesPasswords();
                    GP.IdGrp = grp;
                    GP.IdPass = pass.Id;
                    GP.PasswordCrypPub = pass.PasswordCrypPubs.GetValue(x).ToString();
                    x++;
                    this._context.GroupesPasswords.Add(GP);
                }
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

            }
            return CreatedAtAction("GetPasswords", new { id = passwords.Id });
        }
        [HttpPost]
        public async Task<List<PasswordsRead>> GetPasswordsByUser(Userss u)
        {
            var p = await _context.Passwords.Where(psd => psd.IdUser == u.Id).ToListAsync();
            List<PasswordsRead> passRead = new List<PasswordsRead>();
            foreach (Passwords psd in p)
            {
                psd.IdUserNavigation = null;
                var psdR = new PasswordsRead();
                psdR.Id = psd.Id;
                psdR.Value = psd.Value;
                psdR.IdFldr = psd.IdFldr;
                psdR.IdUser = psd.IdUser;
                psdR.IdWs = psd.IdWs;
                psdR.Login = psd.Login;
                psdR.Groupes = new List<long>();
                var t = await _context.GroupesPasswords.Where(g => g.IdPass == psd.Id).ToListAsync();
                if (t != null)
                {
                    foreach (GroupesPasswords gp in t)
                    {
                        psdR.Groupes.Add(gp.IdGrp);
                    }
                }
                passRead.Add(psdR);
            }
            return passRead;
        }
        // DELETE: api/Passwords/5
        [HttpPost]
        public async Task<ActionResult<long>> DeletePasswords(PasswordsWrite p)
        {
            var passwords = await _context.Passwords.FindAsync(p.Id);
            
            if (passwords == null)
            {
                return NotFound();
            }
            var t = await _context.GroupesPasswords.Where(GP => GP.IdPass == p.Id).ToListAsync();
            foreach (GroupesPasswords GP1 in t)
            {
                _context.GroupesPasswords.Remove(GP1);
            }
            _context.Passwords.Remove(passwords);
            await _context.SaveChangesAsync();

            return 1;
        }

        [HttpPost]
        public async Task<List<PasswordsRead>> GetSharedPasswords(Userss u)
        {
            List<PasswordsRead> PRS=new List<PasswordsRead>();
            var us = await _context.Userss.Where(usr=> usr.Id == u.Id).FirstOrDefaultAsync();
            var G = new List<UserssGroupes>();
            if (us.IsAdmin == 0)
            {
                G = await _context.UserssGroupes.Where(g => g.IdUsr == u.Id).ToListAsync();
            }
            else
            {
                G = await _context.UserssGroupes.ToListAsync();
            }
            foreach(UserssGroupes grp in G)
            {
                var pass = await _context.GroupesPasswords.Where(p => p.IdGrp == grp.IdGrp).ToListAsync();
                foreach(GroupesPasswords Gp in pass)
                {
                    var pr = new PasswordsRead();
                    pr.Id = Gp.IdPass;
                    var p = await _context.Passwords.Where(p1 => p1.Id == Gp.IdPass).FirstOrDefaultAsync();
                    pr.IdGrp = Gp.IdGrp;
                    pr.Login = p.Login;
                    pr.Value = p.Value;
                    pr.Value2 = Gp.PasswordCrypPub;
                    pr.IdUser = p.IdUser;
                    pr.IdWs = p.IdWs;
                    var gr = await _context.Groupes.Where(p1 => p1.Id == Gp.IdGrp).FirstOrDefaultAsync();
                    pr.GrpName = gr.Name;
                    if (pr.IdUser != u.Id)
                    {
                        PRS.Add(pr);
                    }
                }
            }
            return PRS;
        }
        private bool PasswordsExists(long id)
        {
            return _context.Passwords.Any(e => e.Id == id);
        }
        [HttpPost]
        private async Task<Passwords> Sync(PasswordsWrite p)
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
