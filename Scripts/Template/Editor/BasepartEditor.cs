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
        private bool _initialized = false;
        private bool _showSupportedModules = true;
        
        private Type[] _allModules;
        private List<bool> _selectedModules;
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
            Type[] ignored = { typeof(IModule), };
            _allModules ??= ModulaUtilities.GetDerivedFrom<IModule>(ignored);
            
            if (_selectedModules?.Count != _allModules.Length)
            {
                _selectedModules = new List<bool>(_allModules.Length);
                foreach (var t in _allModules)
                {
                    _selectedModules.Add(basepart.supports.Contains(t.Name));
                }
            }
            
            EditorUtility.SetDirty(basepart); 
            _initialized = true;
        }

        private Type GetTypeByName(string name)
        {
            return _allModules.FirstOrDefault(t => t.Name == name);
        }
        private void ModuleManager(BasePart basepart)
        {
            _showSupportedModules = EditorGUILayout.Foldout(_showSupportedModules, "Supported Modules");
            if (_showSupportedModules)
            {
                for (int i = 0; i < _allModules.Length; i++)
                {
                    Type currentType = _allModules[i];
                    _selectedModules[i] = basepart.supports != null && basepart.supports.Contains(currentType.Name);
                    _selectedModules[i] = GUILayout.Toggle(_selectedModules[i], currentType.Name);
                }

                basepart.supports = new List<string>();
                if (basepart.optional) basepart.supports.Add(ModulaSettings.EMPTY_CHOICE);
                for (int i = 0; i < _allModules.Length; i++)
                {
                    if (_selectedModules[i])
                    {
                        basepart.supports.Add(_allModules[i].Name);
                    }
                } 
                EditorUtility.SetDirty(basepart);
            }
        }

        private void LogSupports(BasePart basepart)
        {
            if (GUILayout.Button("Log Supported Modules variable"))
            {
                List<string> log = new List<string>();
                foreach (var moduleType in basepart.supports)
                {
                    log.Add(moduleType);
                }

                Debug.Log(String.Join(", ", log));
            }
        }
    }
}