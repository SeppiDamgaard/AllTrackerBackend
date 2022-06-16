using AllTracker.Controllers.Requests;
using AllTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AllTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumptionRegistrationController : Controller
    {
        AllTrackerDBContext _context;
        IHttpContextAccessor httpContextAccessor;
        UserManager<User> userManager;

        public ConsumptionRegistrationController(AllTrackerDBContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            this._context = dbContext;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpPost("CreateRegistration")]
        public async Task<ActionResult<ConsumptionRegistration>> PostRegistration([FromBody] ConsumptionRegistrationRequest request)
        {
            ConsumptionRegistration registration = await CreateRegistration(request.PostId, request.Amount, request.Date);
            return Ok(registration);
        }

        [Authorize]
        [HttpGet("GetRegistrations/{postId}")]
        public ActionResult<IEnumerable<ConsumptionRegistrationRequest>> GetRegistrations([FromRoute]string postId)
        {
            var registrations = _context.ConsumptionRegistration.Where(cr => cr.Post.Id == postId).Select(cr => new ConsumptionRegistrationRequest
            {
                Id = cr.Id,
                PostId = cr.Post.Id,
                Amount = cr.Amount,
                Date = cr.Date
            });
            var test = registrations.ToList();
            return Ok(test);
        }

        [Authorize]
        [HttpGet("GetDailyRegistration")]
        public async Task<ActionResult<ConsumptionRegistrationRequest>> GetDailyRegistration(
            [FromQuery(Name = "postId")] string postId,
            [FromQuery(Name = "date")] DateTime date)
        {
            var registration = _context.ConsumptionRegistration
                .FirstOrDefault(cr => cr.Post.Id == postId && cr.Date.Date == date.Date);

            if (registration == null) registration = await CreateRegistration(postId, 0, date);

            ConsumptionRegistrationRequest response = new ConsumptionRegistrationRequest
            {
                Id = registration.Id,
                PostId = postId,
                Amount = registration.Amount,
                Date = registration.Date
            };
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("DeleteRegistration")]
        public async Task<ActionResult<ConsumptionRegistration>> DeleteRegistration([FromRoute] string id)
        {
            var reg = await _context.ConsumptionRegistration.FindAsync(id);
            _context.ConsumptionRegistration.Remove(reg);
            await _context.SaveChangesAsync();
            return Ok(reg);
        }


        [Authorize]
        [HttpPatch("PatchRegistration")]
        public async Task<ActionResult<ConsumptionRegistration>> PatchRegistration([FromBody] ConsumptionRegistrationRequest request)
        {
            var reg = await _context.ConsumptionRegistration.Include(cr => cr.Post).FirstOrDefaultAsync(cr => cr.Id == request.Id);

            reg.Amount = request.Amount;
            await _context.SaveChangesAsync();
            return Ok(new ConsumptionRegistrationRequest
            {
                Id = reg.Id,
                PostId = reg.Post.Id,
                Amount = reg.Amount,
                Date = reg.Date
            });
        }

        private async Task<User> GetCurrentUser()
        {
            return await userManager.GetUserAsync(User);
        }

        private async Task<ConsumptionRegistration> CreateRegistration(string postId, double amount, DateTime date)
        {
            ConsumptionRegistration registration = new ConsumptionRegistration
            {
                Post = _context.ConsumptionPost.Find(postId),
                Amount = amount,
                Date = date,
            };
            _context.ConsumptionRegistration.Add(registration);
            await _context.SaveChangesAsync();
            return registration;
        }
    }
}
