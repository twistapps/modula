using Modula.Common;
using UnityEditor;
using UnityEngine;

namespace Modula.Editor
{
    [CustomEditor(typeof(Template), true)]
    public class TemplateEditor : UnityEditor.Editor
    {
        // private bool CheckBaseparts<T>(List<T> baseparts)
        // {
        //     if (baseparts == null || baseparts.Count == 0)
        //     {
        //         return false;
        //     }
        //
        //     return baseparts.Any(basepart => basepart != null);
        // }

        private TypeNames<IModule> _selections;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var template = (Template)target;
            BasepartSelector(template);

            // GUILayout.Space(20);
            // ModulaSettings.EditMode = EditorGUILayout.Toggle(new GUIContent("Edit Mode"),
            //     ModulaSettings.EditMode);
        }

        private void BasepartSelector(Template template)
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
                if (basepartsCount != template.scriptable.baseparts.Count) break;
                var basepart = template.scriptable.baseparts[i];
                //var supportedByBasepart = new TypeNames<IModule>(basepart.supports.ToArray(), false);

                if (ModulaUtilities.IsNullOrEmpty(basepart.supports)) continue;

                int selectedIndex;
                selectedIndex = ModulaUtilities.IsNullOrEmpty(_selections!.Types)
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

        // private void ModuleSelector(Template template)
        // {
        //     TemplateScriptable templateScriptable = template.template;
        //     if (templateScriptable == null) return;
        //     GUILayout.Box("Select Modules:");
        //     var connectedModules = template.GetModules();
        //     var basepartsCount = templateScriptable.baseparts?.Count() ?? 0;
        //     var selections = template.selections;
        //     if (selections == null || selections.Length != basepartsCount)
        //     {
        //         template.selections = new Type[basepartsCount];
        //     }
        //
        //     bool selectionsIsEmpty = false;
        //     foreach (var selection in template.selections)
        //     {
        //         if (selection == null) selectionsIsEmpty = true;
        //     }
        //
        //     if (basepartsCount <= 0) return;
        //     for (int i = 0; i < basepartsCount; i++)
        //     {
        //         var basepart = templateScriptable.baseparts[i];
        //         if ((basepart.supports?.Count() ?? 0) < 1) continue;
        //         //string[] supports = basepart.supports.Select(mod => mod.Name).ToArray();
        //         string[] supports = basepart.supports.ToArray();
        //         string[] selectionsNames = null;
        //         if (!selectionsIsEmpty) 
        //         {
        //             selectionsNames = template.selections.Select(t => t.Name).ToArray();
        //         }
        //         int selectedIndex = selectionsNames == null ? 0 : basepart.supports.IndexOf(selectionsNames[i]);
        //         if (selectedIndex < 0 || selectedIndex >= template.selections.Length) selectedIndex = 0;
        //         template.selections[i] = ModulaUtilities.GetTypeByName<IModule>(
        //             basepart.supports[EditorGUILayout.Popup(basepart.name, selectedIndex, supports)]);
        //     }
        //
        //     EditorUtility.SetDirty(template);
        // }
    }
}