using DataAccessLayer.Repositories;
using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.UnitTests.Repositories
{
    [TestFixture()]
    public class UT_UserRepositoryTests
    {
        private Mock<BBDDContext>? _mockContext;
        private UserRepository? _userRepository;

        [SetUp]
        public void SetUp()
        {
            _mockContext = new Mock<BBDDContext>();
            _userRepository = new UserRepository(_mockContext.Object);
        }

        /// <summary>
        /// Tests the GetUserByEmail method to verify it returns the correct user by email.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetUserByEmail method returns a user object when provided with a valid email.
        /// </remarks>
        [Test()]
        public void GetUserByEmailTest_Success()
        {
            // Arrange
            var email = "test@example.com";
            var expectedUser = new AppUser { Login = email };
            var mockDbSet = new Mock<DbSet<AppUser>>();
            mockDbSet.As<IQueryable<AppUser>>().Setup(m => m.Provider).Returns(new List<AppUser> { expectedUser }.AsQueryable().Provider);
            mockDbSet.As<IQueryable<AppUser>>().Setup(m => m.Expression).Returns(new List<AppUser> { expectedUser }.AsQueryable().Expression);
            mockDbSet.As<IQueryable<AppUser>>().Setup(m => m.ElementType).Returns(new List<AppUser> { expectedUser }.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<AppUser>>().Setup(m => m.GetEnumerator()).Returns(new List<AppUser> { expectedUser }.GetEnumerator());

            _mockContext!.Setup(c => c.AppUsers).Returns(mockDbSet.Object);

            // Act
            var result = _userRepository!.GetUserByEmail(email);

            // Assert
            Assert.That(result, Is.EqualTo(expectedUser));
        }

        /// <summary>
        /// Tests the GetUserByToken method to verify it returns the correct user by token.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetUserByToken method returns a user object when provided with a valid token.
        /// </remarks>
        [Test()]
        public void GetUserByTokenTest_Success()
        {
            // Arrange
            var token = "valid_token";
            var expectedUser = new AppUser { TokenID = token };
            var mockDbSet = new Mock<DbSet<AppUser>>();
            mockDbSet.As<IQueryable<AppUser>>().Setup(m => m.Provider).Returns(new List<AppUser> { expectedUser }.AsQueryable().Provider);
            mockDbSet.As<IQueryable<AppUser>>().Setup(m => m.Expression).Returns(new List<AppUser> { expectedUser }.AsQueryable().Expression);
            mockDbSet.As<IQueryable<AppUser>>().Setup(m => m.ElementType).Returns(new List<AppUser> { expectedUser }.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<AppUser>>().Setup(m => m.GetEnumerator()).Returns(new List<AppUser> { expectedUser }.GetEnumerator());

            _mockContext!.Setup(c => c.AppUsers).Returns(mockDbSet.Object);

            // Act
            var result = _userRepository!.GetUserByToken(token);

            // Assert
            Assert.That(result, Is.EqualTo(expectedUser));
        }

        /// <summary>
        /// Tests the GetUserIdByEmail method to verify it returns the correct user ID by email.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetUserIdByEmail method returns a user ID when provided with a valid email.
        /// </remarks>
        [Test()]
        public void GetUserIdByEmailTest_Success()
        {
            // Arrange
            var email = "test@example.com";
            var expectedUserId = Guid.NewGuid();
            var expectedUser = new AppUser { Login = email, UserId = expectedUserId };
            var mockDbSet = new Mock<DbSet<AppUser>>();
            mockDbSet.As<IQueryable<AppUser>>().Setup(m => m.Provider).Returns(new List<AppUser> { expectedUser }.AsQueryable().Provider);
            mockDbSet.As<IQueryable<AppUser>>().Setup(m => m.Expression).Returns(new List<AppUser> { expectedUser }.AsQueryable().Expression);
            mockDbSet.As<IQueryable<AppUser>>().Setup(m => m.ElementType).Returns(new List<AppUser> { expectedUser }.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<AppUser>>().Setup(m => m.GetEnumerator()).Returns(new List<AppUser> { expectedUser }.GetEnumerator());

            _mockContext!.Setup(c => c.AppUsers).Returns(mockDbSet.Object);

            // Act
            var result = _userRepository!.GetUserIdByEmail(email);

            // Assert
            Assert.That(result, Is.EqualTo(expectedUserId));
        }

        /// <summary>
        /// Tests the GetUserProfile method to verify it returns the correct user profile.
        /// </summary>
        /// <remarks>
        /// This test checks if the GetUserProfile method returns a user profile object when provided with a valid email.
        /// </remarks>
        [Test()]
        public void GetUserProfileTest_Success()
        {
            // Arrange
            var email = "test@example.com";
            var user = new AppUser { Login = email, UserId = Guid.NewGuid(), Name = "Test User" };
            var role = new SysRole { Role = "Admin", Description = "Administrator" };
            var userRole = new AppUsersRole { UserId = user.UserId, Role = "Admin" };

            var mockUserDbSet = new Mock<DbSet<AppUser>>();
            var mockRoleDbSet = new Mock<DbSet<SysRole>>();
            var mockUserRoleDbSet = new Mock<DbSet<AppUsersRole>>();

            mockUserDbSet.As<IQueryable<AppUser>>().Setup(m => m.Provider).Returns(new List<AppUser> { user }.AsQueryable().Provider);
            mockUserDbSet.As<IQueryable<AppUser>>().Setup(m => m.Expression).Returns(new List<AppUser> { user }.AsQueryable().Expression);
            mockUserDbSet.As<IQueryable<AppUser>>().Setup(m => m.ElementType).Returns(new List<AppUser> { user }.AsQueryable().ElementType);
            mockUserDbSet.As<IQueryable<AppUser>>().Setup(m => m.GetEnumerator()).Returns(new List<AppUser> { user }.GetEnumerator());

            mockRoleDbSet.As<IQueryable<SysRole>>().Setup(m => m.Provider).Returns(new List<SysRole> { role }.AsQueryable().Provider);
            mockRoleDbSet.As<IQueryable<SysRole>>().Setup(m => m.Expression).Returns(new List<SysRole> { role }.AsQueryable().Expression);
            mockRoleDbSet.As<IQueryable<SysRole>>().Setup(m => m.ElementType).Returns(new List<SysRole> { role }.AsQueryable().ElementType);
            mockRoleDbSet.As<IQueryable<SysRole>>().Setup(m => m.GetEnumerator()).Returns(new List<SysRole> { role }.GetEnumerator());

            mockUserRoleDbSet.As<IQueryable<AppUsersRole>>().Setup(m => m.Provider).Returns(new List<AppUsersRole> { userRole }.AsQueryable().Provider);
            mockUserRoleDbSet.As<IQueryable<AppUsersRole>>().Setup(m => m.Expression).Returns(new List<AppUsersRole> { userRole }.AsQueryable().Expression);
            mockUserRoleDbSet.As<IQueryable<AppUsersRole>>().Setup(m => m.ElementType).Returns(new List<AppUsersRole> { userRole }.AsQueryable().ElementType);
            mockUserRoleDbSet.As<IQueryable<AppUsersRole>>().Setup(m => m.GetEnumerator()).Returns(new List<AppUsersRole> { userRole }.GetEnumerator());

            _mockContext!.Setup(c => c.AppUsers).Returns(mockUserDbSet.Object);
            _mockContext.Setup(c => c.SysRoles).Returns(mockRoleDbSet.Object);
            _mockContext.Setup(c => c.AppUsersRoles).Returns(mockUserRoleDbSet.Object);

            // Act
            var result = _userRepository!.GetUserProfile(email);

            // Assert
            Assert.That(result.UserLogin, Is.EqualTo(email));
            Assert.That(result.UserName, Is.EqualTo("Test User"));
            Assert.That(result.UserProfile, Is.EqualTo("Administrator"));
        }

        // Additional tests go here following the same pattern
    }
}
