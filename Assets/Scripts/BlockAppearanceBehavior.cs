using System;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    public class BlockAppearanceBehavior : MonoBehaviour
    {
        private TextMeshProUGUI _textMeshProUGUI;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public void SetBlockType(BlockType blockType)
        {
            switch (blockType)
            {
                case BlockType.AddBall:
                    _spriteRenderer.color = Color.blue;
                    break;
                default:
                    _spriteRenderer.color = Color.white;
                    break;
            }
        }

        public void SetHealth(int health)
        {
            _textMeshProUGUI.text = health.ToString();
        }
    }
}