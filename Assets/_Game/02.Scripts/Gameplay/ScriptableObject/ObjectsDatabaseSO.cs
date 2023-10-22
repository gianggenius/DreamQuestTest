using System;
using System.Collections.Generic;
using _Game._02.Scripts.Core;
using UnityEngine;

namespace _Game._02.Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "ObjectsDatabaseSO", menuName = "DreamQuestTest/ObjectsDatabaseSO", order = 0)]
    public class ObjectsDatabaseSO : ScriptableObject
    {
        public List<ObjectData> objectsData = new();
    }

    [Serializable]
    public class ObjectData : IItem
    {
        public string           Name;
        public int              ID;
        public Vector2Int       Size;
        public ObjectController Prefab;
    }

    [Serializable]
    public class ObjectItem
    {
        public int ObjectID;
        public int Amount;
    }
}