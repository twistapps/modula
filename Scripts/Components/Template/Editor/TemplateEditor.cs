using System.Collections.Generic;
using System.Linq;
using Modula.Common;
using Modula.Scripts.Common;
using Modula.Scripts.Common.Editor;
using UnityEditor;
using UnityEngine;

namespace Modula.Editor
{
    [CustomEditor(typeof(Template), true)]
    public class TemplateEditor : ModularBehaviourEditorBase
    {
        private TypeNames<IModule> _selections;
        private bool _dataLayerInitialized;

        private Template template => (Template)target;
        
        public override void OnInspectorGUI()
        {
            if (!template.scriptable) base.OnInspectorGUI();
            else
            {
                if (!DrawBanner())
                {
                    EditorGUILayout.HelpBox(template.scriptable.name + " Template", 
                        MessageType.Info);
                }

                DrawToolbox();
                GUILayout.Space(20);
            }

            DrawBasepartSelectors();
            
            GUILayout.Space(20);
            
            //DrawModuleManager(shouldDrawAvailableModules: false, canRemoveModules: false);
            DrawDebugInfo();
            
            if (!_dataLayerInitialized)
                InitializeDataLayer();
        }

        private bool DrawBanner()
        {
            Texture banner = template.scriptable.banner;
            if (!banner) return false;
            
            float widthRatio = .9f;
            int minWidth     = 340;
            int maxWidth     = 540;
            
            float width = Mathf.Clamp(Screen.width * widthRatio, minWidth, maxWidth);
            float height = width / (banner.width / banner.height);
            
            //Debug.Log(width + " | " + height);
            EditorGUI.DrawPreviewTexture(new Rect(Screen.width * (1 - (width / Screen.width)) * .5f,0, width, height), banner);
            GUILayout.Space(height);
            return true;
        }

        private void DrawToolbox()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Edit template", GUILayout.MaxWidth(100)))
            {
                Selection.activeObject = template.scriptable;
            }
            GUILayout.Space(Screen.width * .05f);
            GUILayout.EndHorizontal();
        }

        private void InitializeDataLayer()
        {
            ModulaEditorUtilities.HandleDataLayer((ModularBehaviour) target);
            _dataLayerInitialized = true;
        }

        
        private bool[] _foldoutActive;
        
        private void DrawBasepartSelectors()
        {
            var template = (Template)target;
            if (template.scriptable == null)
            {
                GUILayout.Box("Select a template above ^^");
                _dataLayerInitialized = false;
                return;
            }
            
            GUILayout.Label("Select Modules:", EditorStyles.boldLabel);

            if (ModulaUtilities.IsNullOrEmpty(template.scriptable.baseparts))
            {
                GUILayout.Box("No baseparts specified in this template.");
                return;
            }

            if (!template.AvailableModules.Any())
            {
                GUILayout.Box("This template has basepart(s) but they don't have any modules available."+
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

            bool selectionsAreEmpty = ModulaUtilities.IsNullOrEmpty(_selections!.Types);
            if (selectionsAreEmpty)
            {
                for (int i = 0; i < _selections.Types.Length; i++)
                {
                    var bp = template.scriptable.baseparts[i];
                    if (bp.optional) continue;
                    _selections[i] = bp.supports[0];
                    HandleSelectionChange(i);
                }
            }

            for (var i = 0; i < basepartsCount; i++)
            {
                if (basepartsCount != template.scriptable.baseparts.Count) break; //just in case count changes during draw process.
                var basepart = template.scriptable.baseparts[i];

                if (ModulaUtilities.IsNullOrEmpty(basepart.supports)) continue;

                var selectedIndex = selectionsAreEmpty ? 0 : basepart.supports.IndexOf(_selections.GetName(i));
                if (selectedIndex == -1)
                {
                    selectedIndex = 0;
                    _selections[0] = basepart.supports[0];
                    if (!basepart.optional) HandleSelectionChange(i);
                }

                GUILayout.BeginHorizontal();
                if (_foldoutActive == null || _foldoutActive.Length != basepartsCount)
                {
                    _foldoutActive = new bool[basepartsCount];
                }
                _foldoutActive[i] = EditorGUILayout.Foldout(_foldoutActive[i], basepart.name);
                EditorGUI.BeginChangeCheck();
                _selections[i] =
                    //basepart.supports[EditorGUILayout.Popup(basepart.name, selectedIndex, basepart.supports.ToArray())];
                    basepart.supports[EditorGUILayout.Popup(selectedIndex, basepart.supports.ToArray())];
                if (EditorGUI.EndChangeCheck())
                {
                    HandleSelectionChange(i);
                    _foldoutActive = new bool[basepartsCount];
                }
                GUILayout.EndHorizontal();
                
                if (_foldoutActive[i])
                {
                    GUILayout.Space(3);
                    //EditorGUILayout.HelpBox("Properties", MessageType.Info);
                    IModule module = template.GetModule(_selections.Types[i]);
                    List<IModule> dependencies = new List<IModule>();// { module };
                    dependencies.AddRange(DependencyWorker.FindDependenciesRecursive(module));

                    bool first = true;
                    foreach (var dep in dependencies)
                    {
                        if (!dep.ShouldSerialize()) continue;
                        string style = first ? "Window" : "CN Box";
                        //GUILayout.BeginVertical(dep.name,"Window");
                        GUILayout.Space(3);
                        GUILayout.BeginVertical(style);
                        if (first) GUILayout.Space(-18);
                        string labelText = dep == dependencies[0] ? "Properties" : dep.GetType().Name;
                        //GUILayout.Label(dep.GetType().Name);
                        var editor = UnityEditor.Editor.CreateEditor(dep as Object);
                        editor.OnInspectorGUI();
                        
                        first = false;
                        GUILayout.Space(3);
                        GUILayout.EndVertical();
                    }
                }
            }
        }

        private void HandleSelectionChange(int i)
        {
            var template = (Template)target;
            template.SetSelection(i, _selections.GetName(i));
            Debug.Log("Selected module: " + template.Selections[i], target);
        }
        
        private void DrawDebugInfo()
        {
            var moduleManager = (ModularBehaviour)target;
            GUILayout.Space(20);
            ModulaSettings.DebugMode = EditorGUILayout.Toggle(new GUIContent("Modular Entities Debug Mode"),
                ModulaSettings.DebugMode);

            foreach (var module in moduleManager.GetAttachments())
                module.hideFlags = ModulaSettings.DebugMode ? HideFlags.None : HideFlags.HideInInspector;
        }
    }
}