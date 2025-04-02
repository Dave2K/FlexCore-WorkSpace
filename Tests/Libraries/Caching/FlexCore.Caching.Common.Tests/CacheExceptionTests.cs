using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using FlexCore.Caching.Common.Exceptions;

namespace FlexCore.Caching.Common.Tests
{
    public class CacheExceptionTests
    {
        [Fact]
        public void CacheException_ShouldContainInnerException()
        {
            var innerEx = new Exception("Test inner");
            var ex = new CacheException("Test message", innerEx); // Passa innerEx

            Assert.Equal(innerEx, ex.InnerException);
        }
    }
}