using DataAccessLayer.Repositories;
using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace Tests.UnitTests.Repositories
{
    [TestFixture()]
    public class UT_BarcodeRepositoryTests
    {
        private Mock<BBDDContext>? _mockContext;
        private BarcodeRepository? _barcodeRepository;

        [SetUp]
        public void SetUp()
        {
            _mockContext = new Mock<BBDDContext>();
            _barcodeRepository = new BarcodeRepository(_mockContext!.Object);
        }

        /// <summary>
        /// Tests the GetCBStaticProducts method to verify it returns the correct paginated list of static barcodes.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetCBStaticProducts method returns a paginated list of static barcodes when provided with a valid userGuid, pageIndex, and pageSize.
        /// </remarks>
        [Test()]
        public async Task GetCBStaticProductsTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var pageIndex = 1;
            var pageSize = 10;
            var expectedProducts = new List<AppCBStatic>
            {
                new AppCBStatic { UserId = userGuid, BarcodeId = Guid.NewGuid(), CBValue = "1234567890123" },
                new AppCBStatic { UserId = userGuid, BarcodeId = Guid.NewGuid(), CBValue = "9876543210987" }
            };
            var mockDbSet = new Mock<DbSet<AppCBStatic>>();
            mockDbSet.As<IAsyncEnumerable<AppCBStatic>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<AppCBStatic>(expectedProducts.GetEnumerator()));
            mockDbSet.As<IQueryable<AppCBStatic>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<AppCBStatic>(expectedProducts.AsQueryable().Provider));
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.Expression).Returns(expectedProducts.AsQueryable().Expression);
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.ElementType).Returns(expectedProducts.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.GetEnumerator()).Returns(expectedProducts.GetEnumerator());

            _mockContext!.Setup(c => c.AppCBStatics).Returns(mockDbSet.Object);

            // Act
            var result = await _barcodeRepository!.GetCBStaticProducts(userGuid, pageIndex, pageSize);

            // Assert
            Assert.That(result.Items.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Tests the GetCBDynamicProducts method to verify it returns the correct paginated list of dynamic barcodes.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetCBDynamicProducts method returns a paginated list of dynamic barcodes when provided with a valid userGuid, pageIndex, and pageSize.
        /// </remarks>
        [Test()]
        public async Task GetCBDynamicProductsTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var pageIndex = 1;
            var pageSize = 10;
            var expectedProducts = new List<AppCBDynamic>
            {
                new AppCBDynamic { UserId = userGuid, BarcodeId = Guid.NewGuid(), CBValue = "1234567890123" },
                new AppCBDynamic { UserId = userGuid, BarcodeId = Guid.NewGuid(), CBValue = "9876543210987" }
            };
            var mockDbSet = new Mock<DbSet<AppCBDynamic>>();
            mockDbSet.As<IAsyncEnumerable<AppCBDynamic>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<AppCBDynamic>(expectedProducts.GetEnumerator()));
            mockDbSet.As<IQueryable<AppCBDynamic>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<AppCBDynamic>(expectedProducts.AsQueryable().Provider));
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.Expression).Returns(expectedProducts.AsQueryable().Expression);
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.ElementType).Returns(expectedProducts.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.GetEnumerator()).Returns(expectedProducts.GetEnumerator());

            _mockContext!.Setup(c => c.AppCBDynamics).Returns(mockDbSet.Object);

            // Act
            var result = await _barcodeRepository!.GetCBDynamicProducts(userGuid, pageIndex, pageSize);

            // Assert
            Assert.That(result.Items.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Tests the GetAllCBStatic method to verify it returns all static barcodes for a user.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetAllCBStatic method returns a list of all static barcodes when provided with a valid userGuid.
        /// </remarks>
        [Test()]
        public void GetAllCBStaticTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var expectedProducts = new List<AppCBStatic>
            {
                new AppCBStatic { UserId = userGuid, BarcodeId = Guid.NewGuid(), CBValue = "1234567890123" },
                new AppCBStatic { UserId = userGuid, BarcodeId = Guid.NewGuid(), CBValue = "9876543210987" }
            };
            var mockDbSet = new Mock<DbSet<AppCBStatic>>();
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.Provider).Returns(expectedProducts.AsQueryable().Provider);
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.Expression).Returns(expectedProducts.AsQueryable().Expression);
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.ElementType).Returns(expectedProducts.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.GetEnumerator()).Returns(expectedProducts.GetEnumerator());

            _mockContext!.Setup(c => c.AppCBStatics).Returns(mockDbSet.Object);

            // Act
            var result = _barcodeRepository!.GetAllCBStatic(userGuid);

            // Assert
            Assert.That(result, Is.EqualTo(expectedProducts));
        }

        /// <summary>
        /// Tests the GetAllCBDynamic method to verify it returns all dynamic barcodes for a user.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetAllCBDynamic method returns a list of all dynamic barcodes when provided with a valid userGuid.
        /// </remarks>
        [Test()]
        public void GetAllCBDynamicTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var expectedProducts = new List<AppCBDynamic>
            {
                new AppCBDynamic { UserId = userGuid, BarcodeId = Guid.NewGuid(), CBValue = "1234567890123" },
                new AppCBDynamic { UserId = userGuid, BarcodeId = Guid.NewGuid(), CBValue = "9876543210987" }
            };
            var mockDbSet = new Mock<DbSet<AppCBDynamic>>();
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.Provider).Returns(expectedProducts.AsQueryable().Provider);
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.Expression).Returns(expectedProducts.AsQueryable().Expression);
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.ElementType).Returns(expectedProducts.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.GetEnumerator()).Returns(expectedProducts.GetEnumerator());

            _mockContext!.Setup(c => c.AppCBDynamics).Returns(mockDbSet.Object);

            // Act
            var result = _barcodeRepository!.GetAllCBDynamic(userGuid);

            // Assert
            Assert.That(result, Is.EqualTo(expectedProducts));
        }

        /// <summary>
        /// Tests the GetCBStaticById method to verify it returns the correct static barcode by its ID.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetCBStaticById method returns the correct static barcode when provided with a valid userGuid and barcode ID.
        /// </remarks>
        [Test()]
        public async Task GetCBStaticByIdTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var barcodeId = Guid.NewGuid();
            var expectedBarcode = new AppCBStatic { UserId = userGuid, BarcodeId = barcodeId, CBValue = "1234567890123" };
            var mockDbSet = new Mock<DbSet<AppCBStatic>>();
            mockDbSet.As<IAsyncEnumerable<AppCBStatic>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<AppCBStatic>(new List<AppCBStatic> { expectedBarcode }.GetEnumerator()));
            mockDbSet.As<IQueryable<AppCBStatic>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<AppCBStatic>(new List<AppCBStatic> { expectedBarcode }.AsQueryable().Provider));
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.Expression).Returns((new List<AppCBStatic> { expectedBarcode }).AsQueryable().Expression);
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.ElementType).Returns((new List<AppCBStatic> { expectedBarcode }).AsQueryable().ElementType);
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.GetEnumerator()).Returns((new List<AppCBStatic> { expectedBarcode }).GetEnumerator());

            _mockContext!.Setup(c => c.AppCBStatics).Returns(mockDbSet.Object);

            // Act
            var result = await _barcodeRepository!.GetCBStaticById(userGuid, barcodeId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedBarcode));
        }

        /// <summary>
        /// Tests the GetCBDynamicById method to verify it returns the correct dynamic barcode by its ID.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetCBDynamicById method returns the correct dynamic barcode when provided with a valid userGuid and barcode ID.
        /// </remarks>
        [Test()]
        public async Task GetCBDynamicByIdTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var barcodeId = Guid.NewGuid();
            var expectedBarcode = new AppCBDynamic { UserId = userGuid, BarcodeId = barcodeId, CBValue = "1234567890123" };
            var mockDbSet = new Mock<DbSet<AppCBDynamic>>();
            mockDbSet.As<IAsyncEnumerable<AppCBDynamic>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<AppCBDynamic>(new List<AppCBDynamic> { expectedBarcode }.GetEnumerator()));
            mockDbSet.As<IQueryable<AppCBDynamic>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<AppCBDynamic>(new List<AppCBDynamic> { expectedBarcode }.AsQueryable().Provider));
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.Expression).Returns((new List<AppCBDynamic> { expectedBarcode }).AsQueryable().Expression);
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.ElementType).Returns((new List<AppCBDynamic> { expectedBarcode }).AsQueryable().ElementType);
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.GetEnumerator()).Returns((new List<AppCBDynamic> { expectedBarcode }).GetEnumerator());

            _mockContext!.Setup(c => c.AppCBDynamics).Returns(mockDbSet.Object);

            // Act
            var result = await _barcodeRepository!.GetCBDynamicById(userGuid, barcodeId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedBarcode));
        }

        /// <summary>
        /// Tests the AddCBStatic method to verify it successfully adds a new static barcode.
        /// </summary>
        /// <remarks>
        /// This test checks if the AddCBStatic method returns the ID of the newly added static barcode when provided with a valid AppCBStatic object.
        /// </remarks>
        [Test()]
        public async Task AddCBStaticTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var newBarcode = new AppCBStatic { UserId = userGuid, CBValue = "1234567890123" };
            _mockContext!.Setup(c => c.AddAsync(It.IsAny<AppCBStatic>(), default)).ReturnsAsync(() =>
            {
                var entityEntry = new Mock<EntityEntry<AppCBStatic>>();
                entityEntry.Setup(e => e.Entity).Returns(newBarcode);
                return entityEntry.Object;
            });

            // Act
            var result = await _barcodeRepository!.AddCBStatic(newBarcode);

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests the AddCBDynamic method to verify it successfully adds a new dynamic barcode.
        /// </summary>
        /// <remarks>
        /// This test checks if the AddCBDynamic method returns the ID of the newly added dynamic barcode when provided with a valid AppCBDynamic object.
        /// </remarks>
        [Test()]
        public async Task AddCBDynamicTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var newBarcode = new AppCBDynamic
            {
                UserId = userGuid,
                BarcodeId = Guid.NewGuid(),
                Description = "UT Dynamic Barcodes Add",
                CBType = "EAN",
                CBValue = "1234567890123"
            };
            var expectedBarcodeId = newBarcode.BarcodeId.ToString();
            _mockContext!.Setup(c => c.AddAsync(It.IsAny<AppCBDynamic>(), default)).ReturnsAsync(() =>
            {
                var entityEntry = new Mock<EntityEntry<AppCBDynamic>>();
                entityEntry.Setup(e => e.Entity).Returns(newBarcode);
                return entityEntry.Object;
            });

            // Act
            var result = await _barcodeRepository!.AddCBDynamic(newBarcode);

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests the UpdateCBStatic method to verify it successfully updates a static barcode.
        /// </summary>
        /// <remarks>
        /// This test checks if the UpdateCBStatic method returns true when provided with valid parameters.
        /// </remarks>
        [Test()]
        public async Task UpdateCBStaticTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var barcodeId = Guid.NewGuid();
            var modBarcode = new AppCBStatic { BarcodeId = barcodeId, CBValue = "1234567890123" };
            var existingBarcode = new AppCBStatic { BarcodeId = barcodeId, CBValue = "1234567890123" };
            var mockDbSet = new Mock<DbSet<AppCBStatic>>();
            mockDbSet.As<IAsyncEnumerable<AppCBStatic>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<AppCBStatic>(new List<AppCBStatic> { existingBarcode }.GetEnumerator()));
            mockDbSet.As<IQueryable<AppCBStatic>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<AppCBStatic>(new List<AppCBStatic> { existingBarcode }.AsQueryable().Provider));
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.Expression).Returns((new List<AppCBStatic> { existingBarcode }).AsQueryable().Expression);
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.ElementType).Returns((new List<AppCBStatic> { existingBarcode }).AsQueryable().ElementType);
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.GetEnumerator()).Returns((new List<AppCBStatic> { existingBarcode }).GetEnumerator());

            _mockContext!.Setup(c => c.AppCBStatics).Returns(mockDbSet.Object);

            // Act
            var result = await _barcodeRepository!.UpdateCBStatic(userGuid, barcodeId, modBarcode);

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests the UpdateCBDynamic method to verify it successfully updates a dynamic barcode.
        /// </summary>
        /// <remarks>
        /// This test checks if the UpdateCBDynamic method returns true when provided with valid parameters.
        /// </remarks>
        [Test()]
        public async Task UpdateCBDynamicTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var barcodeId = Guid.NewGuid();
            var modBarcode = new AppCBDynamic { BarcodeId = barcodeId, CBValue = "1234567890123" };
            var existingBarcode = new AppCBDynamic { BarcodeId = barcodeId, CBValue = "1234567890123" };
            var mockDbSet = new Mock<DbSet<AppCBDynamic>>();
            mockDbSet.As<IAsyncEnumerable<AppCBDynamic>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<AppCBDynamic>(new List<AppCBDynamic> { existingBarcode }.GetEnumerator()));
            mockDbSet.As<IQueryable<AppCBDynamic>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<AppCBDynamic>(new List<AppCBDynamic> { existingBarcode }.AsQueryable().Provider));
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.Expression).Returns((new List<AppCBDynamic> { existingBarcode }).AsQueryable().Expression);
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.ElementType).Returns((new List<AppCBDynamic> { existingBarcode }).AsQueryable().ElementType);
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.GetEnumerator()).Returns((new List<AppCBDynamic> { existingBarcode }).GetEnumerator());

            _mockContext!.Setup(c => c.AppCBDynamics).Returns(mockDbSet.Object);

            // Act
            var result = await _barcodeRepository!.UpdateCBDynamic(userGuid, barcodeId, modBarcode);

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests the DeleteCBStatic method to verify it successfully deletes a static barcode.
        /// </summary>
        /// <remarks>
        /// This test checks if the DeleteCBStatic method returns true when provided with valid parameters.
        /// </remarks>
        [Test()]
        public async Task DeleteCBStaticTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var barcode = new AppCBStatic { BarcodeId = Guid.NewGuid(), CBValue = "1234567890123" };
            var mockDbSet = new Mock<DbSet<AppCBStatic>>();
            mockDbSet.As<IAsyncEnumerable<AppCBStatic>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<AppCBStatic>(new List<AppCBStatic> { barcode }.GetEnumerator()));
            mockDbSet.As<IQueryable<AppCBStatic>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<AppCBStatic>(new List<AppCBStatic> { barcode }.AsQueryable().Provider));
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.Expression).Returns((new List<AppCBStatic> { barcode }).AsQueryable().Expression);
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.ElementType).Returns((new List<AppCBStatic> { barcode }).AsQueryable().ElementType);
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.GetEnumerator()).Returns((new List<AppCBStatic> { barcode }).GetEnumerator());

            _mockContext!.Setup(c => c.AppCBStatics).Returns(mockDbSet.Object);

            // Act
            var result = await _barcodeRepository!.DeleteCBStatic(userGuid, barcode);

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests the DeleteCBDynamic method to verify it successfully deletes a dynamic barcode.
        /// </summary>
        /// <remarks>
        /// This test checks if the DeleteCBDynamic method returns true when provided with valid parameters.
        /// </remarks>
        [Test()]
        public async Task DeleteCBDynamicTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var barcode = new AppCBDynamic { BarcodeId = Guid.NewGuid(), CBValue = "1234567890123" };
            var mockDbSet = new Mock<DbSet<AppCBDynamic>>();
            mockDbSet.As<IAsyncEnumerable<AppCBDynamic>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<AppCBDynamic>(new List<AppCBDynamic> { barcode }.GetEnumerator()));
            mockDbSet.As<IQueryable<AppCBDynamic>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<AppCBDynamic>(new List<AppCBDynamic> { barcode }.AsQueryable().Provider));
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.Expression).Returns((new List<AppCBDynamic> { barcode }).AsQueryable().Expression);
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.ElementType).Returns((new List<AppCBDynamic> { barcode }).AsQueryable().ElementType);
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.GetEnumerator()).Returns((new List<AppCBDynamic> { barcode }).GetEnumerator());

            _mockContext!.Setup(c => c.AppCBDynamics).Returns(mockDbSet.Object);

            // Act
            var result = await _barcodeRepository!.DeleteCBDynamic(userGuid, barcode);

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests the DeleteCBStaticById method to verify it successfully deletes a static barcode by its ID.
        /// </summary>
        /// <remarks>
        /// This test checks if the DeleteCBStaticById method returns true when provided with a valid userGuid and barcode ID.
        /// </remarks>
        [Test()]
        public async Task DeleteCBStaticByIdTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var barcodeId = Guid.NewGuid();
            var barcode = new AppCBStatic { UserId = userGuid, BarcodeId = barcodeId, CBValue = "1234567890123" };
            var mockDbSet = new Mock<DbSet<AppCBStatic>>();
            mockDbSet.As<IAsyncEnumerable<AppCBStatic>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<AppCBStatic>(new List<AppCBStatic> { barcode }.GetEnumerator()));
            mockDbSet.As<IQueryable<AppCBStatic>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<AppCBStatic>(new List<AppCBStatic> { barcode }.AsQueryable().Provider));
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.Expression).Returns((new List<AppCBStatic> { barcode }).AsQueryable().Expression);
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.ElementType).Returns((new List<AppCBStatic> { barcode }).AsQueryable().ElementType);
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.GetEnumerator()).Returns((new List<AppCBStatic> { barcode }).GetEnumerator());

            _mockContext!.Setup(c => c.AppCBStatics).Returns(mockDbSet.Object);

            // Act
            var result = await _barcodeRepository!.DeleteCBStaticById(userGuid, barcodeId);

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests the DeleteCBDynamicById method to verify it successfully deletes a dynamic barcode by its ID.
        /// </summary>
        /// <remarks>
        /// This test checks if the DeleteCBDynamicById method returns true when provided with a valid userGuid and barcode ID.
        /// </remarks>
        [Test()]
        public async Task DeleteCBDynamicByIdTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var barcodeId = Guid.NewGuid();
            var barcode = new AppCBDynamic { UserId = userGuid, BarcodeId = barcodeId, CBValue = "1234567890123" };
            var mockDbSet = new Mock<DbSet<AppCBDynamic>>();
            mockDbSet.As<IAsyncEnumerable<AppCBDynamic>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<AppCBDynamic>(new List<AppCBDynamic> { barcode }.GetEnumerator()));
            mockDbSet.As<IQueryable<AppCBDynamic>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<AppCBDynamic>(new List<AppCBDynamic> { barcode }.AsQueryable().Provider));
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.Expression).Returns((new List<AppCBDynamic> { barcode }).AsQueryable().Expression);
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.ElementType).Returns((new List<AppCBDynamic> { barcode }).AsQueryable().ElementType);
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.GetEnumerator()).Returns((new List<AppCBDynamic> { barcode }).GetEnumerator());

            _mockContext!.Setup(c => c.AppCBDynamics).Returns(mockDbSet.Object);

            // Act
            var result = await _barcodeRepository!.DeleteCBDynamicById(userGuid, barcodeId);

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests the GetCBStaticByCode method to verify it returns the correct static barcode by its code.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetCBStaticByCode method returns the correct static barcode when provided with a valid userGuid and code.
        /// </remarks>
        [Test()]
        public async Task GetCBStaticByCodeTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var code = "1234567890123";
            var expectedBarcode = new AppCBStatic { UserId = userGuid, BarcodeId = Guid.NewGuid(), CBValue = code };
            var mockDbSet = new Mock<DbSet<AppCBStatic>>();
            mockDbSet.As<IAsyncEnumerable<AppCBStatic>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<AppCBStatic>(new List<AppCBStatic> { expectedBarcode }.GetEnumerator()));
            mockDbSet.As<IQueryable<AppCBStatic>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<AppCBStatic>(new List<AppCBStatic> { expectedBarcode }.AsQueryable().Provider));
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.Expression).Returns((new List<AppCBStatic> { expectedBarcode }).AsQueryable().Expression);
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.ElementType).Returns((new List<AppCBStatic> { expectedBarcode }).AsQueryable().ElementType);
            mockDbSet.As<IQueryable<AppCBStatic>>().Setup(m => m.GetEnumerator()).Returns((new List<AppCBStatic> { expectedBarcode }).GetEnumerator());

            _mockContext!.Setup(c => c.AppCBStatics).Returns(mockDbSet.Object);

            // Act
            var result = await _barcodeRepository!.GetCBStaticByCode(userGuid, code);

            // Assert
            Assert.That(result, Is.EqualTo(expectedBarcode));
        }

        /// <summary>
        /// Tests the GetCBDynamicByCode method to verify it returns the correct dynamic barcode by its code.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetCBDynamicByCode method returns the correct dynamic barcode when provided with a valid userGuid and code.
        /// </remarks>
        [Test()]
        public async Task GetCBDynamicByCodeTest_Success()
        {
            // Arrange
            var userGuid = Guid.NewGuid();
            var value = "1234567890123";
            var BarcodeId = Guid.NewGuid();
            var expectedBarcode = new AppCBDynamic { UserId = userGuid, BarcodeId = BarcodeId, CBValue = value };
            var mockDbSet = new Mock<DbSet<AppCBDynamic>>();

            // Setup the mock to support async queries
            mockDbSet.As<IAsyncEnumerable<AppCBDynamic>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<AppCBDynamic>(new List<AppCBDynamic> { expectedBarcode }.GetEnumerator()));
            mockDbSet.As<IQueryable<AppCBDynamic>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<AppCBDynamic>(new List<AppCBDynamic> { expectedBarcode }.AsQueryable().Provider));
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.Expression).Returns((new List<AppCBDynamic> { expectedBarcode }).AsQueryable().Expression);
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.ElementType).Returns((new List<AppCBDynamic> { expectedBarcode }).AsQueryable().ElementType);
            mockDbSet.As<IQueryable<AppCBDynamic>>().Setup(m => m.GetEnumerator()).Returns((new List<AppCBDynamic> { expectedBarcode }).GetEnumerator());

            _mockContext!.Setup(c => c.AppCBDynamics).Returns(mockDbSet.Object);

            // Act
            var result = await _barcodeRepository!.GetCBDynamicByCode(userGuid, value);

            // Assert
            Assert.That(result, Is.EqualTo(expectedBarcode));
        }
    }
}
