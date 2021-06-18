using System;
using UnityEngine;

namespace DefaultNamespace
{
    
    public class BallReflect : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private Vector2 _lastVelocity;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _lastVelocity = _rb.velocity;
            _rb.AddForce(Vector2.down);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("blocks") || other.gameObject.CompareTag("walls"))
            {
                var speed = _lastVelocity.magnitude;
                var direction = Vector2.Reflect(_lastVelocity.normalized, other.contacts[0].normal);

                _rb.velocity = direction * Mathf.Max(speed, 0f);
                // _rb.AddForce(_rb.velocity.normalized * 20f, ForceMode2D.Impulse);
            }
        }
    }
}