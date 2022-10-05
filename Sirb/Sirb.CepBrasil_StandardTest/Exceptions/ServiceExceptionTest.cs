using System;
using Sirb.CepBrasil_Standard.Exceptions;
using Xunit;

namespace Sirb.CepBrasil_StandardTest.Exceptions
{
    public class ServiceExceptionTest
    {
        private const string FallbackMessage = "Ocorreu um erro ao tentar realizar a consulta. Tente novamente mais tarde.";

        [Fact]
        public void Constructor_Valid()
        {
            var exception = new ServiceException();
            Assert.Null(exception.InnerException);
            Assert.NotNull(exception.Message);
            Assert.NotEmpty(exception.Message);
            Assert.Equal(FallbackMessage, exception.Message);
        }

        [Theory]
        [InlineData("Message 1")]
        [InlineData("")]
        [InlineData(null)]
        public void Constructor_Case(string message)
        {
            var exception = new ServiceException(message);
            Assert.NotNull(exception.Message);
            Assert.NotEmpty(exception.Message);
            Assert.Null(exception.InnerException);

            var expected = string.IsNullOrEmpty(message?.Trim()) ? FallbackMessage : message;
            Assert.Equal(expected, exception.Message);
        }

        [Fact]
        public void Constructor_InnerException()
        {
            var inner = new Exception("Test");
            var exception = new ServiceException(inner);
            Assert.NotNull(exception.Message);
            Assert.NotEmpty(exception.Message);
            Assert.NotNull(exception.InnerException);
        }

        [Fact]
        public void Constructor_MessageInnerException()
        {
            var inner = new Exception("Test 2");
            var exception = new ServiceException("Test 1", inner);
            Assert.NotNull(exception.Message);
            Assert.NotEmpty(exception.Message);
            Assert.NotNull(exception.InnerException);
        }

        [Theory]
        [InlineData("message", true)]
        [InlineData("message", false)]
        public void ThrowIf_Test(string message, bool isThrowing)
        {
            if (isThrowing)
            {
                Assert.Throws<ServiceException>(() => ServiceException.ThrowIf(isThrowing, message));
            }
            else
            {
                ServiceException.When(isThrowing, message);
                Assert.False(isThrowing);
            }
        }
    }
}
