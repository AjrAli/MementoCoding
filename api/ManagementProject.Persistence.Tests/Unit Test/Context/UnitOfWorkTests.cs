using DotNetCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ManagementProject.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var mockDbContext = new Mock<ManagementProjectDbContext>(options);
            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(1); // Retourne le nombre d'entités enregistrées
            var unitOfWork = new UnitOfWork<ManagementProjectDbContext>(mockDbContext.Object);

            // Act
            var result = await unitOfWork.SaveChangesAsync();

            // Assert
            Assert.AreEqual(1, result); // Vérifie si le résultat est égal à 1
            mockDbContext.Verify();
        }
        [TestMethod]
        public async Task SaveChangesAsync_ReturnZero()
        {
            // Arrange
            DbContextOptions<ManagementProjectDbContext> options = new DbContextOptions<ManagementProjectDbContext>();
            var mockDbContext = new Mock<ManagementProjectDbContext>(options);
            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(0); // Retourne zéro entité enregistrée
            var unitOfWork = new UnitOfWork<ManagementProjectDbContext>(mockDbContext.Object);

            // Act
            var result = await unitOfWork.SaveChangesAsync();

            // Assert
            Assert.AreEqual(0, result); // Vérifie si le résultat est égal à zéro
            mockDbContext.Verify();
        }


    }
}
