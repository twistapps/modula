using System;
using System.Collections.Generic;
using System.Linq;
using Modula.Common;
using UnityEditor;
using UnityEngine;

namespace Modula.Editor
{
    [CustomEditor(typeof(BasePart), true)]
    public class BasepartEditor : UnityEditor.Editor
    {
        private Type[] _registeredModules;
        private List<bool> _selectedModules;
        private bool _showSelectionMenu = true;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var basepart = (BasePart)target;

            Init(basepart);
            ModuleManager(basepart);
            LogSupports(basepart);
        }

        private void Init(BasePart basepart)
        {
            Type[] ignored = { typeof(IModule), typeof(Module) };
            _registeredModules ??= ModulaUtilities.GetDerivedFrom<IModule>(ignored);

            if (_selectedModules?.Count != _registeredModules.Length)
            {
                _selectedModules = new List<bool>(_registeredModules.Length);
                foreach (var t in _registeredModules) _selectedModules.Add(basepart.supports.Contains(t.Name));
            }

            EditorUtility.SetDirty(basepart);
        }

        private Type GetTypeByName(string name)
        {
            return _registeredModules.FirstOrDefault(t => t.Name == name);
        }
        private void ModuleManager(BasePart basepart)
        {
            _showSelectionMenu = EditorGUILayout.Foldout(_showSelectionMenu, "Supported Modules");
            if (_showSelectionMenu)
            {
                for (var i = 0; i < _registeredModules.Length; i++)
                {
                    var current = _registeredModules[i];
                    _selectedModules[i] = basepart.supports != null && basepart.supports.Contains(current.Name);
                    
                    EditorGUI.BeginChangeCheck();
                    _selectedModules[i] = GUILayout.Toggle(_selectedModules[i], current.Name);
                    if (EditorGUI.EndChangeCheck())
                        OnModuleSelectionChange(basepart);
                }
            }
        }
        
        private void OnModuleSelectionChange(BasePart basepart)
        {
            basepart.supports = new List<string>();
            if (basepart.optional) basepart.supports.Add(ModulaSettings.EMPTY_CHOICE);
            for (var i = 0; i < _registeredModules.Length; i++)
                if (_selectedModules[i])
                    basepart.supports.Add(_registeredModules[i].Name);
            EditorUtility.SetDirty(basepart);
        }

        private void LogSupports(BasePart basepart)
        {
            if (GUILayout.Button("Log Supported Modules"))
            {
                var log = new List<string>();
                foreach (var moduleType in basepart.supports) log.Add(moduleType);

                Debug.Log(string.Join(", ", log));
            }
        }
    }
}