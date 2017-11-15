using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polls.Models
{
    public class JsonItem<T>
    {
        public List<T> item { get; set; }
    }

}