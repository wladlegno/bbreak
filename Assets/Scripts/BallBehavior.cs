using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class BallBehavior : MonoBehaviour
    {
        private Vector3 _startingPosition;
        private Rigidbody2D _rb;

        private void Start()
        {
            _startingPosition = gameObject.transform.position;
            _rb = GetComponent<Rigidbody2D>();
            Physics2D.IgnoreLayerCollision(9, 9, true);
            _rb.freezeRotation = true;
        }

        private void Update()
        {
            var velocity = _rb.velocity;
            _rb.AddForce(velocity.normalized*20f - velocity, ForceMode2D.Impulse);
        }

        private void OnEnable()
        {
            GameEvents.Current.OnBallTouchBottomWall += BallToBase;
        }

        private void OnDisable()
        {
            GameEvents.Current.OnBallTouchBottomWall -= BallToBase;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("wall destroyer"))
            {
                GameEvents.Current.RaiseOnBallTouchBottomWall(gameObject);
            }
        }

        private void BallToBase(GameObject ego)
        {
            ego.SetActive(false);
            ego.transform.position = _startingPosition;
        }
    }
}