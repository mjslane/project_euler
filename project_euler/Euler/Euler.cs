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
                sum = divisors.Sum(x => GeometricSolution(limit, x));
                int duplicatesDivisor = divisors[0] * divisors[1];
                sum -= GeometricSolution(limit, duplicatesDivisor);
            }
            return sum;
        }
    
    private static int GeometricSolution(int limit, int divisor)
    {
        int numberOfElements = (limit / divisor);
        return divisor * numberOfElements * (numberOfElements + 1) / 2;
    }
}
}
