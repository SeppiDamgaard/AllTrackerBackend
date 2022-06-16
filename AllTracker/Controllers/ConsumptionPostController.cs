using AllTracker.Models;
using AllTracker.Controllers.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumptionPostController : Controller
    {
        AllTrackerDBContext dbContext;
        IHttpContextAccessor httpContextAccessor;
        UserManager<User> userManager;

        public ConsumptionPostController(
            AllTrackerDBContext dbContext, 
            IHttpContextAccessor httpContextAccessor, 
            UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        [Authorize]
        [HttpPost("CreatePost")]
        public async Task<ActionResult<ConsumptionPost>> CreatePost([FromBody] ConsumptionPostRequest request)
        {
            ConsumptionPost consumptionPost = new ConsumptionPost
            {
                Name = request.Name,
                Unit = request.Unit,
                User = await GetCurrentUser(),
                CronString = request.CronString,
                FirstIncrement = request.FirstIncrement,
                SecondIncrement = request.SecondIncrement,
                ThirdIncrement = request.ThirdIncrement,
            };

            dbContext.ConsumptionPost.Add(consumptionPost);
            await dbContext.SaveChangesAsync();
            return Ok(consumptionPost);
        }


        [Authorize]
        [HttpGet("GetPosts")]
        public async Task<ActionResult<IEnumerable<ConsumptionPost>>> GetPosts()
        {
            var curUser = await GetCurrentUser();

            var posts = dbContext.ConsumptionPost.Where(cp => cp.User.Id == curUser.Id).Select(cp => new ConsumptionPost
            {
                Id = cp.Id,
                Name = cp.Name,
                Unit = cp.Unit,
                CronString = cp.CronString,
                FirstIncrement = cp.FirstIncrement,
                SecondIncrement = cp.SecondIncrement,
                ThirdIncrement = cp.ThirdIncrement,
            });
            return Ok(posts);
        }

        [Authorize]
        [HttpDelete("DeletePost/{id}")]
        public async Task<ActionResult<ConsumptionPost>> DeletePost([FromRoute]string id)
        {
            var post = await dbContext.ConsumptionPost.FindAsync(id);
            var registrations = dbContext.ConsumptionRegistration.Where(cr => cr.Post.Id == post.Id);
            dbContext.ConsumptionRegistration.RemoveRange(registrations);
            dbContext.ConsumptionPost.Remove(post);
            await dbContext.SaveChangesAsync();
            return Ok(post);
        }

        [Authorize]
        [HttpPut("PutPost")]
        public async Task<ActionResult<ConsumptionPost>> PutPost([FromBody] ConsumptionPostRequest request)
        {
            var post = await dbContext.ConsumptionPost.FindAsync(request.Id);
            post.Name = request.Name;
            post.CronString = request.CronString;
            post.FirstIncrement = request.FirstIncrement;
            post.SecondIncrement = request.SecondIncrement;
            post.ThirdIncrement = request.ThirdIncrement;
            post.Unit = request.Unit;

            await dbContext.SaveChangesAsync();
            return Ok(post);
        }


        private async Task<User> GetCurrentUser()
        {
            return await userManager.GetUserAsync(User);
        }
    }
}
