using Modula.Common;
using Modula.Scripts.Common;
using Modula.Scripts.Common.Editor;
using UnityEditor;
using UnityEngine;

namespace Modula.Editor
{
    [CustomEditor(typeof(ModularBehaviour), true)]
    public class ModularBehaviourEditor : ModularBehaviourEditorBase
    {
        public override void OnInspectorGUI()
        {
            var moduleManager = (ModularBehaviour)target;
            DrawModuleManager();
            ShowDebugInfo();
            ModulaEditorUtilities.HandleDataLayer(moduleManager);
        }

        private void ShowDebugInfo()
        {
            var moduleManager = (ModularBehaviour)target;
            GUILayout.Space(20);
            ModulaSettings.DebugMode = EditorGUILayout.Toggle(new GUIContent("Modular Entities Debug Mode"),
                ModulaSettings.DebugMode);

            if (ModulaSettings.DebugMode) base.OnInspectorGUI();

            foreach (var module in moduleManager.GetAttachments())
                module.hideFlags = ModulaSettings.DebugMode ? HideFlags.None : HideFlags.HideInInspector;

            // potential bugfix
            // if (ModulaSettings.DebugMode)
            // {
            //     foreach (var component in moduleManager.GetComponents(typeof(Component)))
            //         component.hideFlags = HideFlags.None;
            //     base.OnInspectorGUI();
            // }
            // else
            // {
            //     foreach (var module in moduleManager.GetModules())
            //         module.hideFlags = ModulaSettings.DebugMode ? HideFlags.None : HideFlags.HideInInspector;
            // }
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }
    }
}