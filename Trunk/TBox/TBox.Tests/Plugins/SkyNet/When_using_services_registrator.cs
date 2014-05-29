using LightInject;
using Mnk.TBox.Core.Contracts;
using Mnk.TBox.Plugins.SkyNet.Code;
using Mnk.TBox.Plugins.SkyNet.Code.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace Mnk.TBox.Tests.Plugins.SkyNet
{
    [TestFixture]
    class When_using_services_registrator
    {
        private IServiceContainer container;
        
        [SetUp]
        public void SetUp()
        {
            container = ServicesRegistrator.Register(MockRepository.GenerateMock<IPluginContext>());
        }

        [TearDown]
        public void TearDown()
        {
            container.Dispose();
        }

        [Test]
        public void Should_register_settings_logic()
        {
            //Act
            var s = container.GetInstance<ISettingsLogic>();

            //Assert
            Assert.IsNotNull(s);
        }

        [Test]
        public void Should_register_executor()
        {
            //Act
            var s = container.GetInstance<ITaskExecutor>();

            //Assert
            Assert.IsNotNull(s);
        }

    }
}
