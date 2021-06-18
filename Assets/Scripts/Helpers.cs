using System;

namespace DefaultNamespace
{
    public static class Helpers
    {
        private static readonly Random Random = new Random();
        public static bool GetChance(int chance, int seed)
        {
            var randomNumber = Random.Next(100);
            return randomNumber < chance;
        }
    }
}