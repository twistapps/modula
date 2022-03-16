using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modula
{
    [Serializable]
    public class TypedList<T> : IEnumerable<Type>
    {
        [SerializeField] private bool _hasTypes;
        [SerializeField] public List<Type> Types { get; } = new List<Type>();

        public bool Contains(Type behaviour)
        {
            return _hasTypes && Types.Contains(behaviour);
        }

        public bool Contains<TA>() where TA : T
        {
            return _hasTypes && Types.Contains(typeof(TA));
        }

        public TypedList<T> Add<TA>() where TA : T
        {
            Types.Add(typeof(TA));
            _hasTypes = true;
            return this;
        }

        public TypedList<T> Add(Type type)
        {
            if (typeof(T).IsAssignableFrom(type) || type.IsSubclassOf(typeof(T)))
            {
                Types.Add(type);
                _hasTypes = true;
                return this;
            }

            Debug.LogError("Type should be subclass of " + typeof(T).Name);
            return null;
        }

        public IEnumerator<Type> GetEnumerator()
        {
            return Types.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}