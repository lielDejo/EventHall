using EventModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventModels
{
    public class EventHallViewModel
    {
        public List<EventHall> Halls { get; set; }
        public List<City> Citys { get; set; }
        public int MinCapacity { get; set; }
        public int MaxCapacity { get; set; }
        public int Grade { get; set; }
        public string Type { get; set; }
        

    }
}
