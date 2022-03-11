using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modula
{
    [Serializable]
    public class TypeList
    {
        [SerializeField] private bool _hasTypes;
        [SerializeField] public List<Type> Types { get; } = new List<Type>();

        public static TypeList None => new TypeList();

        public bool Contains(Type behaviour)
        {
            return _hasTypes && Types.Contains(behaviour);
        }

        public TypeList Add(params Type[] types)
        {
            Types.AddRange(types);
            _hasTypes = true;
            return this;
        }

        public TypeList Add(Type type)
        {
            Types.Add(type);
            _hasTypes = true;
            return this;
        }
    }
}