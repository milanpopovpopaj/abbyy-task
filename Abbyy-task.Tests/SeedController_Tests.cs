using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Abbyy_task.Controllers;
using Abbyy_task.Data;
using Abbyy_task.Data.Models;
using Xunit;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Abbyy_task.Tests
{
    public class SeedController_Tests
    {
        /// <summary>
        /// Test the CreateDefaultUsers() method
        /// </summary>
        [Fact]
        public async void CreateDefaultUsers()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AbbyyTask")
                .Options;

            var storeOptions = Options.Create(new OperationalStoreOptions());

            // create a IWebHost environment mock instance
            var mockEnv = new Mock<IWebHostEnvironment>().Object;

            // define the variables for the users we want to test
            ApplicationUser user_Admin = null;
            ApplicationUser user_User1 = null;
            ApplicationUser user_User2 = null;
            ApplicationUser user_NotExisting = null;

            // Act
            // create a ApplicationDbContext instance using the in-memory DB
            using (var context = new ApplicationDbContext(options, storeOptions))
            {
                // create a RoleManager instance
                var roleManager = IdentityHelper.GetRoleManager(new RoleStore<IdentityRole>(context));

                // create a UserManager instance
                var userManager = IdentityHelper.GetUserManager(new UserStore<ApplicationUser>(context));

                // create a SeedController instance
                var controller = new SeedController(
                    context,
                    roleManager,
                    userManager,
                    mockEnv
                );

                // execute the SeedController's CreateDefaultUsers()
                // method to create the default users (and roles)
                await controller.CreateDefaultUsers();

                // retrieve the users
                user_Admin = await userManager.FindByEmailAsync("admin@email.com");
                user_User1 = await userManager.FindByEmailAsync("milan@email.com");
                user_User2 = await userManager.FindByEmailAsync("popov@email.com");
                user_NotExisting = await userManager.FindByEmailAsync("notexisting@email.com");
            }

            // Assert
            Assert.NotNull(user_Admin);
            Assert.NotNull(user_User1);
            Assert.NotNull(user_User2);
            Assert.Null(user_NotExisting);
        }


        /// <summary>
        /// Test the Import() method
        /// </summary>
        //[Fact]
        //public async void Import()
        //{
        //    // Arrange
        //    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //        .UseInMemoryDatabase(databaseName: "AbbyyTask")
        //        .Options;

        //    var storeOptions = Options.Create(new OperationalStoreOptions());

        //    // create a IWebHost environment mock instance
        //    var mockEnv = new Mock<IWebHostEnvironment>().Object;

        //    //string solution_dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

        //    // define the variables for the users we want to test
        //    int imported_Products_count = 0;

        //    // Act
        //    // create a ApplicationDbContext instance using the in-memory DB
        //    using (var context = new ApplicationDbContext(options, storeOptions))
        //    {
        //        // create a RoleManager instance
        //        var roleManager = IdentityHelper.GetRoleManager(new RoleStore<IdentityRole>(context));

        //        // create a UserManager instance
        //        var userManager = IdentityHelper.GetUserManager(new UserStore<ApplicationUser>(context));

        //        // create a SeedController instance
        //        var controller = new SeedController(
        //            context,
        //            roleManager,
        //            userManager,
        //            mockEnv
        //        );

        //        // execute the SeedController's Import()
        //        await controller.Import();

        //        // retrieve the imported products count
        //        imported_Products_count = context.Products.CountAsync().Result;
        //    }

        //    // Assert
        //    Assert.True(imported_Products_count == 100);
        //}
    }
}