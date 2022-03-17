using System;
using System.Collections.Generic;
using System.Linq;
using Modula.Common;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Modula
{
    public class Template : ModularBehaviour
    {
        public TemplateScriptable scriptable;
        [HideInInspector][SerializeField] private string[] _selections;
        public string[] Selections
        {
            get => _selections;
            set
            {
                Debug.Log("Selections change");
                HandleSelectionsChange(_selections, value);
                _selections = value;
            }
        }
        
        public string this[int index]
        {
            get => _selections[index];
            set
            {
                var oldValue = new string[_selections.Length];
                for (int i = 0; i < _selections.Length; i++)
                {
                    if (_selections[i] == null)
                    {
                        oldValue[i] = null;
                        continue;
                    } 
                    oldValue[i] = string.Copy(_selections[i]);
                }
                _selections[index] = value;
                HandleSelectionsChange(oldValue, _selections);
            }
        }

        public override TypedList<IModule> AvailableModules
        {
            get
            {
                var modules = new TypedList<IModule>();
                if (scriptable == null) return modules;
                foreach (var basepart in scriptable.baseparts)
                {
                    var types = basepart.supports?.ToTypesList<IModule>();
                    if (types == null) continue;
                    foreach (var type in types) modules.Add(type);
                }

                return modules;
            }
        }

        private void HandleSelectionsChange(string[] oldValue, string[] newValue)
        {
            if (oldValue != null)
            {
                var removed = oldValue.Except(newValue).Where(m => m != ModulaSettings.EMPTY_CHOICE);
                foreach (var moduleName in removed)
                {
                    var toRemove = GetModule(moduleName);
                    if (toRemove == null) continue;
                    RemoveModuleWithDependencies(toRemove);
                } 
            }
            
            var added = oldValue != null ? newValue.Except(oldValue) : newValue;
            added = added.Where(m => m != ModulaSettings.EMPTY_CHOICE);

            foreach (var moduleName in added)
            {
                var toAdd = AvailableModules.FirstOrDefault(m => m.Name == moduleName);
                if (toAdd == null) continue;
                AddModule(toAdd);
            }
            
        }
    }
}