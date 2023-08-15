using DotNetCore.EntityFrameworkCore;
using ManagementProject.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Threading.Tasks;

namespace ManagementProject.Persistence.Tests.Unit_Test.Context
{
    [TestClass]
    public class UnitOfWorkTests
    {
        [TestMethod]
        public async Task SaveChangesAsync_ReturnOne()
        {
            //Arrange
            DbContextOptions<ManagementProjectDbContext> options = new DbContextOptions<ManagementProjectDbContext>();
            var mockDbContext = Substitute.For<ManagementProjectDbContext>(options);
            mockDbContext.SaveChangesAsync(Arg.Any<System.Threading.CancellationToken>()).Returns(1); // Retourne le nombre d'entités enregistrées
            var unitOfWork = new UnitOfWork<ManagementProjectDbContext>(mockDbContext);

            // Act
            var result = await unitOfWork.SaveChangesAsync();

            // Assert
            Assert.AreEqual(1, result); // Vérifie si le résultat est égal à 1
            mockDbContext.Received();
        }
        [TestMethod]
        public async Task SaveChangesAsync_ReturnZero()
        {
            // Arrange
            DbContextOptions<ManagementProjectDbContext> options = new DbContextOptions<ManagementProjectDbContext>();
            var mockDbContext = Substitute.For<ManagementProjectDbContext>(options);
            mockDbContext.SaveChangesAsync(Arg.Any<System.Threading.CancellationToken>()).Returns(0); // Retourne zéro entité enregistrée
            var unitOfWork = new UnitOfWork<ManagementProjectDbContext>(mockDbContext);

            // Act
            var result = await unitOfWork.SaveChangesAsync();

            // Assert
            Assert.AreEqual(0, result); // Vérifie si le résultat est égal à zéro
            mockDbContext.Received();
        }


    }
}
