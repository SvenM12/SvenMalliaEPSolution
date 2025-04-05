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
            var originalPoll = GetPolls(poll.Id).First();
            if (poll.Option1VotesCount == 1)
                originalPoll.Option1VotesCount += 1;
            else if (poll.Option1VotesCount == 2)
                originalPoll.Option2VotesCount += 1;
            else if (poll.Option1VotesCount == 3)
                originalPoll.Option3VotesCount += 1;
            myContext.Update(originalPoll);
            myContext.SaveChanges();
            
        }

        public IQueryable<Poll> GetPolls(int id = -1)
        {
            if (id == -1)
            {
                return myContext.Polls.Select(x => new Poll
                {
                    Title = x.Title,
                    Id = x.Id,
                    DateCreated = x.DateCreated,
                }).AsQueryable();
            }
            else
            {
                return myContext.Polls.Where(x => x.Id == id)
                    .Select(x => new Poll
                    {
                        Title = x.Title,
                        Id = x.Id,
                        Option1Text = x.Option1Text,
                        Option2Text = x.Option2Text,
                        Option3Text = x.Option3Text,
                        Option1VotesCount = x.Option1VotesCount,
                        Option2VotesCount = x.Option2VotesCount,
                        Option3VotesCount = x.Option3VotesCount,
                        DateCreated = x.DateCreated
                    }).AsQueryable();
            }
        }
    }
}
