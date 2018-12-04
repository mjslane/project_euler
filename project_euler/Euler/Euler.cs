using System.Linq;

namespace app.Euler
{
    public static class Euler
    {
        private static int[] divisors = new[] { 3, 5 };
         public static int Sum(int e)
        {
            int sum = 0;
            e -= 1;
            sum = divisors.Sum(x => geometricExpansion(e, x));
            int v = divisors[0] * divisors[1];
            sum -= geometricExpansion(e, v);
            return sum;
        }

        private static int geometricExpansion(int x, int divisor)
        {
            int v = (x / divisor);
            return divisor * v * (v + 1) / 2;
        }
    }
}
