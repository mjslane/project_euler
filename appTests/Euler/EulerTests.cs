using System;
using Xunit;
using app.Euler;

namespace appTests
{
    public class EulerTests
    {
        [Theory]
        [InlineData(10, 23)]
        public void EulerCanCalculateCorrectly(int val, int result)
        {
            int underTest = Euler.Sum(val);
            Assert.Equal(result, underTest);
        }

        //[Theory]
        //[InlineData(-1, new object[] { new int[] { 3, 5 }})]
        //[InlineData(0, new object[] { new int[]{ 3, 5 }})]
        //[InlineData(100, new object[] { new int[]{ -3, 5 }})]
        //public void EulerHandlesNegativeNumbers(int value, int[] divisors)
        //{
        //    Assert.Throws<ArgumentException>(() => Euler.Sum(value, divisors));
        //}
    }
}
