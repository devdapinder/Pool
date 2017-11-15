using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polls.Models
{
    public class PublicViewProfileViewModel
    {
        public List<MyPolls> MyPolls { get; set; }
        public ViewPublicProfileResponse PublicProfileResponse { get; set; }
    }

   
}