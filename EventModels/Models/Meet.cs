using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventModels
{
    public class Meet : Model
    {
        public string HallId { get; set; }
        public string UserId { get; set; }
        public string DateMeet { get; set; }
        public string Hour { get; set; }
        public string ReasonMeet { get; set; }
        public string MeetingSummary { get; set; } = " ";
    }
}
