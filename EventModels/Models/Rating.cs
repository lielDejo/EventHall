using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventModels
{
    public class Rating : Model
    {
        public int Score { get; set; }
        public string UserId { get; set; }
        public string HallId { get; set; }

    }
}
