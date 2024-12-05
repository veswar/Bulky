using BulkyBook.DataAccess.Data;
using BulkyBook.Model.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.DBInitializers
{
    public class DBInitializer : IDBInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DBInitializer(ApplicationDbContext applicationDbContext,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = applicationDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {
            //migration if not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }
            //create roles if they are not created
            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();
                //create admin user if not
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "eswar@monocept.com",
                    Email = "eswar@monocept.com",
                    Name = "Eswar Voleti",
                    State = "Andhra Pradesh",
                    StreetAddress = "Patapeta",
                    PostalCode = "533212"
                }, "Veeru@123").GetAwaiter().GetResult();

                var user = _db.ApplicationUser.FirstOrDefault(d => d.Email == "eswar@monocept.com");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }

            
        }
    }
}
