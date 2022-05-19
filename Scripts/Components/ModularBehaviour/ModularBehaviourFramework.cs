using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modula
{
    public static class ModularBehaviourFramework
    {
        /// <summary>
        ///     Same as GetComponents() but for interfaces.
        ///     Finds all MonoBehaviours that derive from interface T and returns them as list of T.
        /// </summary>
        /// <returns>List of MonoBehaviours deriving from T in gameObject</returns>
        public static List<T> FindComponents<T>(this GameObject obj)
        {
            var allInstances = Object.FindObjectsOfType<MonoBehaviour>().OfType<T>();

            var found = new List<T>();
            foreach (var component in allInstances)
            {
                var mono = component as MonoBehaviour;
                if (mono.gameObject != obj) continue;
                found.Add(component);
            }

            return found;
        }
    }
}