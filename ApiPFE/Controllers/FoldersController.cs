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
    public class FoldersController : ControllerBase
    {
        private readonly userscontext _context;

        public FoldersController(userscontext context)
        {
            _context = context;
        }

        // GET: api/Folders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Folders>>> GetFolders()
        {
            return await _context.Folders.ToListAsync();
        }

        // GET: api/Folders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Folders>> GetFolders(long id)
        {
            var folders = await _context.Folders.FindAsync(id);

            if (folders == null)
            {
                return NotFound();
            }

            return folders;
        }

        // PUT: api/Folders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFolders(long id, Folders folders)
        {
            if (id != folders.Id)
            {
                return BadRequest();
            }

            _context.Entry(folders).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoldersExists(id))
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
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Folders>>> FoldersByUserId(UsersWrite u)
        {
            var t = await _context.Folders.Where(fld=> fld.IdUser==u.Id).ToListAsync();
            foreach(Folders f in t)
            {
                f.IdParentFolderNavigation = null;
            }
            return t;
        }
        [HttpPost]
        public long GetLastID()
        {
            return _context.Folders.ToListAsync().Result.LastOrDefault().Id;     
        }
        // POST: api/Folders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Folders>> PostFolders(FoldersWrite fold)
        {
            var folders = Sync(fold).Result;
            _context.Folders.Add(folders);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FoldersExists(folders.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFolders", new { id = folders.Id , Name = folders.Name,User=folders.IdUserNavigation.Login, IdParentFolder = folders.IdParentFolder,Parent = folders.Parent});
        }

        // DELETE: api/Folders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Folders>> DeleteFolders(long id)
        {
            var folders = await _context.Folders.FindAsync(id);
            if (folders == null)
            {
                return NotFound();
            }

            _context.Folders.Remove(folders);
            await _context.SaveChangesAsync();

            return folders;
        }

        private bool FoldersExists(long id)
        {
            return _context.Folders.Any(e => e.Id == id);
        }
        private async Task<Folders> Sync(FoldersWrite fold)
        {

            var f = new Folders();
            f.Name = fold.Name;
            f.IdParentFolder = fold.IdParentFolder;
            f.IdParentFolderNavigation = await _context.Folders.Where(fld => fld.Id == fold.IdParentFolder).FirstOrDefaultAsync();
            f.IdUser = fold.IdUser;
            f.IdUserNavigation = await _context.Userss.Where(usr => usr.Id == fold.IdUser).FirstOrDefaultAsync();
            return f;
        }
        public async Task<ActionResult<Folders>> GetSharedFolder(Userss u)
        {
            return await _context.Folders.Where(f => f.Name == "Shared").FirstOrDefaultAsync();
        }
    }
}
