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
            _textMeshProUGUI.color = Color.black;
            _spriteRenderer.color = Color.white;
            
            switch (blockType)
            {
                case BlockType.AddBall:
                    _spriteRenderer.color = Color.green;
                    _textMeshProUGUI.color = Color.white;
                    break;
                case BlockType.Default:
                    _spriteRenderer.color = Color.white;
                    break;
                case BlockType.Explosive:
                    _spriteRenderer.color = Color.red;
                    _textMeshProUGUI.color = Color.white;
                    break;
                case BlockType.Shifter:
                    _spriteRenderer.color = Color.blue;
                    _textMeshProUGUI.color = Color.white;
                    break;
            }
        }

        public void SetHealth(int health)
        {
            _textMeshProUGUI.text = health.ToString();
        }
    }
}