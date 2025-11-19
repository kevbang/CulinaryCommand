using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CulinaryCommand.Data.Entities
{
    public class UserLocation
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}