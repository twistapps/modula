using UnityEditor;
using UnityEngine;

namespace Modula.Scripts.Common
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
            if (Application.isEditor && !Application.isPlaying)
            {
                Undo.DestroyObjectImmediate(component);
            }
            else
            {
                GameObject.Destroy(component);
            }
        }
    }
}