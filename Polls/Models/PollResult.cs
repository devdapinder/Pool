using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polls.Models
{

    public class PollResult
    {
        public int thumbsStatus { get; set; }
        public int pollId { get; set; }
        public string ownerId { get; set; }
        public int pollDetailId { get; set; }
        public string workerId { get; set; }
        public DateTime acceptTime { get; set; }
        public int choice { get; set; }
        public string answerText { get; set; }
        public int thumbsUp { get; set; }
        public int thumbsDown { get; set; }
        public bool enablePublic { get; set; }
        public int idCat { get; set; }
        public string gender { get; set; }
        public string age { get; set; }
        public string income { get; set; }
        public string ethnicity { get; set; }
        public string education { get; set; }
        public string relationshipStatus { get; set; }
        public string numberofChildren { get; set; }
        public string smoking { get; set; }
        public string dietType { get; set; }
        public string homeType { get; set; }
        public string community { get; set; }
        public string politics { get; set; }
        public string mobileDevice { get; set; }
        public string literaryPreference { get; set; }
        public string booksReadPerMonth { get; set; }
        public string employmentStatus { get; set; }
    }

}