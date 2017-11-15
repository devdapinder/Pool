using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polls.Models
{
    public class PollResultViewModel
    {
        public MyPolls myPolls { get; set; }
        public List<PollResult> PollResults { get; set; }
    }
}