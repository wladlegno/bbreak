using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class BallBehavior : MonoBehaviour
    {
        public delegate void BallTouchBottomWallAction(GameObject gameObject);

        public static event BallTouchBottomWallAction OnBallTouchBottomWall;

        private Vector3 _startingPosition;
        private Rigidbody2D _rb;
        public bool IsShot { get; private set; }
        public bool IsFlying { get; private set; }

        private void Start()
        {
            _startingPosition = gameObject.transform.position;
            IsShot = false;
            IsFlying = false;
            _rb = GetComponent<Rigidbody2D>();
            Physics2D.IgnoreLayerCollision(9, 9, true);
            _rb.freezeRotation = true;
        }

        private void Update()
        {
            var velocity = _rb.velocity;
            _rb.AddRelativeForce(velocity.normalized * 20f - velocity);
        }

        private void OnEnable()
        {
            OnBallTouchBottomWall += BallToBase;
        }

        private void OnDisable()
        {
            OnBallTouchBottomWall -= BallToBase;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("wall destroyer"))
            {
                OnBallTouchBottomWall?.Invoke(gameObject);
            }
        }

        private void BallToBase(GameObject ego)
        {
            ego.SetActive(false);
            ego.transform.position = _startingPosition;
        }
    }
}