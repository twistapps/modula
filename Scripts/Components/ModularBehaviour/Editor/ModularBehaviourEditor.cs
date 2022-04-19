using System.Linq;
using Modula.Common;
using UnityEditor;
using UnityEngine;

namespace Modula.Editor
{
    [CustomEditor(typeof(ModularBehaviour), true)]
    public class ModularBehaviourEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var moduleManager = (ModularBehaviour)target;
            if (moduleManager.AvailableModules != null)
            {
                ModuleManager();
            }
            else
            {
                EditorGUILayout.HelpBox(new GUIContent("Modules"));
                var textStyle = new GUIStyle { normal = { textColor = Color.gray } };
                GUILayout.Label("This ModularNetBehaviour has no available modules set up.", textStyle);
            }

            ShowDebugInfo();
            HandleDataLayer();
        }

        private void ModuleManager()
        {
            var mb = (ModularBehaviour)target;

            EditorGUILayout.HelpBox(new GUIContent("Attached Modules:"));
            var hasAttachedModules = false;
            foreach (var module in mb.GetAttachments())
            {
                hasAttachedModules = true;
                GUILayout.BeginHorizontal();
                GUILayout.Label(module.GetName());
                var requiredOthers = module.RequiredOtherModules?.Types;
                if (requiredOthers != null && requiredOthers.Count > 0)
                {
                    var requiredLabel = "Dependencies: ";
                    foreach (var other in requiredOthers) requiredLabel += other.Name + "  ";
                    var textStyle = new GUIStyle { normal = { textColor = Color.gray } };
                    GUILayout.Label(requiredLabel, textStyle);
                }

                if (GUILayout.Button("Remove", GUILayout.Width(80)))
                {
                    if (mb.CanRemove(module))
                    {
                        mb.RemoveModule(module, ModuleRemoveReason.RemovedFromGUI);
                    }
                    else
                    {
                        var errorText = "Can't remove module '" + module.GetType().Name +
                                        "' because it is required by these other modules: " +
                                        string.Join(", ", DependencyWorker.FindDependentModuleNames(module));
                        Debug.LogError(errorText, target);
                    }

                    break;
                }

                GUILayout.EndHorizontal();
            }

            if (!hasAttachedModules) GUILayout.Label("No attached modules.");
            GUILayout.Space(20);
            EditorGUILayout.HelpBox(new GUIContent("Other Available Modules:"));
            var hasAvailableModules = false;

            foreach (var moduleType in mb.AvailableModules.Types)
                if (mb.GetAttachments().Count(m => m.GetType() == moduleType) < 1)
                {
                    hasAvailableModules = true;
                    if (GUILayout.Button("Add " + moduleType.Name)) mb.AddModule(moduleType);
                }

            if (!hasAvailableModules) GUILayout.Label("This behaviour has no more available modules.");
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

        private void HandleDataLayer()
        {
            var moduleManager = (ModularBehaviour)target;
            var dataLayerType = moduleManager.GetDataLayerType();
            if (dataLayerType == null) return;
            if (moduleManager.GetData() != null) return;
            var data = moduleManager.gameObject.AddComponent(dataLayerType) as DataLayer;
            moduleManager.OnDataComponentCreated();
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }
    }
}