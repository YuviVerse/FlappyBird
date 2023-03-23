using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ColumnsCollection", menuName = "ScriptableObjects/ColumnsCollection")]
    public class ColumnsCollection : ScriptableObject
    {
        public List<Column> Columns;
    }

    [Serializable]
    public class Column
    {
        public ColumnType ColumnType;
        public GameObject Prefab;
    }
}

public enum ColumnType
{
    GreenPipe,
    RedPipe
}