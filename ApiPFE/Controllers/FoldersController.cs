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
        private PasswordsController pc;

        public FoldersController(userscontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Folders>>> GetFolders()
        {
            return await _context.Folders.ToListAsync();
        }

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

            return CreatedAtAction("GetFolders", new { Id = folders.Id, Name = folders.Name, User = folders.IdUserNavigation.Login, IdParentFolder = folders.IdParentFolder, Parent = folders.Parent });
        }
        [HttpPost]
        public async Task<ActionResult<long>> RenameFolders(FoldersWrite fold)
        {
            var folders = await _context.Folders.FindAsync(fold.Id);
            folders.Name = fold.Name;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return 1;
        }
        * [HttpPost]
        public async Task<ActionResult<long>> MoveFolders(FoldersWrite fold)
        {
            var folders = await _context.Folders.FindAsync(fold.Id);
            folders.IdParentFolder = fold.IdParentFolder;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return 1;
        }
        [HttpPost]
        public async Task<ActionResult<long>> DeleteFolders(Folders f)
        {
            var folders = await _context.Folders.FindAsync(f.Id);
            var Passwords = await _context.Passwords.Where((e)=>e.IdFldr==f.Id).ToListAsync();
            var Folders = await _context.Folders.Where((e) => e.IdParentFolder == f.Id).ToListAsync();
            if (Passwords!=null)
            {
                foreach (Passwords p in Passwords)

                {
                    var t = await _context.GroupesPasswords.Where(GP => GP.IdPass == p.Id).ToListAsync();
                    foreach (GroupesPasswords GP1 in t)
                    {
                        _context.GroupesPasswords.Remove(GP1);
                    }
                    _context.Passwords.Remove(p);
                    await _context.SaveChangesAsync();
                }
            }
            if (Folders != null)
            {
                foreach (Folders fold in Folders)
                {
                    await this.DeleteFolders(fold);
                }
            }
            if (folders == null)
            {
                return NotFound();
            }

            _context.Folders.Remove(folders);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }

            return 1;
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
