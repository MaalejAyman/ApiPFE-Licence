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
    public class NotesController : ControllerBase
    {
        private readonly userscontext _context;

        public NotesController(userscontext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notes>>> GetNotes()
        {
            return await _context.Notes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Notes>> GetNotes(long id)
        {
            var Notes = await _context.Notes.FindAsync(id);

            if (Notes == null)
            {
                return NotFound();
            }

            return Notes;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Notes>>> NotesByUserId(Userss u)
        {
            var t = await _context.Notes.Where(fld => fld.IdUser == u.Id).ToListAsync();
            foreach (Notes f in t)
            {
                f.IdUserNavigation = null;
            }
            return t;
        }

        [HttpPost]
        public async Task<ActionResult<Notes>> UpdateNotes(Notes note)
        {
            var n = await _context.Notes.FindAsync(note.Id);
            n.Text = note.Text;
            n.IdUser = note.IdUser;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

            }
            return Ok();
        }
        [HttpPost]
        public async Task<ActionResult<Notes>> DeleteNotes(Notes note)
        {
            var n = await _context.Notes.FindAsync(note.Id);
            if (n == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(n);
            await _context.SaveChangesAsync();
            return Ok();
        }
        
        [HttpPost]
        public async Task<ActionResult<Notes>> PostNotes(Notes notes)
        {
            _context.Notes.Add(notes);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (NotesExists(notes.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetNotes", new { id = notes.Id });
        }

        private bool NotesExists(long id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }
    }
}
