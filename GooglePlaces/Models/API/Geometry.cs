using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GooglePlaces.Models.API
{
    public class Geometry
    {
        public Location location { get; set; }
        public Viewport viewport { get; set; }
    }
}