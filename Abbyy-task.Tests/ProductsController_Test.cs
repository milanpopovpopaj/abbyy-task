using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Abbyy_task.Controllers;
using Abbyy_task.Data;
using Abbyy_task.Data.Models;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace Abbyy_task.Tests
{
    public class ProductsController_Tests
    {
        /// <summary>
        /// Test the GetProduct() method
        /// </summary>
        [Fact]
        public async void GetProduct()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AbbyyTask")
                .Options;

            var storeOptions = Options.Create(new OperationalStoreOptions());
            using (var context = new ApplicationDbContext(options, storeOptions))
            {
                context.Add(new Product()
                {
                    Id = 1,
                    Name = "Test product 1",
                    Price = 22.22M,
                    Description = "Test product 1 Description"
                });
                context.SaveChanges();
            }

            Product product_existing = null;
            Product product_notExisting = null;

            // Act
            using (var context = new ApplicationDbContext(options, storeOptions))
            {
                var controller = new ProductsController(context);
                product_existing = (await controller.GetProduct(1)).Value;
                product_notExisting = (await controller.GetProduct(111)).Value;
            }

            // Assert
            Assert.NotNull(product_existing);
            Assert.Null(product_notExisting);
        }

        /// <summary>
        /// Test the PutProduct() method
        /// </summary>
        [Fact]
        public async void PutProduct()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AbbyyTask")
                .Options;

            var storeOptions = Options.Create(new OperationalStoreOptions());
            using (var context = new ApplicationDbContext(options, storeOptions))
            {
                context.Add(new Product()
                {
                    Id = 2,
                    Name = "Test product 2",
                    Price = 22.22M,
                    Description = "Test product 2 Description"
                });
                context.SaveChanges();
            }

            Product changed_product = new Product()
            {
                Id = 2,
                Name = "Test product 2 changed",
                Price = 33.33M,
                Description = "Test product 2 Description changed"
            };

            Product updated_product = null;

            // Act
            using (var context = new ApplicationDbContext(options, storeOptions))
            {
                var controller = new ProductsController(context);
                await controller.PutProduct(2, changed_product);

                updated_product = (await controller.GetProduct(2)).Value;
            }

            // Assert
            Assert.NotNull(updated_product);
            Assert.Equal(updated_product.Name, "Test product 2 changed");
            Assert.Equal(updated_product.Price, 33.33M);
            Assert.Equal(updated_product.Description, "Test product 2 Description changed");
        }


        /// <summary>
        /// Test the PostProduct() method
        /// </summary>
        [Fact]
        public async void PostProduct()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AbbyyTask")
                .Options;

            var storeOptions = Options.Create(new OperationalStoreOptions());

            Product product = new Product()
            {
                Id = 3,
                Name = "Test product 3",
                Price = 22.22M,
                Description = "Test product 3 Description"
            };

            Product new_product = null;

            // Act
            using (var context = new ApplicationDbContext(options, storeOptions))
            {
                var controller = new ProductsController(context);
                await controller.PostProduct(product);

                new_product = (await controller.GetProduct(3)).Value;
            }

            // Assert
            Assert.NotNull(new_product);
            Assert.Equal(new_product.Id, 3);
            Assert.Equal(new_product.Name, "Test product 3");
        }


        /// <summary>
        /// Test the DeleteProduct() method
        /// </summary>
        [Fact]
        public async void DeleteProduct()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AbbyyTask")
                .Options;

            var storeOptions = Options.Create(new OperationalStoreOptions());
            using (var context = new ApplicationDbContext(options, storeOptions))
            {
                context.Add(new Product()
                {
                    Id = 4,
                    Name = "Test product 4",
                    Price = 44.44M,
                    Description = "Test product 4 Description"
                });
                context.SaveChanges();
            }

            Product product_existing = null;

            // Act
            using (var context = new ApplicationDbContext(options, storeOptions))
            {
                var controller = new ProductsController(context);
                await controller.DeleteProduct(4);

                product_existing = (await controller.GetProduct(4)).Value;
            }

            // Assert
            Assert.Null(product_existing);
        }


        /// <summary>
        /// Test the GetProducts() method
        /// </summary>
        [Fact]
        public async void GetProducts()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AbbyyTask")
                .Options;

            var storeOptions = Options.Create(new OperationalStoreOptions());
            using (var context = new ApplicationDbContext(options, storeOptions))
            {
                for (int i = 10; i < 20; i++)
                {
                    context.Add(new Product()
                    {
                        Id = i,
                        Name = "Test product " + i,
                        Price = 22.22M,
                        Description = "Test product " + i + " Description"
                    });
                }

                context.SaveChanges();
            }

            List<Product> products_existing = null;

            // Act
            using (var context = new ApplicationDbContext(options, storeOptions))
            {
                var controller = new ProductsController(context);
                products_existing = (await controller.GetProducts()).Value.Data;
            }

            // Assert
            Assert.True(products_existing.Any());
        }
    }
}