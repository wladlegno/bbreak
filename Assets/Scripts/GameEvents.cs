using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class GameEvents : MonoBehaviour
    {
        public static GameEvents Current;

        private void Awake()
        {
            Current = this;
        }

        public event Action OnGameOver;
        public event Action OnNoBallsFlying;
        public event Action<GameObject> OnBallTouchBottomWall;
        public event Action OnAddBall;
        public event Action OnShiftBlock;
        public event Action<GameObject> OnExplodeBlock;


        public void RaiseOnGameOver()
        {
            OnGameOver?.Invoke();
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        public void RaiseOnBallTouchBottomWall(GameObject obj)
        {
            OnBallTouchBottomWall?.Invoke(obj);
        }

        public void RaiseOnNoBallsFlying()
        {
            OnNoBallsFlying?.Invoke();
        }

        public void RaiseOnAddBall()
        {
            OnAddBall?.Invoke();
        }

        public void RaiseOnShiftBlock()
        {
            OnShiftBlock?.Invoke();
        }

        public void RaiseOnExplodeBlock(GameObject obj)
        {
            OnExplodeBlock?.Invoke(obj);
        }
    }
}