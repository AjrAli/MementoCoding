using DotNetCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SchoolProject.Management.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Management.Persistence.Tests.Unit_Test.Context
{
    [TestClass]
    public class UnitOfWorkTests
    {
        [TestMethod]
        public async Task SaveChangesAsync_ReturnOne()
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
        [TestMethod]
        public async Task SaveChangesAsync_ReturnZero()
        {
            // Arrange
            DbContextOptions<SchoolManagementDbContext> options = new DbContextOptions<SchoolManagementDbContext>();
            var mockDbContext = new Mock<SchoolManagementDbContext>(options);
            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(0); // Retourne zéro entité enregistrée
            var mockUnitOfWork = new UnitOfWork<SchoolManagementDbContext>(mockDbContext.Object);

            // Act
            var result = await mockUnitOfWork.SaveChangesAsync();

            // Assert
            Assert.AreEqual(0, result); // Vérifie si le résultat est égal à zéro
            mockDbContext.Verify();
        }


    }
}
