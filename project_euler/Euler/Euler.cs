using System.Linq;

namespace app.Euler
{
    public static class Euler
    {
        private static int[] divisors = new[] { 3, 5 };
         public static int Sum(int e)
        {
            int sum = 0;
            sum = divisors.Sum(x => geometricExpansion(e-1, x));
            return sum;
        }

        private static int geometricExpansion(int x, int divisor)
        {
            int v = (x / divisor);
            return divisor * v * (v + 1) / 2;
        }
    }
}
