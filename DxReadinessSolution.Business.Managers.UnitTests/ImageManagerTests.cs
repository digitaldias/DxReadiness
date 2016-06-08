using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DxReadinessSolution.CrossCutting.Testing;
using FluentAssertions;

namespace DxReadinessSolution.Business.Managers.UnitTests
{
    [TestClass]
    public class ImageManagerTests : TestsFor<ImageManager>
    {
        [TestMethod]
        public void ImageManager_IsConstructedWithDependencies()
        {
            Instance.Should().NotBeNull();
        }
    }
}
