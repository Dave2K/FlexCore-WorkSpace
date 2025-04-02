using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using FlexCore.Caching.Common.Exceptions;

namespace FlexCore.Caching.Core.Tests
{
    public class CacheExceptionTests
    {
        [Fact]
        public void CacheException_ShouldContainInnerException()
        {
            var loggerMock = new Mock<ILogger<CacheException>>();
            var innerEx = new Exception("Test inner");

            var ex = new CacheException("Test message", innerEx);

            Assert.Equal(innerEx, ex.InnerException);
        }
    }
}