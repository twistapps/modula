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
            if (template.selections == null || template.selections.Length != basepartsCount)
                template.selections = new string[basepartsCount];

            if (_selections?.Names != template.selections)
                _selections = new TypeNames<IModule>(template.selections, false);

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
                    template.selections[i] = _selections.GetName(i);
                    Debug.Log("Selected module: " + template.selections[i], target);
                }
            }
        }
    }
}