using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Level
{
    public class GroundDispatcher : IDispatcher
    {

        private GroundCollection groundCollection;
        private List<Transform> _groundList;
        private GameObject _groundPrefab;
        private float groundMoveSpeed = 10f;
        private float _groundWidth = 50;
        private float _groundDestroyXPosition;
        private float _groundYPosition;
        private float _groundSpawnXPosition;

        public GroundDispatcher(GroundCollection groundCollection)
        {
            this.groundCollection = groundCollection;
        }

        public void Initialize(GameObject groundPrefab)
        {
            Vector3 bottomOfTheScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
            // Vector3 leftSideOfTheScreen = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            
            _groundWidth = groundPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            _groundPrefab = groundPrefab;
            _groundYPosition = bottomOfTheScreen.y;
            _groundDestroyXPosition = -_groundWidth;
            _groundSpawnXPosition = _groundWidth;
            
            SpawnInitialGround();
        }

        public int Spawn()
        {
            HandleGround();
            return 0;
        }

        private void SpawnInitialGround()
        {
            Transform groundTransform;
            _groundList = new List<Transform>();

            groundTransform =
                Object.Instantiate(_groundPrefab.transform, new Vector3(-_groundWidth, _groundYPosition, 0), Quaternion.identity);
            _groundList.Add(groundTransform);
            groundTransform = Object.Instantiate(_groundPrefab.transform, new Vector3(0, _groundYPosition, 0),
                Quaternion.identity);
            _groundList.Add(groundTransform);
            groundTransform = Object.Instantiate(_groundPrefab.transform, new Vector3(_groundWidth, _groundYPosition, 0),
                Quaternion.identity);
            _groundList.Add(groundTransform);
        }

        private void HandleGround()
        {
            foreach (Transform groundTransform in _groundList)
            {
                groundTransform.position += new Vector3(-1, 0, 0) * (groundMoveSpeed * Time.deltaTime);

                if (groundTransform.position.x < _groundDestroyXPosition)
                {
                    Vector3 position = groundTransform.position;
                    position = new Vector3(_groundSpawnXPosition + _groundWidth, position.y, position.z);
                    groundTransform.position = position;
                }
            }
        }
    }
}