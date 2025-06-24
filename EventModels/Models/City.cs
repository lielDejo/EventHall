using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventModels
{
    public class City :Model
    {
        public string CityName { get; set; }
        public List<EventHall> Halls { get; set; }
    }
}
