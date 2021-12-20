using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Renomax.Models
{
    public class SearchResult
    {
        public string Label { get; set; }
        public object Value { get; set; }
    }
    public class GroupResult
    {
        public SearchResult[] Result { get; set; }
        public object Tracking { get; set; }
    }
}
