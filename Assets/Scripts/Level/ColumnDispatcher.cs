using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Level
{
    public class ColumnDispatcher : IDispatcher
    {
        float cameraSize = 5f;
        float columnPivot = 10f;
        float _columnDestroyXPosition;
        float _columnSpawnXPosition;
        float birdXPosition = 0f;

        private ColumnsCollection columnsCollection;
        private GameObject _columnPrefab;
        private List<Transform> _columnList;
        private int _columnsPassedCount;
        private int _columnsSpawned;
        private float _gapSize;
        private float _columnSpawnTimer;
        private float _columnSpawnTimerMax;
        private float columnsMoveSpeed = 2f;
        private const float MINHeight = 1;
        private const float MAXHeight = 8;

        public delegate void OnColumnPassed(int correctNumberOfColumnsPassed);
        public event OnColumnPassed SendNumberOfColumns;
        public ColumnDispatcher(ColumnsCollection columnsCollection)
        {
            this.columnsCollection = columnsCollection;
        }

        private enum Difficulty
        {
            Easy,
            Medium,
            Hard,
            Impossible,
        }

        public void Initialize(GameObject columnPrefab)
        {
            Vector3 bottomOfTheScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
            Vector3 topOfTheScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            Vector3 leftOfTheScreen = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2f, 0));
            Vector3 rightOfTheScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2f, 0));

            SetDifficulty(Difficulty.Easy);

            _columnDestroyXPosition = leftOfTheScreen.x - 5f;
            _columnSpawnXPosition = rightOfTheScreen.x + 5f;

            _columnList = new List<Transform>();
            _columnPrefab = columnPrefab;
        }

        public int Spawn()
        {
            HandleColumnsSpawning();
            int numberOfColumnPassed = HandleColumnsMovement();
            return numberOfColumnPassed;
        }

        private void HandleColumnsSpawning()
        {
            _columnSpawnTimer -= Time.deltaTime;
            if (_columnSpawnTimer < 0)
            {
                _columnSpawnTimer += _columnSpawnTimerMax;
                float height = UnityEngine.Random.Range(MINHeight, MAXHeight);
                _gapSize = 23f;
                CreateGapColumns(height, _gapSize, _columnSpawnXPosition);
            }
        }

        private int HandleColumnsMovement()
        {
            bool dontCountBothColumns = true;
            for (int i = 0; i < _columnList.Count; i++)
            {
                Transform column = _columnList[i];
                Vector3 position = column.position;
                bool isToTheRightOfBird = position.x > birdXPosition;
                position += new Vector3(-1, 0, 0) * (columnsMoveSpeed * Time.deltaTime);
                column.position = position;

                if (isToTheRightOfBird && column.position.x <= birdXPosition && dontCountBothColumns)
                {
                    _columnsPassedCount++;
                    dontCountBothColumns = false;
                }
                else
                {
                    dontCountBothColumns = true;
                }

                if (column.position.x < _columnDestroyXPosition)
                {
                    Object.Destroy(column.gameObject);
                    _columnList.Remove(column);
                    i--;
                }
            }

            return _columnsPassedCount;
        }

        private void SetDifficulty(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    _gapSize = 24.5f;
                    _columnSpawnTimerMax = 2.3f;
                    break;
                case Difficulty.Medium:
                    _gapSize = 24f;
                    _columnSpawnTimerMax = 2.2f;
                    break;
                case Difficulty.Hard:
                    _gapSize = 23.5f;
                    _columnSpawnTimerMax = 2.1f;
                    break;
                case Difficulty.Impossible:
                    _gapSize = 23f;
                    _columnSpawnTimerMax = 2f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }

        private Difficulty GetDifficulty()
        {
            if (_columnsSpawned >= 30) return Difficulty.Impossible;
            if (_columnsSpawned >= 20) return Difficulty.Hard;
            if (_columnsSpawned >= 10) return Difficulty.Medium;
            return Difficulty.Easy;
        }

        private void CreateGapColumns(float gapY, float gabSize, float xPosition)
        {
            CreateColumn(xPosition, gapY + gabSize * 0.5f, true);
            CreateColumn(xPosition, cameraSize * 2f - gapY + gabSize * 0.5f, false);
            _columnsSpawned++;
            SendNumberOfColumns?.Invoke(_columnsSpawned);
            SetDifficulty(GetDifficulty());
        }

        private void CreateColumn(float xPosition, float yPosition, bool createOnBottom)
        {
            Transform column = Object.Instantiate(_columnPrefab.transform);
            if (createOnBottom)
            {
                column.position = new Vector2(xPosition, -yPosition + columnPivot);
            }
            else
            {
                column.position = new Vector2(xPosition, yPosition - columnPivot);

                Vector3 localScale = column.localScale;
                localScale = new Vector2(localScale.x, localScale.y * -1);
                column.localScale = localScale;
            }

            _columnList.Add(column);
        }

        public int GetColumnSpawned()
        {
            return _columnsSpawned;
        }

        public int GetColumnsPassedCount()
        {
            return _columnsPassedCount;
        }

        public void GameEnded()
        {
            // int levelReached = columnsPassedCount;
        }
    }
}