using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modula.Common
{
    [Serializable]
    [System.Obsolete("Use TypedList instead.")]
    public class TypeList
    {
        [SerializeField] private bool hasTypes;
        [SerializeField] public List<Type> Types { get; } = new List<Type>();

        public static TypeList None => new TypeList();

        public bool Contains(Type behaviour)
        {
            return hasTypes && Types.Contains(behaviour);
        }

        public TypeList Add(params Type[] types)
        {
            Types.AddRange(types);
            hasTypes = true;
            return this;
        }

        public TypeList Add(Type type)
        {
            Types.Add(type);
            hasTypes = true;
            return this;
        }
    }
}