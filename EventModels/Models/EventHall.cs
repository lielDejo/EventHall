using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventModels
{
    public class EventHall : Model
    {
        public string HallName { get; set; }
        public List<Image_> HallImage { get; set; }
        public string GeographicalLocation { get; set; }
        public string PeopleContent { get; set; }
        public int Rating { get; set; }
        public string TypeHall { get; set; }
        public string DescriptionHall { get; set; }
        public int City { get; set; } = -1;
        public string cityName { get; set; } = "";
        public string OwnerId { get; set; } = " ";
    }
}

