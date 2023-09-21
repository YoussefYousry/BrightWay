using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_DAL.Models
{
    public class User : IdentityUser
    {
        public string ImageUrl { get; set; } = "NotFound";
    }
}
