using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class LevelingManager : MonoBehaviour
    {
        public static int Level { get; set; }

        private void OnEnable()
        {
            PlayerShootBehavior.OnNoBallsFlying += LevelUp;
            GlobalEventManager.OnGameOver += ResetLevel;
        }

        private void OnDisable()
        {
            PlayerShootBehavior.OnNoBallsFlying -= LevelUp;
            GlobalEventManager.OnGameOver -= ResetLevel;
        }

        private void LevelUp()
        {
            Level++;
        }

        private void ResetLevel()
        {
            Level = 0;
        }
    }
}