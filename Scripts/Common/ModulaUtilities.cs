using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Modula.Common
{
    public static class ModulaUtilities
    {
        //cache results of GetDerivedFrom() because it's a pretty expensive method
        private static readonly Dictionary<Type, Type[]> DerivativesDictionary = new Dictionary<Type, Type[]>();

        public static Type[] GetDerivedFrom<T>(Type[] ignored = null)
        {
            Type[] foundArr;
            if (DerivativesDictionary.ContainsKey(typeof(T)))
            {
                // return cached result if GetDerivedFrom() has already been invoked before
                foundArr = DerivativesDictionary[typeof(T)];
            }
            else
            {
                var found = from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                    from assemblyType in domainAssembly.GetTypes()
                    where typeof(T).IsAssignableFrom(assemblyType)
                    select assemblyType;

                foundArr = found as Type[] ?? found.ToArray();

                DerivativesDictionary.Add(typeof(T), foundArr);
            }

            if (ignored != null)
                foundArr = foundArr.Where(t => !ignored.Contains(t)).ToArray();

            return foundArr;
        }

        public static Type[] ToTypesArray<TParentClass>(this string[] typeNames)
        {
            var types = new Type[typeNames.Length];
            for (var i = 0; i < types.Length; i++) types[i] = GetTypeByName<TParentClass>(typeNames[i]);
            return types;
        }

        public static List<Type> ToTypesList<TParentClass>(this List<string> typeNames)
        {
            return ToTypesArray<TParentClass>(typeNames.ToArray()).ToList();
        }

        public static Type GetTypeByName<TParentClass>(string name)
        {
            if (name == null) return null;
            var derivatives = GetDerivedFrom<TParentClass>();
            return derivatives.FirstOrDefault(t => t.Name == name);
        }

        public static bool IsNullOrEmpty<T>(List<T> list)
        {
            if (list == null || list.Count == 0) return true;
            return list.All(t => t == null);
        }

        public static bool IsNullOrEmpty<T>(T[] arr)
        {
            if (arr == null || arr.Length == 0) return true;
            return arr.All(t => t == null);
        }

        public static bool IsNullOrEmpty<T>(ReadOnlyCollection<T> col)
        {
            if (col == null || col.Count == 0) return true;
            return col.All(t => t == null);
        }
    }
}