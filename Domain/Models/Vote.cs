using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Vote
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public int? PollId { get; set; }
    }
}
