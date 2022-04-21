using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modula.Common
{
    [Serializable]
    public class TypedList<T> : IEnumerable<Type>
    {
        [SerializeField] private bool hasTypes;
        [SerializeField] public List<Type> Types { get; } = new();

        public IEnumerator<Type> GetEnumerator()
        {
            return Types.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(Type behaviour)
        {
            return hasTypes && Types.Contains(behaviour);
        }

        public bool Contains<TA>() where TA : T
        {
            return hasTypes && Types.Contains(typeof(TA));
        }

        public TypedList<T> Add<TA>() where TA : T
        {
            Types.Add(typeof(TA));
            hasTypes = true;
            return this;
        }

        public TypedList<T> Add(Type type)
        {
            if (type == null) return this;
            if (typeof(T).IsAssignableFrom(type) || type.IsSubclassOf(typeof(T)))
            {
                Types.Add(type);
                hasTypes = true;
                return this;
            }

            Debug.LogError("Type should be subclass of " + typeof(T).Name);
            return null;
        }
    }
}