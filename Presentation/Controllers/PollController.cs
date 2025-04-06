using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;

namespace Presentation.Controllers
{
    public class PollController : Controller
    {
        public IActionResult Index([FromServices] PollFileRepository pollFileRepository)
        {
            var pollList = pollFileRepository.GetPolls().OrderByDescending(x => x.DateCreated).Select(x => new Poll
            {
                Id = x.Id,
                DateCreated = x.DateCreated,
                Title = x.Title,
            });
            return View(pollList);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromServices] PollFileRepository pollFileRepository, Poll poll)
        {
            if (ModelState.IsValid)
            {
                pollFileRepository.CreatePoll(poll);
                return RedirectToAction("Index");
            }
            return View(poll);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Vote(int id, [FromServices] PollFileRepository pollFileRepository)
        {
            var poll = pollFileRepository.GetPolls().Where(x => x.Id == id).Select(x => new Poll
            {
                Id = x.Id,
                Title = x.Title,
                DateCreated = x.DateCreated,
                Option1Text = x.Option1Text,
                Option2Text = x.Option2Text,
                Option3Text = x.Option3Text,
            }).First();
            if (poll == null)
                return RedirectToAction("Index");
            return View(poll);
        }

        [HttpPost]
        [ServiceFilter(typeof(VoteActionFilter))]
        public IActionResult Vote([FromServices] PollFileRepository pollRepository, Poll poll)
        {
            pollRepository.Vote(poll);
            return RedirectToAction("VoteResult", new { id = poll.Id});
        }

        [HttpGet]
        public IActionResult VoteResult(int id, [FromServices] PollFileRepository pollRepository)
        {
            var poll = pollRepository.GetPolls().Select(x => new Poll
            {
                Id = x.Id,
                Title = x.Title,
                Option1Text = x.Option1Text,
                Option2Text = x.Option2Text,
                Option3Text = x.Option3Text,
                Option1VotesCount = x.Option1VotesCount,
                Option2VotesCount = x.Option2VotesCount,
                Option3VotesCount = x.Option3VotesCount

            }).First(x => x.Id == id);
            if (poll == null)
                return RedirectToAction("Index");
            return View(poll);
        }
    }
}
