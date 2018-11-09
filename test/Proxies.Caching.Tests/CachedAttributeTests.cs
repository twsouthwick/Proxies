using System;
using Xunit;

namespace Proxies.Caching.Tests
{
    public class CachedAttributeTests
    {
        [Fact]
        public void DefaultConstructor()
        {
            var attribute = new CachedAttribute();

            Assert.False(attribute.Duration.HasValue);
        }

        [Fact]
        public void NegativeMinutes()
        {
            var attribute = new CachedAttribute(minutes: -1);

            Assert.False(attribute.Duration.HasValue);
        }

        [Fact]
        public void NegativeHours()
        {
            var attribute = new CachedAttribute(hours: -1);

            Assert.False(attribute.Duration.HasValue);
        }

        [Fact]
        public void NegativeDays()
        {
            var attribute = new CachedAttribute(days: -1);

            Assert.False(attribute.Duration.HasValue);
        }

        [Theory]
        [InlineData(0, 0, 1)]
        [InlineData(0, 1, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(1, 2, 3)]
        public void TimespanSet(int days, int hours, int minutes)
        {
            var attribute = new CachedAttribute(days, hours, minutes);
            var expected = new TimeSpan(days, hours, minutes, 0);

            Assert.True(attribute.Duration.HasValue);
            Assert.Equal(expected, attribute.Duration.Value);
        }
    }
}
