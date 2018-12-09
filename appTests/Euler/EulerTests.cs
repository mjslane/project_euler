using System;
using Xunit;
using app.Euler;

namespace appTests
{
    public class EulerTests
    {
        [Theory]
        [InlineData(10, 23)]
        [InlineData(1000, 233168)]
        [InlineData(0,0)]
        [InlineData(-1,0)]
        [InlineData(-10,0)]
        public void EulerCanCalculateCorrectly(int val, int expected)
        {
            int underTest = Euler.Sum(val);
            Assert.True(expected.Equals(underTest),$"Expected {expected} but got {underTest}");
        }
    }
}
