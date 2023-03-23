using System;
using System.Collections.Generic;
using UnityEngine;


namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "BirdsCollection", menuName = "ScriptableObjects/BirdsCollection")]
    public class BirdCollection : ScriptableObject
    {
        public List<Bird> Birds;
    }

    [Serializable]
    public class Bird
    {
        public BirdType BirdType;
        public GameObject Prefab;
    }
}

public enum BirdType
{
    YellowBird,
    BlueBird,
    RedBird
}
