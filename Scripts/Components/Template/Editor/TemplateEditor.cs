using System.Collections.Generic;
using System.Linq;
using Modula.Common;
using UnityEditor;
using UnityEngine;

namespace Modula.Editor
{
    [CustomEditor(typeof(Template), true)]
    public class TemplateEditor : UnityEditor.Editor
    {
        private TypeNames<IModule> _selections;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var template = (Template)target;
            DrawBasepartSelectors(template);
            GUILayout.Space(20);
            ModuleManager();
            ShowDebugInfo();

            // GUILayout.Space(20);
            // ModulaSettings.EditMode = EditorGUILayout.Toggle(new GUIContent("Edit Mode"),
            //     ModulaSettings.EditMode);
        }

        // private bool HasBasepartsWithContent(List<BasePart> baseparts)
        // {
        //     foreach (var bp in baseparts)
        //     {
        //         if (bp != null && !ModulaUtilities.IsNullOrEmpty(bp.supports))
        //         {
        //             return true;
        //         }
        //
        //         return false;
        //     }
        // }

        private void DrawBasepartSelectors(Template template)
        {
            if (template.scriptable == null)
            {
                GUILayout.Box("Select a template above ^^");
                return;
            }

            GUILayout.Space(20);
            GUILayout.Label("Select Modules:", EditorStyles.boldLabel);

            if (ModulaUtilities.IsNullOrEmpty(template.scriptable.baseparts))
            {
                GUILayout.Box("No baseparts specified in this template.");
                return;
            }

            //if (!HasBasepartsWithContent(template.scriptable.baseparts))
            if (!template.AvailableModules.Any())
            {
                GUILayout.Box("This template has basepart(s) but these baseparts have no available modules."+
                              " Please specify at least one supported module in a basepart.");
                return;
            }

            //
            //if template is selected and there is at least one not-null basepart:
            //

            var basepartsCount = template.scriptable.baseparts.Count;

            //if it is first render or baseparts of this template changed
            if (template.Selections == null || template.Selections.Length != basepartsCount)
                template.Selections = new string[basepartsCount];

            if (_selections?.Names != template.Selections)
                _selections = new TypeNames<IModule>(template.Selections, false);

            for (var i = 0; i < basepartsCount; i++)
            {
                if (basepartsCount != template.scriptable.baseparts.Count) break; //just in case count changes during draw process.
                var basepart = template.scriptable.baseparts[i];

                if (ModulaUtilities.IsNullOrEmpty(basepart.supports)) continue;

                var selectedIndex = ModulaUtilities.IsNullOrEmpty(_selections!.Types)
                    ? 0
                    : basepart.supports.IndexOf(_selections.GetName(i));
                
                if (selectedIndex == -1)
                {
                    selectedIndex = 0;
                    _selections[0] = basepart.supports[0];
                }

                EditorGUI.BeginChangeCheck();
                _selections[i] =
                    basepart.supports[EditorGUILayout.Popup(basepart.name, selectedIndex, basepart.supports.ToArray())];
                if (EditorGUI.EndChangeCheck())
                {
                    //template[i] = _selections.GetName(i);
                    template.SetSelection(i, _selections.GetName(i));
                    Debug.Log("Selected module: " + template.Selections[i], target);
                }
            }
        }
        
        private void ShowDebugInfo()
        {
            var moduleManager = (ModularBehaviour)target;
            GUILayout.Space(20);
            ModulaSettings.DebugMode = EditorGUILayout.Toggle(new GUIContent("Modular Entities Debug Mode"),
                ModulaSettings.DebugMode);

            //if (ModulaSettings.DebugMode) base.OnInspectorGUI();

            foreach (var module in moduleManager.GetAttachments())
                module.hideFlags = ModulaSettings.DebugMode ? HideFlags.None : HideFlags.HideInInspector;
        }
        
        private void ModuleManager()
        {
            var moduleManager = (ModularBehaviour)target;

            EditorGUILayout.HelpBox(new GUIContent("Attached Modules:"));
            var hasAttachedModules = false;
            foreach (var module in moduleManager.GetAttachments())
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

                GUI.enabled = false;
                GUILayout.Button("Remove", GUILayout.Width(80));
                GUI.enabled = true;

                GUILayout.EndHorizontal();
            }

            if (!hasAttachedModules) GUILayout.Label("No attached modules.");
            GUILayout.Space(20);
            EditorGUILayout.HelpBox(new GUIContent("Other Available Modules:"));
            var hasAvailableModules = false;

            foreach (var moduleType in moduleManager.AvailableModules.Types)
                if (moduleManager.GetAttachments().Count(m => m.GetType() == moduleType) < 1)
                {
                    hasAvailableModules = true;
                    GUI.enabled = false;
                    GUILayout.Button("Add " + moduleType.Name);
                    GUI.enabled = true;
                }

            if (!hasAvailableModules) GUILayout.Label("This behaviour has no more available modules.");
        }
    }
}