using DxReadinessSolution.CrossCutting.Testing;
using DxReadinessSolution.Domain.Contracts;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace DxReadinessSolution.Business.UnitTests
{
    [TestClass]
    public class ExceptionHandlerTests : TestsFor<ExceptionHandler>
    {
        [TestMethod]
        public void ExceptionHandler_CanBeConstructedByTestFramework()
        {
            Instance.Should().NotBeNull();
        }


        [TestMethod]
        public void ExceptionHandler_Get_CanGetUnsafeFunctionsWithoutCrashing()
        {
            // Act
            var result = Instance.Get(CrashingFunction);

            // Assert
            result.Should().Be(0, "the default of integer was expected to be 0");
        }


        [TestMethod]
        public void ExceptionHandler_Get_LogsExceptions()
        {
            // Act
            var result = Instance.Get(CrashingFunction);

            // Assert
            GetMockFor<ILogger>()
                .Verify(logger => logger.LogException(It.IsAny<Exception>()), Times.Once());
        }


        [TestMethod]
        public void ExceptionHandler_Get_WillReturnTheFunctionResult()
        {
            Instance.Get(SafeFunction).Should().Be(10, "the safe function should have returned 10");
        }


        [TestMethod]
        public void ExceptionHandler_Run_CanRunUnsafeActionsWithoutCrashing()
        {
            Instance.Run(UnsafeAction);
        }


        [TestMethod]
        public void ExceptionHandler_Run_LogsExceptions()
        {
            // Act
            Instance.Run(UnsafeAction);

            // Assert
            GetMockFor<ILogger>()
                .Verify(logger => logger.LogException(It.IsAny<Exception>()), Times.Once());
        }


        [TestMethod]
        public void ExceptionHandler_Run_WillExecuteTheProvidedAction()
        {
            // Arrange
            int i = 0;
            Action action = () => i++;

            // Act
            Instance.Run(action);

            // Assert
            i.Should().Be(1, "we incremented it from zero");

        }


        private Action UnsafeAction
        {
            get
            {
                return new Action(() => {
                    var ten = 10;
                    var zero = 0;
                    var i = ten / zero;
                });
            }
        }


        private Func<int> CrashingFunction
        {
            get
            {
                var ten = 10;
                var zero = 0;
                return () => ten / zero;
            }
        }


        private Func<int> SafeFunction
        {
            get { return () => 10; }
        }
    }
}
