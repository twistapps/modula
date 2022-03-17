using System;
using System.Collections.Generic;
using System.Linq;
using Modula.Common;
using UnityEditor;
using UnityEngine;

namespace Modula
{
    public abstract class ModularBehaviour : MonoBehaviour
    {
        private List<IModule> _modules;

        public abstract TypedList<IModule> AvailableModules { get; }

        private bool couldBeModifiedFromEditorUI => ModulaSettings.DebugMode;

        public DataLayer GetData()
        {
            return GetComponent<DataLayer>();
        }
        
        public virtual Type GetDataLayerType()
        {
            return null;
        }

        public virtual void OnDataComponentCreated()
        {
        }

        private void OnModuleAdded(IModule module)
        {
            _modules.Add(module);
            module.OnAdd();
            UpdateModules();
        }

        public void AddModule(Type moduleType)
        {
            IModule module;
            if (Application.isEditor && !Application.isPlaying)
                module = Undo.AddComponent(gameObject, moduleType) as IModule;
            else
                module = gameObject.AddComponent(moduleType) as IModule;
            OnModuleAdded(module);
        }

        public List<IModule> GetModules()
        {
            UpdateModules();
            return _modules;
        }

        public T GetModule<T>() where T : IModule
        {
            return (T)_modules.First(m => m.GetType() == typeof(T));
        }

        private string GetReasonString(ModuleRemoveReason reason)
        {
            switch (reason)
            {
                case ModuleRemoveReason.NotSupportedByThisBehaviour:
                    return "Module is not supported by ModularNetBehaviour of this type";
                case ModuleRemoveReason.RemovedFromGUI:
                    return "Module is removed using Editor GUI";
                case ModuleRemoveReason.NotSpecified:
                default:
                    return "Reason is not specified.";
            }
        }

        //todo: recursively check dependencies of dependencies
        public bool CanRemove(IModule module)
        {
            // return _modules.All(m =>
            // {
            //     var required = m.RequiredOtherModules;
            //     return required == null || !required.Contains(module.GetType());
            // });

            return FindDependent(module).Length > 0;
        }

        public IModule[] FindDependent(IModule module)
        {
            var dependent = _modules.Where(m =>
            {
                var required = m.RequiredOtherModules;
                return required != null && required.Contains(module.GetType());
            });
            
            return dependent.ToArray();
        }

        public string[] FindDependentModuleNames(IModule module)
        {
            var dependent = FindDependent(module);
            return dependent.Select(m => m.GetName()).ToArray();
        }

        public void RemoveModule(IModule module, ModuleRemoveReason reason = ModuleRemoveReason.NotSpecified)
        {
            _modules.Remove(module);
            if (Application.isEditor && !Application.isPlaying)
            {
                Debug.Log(string.Concat("Removing module: ", module.GetType().Name,
                    ", reason: ", GetReasonString(reason), " (class ", GetType().Name, ")"), gameObject);
                Undo.DestroyObjectImmediate(module as MonoBehaviour);
                //DestroyImmediate(module as MonoBehaviour);
            }
            else
            {
                Destroy(module as MonoBehaviour);
            }
        }

        /// <summary>
        ///     Check if a module has dependencies that are not present in this GameObject.
        ///     Adds these 'unresolved' dependencies to the GameObject.
        /// </summary>
        /// <param name="module">The module to check dependencies of.</param>
        /// <returns>true, if any module(s) have been added to this behaviour during the method run.</returns>
        private bool ResolveDependencies(IModule module)
        {
            var modulesModified = false;
            foreach (var requirement in module.RequiredOtherModules)
                if (_modules.Find(m => m.GetType() == requirement) == null)
                {
                    AddModule(requirement);
                    modulesModified = true;
                }

            return modulesModified;
        }

        private void UpdateModules()
        {
            _modules = gameObject.FindComponents<IModule>();
            //_modules = GetComponents<Module>().ToList();
            foreach (var module in _modules)
            {
                if (couldBeModifiedFromEditorUI)
                    // check if user did remove a module using Unity Editor GUI. If yes, return;
                    // because ResolveDependencies implicitly calls UpdateModules() again if _modules has been modified.
                    if (ResolveDependencies(module))
                        return;
                if (AvailableModules.Contains(module.GetType())) continue;
                if (CanRemove(module)) RemoveModule(module);
            }
        }
    }
}