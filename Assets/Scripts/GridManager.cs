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
            GameEvents.Current.OnNoBallsFlying += RaiseOnNoBallsFlying;
            GameEvents.Current.OnExplodeBlock += RaiseOnExplodeBlock;
            GameEvents.Current.OnShiftBlock += RaiseOnShiftBlock;
        }

        private void OnDisable()
        {
            GameEvents.Current.OnNoBallsFlying -= RaiseOnNoBallsFlying;
            GameEvents.Current.OnExplodeBlock -= RaiseOnExplodeBlock;
            GameEvents.Current.OnShiftBlock -= RaiseOnShiftBlock;
        }

        private void RaiseOnNoBallsFlying()
        {
            ShiftRows();
            AddRows();
            _grid[0] = GenerateRow(0);
            _grid.RemoveAll(row => row.Count(o => o != null) == 0);

            if (_grid.Count == 9)
            {
                GameEvents.Current.RaiseOnGameOver();
            }
        }

        private void RaiseOnExplodeBlock(GameObject ego)
        {
            ExplodeBlock(ego);
        }

        private void RaiseOnShiftBlock()
        {
            ShiftRowsReverse();
            RemoveRows();
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

                GenerateBlockType(gridBlock, blockBehavior);
                SetBlockHealth(gridBlock, blockBehavior);

                newRow.Add(gridBlock);
            }

            Destroy(prefabBlock);

            return newRow;
        }

        private void ShiftRows(int amount = 1)
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

        private void ShiftRowsReverse(int amount = 1)
        {
            for (var i = 0; i < amount; i++)
            {
                for (var row = 0; row < _grid.Count; row++)
                {
                    for (var col = 0; col < _grid[row].Count; col++)
                    {
                        if (_grid[row][col] != null)
                        {
                            _grid[row][col].transform.localPosition = GetBlockPosition(row - 1, col);
                        }
                    }
                }
            }
        }

        private void AddRows(int amount = 1)
        {
            for (var i = 0; i < amount; i++)
            {
                _grid.Insert(0, new List<GameObject>());
            }
        }

        private void RemoveRows(int amount = 1)
        {
            for (var i = 0; i < amount; i++)
            {
                if (i < _grid.Count)
                    _grid.RemoveAt(0);
                else
                    break;
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

            if (blockBehavior.BlockType == BlockType.Explosive
                || blockBehavior.BlockType == BlockType.Shifter)
            {
                blockBehavior.HealthPoints += 10;
            }
        }

        private void GenerateBlockType(GameObject gridBlock, BlockBehavior blockBehavior)
        {
            blockBehavior.BlockType = BlockType.Default;
            if (Helpers.GetChance(20, gridBlock.GetInstanceID()))
                blockBehavior.BlockType = BlockType.AddBall;
            else if (Helpers.GetChance(5, gridBlock.GetInstanceID()))
                blockBehavior.BlockType = BlockType.Explosive;
            else if (Helpers.GetChance(5, gridBlock.GetInstanceID()))
                blockBehavior.BlockType = BlockType.Shifter;
        }

        private (int Row, int Col) GetGridRowAndCol(GameObject ego)
        {
            var egoRow = _grid.FindIndex(row => row.Contains(ego));
            var egoCol = _grid[egoRow].FindIndex(o => o == ego);

            return (egoRow, egoCol);
        }

        private (int RowStart, int RowEnd, int ColStart, int ColEnd) GetSurroundingBlockIndexes((int Row, int Col) blockCoords)
        {
            (int RowStart, int RowEnd, int ColStart, int ColEnd) coords = (-1, -1, -1, -1);
            coords.RowStart = blockCoords.Row - 1;
            coords.RowEnd = blockCoords.Row + 1;
            coords.ColStart = blockCoords.Col - 1;
            coords.ColEnd = blockCoords.Col + 1;

            if (coords.RowStart < 0)
                coords.RowStart = 0;
            if (coords.RowEnd > _grid.Count - 1)
                coords.RowEnd = _grid.Count - 1;
            if (coords.ColStart < 0)
                coords.ColStart = 0;
            if (coords.ColEnd > _grid[blockCoords.Row].Count - 1)
                coords.ColEnd = _grid[blockCoords.Row].Count - 1;

            return coords;
        }

        private void ExplodeBlock(GameObject ego)
        {
            var coords = GetGridRowAndCol(ego);
            var surroundingBlockIndexes = GetSurroundingBlockIndexes(coords);
            for (var row = surroundingBlockIndexes.RowStart; row <= surroundingBlockIndexes.RowEnd; row++)
            {
                for (var col = surroundingBlockIndexes.ColStart; col <= surroundingBlockIndexes.ColEnd; col++)
                {
                   Destroy(_grid[row][col]);
                }
            }
        }
    }
}