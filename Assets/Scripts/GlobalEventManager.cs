using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class GlobalEventManager : MonoBehaviour
    {
        public delegate void GameOverAction();

        public static event GameOverAction OnGameOver;

        public static void RaiseOnGameOver()
        {
            OnGameOver?.Invoke();
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}