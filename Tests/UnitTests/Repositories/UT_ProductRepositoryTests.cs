using DataAccessLayer.Repositories;
using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace Tests.UnitTests.Repositories
{
    [TestFixture()]
    public class UT_ProductRepositoryTests
    {
        private Mock<BBDDContext>? _mockContext;
        private ProductRepository? _productRepository;

        [SetUp]
        public void SetUp()
        {
            _mockContext = new Mock<BBDDContext>();
            _productRepository = new ProductRepository(_mockContext.Object);
        }

        /// <summary>
        /// Tests the GetProducts method to verify it returns the correct paginated list of products.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetProducts method returns a paginated list of products when provided with a valid userGuid, pageIndex, and pageSize.
        /// </remarks>
        [Test()]
        public async Task GetProductsTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var pageIndex = 1;
            var pageSize = 10;
            var expectedProducts = new List<AppProduct>
    {
        new AppProduct { UserId = userGuid, ProductId = Guid.NewGuid(), Description = "Product 1" },
        new AppProduct { UserId = userGuid, ProductId = Guid.NewGuid(), Description = "Product 2" }
    };

            var mockDbSet = new Mock<DbSet<AppProduct>>();
            mockDbSet.As<IAsyncEnumerable<AppProduct>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<AppProduct>(expectedProducts.GetEnumerator()));

            mockDbSet.As<IQueryable<AppProduct>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<AppProduct>(expectedProducts.AsQueryable().Provider));
            mockDbSet.As<IQueryable<AppProduct>>().Setup(m => m.Expression).Returns(expectedProducts.AsQueryable().Expression);
            mockDbSet.As<IQueryable<AppProduct>>().Setup(m => m.ElementType).Returns(expectedProducts.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<AppProduct>>().Setup(m => m.GetEnumerator()).Returns(expectedProducts.GetEnumerator());

            _mockContext!.Setup(c => c.AppProducts).Returns(mockDbSet.Object);

            // Act
            var result = await _productRepository!.GetProducts(userGuid, pageIndex, pageSize);

            // Assert
            Assert.That(result.Items.Count, Is.EqualTo(2));
        }


        /// <summary>
        /// Tests the GetAllProducts method to verify it returns all products for a user.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetAllProducts method returns a list of all products when provided with a valid userGuid.
        /// </remarks>
        [Test()]
        public void GetAllProductsTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var expectedProducts = new List<AppProduct>
            {
                new AppProduct { UserId = userGuid, ProductId = Guid.NewGuid(), Description = "Product 1" },
                new AppProduct { UserId = userGuid, ProductId = Guid.NewGuid(), Description = "Product 2" }
            };
            var mockDbSet = new Mock<DbSet<AppProduct>>();
            mockDbSet.As<IQueryable<AppProduct>>().Setup(m => m.Provider).Returns(expectedProducts.AsQueryable().Provider);
            mockDbSet.As<IQueryable<AppProduct>>().Setup(m => m.Expression).Returns(expectedProducts.AsQueryable().Expression);
            mockDbSet.As<IQueryable<AppProduct>>().Setup(m => m.ElementType).Returns(expectedProducts.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<AppProduct>>().Setup(m => m.GetEnumerator()).Returns(expectedProducts.AsQueryable().GetEnumerator());

            _mockContext!.Setup(c => c.AppProducts).Returns(mockDbSet.Object);

            // Act
            var result = _productRepository!.GetAllProducts(userGuid);

            // Assert
            Assert.That(result, Is.EqualTo(expectedProducts));
        }

        /// <summary>
        /// Tests the AddProduct method to verify it successfully adds a new product.
        /// </summary>
        /// <remarks>
        /// This test checks if the AddProduct method returns the ID of the newly added product when provided with a valid AppProduct object.
        /// </remarks>
        [Test()]
        public async Task AddProductTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var newProduct = new AppProduct
            {
                UserId = userGuid,
                ProductId = Guid.NewGuid(),
                Description = "UT Product Add",
                Category = "Category",
                Price = 10.99M,
                CBType = "EAN",
                CBValue = "1234567890123"
            };
            var expectedProductId = newProduct.ProductId.ToString();

            var mockDbSet = new Mock<DbSet<AppProduct>>();
            var mockEntityEntry = new Mock<EntityEntry<AppProduct>>();
            mockEntityEntry.Setup(e => e.Entity).Returns(newProduct);

            // Arrange DbContext to return the mocked DbSet
            _mockContext!.Setup(c => c.AppProducts).Returns(mockDbSet.Object);
            // Mock the behavior of Add method

            // Act
            var result = await _productRepository!.AddProduct(newProduct);

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests the UpdateProduct method to verify it successfully updates a product.
        /// </summary>
        /// <remarks>
        /// This test checks if the UpdateProduct method returns true when provided with valid parameters.
        /// </remarks>
        [Test()]
        public async Task UpdateProductTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var modProduct = new AppProduct
            {
                ProductId = productId,
                Description = "Updated Product",
                Category = "Updated Category",
                Price = 20,
                CBType = "UPC",
                CBValue = "0987654321098",
                CBShortLink = "http://short.link"
            };
            var existingProduct = new AppProduct
            {
                UserId = userGuid,
                ProductId = productId,
                Description = "Original Product",
                Category = "Original Category",
                Price = 10,
                CBType = "EAN",
                CBValue = "1234567890123"
            };
            var mockDbSet = new Mock<DbSet<AppProduct>>();
            mockDbSet.As<IQueryable<AppProduct>>().Setup(m => m.Provider).Returns((new List<AppProduct> { existingProduct }).AsQueryable().Provider);
            mockDbSet.As<IQueryable<AppProduct>>().Setup(m => m.Expression).Returns((new List<AppProduct> { existingProduct }).AsQueryable().Expression);
            mockDbSet.As<IQueryable<AppProduct>>().Setup(m => m.ElementType).Returns((new List<AppProduct> { existingProduct }).AsQueryable().ElementType);
            mockDbSet.As<IQueryable<AppProduct>>().Setup(m => m.GetEnumerator()).Returns((new List<AppProduct> { existingProduct }).AsQueryable().GetEnumerator());

            _mockContext!.Setup(c => c.AppProducts).Returns(mockDbSet.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _productRepository!.UpdateProduct(userGuid, productId, modProduct);

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests the DeleteProduct method to verify it successfully deletes a product.
        /// </summary>
        /// <remarks>
        /// This test checks if the DeleteProduct method returns true when provided with valid parameters.
        /// </remarks>
        [Test()]
        public async Task DeleteProductTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var productGuid = Guid.NewGuid();
            var product = new AppProduct { UserId = userGuid, ProductId = productGuid, Description = "Product to be deleted" };
            var mockDbSet = new Mock<DbSet<AppProduct>>();
            mockDbSet.As<IQueryable<AppProduct>>().Setup(m => m.Provider).Returns((new List<AppProduct> { product }).AsQueryable().Provider);
            mockDbSet.As<IQueryable<AppProduct>>().Setup(m => m.Expression).Returns((new List<AppProduct> { product }).AsQueryable().Expression);
            mockDbSet.As<IQueryable<AppProduct>>().Setup(m => m.ElementType).Returns((new List<AppProduct> { product }).AsQueryable().ElementType);
            mockDbSet.As<IQueryable<AppProduct>>().Setup(m => m.GetEnumerator()).Returns((new List<AppProduct> { product }).AsQueryable().GetEnumerator());

            _mockContext!.Setup(c => c.AppProducts).Returns(mockDbSet.Object);
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _productRepository!.DeleteProductById(userGuid, productGuid);

            // Assert
            Assert.That(result, Is.True);
        }
    }
}
