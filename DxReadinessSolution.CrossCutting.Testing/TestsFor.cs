using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap.AutoMocking.Moq;

namespace DxReadinessSolution.CrossCutting.Testing
{
    [TestClass]
    public class TestsFor<TEntity> where TEntity : class
    {
        protected TEntity Instance { get; set; }

        protected MoqAutoMocker<TEntity> AutoMocker { get; set; }


        [TestInitialize]
        public void Before_Each()
        {
            AutoMocker = new MoqAutoMocker<TEntity>();

            Instance = AutoMocker.ClassUnderTest;
        }


        public Mock<TContract> GetMockFor<TContract>() where TContract : class
        {
            return Mock.Get(AutoMocker.Get<TContract>());
        }

    }
}
