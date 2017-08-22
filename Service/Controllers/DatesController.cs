using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DataAccess;
using DataModel;

namespace Service.Controllers
{
    public class DatesController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Dates
        public IQueryable<Date> GetDates()
        {

            return db.Dates.Include(d => d.Assignments);
        }

        // GET: api/Dates/5
        [ResponseType(typeof(Date))]
        public async Task<IHttpActionResult> GetDate(int id)
        {
            Date date = await db.Dates.FindAsync(id);
            if (date == null)
            {
                return NotFound();
            }

            return Ok(date);
        }

        // PUT: api/Dates/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDate(int id, Date date)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != date.DateId)
            {
                return BadRequest();
            }

            db.Entry(date).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Dates
        [ResponseType(typeof(Date))]
        public async Task<IHttpActionResult> PostDate(Date date)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Dates.Add(date);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = date.DateId }, date);
        }

        // DELETE: api/Dates/5
        [ResponseType(typeof(Date))]
        public async Task<IHttpActionResult> DeleteDate(int id)
        {
            Date date = await db.Dates.FindAsync(id);
            if (date == null)
            {
                return NotFound();
            }

            db.Dates.Remove(date);
            await db.SaveChangesAsync();

            return Ok(date);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DateExists(int id)
        {
            return db.Dates.Count(e => e.DateId == id) > 0;
        }
    }
}