using DataAccess.DataContext;
using Domain.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class PollRepository
    {
        private PollDbContext myContext;
        public PollRepository(PollDbContext _context)
        {
            myContext = _context;
        }

        public void CreatePoll(Poll poll)
        {
            poll.DateCreated = DateTime.Now;
            myContext.Polls.Add(poll);
            myContext.SaveChanges();
        }

        public void Vote(Poll poll)
        {
            var originalPoll = GetPolls().First(x => x.Id == poll.Id);
            if (poll.Option1VotesCount == 1)
                originalPoll.Option1VotesCount += 1;
            else if (poll.Option1VotesCount == 2)
                originalPoll.Option2VotesCount += 1;
            else if (poll.Option1VotesCount == 3)
                originalPoll.Option3VotesCount += 1;
            myContext.Update(originalPoll);
            myContext.SaveChanges();
            
        }

        public IQueryable<Poll> GetPolls()
        {
                return myContext.Polls.AsQueryable();
        }
    }
}
