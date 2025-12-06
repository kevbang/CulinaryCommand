using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CulinaryCommand.Models
{
    public class FullSignupRequest
    {
        public CompanySignupModel Company { get; set; } = new();
        public LocationSignupModel Location { get; set; } = new();
        public AdminSignupModel Admin { get; set; } = new();
    }
}