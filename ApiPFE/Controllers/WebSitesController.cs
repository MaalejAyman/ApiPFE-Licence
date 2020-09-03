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
    public class WebSitesController : ControllerBase
    {
        private readonly userscontext _context;

        public WebSitesController(userscontext context)
        {
            _context = context;
        }

        // GET: api/WebSites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WebSites>>> GetWebSites()
        {
            return await _context.WebSites.ToListAsync();
        }

        // GET: api/WebSites/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WebSites>> GetWebSites(long id)
        {
            var webSites = await _context.WebSites.FindAsync(id);

            if (webSites == null)
            {
                return NotFound();
            }

            return webSites;
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<WebSites>>> WebSitesByUserId()
        {
            var t = await _context.WebSites.ToListAsync();
            return t;
        }

        // PUT: api/WebSites/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWebSites(long id, WebSites webSites)
        {
            if (id != webSites.Id)
            {
                return BadRequest();
            }

            _context.Entry(webSites).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WebSitesExists(id))
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

        // POST: api/WebSites
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<WebSites>> PostWebSites(WebSitesWrite WS)
        {
            var webSites = Sync(WS).Result;
            _context.WebSites.Add(webSites);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (WebSitesExists(webSites.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetWebSites", new { id = webSites.Id });
        }
        [HttpPost]
        public async Task<ActionResult<WebSites>> UpdateWebSites(WebSitesWrite WS)
        {
            var webSites = Sync(WS).Result;
            var w = await _context.WebSites.FindAsync(WS.Id);
            w.Name = webSites.Name;
            w.Link = webSites.Link;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

            }
            return CreatedAtAction("GetWebSites", new { id = webSites.Id });
        }

        // DELETE: api/WebSites/5
        [HttpPost]
        public async Task<ActionResult<long>> DeleteWebSites(WebSitesWrite ws)
        {
            var webSites = await _context.WebSites.FindAsync(ws.Id);
            var p = await _context.Passwords.Where(pass => pass.IdWs == ws.Id).ToListAsync();
            if (p.Count() != 0)
            {
                return 0;
            }
            if (webSites == null)
            {
                return NotFound();
            }

            _context.WebSites.Remove(webSites);
            await _context.SaveChangesAsync();

            return 1;
        }

        private bool WebSitesExists(long id)
        {
            return _context.WebSites.Any(e => e.Id == id);
        }
        private async Task<WebSites> Sync(WebSitesWrite WS)
        {
            var webs = new WebSites();
            webs.Name = WS.Name;
            webs.Link = WS.Link;
            return webs;
        }
    }
}
