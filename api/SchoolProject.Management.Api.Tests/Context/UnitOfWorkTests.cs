using DotNetCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SchoolProject.Management.Persistence.Context;
using System.Threading.Tasks;

namespace SchoolProject.Management.Api.Tests.Context
{
    [TestClass]
    public class UnitOfWorkTests
    {
        [TestMethod]
        public async Task CheckSaveChangesAsync()
        {
            //Arrange
            DbContextOptions<SchoolManagementDbContext> options = new DbContextOptions<SchoolManagementDbContext>();
            var mockDbContext = new Mock<SchoolManagementDbContext>(options);
            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(1); // Retourne le nombre d'entités enregistrées
            var mockUnitOfWork = new UnitOfWork<SchoolManagementDbContext>(mockDbContext.Object);

            // Act
            var result = await mockUnitOfWork.SaveChangesAsync();

            // Assert
            Assert.AreEqual(1, result); // Vérifie si le résultat est égal à 1
            mockDbContext.Verify();
        }


    }
}
