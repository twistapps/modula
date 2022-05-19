using UnityEditor;
using UnityEngine;

namespace Modula.Common
{
    public static class ModulaEditorUtilities
    {
        public static void HandleDataLayer(ModularBehaviour mb)
        {
            var dataLayerType = mb.GetDataLayerType();
            if (dataLayerType == null) return;
            var data = mb.GetData();
            if (data != null) return;
            // {
            //     if (data.GetType() != mb.GetDataLayerType())
            //     {
            //         DestroyComponentBasedOnEnvironment(data);
            //     }
            //     else
            //     {
            //         return;
            //     }
            // }

            mb.gameObject.AddComponent(dataLayerType);
            mb.OnDataComponentCreated();
        }

        private static void DestroyComponentBasedOnEnvironment(Component component)
        {
            #if UNITY_EDITOR
            if (Application.isEditor && !Application.isPlaying)
                Undo.DestroyObjectImmediate(component);
            else
            #endif
                Object.Destroy(component);
        }
    }
}