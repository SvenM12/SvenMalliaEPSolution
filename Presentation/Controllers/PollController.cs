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
            var pollList = pollFileRepository.GetPolls().OrderByDescending(x => x.DateCreated);
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
            var poll = pollFileRepository.GetPolls(id).FirstOrDefault();
            if (poll == null)
                return RedirectToAction("Index");
            return View(poll);
        }

        [HttpPost]
        [ServiceFilter(typeof(VoteActionFilter))]
        public IActionResult Vote([FromServices] PollRepository pollRepository, Poll poll)
        {
            pollRepository.Vote(poll);
            return RedirectToAction("VoteResult", new { id = poll.Id});
        }

        [HttpGet]
        public IActionResult VoteResult(int id, [FromServices] PollRepository pollRepository)
        {
            var poll = pollRepository.GetPolls(id).First();
            if (poll == null)
                return RedirectToAction("Index");
            return View(poll);
        }
    }
}
