using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Abbyy_task.Data;
using Abbyy_task.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace Abbyy_task.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public SeedController(
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment env)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _env = env;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Import()
        {
            // prevents non-development environments from running this method
            //if (!_env.IsDevelopment())
            //    throw new SecurityException("Not allowed");

            var path = Path.Combine(_env.ContentRootPath, "Data/Source/Products.xlsx");
            using var stream = System.IO.File.OpenRead(path);
            using var excelPackage = new ExcelPackage(stream);

            // get the first worksheet
            var worksheet = excelPackage.Workbook.Worksheets[0];

            // define how many rows we want to process
            var nEndRow = worksheet.Dimension.End.Row;

            // initialize the record counters
            var numberOfProductsAdded = 0;

            // create a lookup dictionary
            // containing all the products already existing
            // into the Database (it will be empty on first run).
            var products = _context.Products
                .AsNoTracking()
                .ToDictionary(x => (
                    Name: x.Name,
                    Price: x.Price,
                    Description: x.Description));

            // iterates through all rows, skipping the first one
            for (int nRow = 2; nRow <= nEndRow; nRow++)
            {
                var row = worksheet.Cells[nRow, 1, nRow, worksheet.Dimension.End.Column];

                var name = row[nRow, 1].GetValue<string>();
                var priceStr = row[nRow, 2].GetValue<string>();
                var price = decimal.Parse(priceStr.Replace("$", ""));
                var description = row[nRow, 3].GetValue<string>();

                // skip this product if it already exists in the database
                if (products.ContainsKey((
                    Name: name,
                    Price: price,
                    Description: description)))
                    continue;

                // create the Product entity and fill it with xlsx data
                var product = new Product
                {
                    Name = name,
                    Price = price,
                    Description = description
                };

                // add the new product to the DB context
                _context.Products.Add(product);

                // increment the counter
                numberOfProductsAdded++;
            }
            // save all the products into the Database
            if (numberOfProductsAdded > 0)
                await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                Products = numberOfProductsAdded
            });
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> CreateDefaultUsers()
        {
            // setup the default role names
            string role_RegisteredUser = "RegisteredUser";
            string role_Administrator = "Administrator";

            // create the default roles (if they don't exist yet)
            if (await _roleManager.FindByNameAsync(role_RegisteredUser) == null)
                await _roleManager.CreateAsync(new IdentityRole(role_RegisteredUser));

            if (await _roleManager.FindByNameAsync(role_Administrator) == null)
                await _roleManager.CreateAsync(new IdentityRole(role_Administrator));

            // create a list to track the newly added users
            var addedUserList = new List<ApplicationUser>();

            // check if the admin user already exists
            var email_Admin = "admin@email.com";
            if (await _userManager.FindByNameAsync(email_Admin) == null)
            {
                // create a new admin ApplicationUser account
                var user_Admin = new ApplicationUser()
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = email_Admin,
                    Email = email_Admin,
                };

                // insert the admin user into the DB
                await _userManager.CreateAsync(user_Admin, "Pa$$w0rd");

                // assign the "RegisteredUser" and "Administrator" roles
                await _userManager.AddToRoleAsync(user_Admin, role_RegisteredUser);
                await _userManager.AddToRoleAsync(user_Admin, role_Administrator);

                // confirm the e-mail and remove lockout
                user_Admin.EmailConfirmed = true;
                user_Admin.LockoutEnabled = false;

                // add the admin user to the added users list
                addedUserList.Add(user_Admin);
            }

            // check if the first standard user already exists
            var email_User1 = "milan@email.com";
            if (await _userManager.FindByNameAsync(email_User1) == null)
            {
                // create a new standard ApplicationUser account
                var user_User = new ApplicationUser()
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = email_User1,
                    Email = email_User1
                };

                // insert the standard user into the DB
                await _userManager.CreateAsync(user_User, "Pa$$w0rd");

                // assign the "RegisteredUser" role
                await _userManager.AddToRoleAsync(user_User, role_RegisteredUser);

                // confirm the e-mail and remove lockout
                user_User.EmailConfirmed = true;
                user_User.LockoutEnabled = false;

                // add the standard user to the added users list
                addedUserList.Add(user_User);
            }

            // check if the second standard user already exists
            var email_User2 = "popov@email.com";
            if (await _userManager.FindByNameAsync(email_User2) == null)
            {
                // create a new standard ApplicationUser account
                var user_User2 = new ApplicationUser()
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = email_User2,
                    Email = email_User2
                };

                // insert the standard user into the DB
                await _userManager.CreateAsync(user_User2, "Pa$$w0rd");

                // assign the "RegisteredUser" role
                await _userManager.AddToRoleAsync(user_User2, role_RegisteredUser);

                // confirm the e-mail and remove lockout
                user_User2.EmailConfirmed = true;
                user_User2.LockoutEnabled = false;

                // add the standard user to the added users list
                addedUserList.Add(user_User2);
            }

            // if we added at least one user, persist the changes into the DB
            if (addedUserList.Count > 0)
                await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                Count = addedUserList.Count,
                Users = addedUserList
            });
        }
    }
}
