using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Debug = System.Diagnostics.Debug;

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

            #if UNITY_2021_2_OR_NEWER
            //prefab mode support
            var allInstances = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null 
                ? Resources.FindObjectsOfTypeAll<MonoBehaviour>().OfType<T>() 
                : Object.FindObjectsOfType<MonoBehaviour>().OfType<T>();
            #else
            var allInstances = Object.FindObjectsOfType<MonoBehaviour>().OfType<T>();
            #endif
            
            var found = new List<T>();
            foreach (var component in allInstances)
            {
                var mono = component as MonoBehaviour;
                Debug.Assert(mono != null, nameof(mono) + " != null");
                if (mono.gameObject != obj) continue;
                found.Add(component);
            }

            return found;
        }
    }
}