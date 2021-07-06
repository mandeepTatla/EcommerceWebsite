using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarbleMarket.Models
{
    public class ApplicationUser : IdentityUser
    {
        public String FullName { get; set; }
    }
}
