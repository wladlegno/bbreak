using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;
using Timer = System.Timers.Timer;

namespace DefaultNamespace
{
    public class PlayerShootBehavior : MonoBehaviour
    {
        public delegate void NoBallsFlyingAction();
        public static event NoBallsFlyingAction OnNoBallsFlying;
        
        private readonly List<GameObject> _balls = new List<GameObject>(5);
        public GameObject ballPrefab;

        private static bool CanShoot => BallsFlying == 0;

        private GameObject _ballsArray;
        public static int BallsFlying { get; set; }

        private void Start()
        {
            _ballsArray = GameObject.Find("BallsArray");
            ballPrefab = Resources.Load("Prefabs/Ball") as GameObject;
        }

        private void OnEnable()
        {
            BallBehavior.OnBallTouchBottomWall += RaiseOnBallTouchBottomWall;
            BlockBehavior.OnAddBall += RaiseOnAddBall;
            OnNoBallsFlying += AddBallsToCapacity;
        }

        private void OnDisable()
        {
            BallBehavior.OnBallTouchBottomWall -= RaiseOnBallTouchBottomWall;
            BlockBehavior.OnAddBall -= RaiseOnAddBall;
            OnNoBallsFlying -= AddBallsToCapacity;
        }

        public IEnumerator Shoot(Vector2 direction)
        {
            if (!CanShoot) yield break;
            
            BallsFlying = _balls.Capacity;
            if (_balls.Count > 0)
            {
                foreach (var ballInstance in _balls)
                {
                    // ballInstance.transform.position = transform.position;

                    ballInstance.SetActive(true);
                    
                    var rb = ballInstance.GetComponent<Rigidbody2D>();
                    rb.velocity = direction * 20f;

                    yield return new WaitForSecondsRealtime(50f / 1000);
                }
            }
            else
            {
                for (int i = 0; i < _balls.Capacity; i++)
                {
                    var ballInstance = CreateBall();
                    _balls.Add(ballInstance);

                    var rb = ballInstance.GetComponent<Rigidbody2D>();
                    rb.velocity = direction * 20f;

                    yield return new WaitForSecondsRealtime(50f / 1000);
                }
            }
        }

        private GameObject CreateBall()
        {
            var ballInstance = Instantiate(ballPrefab, _ballsArray.transform, true);
            ballInstance.transform.position = transform.position;

            return ballInstance;
        }

        private void RaiseOnBallTouchBottomWall(GameObject ego)
        {
            --BallsFlying;
            print($"balls flying {BallsFlying}");

            if (BallsFlying == 0)
                OnNoBallsFlying?.Invoke();
        }

        private void RaiseOnAddBall()
        {
            _balls.Capacity++;
        }

        private void AddBallsToCapacity()
        {
            for (var i = _balls.Count; i < _balls.Capacity; i++)
            {
                var ballInstance = CreateBall();
                _balls.Add(ballInstance);
                ballInstance.SetActive(false);
            }
        }
    }
}