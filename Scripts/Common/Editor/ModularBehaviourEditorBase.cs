using System.Collections.Generic;
using System.Linq;
using Modula.Common;
using UnityEditor;
using UnityEngine;

namespace Modula.Editor
{
    public class ModularBehaviourEditorBase : UnityEditor.Editor
    {
        private Dictionary<string, bool> _foldoutActive;

        private void ShowSetupIncompleteNotice()
        {
            EditorGUILayout.HelpBox(new GUIContent("Modules"));
            var textStyle = new GUIStyle { normal = { textColor = Color.gray } };
            GUILayout.Label("This ModularBehaviour has no available modules set up.", textStyle);
        }

        private void DrawAvailableModules(bool canAddModules = true)
        {
            var mb = (ModularBehaviour)target;
            EditorGUILayout.HelpBox(new GUIContent("Other Available Modules:"));
            var hasAvailableModules = false;

            foreach (var moduleType in mb.AvailableModules.Types)
                if (mb.GetAttachments().Count(m => m.GetType() == moduleType) < 1)
                {
                    hasAvailableModules = true;
                    GUI.enabled = canAddModules;
                    if (GUILayout.Button("Add " + moduleType.Name)) mb.AddModule(moduleType);
                    GUI.enabled = true;
                }

            if (!hasAvailableModules) GUILayout.Label("This behaviour has no more available modules.");
        }

        private void DrawAttachedModules(bool canRemoveModules = true)
        {
            var mb = (ModularBehaviour)target;
            EditorGUILayout.HelpBox(new GUIContent("Attached Modules:"));
            var hasAttachedModules = false;
            var attachments = mb.GetAttachments();
            var attachmentNames = attachments.Select(a => a.GetType().Name).ToArray();

            if (_foldoutActive == null || _foldoutActive.Keys.Count != attachmentNames.Count())
            {
                _foldoutActive = new Dictionary<string, bool>(attachments.Count);
                foreach (var module in attachments) _foldoutActive[module.GetType().Name] = false;
                //Debug.Log("Foldout info rebuilt");
            }

            // string deb = "";
            // foreach (var name in attachmentNames)
            // {
            //     //deb += name.Key + " | " + name.Value + "\n";
            //     deb += name + "\n";
            // }
            // Debug.Log(deb);

            foreach (var module in attachments)
            {
                hasAttachedModules = true;
                GUILayout.BeginHorizontal();
                GUILayout.Label(module.GetName());
                GUILayout.FlexibleSpace();

                _foldoutActive[module.GetType().Name] =
                    GUILayout.Toggle(_foldoutActive[module.GetType().Name], "Edit Properties");

                if (canRemoveModules)
                {
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
                }
                else
                {
                    GUI.enabled = false;
                    GUILayout.Button("Remove", GUILayout.Width(80));
                    GUI.enabled = true;
                }

                GUILayout.EndHorizontal();

                if (_foldoutActive[module.GetType().Name])
                {
                    GUILayout.BeginVertical("Properties", "Window");
                    var editor = CreateEditor(module as Object);
                    //editor.OnInspectorGUI();
                    editor.DrawDefaultInspector();
                    GUILayout.Space(8);
                    var requiredOthers = module.RequiredOtherModules?.Types;
                    if (requiredOthers != null && requiredOthers.Count > 0)
                    {
                        var requiredLabel = "Dependencies: ";
                        //foreach (var other in requiredOthers) requiredLabel += other.Name + "  ";
                        requiredLabel += string.Join(", ", requiredOthers.Select(other => other.Name));
                        var textStyle = new GUIStyle { normal = { textColor = Color.gray } };
                        GUILayout.Label(requiredLabel, textStyle);
                    }

                    GUILayout.EndVertical();
                }
            }

            if (!hasAttachedModules) GUILayout.Label("No attached modules.");
            GUILayout.Space(20);
        }

        protected void DrawModuleManager(
            bool shouldDrawAttachedModules = true,
            bool shouldDrawAvailableModules = true,
            bool canAddModules = true,
            bool canRemoveModules = true)
        {
            var mb = (ModularBehaviour)target;

            if (mb.AvailableModules == null)
            {
                ShowSetupIncompleteNotice();
                return;
            }

            if (shouldDrawAttachedModules) DrawAttachedModules(canRemoveModules);

            if (shouldDrawAvailableModules) DrawAvailableModules(canAddModules);
        }
    }
}