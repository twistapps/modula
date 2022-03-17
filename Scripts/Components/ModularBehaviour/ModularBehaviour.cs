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
            if (module == null) return;
            //Debug.Log("Added module: " + module.GetType().Name);
            _modules.Add(module);
            module.OnAdd();
            UpdateModules();
        }

        public void AddModule(Type moduleType)
        {
            if (_modules.Any(m => m.GetType() == moduleType)) return;
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

        public IModule GetModule(string typeName)
        {
            return _modules.Find(m => m.GetName() == typeName);
        }

        public IModule GetModule(Type moduleType)
        {
            return _modules.Find(m => m.GetType() == moduleType);
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
            return _modules.All(m =>
            {
                var required = m.RequiredOtherModules;
                return required == null || !required.Contains(module.GetType());
            });
        }

        private IModule[] FindDependent(IModule module)
        {
            var dependent = _modules.Where(m =>
            {
                var required = m.RequiredOtherModules;
                return required != null && required.Contains(module.GetType());
            });
            
            return dependent.ToArray();
        }

        private IModule[] FindDependencies(IModule module)
        {
            return _modules.Where(m =>
                module.RequiredOtherModules.Contains(m.GetType())).ToArray();
        }

        private HashSet<IModule> FindDependenciesRecursive(IModule module, HashSet<IModule> dependencies = null)
        {
            dependencies ??= new HashSet<IModule> { module };
            dependencies.Add(module);
            
            var found = FindDependencies(module).Where(d => !dependencies.Contains(d)).ToArray();
            
            if (found.Length == 0)
            {
                dependencies.Add(module);
                return dependencies;
            }
            
            foreach (var m in found)
            {
                dependencies.UnionWith(FindDependenciesRecursive(m, dependencies));
            }
            
            return dependencies;
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
        /// 
        /// </summary>
        /// <param name="module"></param>
        /// <param name="forceDelete">Remove dependencies even if other modules depend on them</param>
        /// <param name="reason"></param>
        /// <param name="ignore"></param>
        public void RemoveModuleWithDependencies(IModule module, bool forceDelete=false,
            ModuleRemoveReason reason = ModuleRemoveReason.NotSpecified)
        {
            var dependencies = FindDependenciesRecursive(module);//.Reverse().ToArray();
            foreach (var dependency in dependencies)
            {
                bool whitelist = false;
                if (!forceDelete)
                {
                    foreach (var other in _modules.Where(m => !dependencies.Contains(m)) )
                    {
                        if (FindDependencies(other).Contains(dependency))
                        {
                            whitelist = true;
                        }
                    }
                }
                if (!whitelist) RemoveModule(dependency);
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

        private void UpdateModules(bool shouldResolveDependencies=true)
        {
            _modules = gameObject.FindComponents<IModule>();
            //_modules = GetComponents<Module>().ToList();
            foreach (var module in _modules)
            {
                if (shouldResolveDependencies || couldBeModifiedFromEditorUI)
                    // check if user did remove a module using Unity Editor GUI. If yes, return;
                    // because ResolveDependencies implicitly calls UpdateModules() again if _modules has been modified.
                    if (ResolveDependencies(module))
                        return;
                if (AvailableModules.Contains(module.GetType())) continue;
                else if (CanRemove(module))
                {
                    RemoveModule(module);
                    return;
                }
            }
        }
    }
}