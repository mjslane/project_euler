using System.Linq;

namespace app.Euler
{
    public static class Euler
    {
        private static int[] divisors = new[] { 3, 5 };
         public static int Sum(int limit)
        {
            int sum = 0;
            limit -= 1;
            sum = divisors.Sum(x => geometricExpansion(limit, x));
            int duplicatesDivisor = divisors[0] * divisors[1];
            sum -= geometricExpansion(limit, duplicatesDivisor);
            return sum;
        }

        private static int geometricExpansion(int limit, int divisor)
        {
            int numberOfElements = (x / divisor);
            return divisor * numberOfElements * (numberOfElements + 1) / 2;
        }
    }
}
