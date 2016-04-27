using System;
using System.CodeDom;
using System.Data;
using System.Linq;
using FizzWare.NBuilder;
using GeneralStore.Controllers.api;
using GeneralStore.Data;
using GeneralStore.Data.Models;
using NUnit.Framework;

namespace GeneralStore.Tests.Controllers.api
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
            Assert.NotNull(result);
            Assert.That(result.Count(),Is.EqualTo(1));
            Assert.That(result.First().Name,Is.EqualTo("one"));
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
            Assert.NotNull(result);
            Assert.That(result.Id, Is.EqualTo(id));
            Assert.That(result.Name, Is.EqualTo("one"));
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
            Assert.That(getProductResult.Id, Is.EqualTo(product.Id));
            Assert.That(getProductResult.Name, Is.EqualTo(product.Name));
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
            Assert.Throws<ArgumentException>(() => controller.Post(product));
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
            Assert.That(getProductResult.Name,Is.EqualTo(updatedProduct.Name));
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
            Assert.Throws<ArgumentOutOfRangeException>(() => controller.Put(otherProductId, new Product { Name = "something else" }));
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
            Assert.Throws<ArgumentNullException>(() => controller.Put(existingProduct.Id, null));
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
            Assert.That(controller.Get(), Is.Empty);
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
            Assert.Throws<ArgumentOutOfRangeException>(() => { controller.Delete(otherProductId); });
        }
    }
}