using System;

namespace Misc
{
    internal static class RandomHandler
    {
        public static Random _random = new();

        public static double GenerateRandomDouble(double minimum, double maximum)
        {
            return _random.NextDouble() * (maximum - minimum) + minimum;
        }

        public static float GenerateRandomFloat(float minimum, float maximum)
        {
            return (float)_random.NextDouble() * (maximum - minimum) + minimum;
        }

        public static int GenerateRandomInt(int minimum, int maximum)
        {
            return _random.Next(minimum, maximum + 1);
        }

        public static bool GenerateRandomBool()
        {
            return _random.NextDouble() >= 0.5;
        }
    }
}
