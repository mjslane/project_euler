using System.Linq;

namespace app.Euler
{
    public static class Euler
    {
        private static int[] divisors = new[] { 3, 5 };
        public static int Sum(int limit)
        {
            int sum = 0;
            if (limit > 0)
            {
                limit -= 1;
                sum = divisors.Sum(x => geometricSolution(limit, x));
                int duplicatesDivisor = divisors[0] * divisors[1];
                sum -= geometricSolution(limit, duplicatesDivisor);
            }
            return sum;
        }
    
    private static int geometricSolution(int limit, int divisor)
    {
        int numberOfElements = (limit / divisor);
        return divisor * numberOfElements * (numberOfElements + 1) / 2;
    }
}
}
