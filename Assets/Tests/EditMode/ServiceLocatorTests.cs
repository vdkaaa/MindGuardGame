namespace MindGuard.Tests
{
    using NUnit.Framework;
    using MindGuard.Core.ServiceLocator;
    using System;

    public class ServiceLocatorTests
    {
        private ServiceLocator _serviceLocator;

        /// <summary>
        /// Interfaz de prueba para los tests.
        /// </summary>
        private interface ITestService
        {
            string GetName();
        }

        /// <summary>
        /// Implementación de prueba de ITestService.
        /// </summary>
        private class TestService : ITestService
        {
            public string GetName() => "TestService";
        }

        [SetUp]
        public void Setup()
        {
            _serviceLocator = new ServiceLocator();
        }

        [Test]
        public void Register_And_Get_ReturnsCorrectService()
        {
            // Arrange
            var service = new TestService();
            _serviceLocator.Register<ITestService>(service);

            // Act
            var retrievedService = _serviceLocator.Get<ITestService>();

            // Assert
            Assert.AreEqual(service, retrievedService, "El servicio recuperado debe ser la misma instancia");
            Assert.AreEqual("TestService", retrievedService.GetName());
        }

        [Test]
        public void Get_WithoutRegistration_ThrowsInvalidOperationException()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
            {
                _serviceLocator.Get<ITestService>();
            });

            Assert.That(exception.Message, Does.Contain("ITestService"));
            Assert.That(exception.Message, Does.Contain("no registrado en ServiceLocator"));
        }

        [Test]
        public void HasService_ReturnsFalseWhenNotRegistered()
        {
            // Act
            var hasService = _serviceLocator.HasService<ITestService>();

            // Assert
            Assert.IsFalse(hasService, "HasService debe retornar false para un servicio no registrado");
        }

        [Test]
        public void HasService_ReturnsTrueWhenRegistered()
        {
            // Arrange
            var service = new TestService();
            _serviceLocator.Register<ITestService>(service);

            // Act
            var hasService = _serviceLocator.HasService<ITestService>();

            // Assert
            Assert.IsTrue(hasService, "HasService debe retornar true para un servicio registrado");
        }
    }
}
