using System;
using System.Linq;
using UnityEngine;

namespace Modula.Common
{
    /// <summary>
    ///     This class associates Types with their names in string format. This could be useful if you need to get Type by its
    ///     name: 'GetType(string name)'
    ///     All types should be derived from the same interface/class
    /// </summary>
    /// <typeparam name="TParentClass">the parent class/interface for all associated types</typeparam>
    public class TypeNames<TParentClass>
    {
        // [ItemCanBeNull] public ReadOnlyCollection<string> Names => _names == null ? null : Array.AsReadOnly(_names); 
        // [ItemCanBeNull] public ReadOnlyCollection<Type> Types => _types == null ? null : Array.AsReadOnly(_types);

        public TypeNames(string[] typeNames, bool silentExceptions = true)
        {
            Names = new string[typeNames.Length];
            Array.Copy(typeNames, Names, Names.Length);
            Types = Names.ToTypesArray<TParentClass>();
        }

        //todo: copy types or use struct
        public TypeNames(Type[] types, bool silentExceptions = true)
        {
            if (types == null)
            {
                Debug.LogWarning("types array should not be null when creating TypeNames instance");
                return;
            }

            Types = types.Where(t =>
                    t == null || t.IsSubclassOf(typeof(TParentClass)) || typeof(TParentClass).IsAssignableFrom(t))
                .ToArray();
            if (!silentExceptions && Types.Length != types.Length)
                Debug.LogWarning("Passed types array contains elements that are not derived from " +
                                 typeof(TParentClass).Name +
                                 ". They were removed from this instance of TypeNames");

            Names = types.Select(t => t?.Name).ToArray();
        }

        public string[] Names { get; }

        public Type[] Types { get; }

        public string this[int index]
        {
            set
            {
                Names[index] = value;
                Types[index] = ModulaUtilities.GetTypeByName<TParentClass>(value);
            }
        }

        public bool IsNull()
        {
            return Types == null;
        }

        public string GetName(int index)
        {
            return Names[index];
        }

        public Type GetType(int index)
        {
            return Types[index];
        }

        public Type GetType(string name)
        {
            return Types[Array.IndexOf(Names, name)];
        }
    }
}