using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class LevelingManager : MonoBehaviour
    {
        public static int Level { get; set; }

        private void OnEnable()
        {
            GameEvents.Current.OnNoBallsFlying += LevelUp;
            GameEvents.Current.OnGameOver += ResetLevel;
        }

        private void OnDisable()
        {
            GameEvents.Current.OnNoBallsFlying -= LevelUp;
            GameEvents.Current.OnGameOver -= ResetLevel;
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