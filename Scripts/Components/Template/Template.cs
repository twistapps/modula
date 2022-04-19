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

        public void SetSelection(int index, string value)
        {
            var oldValue = _selections.Copy();
            _selections[index] = value;
            HandleSelectionsChange(oldValue, _selections);
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
                var toRemove = oldValue.Except(newValue).Where(m => m != ModulaSettings.EMPTY_CHOICE).ToArray()
                    .ToTypesArray<IModule>() as IModule[];
                
                DependencyWorker.RemoveWithDependencies(toRemove, this);
            }
            
            var added = oldValue != null ? newValue.Except(oldValue) : newValue;
            added = added.Where(m => m != ModulaSettings.EMPTY_CHOICE).ToArray();

            AddModules(added.ToDerivedFrom<IModule>());

            // foreach (var moduleName in added)
            // {
            //     var toAdd = AvailableModules.FirstOrDefault(m => m.Name == moduleName);
            //     if (toAdd == null) continue;
            //     AddModule(toAdd);
            // }
            
        }
    }
}