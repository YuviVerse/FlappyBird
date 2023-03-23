using UnityEngine;

namespace Level
{
    public interface IDispatcher
    {
        public void Initialize(GameObject gameObject);
        public int Spawn();
    }
}