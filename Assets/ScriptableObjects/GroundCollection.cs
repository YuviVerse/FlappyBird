using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GroundCollection", menuName = "ScriptableObjects/GroundCollection")]
    public class GroundCollection : ScriptableObject
    {
        public List<Ground> Grounds;
    }

    [Serializable]
    public class Ground
    {
        public GroundType GroundType;
        public GameObject Prefab;
    }
}

public enum GroundType
{
    Grass
}