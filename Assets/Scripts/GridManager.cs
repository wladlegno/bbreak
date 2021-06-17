using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace DefaultNamespace
{
    public class GridManager : MonoBehaviour
    {
        private int _rows = 4;
        private int _cols = 5;
        private float _tileSize = 1f;
        private List<List<GameObject>> _grid;

        private void Start()
        {
            _grid = new List<List<GameObject>>();
            GenerateInitialGrid();
        }

        private void OnEnable()
        {
            PlayerShootBehavior.OnNoBallsFlying += RaiseOnNoBallsFlying;
        }

        private void OnDisable()
        {
            PlayerShootBehavior.OnNoBallsFlying -= RaiseOnNoBallsFlying;
        }

        private void RaiseOnNoBallsFlying()
        {
            ShiftRows(1);
            AddRows(1);
            _grid[0] = GenerateRow(0);
            if (_grid.Count == 9)
            {
                GlobalEventManager.RaiseOnGameOver();
            }

            _grid.RemoveAll(row => row.Count(o => o != null) == 0);
        }

        private void GenerateInitialGrid()
        {
            for (int row = 0; row < _rows; row++)
            {
                _grid.Add(GenerateRow(row));
            }

            // transform.position = new Vector3(-2, 4.5f, 0);
        }

        private List<GameObject> GenerateRow(int rowNumber)
        {
            var prefabBlock = (GameObject) Instantiate(Resources.Load("Prefabs/Block"));
            List<GameObject> newRow = new List<GameObject>();

            for (var col = 0; col < _cols; col++)
            {
                var gridBlock = Instantiate(prefabBlock, transform);
                gridBlock.transform.localPosition = GetBlockPosition(rowNumber, col);
                var blockBehavior = gridBlock.GetComponent<BlockBehavior>();

                SetBlockHealth(gridBlock, blockBehavior);
                SetBlockType(gridBlock, blockBehavior);

                newRow.Add(gridBlock);
            }

            Destroy(prefabBlock);

            return newRow;
        }

        private void ShiftRows(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                for (var row = 0; row < _grid.Count; row++)
                {
                    for (var col = 0; col < _grid[row].Count; col++)
                    {
                        if (_grid[row][col] != null)
                        {
                            _grid[row][col].transform.localPosition = GetBlockPosition(row + 1, col);
                        }
                    }
                }
            }
        }

        private void AddRows(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                _grid.Insert(0, new List<GameObject>());
            }
        }

        private Vector2 GetBlockPosition(int row, int col)
        {
            float posX = col * _tileSize;
            float posY = row * -_tileSize;

            return new Vector2(posX, posY);
        }

        private void SetBlockHealth(GameObject gridBlock, BlockBehavior blockBehavior)
        {
            Random random = new Random(gridBlock.GetInstanceID());
            blockBehavior.HealthPoints = random.Next(1 + LevelingManager.Level, 6 + LevelingManager.Level);
        }

        private void SetBlockType(GameObject gridBlock, BlockBehavior blockBehavior)
        {
            blockBehavior.BlockType = BlockType.Default;
            if (Helpers.GetChance(20, gridBlock.GetInstanceID()))
                blockBehavior.BlockType = BlockType.AddBall;
        }
    }
}