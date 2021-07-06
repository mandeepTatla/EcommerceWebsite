using MarbleMarket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarbleMarket.Data.Intializer
{
    public class DbInitializer : IDbInitializer
    {
        // Need access to database along with user manager and user role that we dealing inside the register page
        //Use dependency injection to get all the three required fields
        
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        //Configure the initializer method 
        public void Initialize()
        {
            try
            {
                // check if there is any pending migration and if there is any migrate them
                if(_db.Database.GetPendingMigrations().Count()> 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch(Exception ex)
            {

            }
            if (_db.Roles.Any(r => r.Name == WC.AdminRole)) return;

            // After pushing all the pending migration, use role manager

            _roleManager.CreateAsync(new IdentityRole(WC.AdminRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(WC.CustomerRole)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                FullName = "Mandeep Tatla"

            },"Admin01@").GetAwaiter().GetResult();

            ApplicationUser user = _db.ApplicationUser.Where(u => u.Email == "admin@gmail.com").FirstOrDefault();

            _userManager.AddToRoleAsync(user, WC.AdminRole).GetAwaiter().GetResult();
        }
    }
}
