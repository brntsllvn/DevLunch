using System;
using System.Linq;
using DevLunch.Controllers.api;
using DevLunch.Data;
using DevLunch.Data.Models;
using FizzWare.NBuilder;
using NUnit.Framework;
using Shouldly;

namespace DevLunch.Tests.Controllers.api
{
    [TestFixture]
    public class ProductControllerTests
    {
        [Test]
        public void Get_NoParameters_ReturnsAllProduct()
        {
            // Arrange
            var context = new GeneralStoreDbContext(Effort.DbConnectionFactory.CreateTransient());
            context.Products.Add(new Product {Name = "one"});
            context.SaveChanges();

            var controller = new ProductController(context);

            // Act
            var result = controller.Get();

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(1);
            result.First().Name.ShouldBe("one");
        }

        [Test]
        public void Get_WithParam_ReturnsOneProduct()
        {
            // Arrange
            var context = new GeneralStoreDbContext(Effort.DbConnectionFactory.CreateTransient());
            context.Products.Add(new Product { Name = "one" });
            context.SaveChanges();

            var controller = new ProductController(context);

            // Act
            var id = context.Products.First().Id;
            var result = controller.Get(id);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(id);
            result.Name.ShouldBe("one");
        }

        [Test]
        public void Post_CreatesNewProduct()
        {
            // Arrange
            var context = new GeneralStoreDbContext(Effort.DbConnectionFactory.CreateTransient());

            var controller = new ProductController(context);
            
            var product = Builder<Product>.CreateNew().Build();

            // Act
            controller.Post(product);

            // Assert
            var getProductResult = controller.Get(product.Id);
            getProductResult.Id.ShouldBe(product.Id);
            getProductResult.Name.ShouldBe(product.Name);
        }

        [Test]
        public void Post_WithExistingId_Fails()
        {
            // Arrange
            var existingProduct = Builder<Product>.CreateNew().Build();
            var context = new GeneralStoreDbContext(Effort.DbConnectionFactory.CreateTransient());
            context.Products.Add(existingProduct);
            context.SaveChanges();

            var controller = new ProductController(context);

            var product = Builder<Product>.CreateNew().Build();
            product.Id = existingProduct.Id;

            // Act
            Should.Throw<ArgumentException>(() => controller.Post(product));
        }

        [Test]
        public void Put_ExistingProduct_UpdatesProduct()
        {
            // Arrange
            var existingProduct = Builder<Product>.CreateNew().Build();
            var context = new GeneralStoreDbContext(Effort.DbConnectionFactory.CreateTransient());
            context.Products.Add(existingProduct);
            context.SaveChanges();

            var controller = new ProductController(context);

            var productId = existingProduct.Id;

            // Act
            var updatedProduct = new Product {Name = "Some other name"};
            
            controller.Put(productId,updatedProduct);

            // Assert
            var getProductResult = controller.Get(productId);
            getProductResult.Name.ShouldBe(updatedProduct.Name);
        }

        [Test]
        public void Put_NonExistentProduct_Fails()
        {
            // Arrange
            var existingProduct = Builder<Product>.CreateNew().Build();
            var context = new GeneralStoreDbContext(Effort.DbConnectionFactory.CreateTransient());
            context.Products.Add(existingProduct);
            context.SaveChanges();

            var controller = new ProductController(context);

            var otherProductId = existingProduct.Id + 1;

            // Act
            Should.Throw<ArgumentOutOfRangeException>(() => controller.Put(otherProductId, new Product { Name = "something else" }));
        }

        [Test]
        public void Put_PassNullProduct_Fails()
        {
            // Arrange
            var existingProduct = Builder<Product>.CreateNew().Build();
            var context = new GeneralStoreDbContext(Effort.DbConnectionFactory.CreateTransient());
            context.Products.Add(existingProduct);
            context.SaveChanges();

            var controller = new ProductController(context);

            // Act
            Should.Throw<ArgumentNullException>(() => controller.Put(existingProduct.Id, null));
        }

        [Test]
        public void Delete_RemovesProductFromDatabase()
        {
            // Arrange
            var existingProduct = Builder<Product>.CreateNew().Build();
            var context = new GeneralStoreDbContext(Effort.DbConnectionFactory.CreateTransient());
            context.Products.Add(existingProduct);
            context.SaveChanges();

            var controller = new ProductController(context);
            var productId = existingProduct.Id;

            // Act
            controller.Delete(productId);

            // Assert
            controller.Get().ShouldBeEmpty();
        }

        [Test]
        public void Delete_NonexistentId_Fails()
        {
            // Arrange
            var existingProduct = Builder<Product>.CreateNew().Build();
            var context = new GeneralStoreDbContext(Effort.DbConnectionFactory.CreateTransient());
            context.Products.Add(existingProduct);
            context.SaveChanges();

            var controller = new ProductController(context);

            var otherProductId = existingProduct.Id + 1;

            // Act  / Assert
            Should.Throw<ArgumentOutOfRangeException>(() => { controller.Delete(otherProductId); });
        }
    }
}