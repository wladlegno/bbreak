using System;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Random = System.Random;

namespace DefaultNamespace
{
    public enum BlockType
    {
        Default,
        AddBall
    }

    public class BlockBehavior : MonoBehaviour
    {
        public delegate void AddBallAction();

        public static event AddBallAction OnAddBall;
        private BlockType _blockType;

        public BlockType BlockType
        {
            get => _blockType;
            set
            {
                _blockType = value;
                _blockAppearanceBehavior.SetBlockType(value);
            }
        }

        private int _healthPoints;
        public int HealthPoints
        {
            get => _healthPoints;
            set
            {
                _healthPoints = value;
                _blockAppearanceBehavior.SetHealth(value);
                ScorePoints = value;
            }
        }
        
        public int ScorePoints { get; set; }

        private TextMeshProUGUI _healthDisplay;
        private BlockAppearanceBehavior _blockAppearanceBehavior;

        private void Awake()
        {
            _blockAppearanceBehavior = GetComponent<BlockAppearanceBehavior>();
        }

        private void OnEnable()
        {
            _blockAppearanceBehavior.SetBlockType(BlockType);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<BallBehavior>())
            {
                DamageBlock();
            }
        }

        private void DamageBlock()
        {
            HealthPoints -= 1;
            if (HealthPoints == 0)
            {
                if (BlockType == BlockType.AddBall)
                    OnAddBall?.Invoke();
                Destroy(gameObject);
            }

            _blockAppearanceBehavior.SetHealth(HealthPoints);
        }
    }
}