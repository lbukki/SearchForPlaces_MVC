using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GooglePlaces.Models.API
{
    public class Places
    {
        public List<object> html_attributions { get; set; }
        public string next_page_token { get; set; }
        public List<Result> results { get; set; }
        public string status { get; set; }
    }
}