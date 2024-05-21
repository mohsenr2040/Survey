using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Staff_Survey.Models.Entities
{
    public class Role : IdentityRole
    {
        public string Description { get; set; }
    }
}
