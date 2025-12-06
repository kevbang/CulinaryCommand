using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CulinaryCommand.Models
{
    public class CreateUserForLocationRequest
    {
        public int LocationId { get; set; }

        // basic identity
        public string FirstName { get; set; } = "";
        public string LastName  { get; set; } = "";
        public string Email     { get; set; } = "";

        // "Manager" or "Employee" (you can later switch to an enum)
        public string Role      { get; set; } = "";
    }
}