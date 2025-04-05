using DataAccess.DataContext;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.ActionFilters
{
    public class VoteActionFilter : ActionFilterAttribute
    {
        private readonly PollDbContext _pollDbContext;

        public VoteActionFilter(PollDbContext pollDbContext)
        {
            _pollDbContext = pollDbContext;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var poll = context.ActionArguments["poll"] as Poll;
            if (poll != null)
            {
                var userEmail = context.HttpContext.User.Identity.Name;
                var list = _pollDbContext.Votes.ToList();
                var existingVote = list.Find(x => x.PollId == poll.Id && x.Email == userEmail);
                if (existingVote == null)
                {
                    Vote vote = new Vote
                    {
                        PollId = poll.Id,
                        Email = userEmail,
                    };
                    _pollDbContext.Votes.Add(vote);
                    _pollDbContext.SaveChanges();
                }
                else
                {
                    context.Result = new RedirectToActionResult("Index", "Poll", null);
                    return;
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
