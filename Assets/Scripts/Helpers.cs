using System;

namespace DefaultNamespace
{
    public static class Helpers
    {
        public static bool GetChance(int chance, int seed)
        {
            var randomNumber = new Random(seed).Next(100);
            return randomNumber < chance;
        }
    }
}