using DataAccess.DataContext;
using Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class PollFileRepository
    {
        private string fileName = "polls.json";
        private PollDbContext myContext;
        public PollFileRepository(PollDbContext _context)
        {
            myContext = _context;
        }

        public void CreatePoll(Poll poll)
        {
            poll.DateCreated = DateTime.Now;
            if (!System.IO.File.Exists(fileName))
            {
                using (var myFile = File.CreateText(fileName))
                {
                    List<Poll> pollList = new List<Poll>();
                    poll.Id = 1;
                    pollList.Add(poll);
                    string fileContents = JsonConvert.SerializeObject(pollList);
                    myFile.Write(fileContents);
                }
            }
            else
            {
                var polls = GetPolls().ToList();
                int nextId = polls.Max(l => l.Id) + 1;
                poll.Id = nextId;
                polls.Add(poll);
                string fileContent = JsonConvert.SerializeObject(polls);
                File.WriteAllText(fileName, fileContent);
            }
        }

        public IQueryable<Poll> GetPolls(int id = -1)
        {
            List<Poll> polls = new List<Poll>();
            string fileContents = "";
            if (!File.Exists(fileName))
            {
                return polls.AsQueryable();
            }

            using (var myFile = File.OpenText(fileName))
            {
                fileContents = myFile.ReadToEnd();
            }

            if (string.IsNullOrEmpty(fileContents))
                return polls.AsQueryable();
            else
            {
                polls = JsonConvert.DeserializeObject<List<Poll>>(fileContents);
                if (id == -1)
                {
                    return polls.Select(x => new Poll
                    {
                        Title = x.Title,
                        Id = x.Id,
                        DateCreated = x.DateCreated,
                    }).AsQueryable();
                }
                else
                {
                    return polls.Where(x => x.Id == id).Select(x => new Poll
                    {
                        Title = x.Title,
                        Id = x.Id,
                        Option1Text = x.Option1Text,
                        Option2Text = x.Option2Text,
                        Option3Text = x.Option3Text,
                        Option1VotesCount = x.Option1VotesCount,
                        Option2VotesCount = x.Option2VotesCount,
                        Option3VotesCount = x.Option3VotesCount,
                    }).AsQueryable();
                }
            }
        }

        public void Vote(Poll poll)
        {
            string fileContents = File.ReadAllText(fileName);
            var data = JsonConvert.DeserializeObject<List<Poll>>(fileContents);
            if (data != null)
            {
                var originalPoll = data.Find(x => x.Id == poll.Id);
                if (originalPoll != null)
                {
                    if (poll.Option1VotesCount == 1)
                        originalPoll.Option1VotesCount += 1;
                    else if (poll.Option1VotesCount == 2)
                        originalPoll.Option2VotesCount += 1;
                    else if (poll.Option1VotesCount == 3)
                        originalPoll.Option3VotesCount += 1;
                    fileContents = JsonConvert.SerializeObject(data);
                    File.WriteAllText(fileName, fileContents);
                }
            }
        }
    }
}
