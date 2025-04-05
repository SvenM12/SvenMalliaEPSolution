using Domain.Models;

namespace Presentation.Models
{
    public class PollViewModel
    {
        public Poll Poll { get; set; }
        public string UserEmail { get; set; }
        public bool HasVoted { get; set; }
    }
}
