using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiPFE.Models;
using ApiPFE.Models.Read;

namespace ApiPFE.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WebSitesController : ControllerBase
    {
        private readonly UserContext _context;

        public WebSitesController(UserContext context)
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
        public async Task<ActionResult<WebSites>> PostWebSites(WebSitesRead WS)
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

            return CreatedAtAction("GetWebSites", new { id = webSites.Id }, webSites);
        }

        // DELETE: api/WebSites/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<WebSites>> DeleteWebSites(long id)
        {
            var webSites = await _context.WebSites.FindAsync(id);
            if (webSites == null)
            {
                return NotFound();
            }

            _context.WebSites.Remove(webSites);
            await _context.SaveChangesAsync();

            return webSites;
        }

        private bool WebSitesExists(long id)
        {
            return _context.WebSites.Any(e => e.Id == id);
        }
        private async Task<WebSites> Sync(WebSitesRead WS)
        {
            var webs = new WebSites();
            webs.Name = WS.Name;
            webs.Link = WS.Link;
            webs.IdUser = WS.IdUser;
            webs.IdUserNavigation = await _context.Userss.Where(usr => usr.Id == webs.IdUser).FirstOrDefaultAsync();
            return webs;
        }
    }
}
